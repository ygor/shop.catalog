#addin "nuget:?package=Cake.Sonar"
#tool "nuget:?package=MSBuild.SonarQube.Runner.Tool"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solution = "./../Shop.Catalog.sln";
var projectName = "rtl-entitlement";
var branchname = TFBuild.Environment.Repository.Branch;

var sonarUrl = "https://sonarqube.rtlnederland.nl/";
var sonarLogin = "3e16e1c6145703d400174a776d8b6aea8b611e64";

var testProjects = new Dictionary<string, string> {
    { "application", "./../test/Shop.Catalog.Application.Test/Shop.Catalog.Application.Test.csproj" },
    };

Task("Clean")
    .Does(() => 
{
    DotNetCoreClean(solution);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore(solution);
});    

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetCoreBuild(solution,
        new DotNetCoreBuildSettings()
        {
            Configuration = configuration,
            NoRestore = true,
            ArgumentCustomization = args => args.Append($"/p:DebugType=Full")
        });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    foreach(var project in testProjects) {
		DotNetCoreTest(project.Value, 
            new DotNetCoreTestSettings {
				Configuration = configuration,
				NoBuild = true,
				ArgumentCustomization = args => args.Append(" -l trx")
            });
    }
});

Task("SonarBegin")
  .Does(() => {
     SonarBegin(new SonarBeginSettings {
        Url = sonarUrl,
        Login = sonarLogin,
        Verbose = false,
        Key = projectName,
        OpenCoverReportsPath = "**/opencover.xml",
        VsTestReportsPath = "**/*.trx",
        Name = projectName + "-" + branchname
     });
  });

Task("SonarEnd")
  .Does(() => {
     SonarEnd(new SonarEndSettings{
        Login = sonarLogin
     });
  });

Task("Sonar")
  .IsDependentOn("SonarBegin")
  .IsDependentOn("Test")
  .IsDependentOn("SonarEnd");
  
Task("Default")
    .IsDependentOn("Sonar");

RunTarget(target);