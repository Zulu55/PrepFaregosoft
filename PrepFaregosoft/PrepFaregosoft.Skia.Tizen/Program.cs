using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace PrepFaregosoft.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new PrepFaregosoft.App(), args);
            host.Run();
        }
    }
}
