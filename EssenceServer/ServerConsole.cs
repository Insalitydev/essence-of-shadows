using System;
using EssenceShared;

namespace EssenceServer {
    /** Содержит всю логику по работе с сервером из консоли */

    internal class ServerConsole {

        public void Start() {
            while (true){
                Console.Write(">>> ");
                ProcessCommand(Console.ReadLine());
            }
        }

        private void ProcessCommand(string command) {
            switch (command){
                case "say":
                    Log.Print("some say action", LogType.INFO, false);
                    break;
                case "list":
                    Log.Print("Currently online: ", LogType.INFO, false);
                    break;
                case "help":
                    Log.Print("Help console stub", LogType.INFO, false);
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    Log.Print("Unknown command", LogType.INFO, false);
                    break;
            }
        }
    }
}