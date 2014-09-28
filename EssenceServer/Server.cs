using System;
using System.Threading;
using CocosSharp;
using EssenceShared;
using EssenceShared.Entities;
using EssenceShared.Entities.Player;
using EssenceShared.Entities.Projectiles;
using Lidgren.Network;
using Newtonsoft.Json;

namespace EssenceServer {
    /** Слушает сокет и подключает новых игроков, запускает остальные службы*/

    internal class Server {
        private static NetServer _server;

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
            var application = new CCApplication(false, null);

            _serverGame = new ServerGame();
            application.ApplicationDelegate = _serverGame;
            application.StartGame();
        }

        private static void ServerHandleConsole(object obj) {
            /** TODO: Ждать пока сервер полностью не загрузится */
            Thread.Sleep(2000);
            var serverConsole = new ServerConsole();
            serverConsole.Start();
        }

        private static void ServerHandleConnections(object obj) {
            Log.Print("Starting Listen connections");

            var config = new NetPeerConfiguration(Settings.GameIdentifier) {
                Port = Settings.Port,
                MaximumConnections = Settings.MaxConnections
            };

            _server = new NetServer(config);
            _server.Start();

            NetIncomingMessage msg;

            while (true){
                while ((msg = _server.ReadMessage()) != null){
                    switch (msg.MessageType){
                        case NetIncomingMessageType.VerboseDebugMessage:
                        case NetIncomingMessageType.DebugMessage:
                        case NetIncomingMessageType.WarningMessage:
                            Log.Print(msg.ReadString(), LogType.Network);
                            break;

                        case NetIncomingMessageType.ErrorMessage:
                            Log.Print(msg.ReadString(), LogType.Error);
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
                                      " bytes " + msg.DeliveryMethod + "|" + msg.SequenceChannel, LogType.Info);
                            break;
                    }

                    _server.Recycle(msg);
                } // End while msg != null

                Thread.Sleep(1);
            } // End while true
        }

        /** Обработка входящего сообщения сервером */

        private static void ProcessIncomingData(NetIncomingMessage msg) {
            string data = msg.ReadString();

            if (data.StartsWith("{\"")){
                NetCommand nc = NetCommand.Deserialize(data);
                switch (nc.Type){
                    case NetCommandType.Connect:
                        ConnectNewPlayer(msg);
                        break;
                    case NetCommandType.Disconnect:
                        break;
                    case NetCommandType.Say:
                        SendChatMessage(nc.Data);
                        break;
                    case NetCommandType.UpdatePlayerstate:
                        var ps = JsonConvert.DeserializeObject<EntityState>(nc.Data);
                        _serverGame.ServerScene.AppendPlayerState(ps);
                        break;
                    case NetCommandType.CallPlayerMethod:
                        CallPlayerMethod(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier), nc.Data);
                        break;
                }
            }
        }

        private static void CallPlayerMethod(string playerid, string data) {
            Log.Print("Player call");
            Log.Print(data);
            var pl = _serverGame.ServerScene.GameLayer.Entities.Find(x=>x.Id == playerid) as Player;
            var args = data.Split('.');
            if (args[0] == "attack"){
                var ent = new MysticProjectile(GetUniqueId()) {
                    PositionX = pl.PositionX,
                    PositionY = pl.PositionY,
                    Direction = Entity.AngleBetweenPoints(new CCPoint(pl.PositionX, pl.PositionY), new CCPoint(Int32.Parse(args[1]), Int32.Parse(args[2])) )
                };
                _serverGame.ServerScene.GameLayer.AddEntity(ent);
            }
            else{
                Log.Print("Not found player method;", LogType.Error);
            }
            
        }

        public static void SendChatMessage(string chatMsg) {
            var nc = new NetCommand(NetCommandType.Say, chatMsg);
            NetOutgoingMessage om = _server.CreateMessage();
            om.Write(nc.Serialize());
            _server.SendMessage(om, _server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }


        public static void SendGameStateToAll() {
            if (_server.ConnectionsCount > 0){
                GameState gs = _serverGame.ServerScene.GameLayer.GetGameState();

                var nc = new NetCommand(NetCommandType.UpdateGamestate, gs.Serialize());

                NetOutgoingMessage om = _server.CreateMessage();
                om.Write(nc.Serialize());
                _server.SendMessage(om, _server.Connections, NetDeliveryMethod.Unreliable, 0);
            }
        }

        private static void ConnectNewPlayer(NetIncomingMessage msg) {
            Log.Print("Creating new player");
            /** Создаем нового игрока в игре */
            InitNewPlayer(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier));

            /** Отдаем новому игроку его уникальный ид */
            var nc = new NetCommand(NetCommandType.Connect,
                (NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier)));

            NetOutgoingMessage om = _server.CreateMessage();
            om.Write(nc.Serialize());
            _server.SendMessage(om, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
        }

        private static void InitNewPlayer(string id) {
            string type = "Mystic";
            switch (new Random().Next(3)){
                case 0:
                    type = "Reaper";
                    break;
                case 1:
                    type = "Sniper";
                    break;
            }
            _serverGame.AddNewPlayer(id, 300, 300, type);
        }

        public static string GetUniqueId() {
            return (_lastId++).ToString();
        }
    }
}