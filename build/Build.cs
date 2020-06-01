using System;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Tools.SonarScanner;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
// ReSharper disable once CheckNamespace
class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main() => Execute<Build>(x => x.Test);

    const string Framework = "netcoreapp3.1";

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;
    [GitVersion(Framework = Framework)] readonly GitVersion GitVersion;

    [Parameter("The SonarQube login token")] readonly string SonarLogin = Environment.GetEnvironmentVariable("SONAR_LOGIN");
    [Parameter("The SonarQube server")] readonly string SonarServer = IsLocalBuild ? "http://localhost:9000" : "https://sonarcloud.io";
    [Parameter("The SonarQube organization")] readonly string SonarOrganization = "awesome-inc";


    [Parameter("Enable coverlet diagnostics (log.*.txt)")] readonly bool CoverletDiag;
    static AbsolutePath TestsDirectory => RootDirectory / "tests";


    // ReSharper disable once UnusedMember.Local
    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            TestsDirectory
                .GlobDirectories("**/bin", "**/obj", "**/TestResults")
                .ForEach(DeleteDirectory);
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            DotNetRestore(s => s
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(s => s
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore()
            );
        });

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(settings => settings
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .EnableNoBuild()
                .EnableBlameMode()
                .SetLogger("trx")
                .EnableCollectCoverage()
                .SetDataCollector("XPlat Code Coverage")
                .SetSettingsFile("coverage.runsettings")
                .SetProperty("CopyLocalLockFileAssemblies", true)
                .SetDiagnosticsFile(CoverletDiag ? "log.txt" : null)
            );
        });

    Target Coverage => _ => _
        .DependsOn(Test)
        .Executes(() =>
        {
            ReportGeneratorTasks.ReportGenerator(settings => settings
                .SetFramework("netcoreapp3.0")
                .SetReports("**/coverage.opencover.xml")
                .SetReportTypes(ReportTypes.Cobertura, ReportTypes.SonarQube, ReportTypes.Html)
                .SetTargetDirectory(".coverage/")
            );

        });

    Target Sonar => _ => _
            .Description("SonarQube analysis")
            .DependsOn(SonarBegin)
            .DependsOn(Coverage)
            .DependsOn(SonarEnd)
            .Executes(() =>
            {
            });

    Target SonarBegin => _ => _
        .Before(Test)
        .Executes(() =>
        {
            SonarScannerTasks.SonarScannerBegin(settings => settings
                    .SetProjectKey("awesome-inc_FontAwesome.Sharp")
                    .SetName("FontAwesome.Sharp")
                    .SetVersion(GitVersion.FullSemVer)
                    .SetVSTestReports("**/*.trx")
                    // cf.: https://github.com/nuke-build/nuke/issues/377#issuecomment-595276623
                    .SetFramework("netcoreapp3.0")
                    .SetLogin(SonarLogin) // TODO: should be secret -> SetArgument
                    .SetServer(SonarServer)
                    .SetArgumentConfigurator(args =>
                    {
                        // monorepo hack: tell Sonar to scan sub-project only
                        args.Add($"/d:sonar.projectBaseDir={RootDirectory}");
                        // generic coverage data, cf.: https://docs.sonarqube.org/latest/analysis/generic-test/
                        args.Add($"/d:sonar.coverageReportPaths=.coverage/SonarQube.xml");
                        if (!string.IsNullOrWhiteSpace(SonarLogin))
                        {
                            // set organization & branch name, cf.:
                            // - https://github.com/nuke-build/nuke/issues/304#issuecomment-522250591
                            // - http://www.nuke.build/docs/authoring-builds/cli-tools.html#custom-arguments
                            args.Add($"/o:{SonarOrganization}")
                                .Add($"/d:sonar.branch.name={GitVersion.BranchName}");
                        }
                        return args;
                    })
            );
        });

    Target SonarEnd => _ => _
        .After(Coverage)
        .Executes(() =>
        {
            SonarScannerTasks.SonarScannerEnd(settings => settings
                .SetLogin(SonarLogin) // TODO: should be secret -> SetArgument
                                      // cf.: https://github.com/nuke-build/nuke/issues/377#issuecomment-595276623
                .SetFramework("netcoreapp3.0")
            );
        });

// ReSharper disable once UnusedMember.Local
    Target DevOpsBuild => _ => _
        .Description("DevOps build target")
        .DependsOn(Sonar)
        //.DependsOn(Push)
        .Executes(() =>
        {
        });

}
