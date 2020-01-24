//-------------------------------------------------------------
// Run a target (exclusively), cf.: https://cakebuild.net/docs/fundamentals/running-targets
// .\build.ps1 -Target Test --exclusive
// cake --target=Test --exclusive
//-------------------------------------------------------------

var project = Argument("project", "./FontAwesome.Sharp.sln");
var configuration = Argument("configuration", "Release");
var verbosity = Argument("verbosity", DotNetCoreVerbosity.Minimal);
var target = Argument("target", "Default");

var isCiBuild = AppVeyor.IsRunningOnAppVeyor;
// https://www.appveyor.com/docs/environment-variables/
var isTag = (EnvironmentVariable("APPVEYOR_REPO_TAG") ?? "-unset-") == "true";
//---
// detect if running in Azure DevOps, cf.:
// - https://docs.microsoft.com/en-us/azure/devops/pipelines/build/variables?view=azure-devops&tabs=yaml#system-variables
var inAzure = HasEnvironmentVariable("TF_BUILD");
GitVersion gitVersion = null;

//-------------------------------------------------------------
var coverageDirectory = ".coverage";

Task("Clean")
    .Does(() =>
{
    CleanDirectories($"**/bin/{configuration}");
    CleanDirectories($"**/obj/{configuration}");
    CleanDirectory(coverageDirectory);
});

//-------------------------------------------------------------
Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore();
});

//-------------------------------------------------------------
// https://www.nuget.org/packages/GitVersion.CommandLine
#tool nuget:?package=GitVersion.CommandLine&version=5.1.3
Task("Version")
    .Does(() =>
{
    gitVersion = GitVersion(new GitVersionSettings {
        OutputType = isCiBuild ? GitVersionOutput.BuildServer : GitVersionOutput.Json
    });
    
    Information($"GitVersion: {gitVersion.SemVer}");
});

//-------------------------------------------------------------
Task("Build")
    .IsDependentOn("Restore")
    .IsDependentOn("Version")
    .Does(() =>
{
    // cf.: https://cakebuild.net/api/Cake.Common.Tools.DotNetCore.Build/DotNetCoreBuildSettings/
    var settings = new DotNetCoreBuildSettings
    {
        NoRestore = true,
        Configuration = configuration,
        Verbosity = verbosity
    };
    DotNetCoreBuild(project, settings);
});

//-------------------------------------------------------------
Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    var runSettings = MakeAbsolute(File("./coverage.runsettings"));
    var settings = new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true, // avoid a second build, we already depend on build (see above)
        //NoRestore = true, // implied with `NoBuild`
        Verbosity = verbosity,
        // https://github.com/Microsoft/vstest-docs/blob/master/RFCs/0021-CodeCoverageForNetCore.md
        ArgumentCustomization = args => args
            .Append("--blame")
            .Append("--logger").Append("trx")
            .Append("--collect").AppendQuoted("Code Coverage")
            // cf.: https://github.com/tonerdo/coverlet#vstest-integration
            .Append("--collect:\"XPlat Code Coverage\"")
            // - https://github.com/Microsoft/vstest/issues/1917
            // - https://docs.microsoft.com/en-us/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file?view=vs-2019#example-runsettings-file
            // - https://github.com/tonerdo/coverlet/blob/master/Documentation/VSTestIntegration.md#advanced-options-supported-via-runsettings
            .Append($"--settings:{runSettings.FullPath}")
    };

    DotNetCoreTest(project, settings);

});

//-------------------------------------------------------------
// cf.: https://medium.com/@pavel.sulimau/dotnetcore-xunit-coverlet-reportgenerator-cake-codecoveragereport-1ed4adf408d2
#tool nuget:?package=ReportGenerator&version=4.2.2
Task("CoverageReport")
    .IsDependentOn("Test")
    .Does(() =>
{
    // // Use "Html" value locally for performance and file size.
    // var reportTypes = inAzure ? "HtmlInline_AzurePipelines" : "Html";

    // var reportSettings = new ReportGeneratorSettings
    // {
    //     ArgumentCustomization = args => args.Append($"-reportTypes:{reportTypes}")
    // };

    if (!DirectoryExists(coverageDirectory))
        CreateDirectory(coverageDirectory);
    else
        CleanDirectory(coverageDirectory);

    var coverageFiles = GetFiles("**/coverage.opencover.xml");
    ReportGenerator(coverageFiles, coverageDirectory); //, reportSettings);
});

//-------------------------------------------------------------
// cf.: https://github.com/AgileArchitect/Cake.Sonar
#addin nuget:?package=Cake.Sonar&version=1.1.22
#tool nuget:?package=MSBuild.SonarQube.Runner.Tool&version=4.6.0 //4.3.1 //4.6.0 breaks ImportBefore.targets
var sonarLogin = EnvironmentVariable("SONAR_LOGIN") ?? "-unset-";

Task("Sonar")
    .IsDependentOn("SonarBegin")
    .IsDependentOn("Test")
    .IsDependentOn("SonarEnd");

Task("SonarBegin")
    .IsDependentOn("Version")
    .Does(() =>
{
    var settings = new SonarBeginSettings
    {
        Key = "awesome-inc_FontAwesome.Sharp",
        Name = "FontAwesome.Sharp",
        Version = gitVersion.FullSemVer,
        VsTestReportsPath = "**/*.trx",
        OpenCoverReportsPath = "**/coverage.opencover.xml",
        Exclusions = "**/*.css" // Exclude imported CSS
    };
    if (!string.IsNullOrEmpty(sonarLogin)) {
        settings.Url = "https://sonarcloud.io";
        settings.Login = sonarLogin;
        settings.Organization = "awesome-inc";
        settings.Branch = gitVersion.BranchName;
    }
    SonarBegin(settings);
});

Task("SonarEnd")
    .Does(() =>
{
    var settings = new SonarEndSettings();
    if (!string.IsNullOrEmpty(sonarLogin)) settings.Login = sonarLogin;
    SonarEnd(settings);
});

//-------------------------------------------------------------
Task("Package")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new DotNetCorePackSettings
     {
         Configuration = configuration,
         OutputDirectory = "./artifacts/"
     };
    DotNetCorePack(project, settings);
});

Task("Push")
    .IsDependentOn("Package")
    .Does(() =>
{
    if (isCiBuild && !isTag) {
        Information("Skipping (no tag)");
        return;
    }

    DotNetCoreNuGetPush($"./artifacts/FontAwesome.Sharp.{gitVersion.NuGetVersion}.nupkg", new DotNetCoreNuGetPushSettings {
      Source = "https://www.nuget.org",
      ApiKey = EnvironmentVariable("NUGET_API_KEY") ?? "-unset-"
    });
});

//-------------------------------------------------------------
Task("CiBuild")
    .IsDependentOn("Sonar")
    .IsDependentOn("Push");

//-------------------------------------------------------------
Task("Default")
    .IsDependentOn("Test");

//-------------------------------------------------------------

RunTarget(target);
