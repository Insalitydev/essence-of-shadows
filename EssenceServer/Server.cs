using System;
using System.Threading;
using CocosSharp;
using EssenceShared;
using EssenceShared.Entities;
using EssenceShared.Entities.Players;
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
        private static long _lastId = 1;
        public static ServerGame ServerGame { get; private set; }

        public static void Start() {
            _serverConnections = new Thread(ServerHandleConnections);
            _serverConnections.Start();

            _serverConsole = new Thread(ServerHandleConsole);
            _serverConsole.Start();

            _serverScene = new Thread(ServerHandleGame);
            _serverScene.Start();
        }

        private static void ServerHandleGame(object obj) {
            ServerGame = new ServerGame();
            var application = new CCApplication(false, new CCSize(1, 1));

            ServerGame = new ServerGame();
            application.ApplicationDelegate = ServerGame;
            application.StartGame();
        }

        private static void ServerHandleConsole(object obj) {
            /** TODO: Ждать пока сервер полностью не загрузится */
            Thread.Sleep(2000);
            var serverConsole = new ServerConsole(_server);
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
                            if (status == NetConnectionStatus.Disconnected){
                                RemoveDisconnectedPlayer(
                                    NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier));
                            }
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
                        ServerGame.ServerScene.AppendPlayerState(ps);
                        break;
                    case NetCommandType.CallPlayerMethod:
                        CallPlayerMethod(NetUtility.ToHexString(msg.SenderConnection.RemoteUniqueIdentifier), nc.Data);
                        break;
                }
            }
        }

        private static void CallPlayerMethod(string playerid, string data) {
            Log.Print("Player invoke method: " + data);
            var pl = ServerGame.ServerScene.GameLayer.Entities.Find(x=>x.Id == playerid) as Player;
            string[] args = data.Split('.');
            if (args[0] == "attack"){
                //TODO: вынести в отдельные методы
                var ent = new MysticProjectile(GetUniqueId()) {
                    PositionX = pl.PositionX,
                    PositionY = pl.PositionY,
                    Direction =
                        Entity.AngleBetweenPoints(new CCPoint(pl.PositionX, pl.PositionY),
                            new CCPoint(Int32.Parse(args[1]), Int32.Parse(args[2]))),
                    OwnerId = playerid
                };
                var ent2 = new MysticProjectile(GetUniqueId()) {
                    PositionX = pl.PositionX,
                    PositionY = pl.PositionY,
                    Direction =
                        Entity.AngleBetweenPoints(new CCPoint(pl.PositionX, pl.PositionY),
                            new CCPoint(Int32.Parse(args[1]), Int32.Parse(args[2])))+30,
                    OwnerId = playerid
                };
                var ent3 = new MysticProjectile(GetUniqueId()) {
                    PositionX = pl.PositionX,
                    PositionY = pl.PositionY,
                    Direction =
                        Entity.AngleBetweenPoints(new CCPoint(pl.PositionX, pl.PositionY),
                            new CCPoint(Int32.Parse(args[1]), Int32.Parse(args[2])))-30,
                    OwnerId = playerid
                };
                ServerGame.ServerScene.GameLayer.AddEntity(ent);
                ServerGame.ServerScene.GameLayer.AddEntity(ent2);
                ServerGame.ServerScene.GameLayer.AddEntity(ent3);
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

                foreach (var netConnection in _server.Connections){
                    // TODO: не формировать каждый раз одни и те же данные
                    GameState gs = ServerGame.ServerScene.GetGameState(NetUtility.ToHexString(netConnection.RemoteUniqueIdentifier));

                    var nc = new NetCommand(NetCommandType.UpdateGamestate, gs.Serialize());

                    NetOutgoingMessage om = _server.CreateMessage();
                    om.Write(nc.Serialize());
                    try {
                        _server.SendMessage(om, netConnection, NetDeliveryMethod.Unreliable, 0);
                    } catch (NetException e) {
                        Log.Print("NETWORK ERROR: " + e.StackTrace, LogType.Error);
                    }
                }
                
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

            // TODO: отдать начальное состояние мира (карта)
            nc = new NetCommand(NetCommandType.SendMap, ServerGame.ServerScene.GameLayer.SerializeMap());
            om = _server.CreateMessage();
            om.Write(nc.Serialize());
            _server.SendMessage(om, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
        }

        private static void RemoveDisconnectedPlayer(string playerid) {
            Log.Print("Player " + playerid + " distonencted. Removing his player...");
            ServerGame.RemovePlayer(playerid);
            
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
            ServerGame.AddNewPlayer(id, 300, 300, type);
        }

        public static string GetUniqueId() {
            return (_lastId++).ToString();
        }
    }
}