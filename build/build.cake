var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var solution = "./../Shop.Catalog.sln";

var testProjects = new Dictionary<string, string> {
    { "application", "./../test/Shop.Catalog.Application.Test/Shop.Catalog.Application.Test.csproj" },
    };

Task("Restore")
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

Task("Default")
    .IsDependentOn("Test");

RunTarget(target);