using System.Threading;
using CocosSharp;
using EssenceShared;
using Lidgren.Network;
using Newtonsoft.Json;

namespace EssenceServer {
    /** Слушает сокет и подключает новых игроков, запускает остальные службы*/

    internal class Server {
        private static NetServer server;

        private static Thread _serverConnections;
        private static Thread _serverConsole;
        private static Thread _serverScene;
        private static ServerGame _serverGame;
        private static long _lastId = 1;

        public static void Start() {
            _serverConnections = new Thread(ServerHandleConnections);
            _serverConnections.Start();

            _serverConsole = new Thread(ServerHandleConsole);
            _serverConsole.Start();

            _serverScene = new Thread(ServerHandleGame);
            _serverScene.Start();
        }

        private static void ServerHandleGame(object obj) {
            _serverGame = new ServerGame();
            var _application = new CCApplication(false, null);

            _serverGame = new ServerGame();
            _application.ApplicationDelegate = _serverGame;
            _application.StartGame();
        }

        private static void ServerHandleConsole(object obj) {
            /** TODO: Ждать пока сервер полностью не загрузится */
            Thread.Sleep(2000);
            var serverConsole = new ServerConsole();
            serverConsole.Start();
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
                            Log.Print((NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier)) +
                                      " " + status + ": " + reason);
                            break;

                            /** Обработка полезных данных, пришедших от клиентов */
                        case NetIncomingMessageType.Data:
                            ProcessIncomingData(msg);
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

        /** Обработка входящего сообщения сервером */

        private static void ProcessIncomingData(NetIncomingMessage msg) {
            string data = msg.ReadString();
            //            Log.Print("Got data: " + data, LogType.NETWORK);

            if (data.StartsWith("{\"")){
                NetCommand nc = NetCommand.Deserialize(data);
                switch (nc.Type){
                    case NetCommandType.CONNECT:
                        ConnectNewPlayer(msg);
                        break;
                    case NetCommandType.DISCONNECT:
                        break;
                    case NetCommandType.SAY:
                        SendChatMessage(nc.Data);
                        break;
                    case NetCommandType.UPDATE_PLAYERSTATE:
                        var ps = JsonConvert.DeserializeObject<PlayerState>(nc.Data);
                        _serverGame.UpdateGameState(ps);
                        break;
                }
            }
        }

        private static void SendChatMessage(string chatMsg) {
            var nc = new NetCommand(NetCommandType.SAY, chatMsg);
            NetOutgoingMessage om = server.CreateMessage();
            om.Write(nc.Serialize());
            server.SendMessage(om, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }


        public static void SendGameStateToAll() {
            if (server.ConnectionsCount > 0){
                string gs = _serverGame.GetGameState();

                var nc = new NetCommand(NetCommandType.UPDATE_GAMESTATE, gs);

                NetOutgoingMessage om = server.CreateMessage();
                om.Write(nc.Serialize());
                server.SendMessage(om, server.Connections, NetDeliveryMethod.Unreliable, 0);
            }
        }

        private static void ConnectNewPlayer(NetIncomingMessage msg) {
            /** Создаем нового игрока в игре */
            InitNewPlayer(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier));

            /** Отдаем новому игроку его уникальный ид */
            var tmpNC = new NetCommand(NetCommandType.CONNECT,
                (NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier)));

            NetOutgoingMessage om = server.CreateMessage();
            om.Write(tmpNC.Serialize());
            server.SendMessage(om, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
        }

        private static void InitNewPlayer(string id) {
            _serverGame.AddNewPlayer(id, 300, 300);
        }

        public static string GetUniqueId() {
            return (_lastId++).ToString();
        }
    }
}