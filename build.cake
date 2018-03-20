#addin "nuget:?package=Cake.Sonar"
#tool "nuget:?package=MSBuild.SonarQube.Runner.Tool"
#tool "nuget:?package=OpenCover"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solution = "./Rtl.Keys.sln";
var projectName = "rtl-keys";
var framework = "netcoreapp2.0";

var testProjects = new Dictionary<string, string> {
    { "application", "./tests/Rtl.Keys.Application.Tests/Rtl.Keys.Application.Tests.csproj" },
    { "api", "./tests/Rtl.Keys.Api.Tests/Rtl.Keys.Api.Tests.csproj" },
    { "data", "./tests/Rtl.Keys.Data.Tests/Rtl.Keys.Data.Tests.csproj" }
    };

var sonarUrl = "http://sonarqube.service.consul:27001";
var sonarLogin = "b8224b9d2e7b5275ef0b2b5e17070ae62a57342d";

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

/// Whole solution execution
Task("Restore")
    .Does(() =>
{
    DotNetCoreRestore();
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
    .Does(() =>
{
    foreach(var file in testProjects) {
        OpenCover(tool => {
            tool.DotNetCoreTest(file.Value, 
                new DotNetCoreTestSettings {
                    Configuration = configuration,
                    NoBuild = true,
                    ArgumentCustomization = args => args
                        .Append(" -l trx;LogFileName=" + new FilePath(tool.Environment.WorkingDirectory.FullPath + $"/{file.Key}.trx"))
                });
            },
            new FilePath("./opencover.xml"),
            new OpenCoverSettings { 
                OldStyle = true, 
                MergeOutput = true,
                ArgumentCustomization = args => args
                    .Append($" -searchdirs:\"tests\\Rtl.Keys.Api.Tests\\bin\\{configuration}\\{framework};tests\\Rtl.Keys.Application.Tests\\bin\\{configuration}\\{framework}; tests\\Rtl.Keys.Data.Tests\\bin\\{configuration}\\{framework}\"")
                    .Append(" -hideskipped:All")
            }
            .WithFilter("+[Rtl*]*")
            .WithFilter("-[Rtl*]*Tests")
        );
    }
});


Task("Sonar")
  .IsDependentOn("SonarBegin")
  .IsDependentOn("Build")
  .IsDependentOn("Test")
  .IsDependentOn("SonarEnd");
 
Task("SonarBegin")
  .Does(() => {

     SonarBegin(new SonarBeginSettings {
        Url = sonarUrl,
        Login = sonarLogin,
        Verbose = false,
        Key = projectName,
        OpenCoverReportsPath = "./opencover.xml",
        VsTestReportsPath = "./*.trx",     
        Exclusions = "*.xml",
        Name = projectName,
     });
  });

Task("SonarEnd")
  .Does(() => {
     SonarEnd(new SonarEndSettings{
        Login = sonarLogin
     });
  });

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Sonar");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);