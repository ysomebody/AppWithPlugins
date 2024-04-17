using PluginInterface;
using System.Drawing;

namespace Plugin2
{
    public class Plugin2 : IPlugin
    {
        public string GetInfo()
        {
            return "Plugin2 is using " + typeof(Icon).Assembly.FullName;
        }
    }
}
