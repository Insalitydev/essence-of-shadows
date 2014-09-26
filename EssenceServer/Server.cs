using System;
using System.Threading;
using EssenceShared;
using Lidgren.Network;
using System.Collections.Generic;

namespace EssenceServer {
    /** Слушает сокет и подключает новых игроков, запускает остальные службы*/

    internal class Server {
        private static NetServer server;

        private static Thread _serverConnections;
        private static Thread _serverConsole;
        private static Thread _serverScene;
        private static ServerGame _serverGame;

        public static void Start() {
            _serverConnections = new Thread(ServerHandleConnections);
            _serverConnections.Start();

//            _serverConsole = new Thread(ServerHandleConsole);
//            _serverConsole.Start();

//            _serverScene = new Thread(ServerHandleGame);
//            _serverScene.Start();
        }

        private static void ServerHandleGame(object obj) {
            throw new System.NotImplementedException();
        }

        private static void ServerHandleConsole(object obj) {
            throw new System.NotImplementedException();
        }

        private static void ServerHandleConnections(object obj) {

            Log.Print("Starting Listen connections");

            var _config = new NetPeerConfiguration(Settings.GAME_IDENTIFIER);
            _config.Port = Settings.PORT;
            _config.MaximumConnections = Settings.MAX_CONNECTIONS;

            server = new NetServer(_config);
            server.Start();

            NetIncomingMessage msg;

            while (true){
                while ((msg = server.ReadMessage()) != null){
                    switch (msg.MessageType){
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                            Log.Print(msg.ReadString(), LogType.NETWORK);
                            break;

                        case NetIncomingMessageType.ErrorMessage:
                            Log.Print(msg.ReadString(), LogType.ERROR);
                            break;

                        case NetIncomingMessageType.StatusChanged:
                            var status = (NetConnectionStatus) msg.ReadByte();
                            string reason = msg.ReadString();
                            Log.Print(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) +
                                      " " + status + ": " + reason);
                            break;

                            /** Обработка полезных данных, пришедших от клиентов */
                        case NetIncomingMessageType.Data:
                            string data = msg.ReadString();
                            Log.Print("Got data: " + data, LogType.NETWORK);

                            if (data.StartsWith("{\"")){
                                throw new NotImplementedException("No implement deserialize JSON Server GET Data");
                            }

                            /** Получаем все активные соединения кроме отправителя */
                            List<NetConnection> all = server.Connections;
                            all.Remove(msg.SenderConnection);

                            /** отправляем им пришедшее сообщение от отправителя (broadcast) */
                            if (all.Count > 0){
                                NetOutgoingMessage om = server.CreateMessage();
                                om.Write(
                                    NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier) +
                                    "said: " + data);
                                server.SendMessage(om, all, NetDeliveryMethod.ReliableOrdered, 0);
                            }
                            break;

                        default:
                            Log.Print("Unhandled type: " + msg.MessageType + " " + msg.LengthBytes +
                                      " bytes " + msg.DeliveryMethod + "|" + msg.SequenceChannel, LogType.INFO);
                            break;
                    }

                    server.Recycle(msg);
                } // End while msg != null

                Thread.Sleep(1);
            } // End while true
        }
    }
}