using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace Faregosoft.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new Faregosoft.App(), args);
            host.Run();
        }
    }
}
