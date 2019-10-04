//-------------------------------------------------------------
// Run a target (exclusively), cf.: https://cakebuild.net/docs/fundamentals/running-targets
// .\build.ps1 -Target Test --exclusive
// cake --target=Test --exclusive
//-------------------------------------------------------------

var project = Argument("project", "./FontAwesome.Sharp.sln");
var configuration = Argument("configuration", "Release");
var verbosity = Argument("verbosity", Verbosity.Minimal);
var target = Argument("target", "Default");

// detect if running in Azure DevOps, cf.:
// - https://docs.microsoft.com/en-us/azure/devops/pipelines/build/variables?view=azure-devops&tabs=yaml#system-variables
var inAzure = HasEnvironmentVariable("TF_BUILD");

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
    NuGetRestore(project, new NuGetRestoreSettings { PackagesDirectory="packages"});
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    MSBuild(project, settings =>
        settings.SetConfiguration(configuration)
            .UseToolVersion(MSBuildToolVersion.VS2019)
            .SetVerbosity(verbosity));
});

//-------------------------------------------------------------
Task("Test")
    .IsDependentOn("TestDotNet")
    .Does(() =>
{
});

//-------------------------------------------------------------
#tool nuget:?package=OpenCover&version=4.7.922
#tool nuget:?package=NUnit.ConsoleRunner&version=3.10.0
Task("TestDotNet")
    .IsDependentOn("Build")
    .Does(() =>
{
    var openCoverSettings = new OpenCoverSettings
    {
        Register = "user",
        SkipAutoProps = true,
        ArgumentCustomization = args => args.Append("-coverbytest:*.Tests.dll").Append("-mergebyhash")
    };

    var resultsDir = "TestResults";
    if (!DirectoryExists(resultsDir))
        CreateDirectory(resultsDir);
    else
        CleanDirectory(resultsDir);

    var openCoverOutput = $"{resultsDir}/coverage.opencover.xml";

    OpenCover(tool => {
            var settings = new NUnit3Settings {
                 NoResults = false,
                // worakround OpenCover issue, cf.: https://github.com/OpenCover/opencover/issues/677
                // Standard Azure Agent DS2v2 series has 2 vCPUs, cf.: https://docs.microsoft.com/en-us/azure/virtual-machines/windows/sizes-general#dsv2-series
                AppDomainUsage = inAzure ? NUnit3AppDomainUsage.Single : NUnit3AppDomainUsage.Default
            };

            var testAssemblies = GetFiles($"*.Tests/**/bin/{configuration}/**/*.Tests.dll");
            tool.NUnit3(testAssemblies, settings);
        },
        openCoverOutput,
        openCoverSettings
            .WithFilter("+[FontAwesome.*]*")
            .WithFilter("-[*.Tests]*")
    );
});

//-------------------------------------------------------------
// cf.: https://medium.com/@pavel.sulimau/dotnetcore-xunit-coverlet-reportgenerator-cake-codecoveragereport-1ed4adf408d2
#tool nuget:?package=ReportGenerator&version=4.2.2
Task("CoverageReport")
    .IsDependentOn("Test")
    .Does(() =>
{
    // Use "Html" value locally for performance and file size.
    var reportTypes = inAzure ? "HtmlInline_AzurePipelines" : "Html";

    var reportSettings = new ReportGeneratorSettings
    {
        ArgumentCustomization = args => args.Append($"-reportTypes:{reportTypes}")
    };

    if (!DirectoryExists(coverageDirectory))
        CreateDirectory(coverageDirectory);
    else
        CleanDirectory(coverageDirectory);

    var coverageFiles = GetFiles("**/coverage.opencover.xml");
    ReportGenerator(coverageFiles, coverageDirectory, reportSettings);
});

//-------------------------------------------------------------
// cf.: https://github.com/AgileArchitect/Cake.Sonar
#addin nuget:?package=Cake.Sonar&version=1.1.22
#tool nuget:?package=MSBuild.SonarQube.Runner.Tool&version=4.6.0 //4.3.1 //4.6.0 breaks ImportBefore.targets
var sonarLogin = "347c73b514310cf66d52f69bf3e8212a86eaca4c";

Task("Sonar")
    .IsDependentOn("SonarBegin")
    .IsDependentOn("Test")
    .IsDependentOn("SonarEnd");

Task("SonarBegin")
    .Does(() =>
{
    var settings = new SonarBeginSettings
    {
        Url = "https://sonarcloud.io",
        Login = sonarLogin,
        Organization = "tracto-technik",
        Key = "tracto.technik.hmi",
        Version = GitVersion().FullSemVer,
        VsTestReportsPath = "**/*.trx", // dotnet core
        NUnitReportsPath = "**/TestResult.xml",
        OpenCoverReportsPath = "**/coverage.opencover.xml"
    };
    SonarBegin(settings);
});

Task("SonarEnd")
    .Does(() =>
{
    var settings = new SonarEndSettings
    {
        Login = sonarLogin
    };
    SonarEnd(settings);
});

//-------------------------------------------------------------
Task("AzureBuild")
    .IsDependentOn("Sonar")
    .IsDependentOn("CoverageReport");

//-------------------------------------------------------------
Task("Default")
    .IsDependentOn("Build");

RunTarget(target);
