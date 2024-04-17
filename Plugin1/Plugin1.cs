using PluginInterface;
using System.Drawing;

namespace Plugin1
{
    public class Plugin1 : IPlugin
    {
        public string GetInfo()
        {
            return "Plugin1 is using " + typeof(Icon).Assembly.FullName;
        }
    }
}
