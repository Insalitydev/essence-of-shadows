using EssenceShared;

namespace EssenceServer {
    internal class Program {
        private static void Main(string[] args) {
            Log.Print("Starting Essence Server");
            Server.Start();
        }
    }
}