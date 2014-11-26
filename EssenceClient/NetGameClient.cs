using System;
using System.Threading;
using EssenceClient.Scenes.Game;
using EssenceShared;
using Lidgren.Network;
using Newtonsoft.Json;

namespace EssenceClient {
    /// <summary>
    ///     Этот класс является обработчиком всей сетевой части клиента
    ///     Принимает и отправляет пакеты
    /// </summary>
    internal class NetGameClient {
        public static NetClient Client;
        private static GameScene _scene;
        private readonly string _ip;
        private readonly Object _lockThis = new Object();

        public NetGameClient(string ip, GameScene scene) {
            _ip = ip;
            _scene = scene;
        }

        public void ConnectToServer(string nickname) {
            Log.Print("Hello");
            Log.Print(String.Format("Connecting to the server {0}:{1}", _ip, Settings.Port), LogType.Network);

            var config = new NetPeerConfiguration(Settings.GameIdentifier);

            Client = new NetClient(config);
            Client.RegisterReceivedCallback(GotMessage);

            Client.Start();

            NetOutgoingMessage hail = Client.CreateMessage("Hail message");
            Log.Print("Before connect", LogType.Network);
            Client.Connect(_ip, Settings.Port, hail);

            // TODO: Сделать вразумительное ожидание завершения подключения...
            Thread.Sleep(400);

            Log.Print("NetStatus: " + Client.ConnectionStatus, LogType.Network);
            var nc = new NetCommand(NetCommandType.Connect, nickname);
            Send(nc, NetDeliveryMethod.ReliableOrdered);
        }

        /// <summary>
        ///     Оптравляет пакет серверу указанным методом
        /// </summary>
        public void Send(NetCommand command, NetDeliveryMethod method) {
            NetOutgoingMessage om = Client.CreateMessage(command.Serialize());
            Client.SendMessage(om, method);
        }

        /// <summary>
        ///     Отправляет текстовое сообщение в чат на сервер
        /// </summary>
        public void SendChatMessage(string text) {
            var nc = new NetCommand(NetCommandType.Say, text);
            Send(nc, NetDeliveryMethod.ReliableUnordered);
        }

        /// <summary>
        ///     Обработчик всех поступающих пакетов с сервера
        /// </summary>
        private void GotMessage(object data) {
            NetIncomingMessage im;
            while ((im = Client.ReadMessage()) != null) {
                string tmp = im.ReadString();
                lock (_lockThis) {
                    if (tmp.StartsWith("{\"")) {
                        NetCommand nc = NetCommand.Deserialize(tmp);
                        //                        Log.Print("Got data" + nc.Type + "Data: " + nc.Data.Length, LogType.Network);
                        switch (nc.Type) {
                                /** Ответ на запрос соединения */
                            case NetCommandType.Connect:
                                _scene.SetMyId(nc.Data);
                                break;
                            case NetCommandType.Disconnect:
                                Log.Print("Disconnected by server: ", LogType.Network);
                                break;
                            case NetCommandType.Say:
                                Log.Print("Incoming message from server: ", LogType.Network);
                                _scene.AppendChatMessage(nc.Data);
                                break;
                                /** Обновляем все необходимые данные об игровом состоянии */
                            case NetCommandType.SendMap:
                                _scene.GameLayer.DeserializeMap(nc.Data);
                                break;
                            case NetCommandType.UpdateGamestate:
                                var gs = JsonConvert.DeserializeObject<GameState>(nc.Data);

                                _scene.GameLayer.AppendGameState(gs, _scene.Id);
                                break;
                            default:
                                Log.Print("Unknown NetcommandType delivered", LogType.Error);
                                break;
                        }
                    }
                }
                Client.Recycle(im);
            }
            Thread.Sleep(1);
        }
    }
}