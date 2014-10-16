using System;
using System.Collections.Generic;
using System.Linq;
using EssenceShared;
using EssenceShared.Game;
using Lidgren.Network;

namespace EssenceServer {
    /// <summary>
    ///     Содержит всю логику по работе с сервером из консоли
    /// </summary>
    internal class ServerConsole {
        private readonly NetServer _server;

        private readonly List<string> commandsList = new List<string> {
            "say",
            "restart",
            "online",
            "count",
            "help",
            "exit"
        };

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
            string arg = command.Split(' ').Last();
            command = command.Split(' ').First();
            switch (command){
                case "say":
                    Server.SendChatMessage("Server: " + arg);
                    break;
                case "restart":
                    break;
                case "online":
                    Log.Print("Currently online: " + _server.ConnectionsCount, LogType.Info, false);
                    break;
                case "count":
                    // TODO: Долой безопасность, даёшь паблик поля везде!
                    Log.Print(
                        "Entities count: " + Server.ServerGame.ServerScene.GetGameLayer(Locations.Desert).Entities.Count,
                        LogType.Info,
                        false);
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