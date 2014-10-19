using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CocosSharp;
using EssenceShared;
using EssenceShared.Entities;
using EssenceShared.Entities.Players;
using EssenceShared.Entities.Projectiles;
using EssenceShared.Game;
using Lidgren.Network;
using Newtonsoft.Json;

namespace EssenceServer {
    /// <summary>
    ///     Слушает серверный сокет, подключает новых игроков запускает остальные службы для работы сервера
    /// </summary>
    internal class Server {
        private static NetServer _server;

        private static Thread _serverConnections;
        private static Thread _serverConsole;
        private static Thread _serverScene;
        public static ServerGame ServerGame { get; private set; }

        public static void Start() {
            _serverConnections = new Thread(ServerHandleConnections);
            _serverConnections.Start();

            _serverConsole = new Thread(ServerHandleConsole);
            _serverConsole.Start();

            _serverScene = new Thread(ServerHandleGame);
            _serverScene.Start();
        }

        /// <summary>
        ///     Метод для работы потока обработчика логики игры
        /// </summary>
        private static void ServerHandleGame(object obj) {
            ServerGame = new ServerGame();
            var application = new CCApplication(false, new CCSize(1, 1));

            ServerGame = new ServerGame();
            application.ApplicationDelegate = ServerGame;
            application.StartGame();
        }


        /// <summary>
        ///     Метод для работы потока обработчика работы с сервером из консоли
        /// </summary>
        private static void ServerHandleConsole(object obj) {
            /** TODO: Ждать пока сервер полностью не загрузится */
            Thread.Sleep(2000);
            var serverConsole = new ServerConsole(_server);
            serverConsole.Start();
        }

        /// <summary>
        ///     Метод для работып отока обработчика входящих новых соединений с сервером
        /// </summary>
        private static void ServerHandleConnections(object obj) {
            Log.Print("Starting Listen connections", LogType.Network);

            var config = new NetPeerConfiguration(Settings.GameIdentifier) {
                Port = Settings.Port,
                MaximumConnections = Settings.MaxConnections,
                SendBufferSize = 400000,
                UseMessageRecycling = true,
            };

            /* Получаем возможные адреса сервера */
            Log.Print("Server IP's:", LogType.Network);
            Log.Print("-------", LogType.Network);
            IPAddress[] ipList = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress ip in ipList){
                if (ip.AddressFamily == AddressFamily.InterNetwork){
                    Log.Print(ip.ToString(), LogType.Network);
                }
            }
            Log.Print("-------", LogType.Network);

            _server = new NetServer(config);
            _server.Start();

            // Запускаем обработчик пакетов
            StartProcessIncomingMessages();
        }

        /// <summary>
        ///     Обрабатывает все входящие пакеты от клиентов
        /// </summary>
        private static void StartProcessIncomingMessages() {
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
                            Log.Print(GetId(msg.SenderConnection) + " " + status + ": " + reason);
                            if (status == NetConnectionStatus.Disconnected){
                                RemoveDisconnectedPlayer(GetId(msg.SenderConnection));
                            }
                            break;

                            /* Обработка данных, пришедших от клиентов */
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

        /** Обработка входящих данных (Команд) от клиентов*/

        private static void ProcessIncomingData(NetIncomingMessage msg) {
            string data = msg.ReadString();

            if (data.StartsWith("{\"")){
                NetCommand nc = NetCommand.Deserialize(data);
                switch (nc.Type){
                    case NetCommandType.Connect:
                        Log.Print(
                            "Connected: " + msg.SenderConnection.RemoteEndpoint.Address + ":" +
                            msg.SenderConnection.RemoteEndpoint.Port, LogType.Network);
                        ConnectNewPlayer(msg, nc.Data);
                        break;
                    case NetCommandType.Disconnect:
                        RemoveDisconnectedPlayer(GetId(msg.SenderConnection));
                        break;
                    case NetCommandType.Say:
                        SendChatMessage(
                            GetId(msg.SenderConnection).Substring(0, 4) + ": " + nc.Data);
                        break;
                    case NetCommandType.UpdatePlayerstate:
                        var ps = JsonConvert.DeserializeObject<EntityState>(nc.Data);
                        ServerGame.ServerScene.AppendPlayerState(ps);
                        break;
                    case NetCommandType.CallPlayerMethod:
                        CallPlayerMethod(GetId(msg.SenderConnection), nc.Data);
                        break;
                }
            }
        }

        /// <summary>
        ///     Вызывает метод у игрока. Формат должен быть следующим:
        ///     {MethodName}.{arg[0]}.{arg[1]}
        /// </summary>
        private static void CallPlayerMethod(string playerid, string data) {
            Player pl = ServerGame.GetPlayer(playerid);
            string[] args = data.Split('.');
            if (args[0] == "attack"){
                //TODO: вынести в отдельные методы
                var ent = new MysticProjectile(pl.AttackDamage, Util.GetUniqueId()) {
                    PositionX = pl.PositionX,
                    PositionY = pl.PositionY,
                    Direction =
                        Entity.AngleBetweenPoints(new CCPoint(pl.PositionX, pl.PositionY),
                            new CCPoint(Int32.Parse(args[1]), Int32.Parse(args[2]))),
                    OwnerId = playerid
                };
                ServerGame.ServerScene.GetGameLayer(pl.AccState.Location).AddEntity(ent);
            }
            else{
                Log.Print("Not found player method;", LogType.Error);
            }
        }

        /// <summary>
        ///     Отсылает всем подключенным клиентам сообщение в чат
        /// </summary>
        public static void SendChatMessage(string chatMsg) {
            var nc = new NetCommand(NetCommandType.Say, chatMsg);
            NetOutgoingMessage om = _server.CreateMessage();
            om.Write(nc.Serialize());
            _server.SendMessage(om, _server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
        }


        /// <summary>
        ///     Отсылает всем подключенным клиентам текущее игровое состояние
        /// </summary>
        public static void SendGameStateToAll() {
            if (_server.ConnectionsCount > 0){
                foreach (NetConnection netConnection in _server.Connections){
                    // TODO: не формировать каждый раз одни и те же данные

                    // TODO: Отсылать пакет с состоянием игры по частям (по 10 сущностей, например)
                    if (netConnection.Status == NetConnectionStatus.Connected){
                        // TODO: Временное решение. Вместо убирания ненужных элементов из полного геймстейта, сразу формируем с нуля для каждого игрока
                        GameState gs =
                            ServerGame.ServerScene.GetGameState(GetId(netConnection));
                        var nc = new NetCommand(NetCommandType.UpdateGamestate, gs.Serialize());

                        NetOutgoingMessage om = _server.CreateMessage();
                        om.Write(nc.Serialize());
                        try{
                            _server.SendMessage(om, netConnection, NetDeliveryMethod.Unreliable, 0);
                        }
                        catch (NetException e){
                            Log.Print("NETWORK ERROR: " + e.StackTrace, LogType.Error);
                        }
                    }
                }
            }
        }


        public static void SendMap(string uniqueId, Locations location) {
            foreach (NetConnection client in _server.Connections){
                if (GetId(client) == uniqueId){
                    SendMap(client, location);
                    break;
                }
            }
        }

        public static void SendMap(NetConnection client, Locations location) {
            // TODO: отдаём начальное состояние мира (карта)
            var nc = new NetCommand(NetCommandType.SendMap,
                ServerGame.ServerScene.GetGameLayer(location).SerializeMap());
            NetOutgoingMessage om = _server.CreateMessage();
            om.Write(nc.Serialize());
            _server.SendMessage(om, client, NetDeliveryMethod.ReliableOrdered);
        }

        private static void ConnectNewPlayer(NetIncomingMessage msg, string nickname) {
            // Проверяем что этот аккаунт еще не зайден:
            if (ServerGame.ServerScene.Accounts.FindIndex(x=>x.nickname == nickname) == -1){
                SendMap(msg.SenderConnection, Locations.Town);

                Log.Print("Creating new player: " + nickname);
                /* Создаем нового игрока в игре */
                InitNewPlayer(GetId(msg.SenderConnection), nickname);

                /* Отдаем новому игроку его уникальный ид */
                var nc = new NetCommand(NetCommandType.Connect, (GetId(msg.SenderConnection)));

                NetOutgoingMessage om = _server.CreateMessage();
                om.Write(nc.Serialize());
                _server.SendMessage(om, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
            }
            else{
                // отвергаем соединение
                Log.Print("Connection deny. Nickname " + nickname);
                msg.SenderConnection.Deny("This account already in the game");
                msg.SenderConnection.Disconnect("Bye");
            }
        }

        /// <summary>
        ///     Создает нового игрока в игре
        /// </summary>
        private static void InitNewPlayer(string id, string nickname) {
            ServerGame.AddNewPlayer(id, nickname, 300, 300);
        }

        private static void RemoveDisconnectedPlayer(string playerid) {
            Log.Print("Player " + playerid + " disconected. Removing his player...");
            ServerGame.RemovePlayer(playerid);
        }

        public static string GetId(NetConnection nc) {
            return NetUtility.ToHexString(nc.RemoteUniqueIdentifier);
        }
    }
}