// See https://aka.ms/new-console-template for more information
using PluginInterface;
using System.Reflection;
using System.Runtime.Loader;

Console.WriteLine("Hello, World!");

// Get the path of the directory where the executable is located
string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

// List to hold all loaded plugins
var plugins = new List<IPlugin>();

// Scan each subdirectory for .dll files
foreach (var subdirectory in Directory.GetDirectories(appPath))
{
    // Create a separate context for each plugin
    var pluginContext = new AssemblyLoadContext(subdirectory);
    pluginContext.Resolving += (context, assemblyName) =>
    {
        // Load dependencies from the plugin's directory
        Console.WriteLine($"Resolving {context.Name} -- {assemblyName.FullName}.");
        var pluginDirectory = context.Name;
        var resolvedAssembly =  context.LoadFromAssemblyPath(Path.Combine(pluginDirectory, assemblyName.Name + ".dll"));
        Console.WriteLine($"Resolved {resolvedAssembly.Location}.");
        return resolvedAssembly;
    };

    foreach (var dll in Directory.GetFiles(subdirectory, "Plugin.*.dll"))
    {
        try
        {
            // Load the assembly into the plugin context
            var pluginAssembly = pluginContext.LoadFromAssemblyPath(dll);

            // Scan the assembly for types implementing IPlugin
            foreach (Type type in pluginAssembly.GetTypes())
            {
                if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface)
                {
                    // Create an instance of the type and add it to the plugins list
                    IPlugin pluginInstance = (IPlugin)Activator.CreateInstance(type);
                    plugins.Add(pluginInstance);
                    Console.WriteLine($"Loaded plugin from {dll}");
                }
            }
        }
        catch (Exception ex)
        { 
            Console.WriteLine($"Error loading plugin from {dll}: {ex.Message}");
        }
    }
}

// Execute each plugin's method as an example of usage
foreach (var plugin in plugins)
{
    var info = plugin.GetInfo();
    Console.WriteLine(info);
}
Console.WriteLine("Bye!");

