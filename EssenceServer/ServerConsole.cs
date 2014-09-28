using System;
using System.Collections.Generic;
using System.Linq;
using EssenceShared;
using Lidgren.Network;

namespace EssenceServer {
    /** Содержит всю логику по работе с сервером из консоли */
    
    internal class ServerConsole {

        private List<string> commandsList = new List<string>() {
            "say",
            "restart",
            "online",
            "count",
            "help",
            "exit"
        };
        private NetServer _server;

        public ServerConsole(NetServer _server) {
            this._server = _server;
        }

        public void Start() {
            while (true){
                Console.Write(">>> ");
                ProcessCommand(Console.ReadLine());
            }
        }

        private void ProcessCommand(string command) {
            // TODO: реализовать нормальное разделение на комманду:аргументы
            var arg = command.Split(' ').Last();
            command = command.Split(' ').First();
            switch (command){
                case "say":
                    Server.SendChatMessage(arg);
                    break;
                case "restart":
                    break;
                case "online":
                    Log.Print("Currently online: " + _server.ConnectionsCount, LogType.Info, false);
                    break;
                case "count":
                    // TODO: Долой безопасность, даёшь паблик поля везде!
                    Log.Print("Entities count: " + Server.ServerGame.ServerScene.GameLayer.Entities.Count, LogType.Info, false);
                    break;
                case "help":
                    Log.Print(String.Join("\n", commandsList), false);
                    break;
                case "exit":
                    Environment.Exit(0);
                    break;
                default:
                    Log.Print("Unknown command", LogType.Info, false);
                    break;
            }
        }
    }
}