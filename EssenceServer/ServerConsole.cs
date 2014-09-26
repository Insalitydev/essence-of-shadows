using System;
using EssenceShared;

namespace EssenceServer {
    /** Содержит всю логику по работе с сервером из консоли */

    internal class ServerConsole {
        private void ProcessCommand(string command) {
            switch (command){
                case "say":
                    Log.Print("some say action");
                    break;
                case "list":
                    Log.Print("Currently online: ");
                    break;
                case "help":
                    Log.Print("Help console stub");
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    Log.Print("Unknown command");
                    break;
            }
        }
    }
}