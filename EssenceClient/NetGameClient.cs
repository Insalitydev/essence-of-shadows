using System;
using System.Threading;
using EssenceClient.Scenes.Game;
using EssenceShared;
using Lidgren.Network;
using Newtonsoft.Json;

namespace EssenceClient {
    /** Обрабатывает всю сетевую часть клиента */

    internal class NetGameClient {
        public static NetClient Client;
        private readonly string _ip;
        private readonly Object lockThis = new Object();

        private static GameScene _scene;

        public NetGameClient(string ip, GameScene scene) {
            _ip = ip;
            _scene = scene;
        }

        public void ConnectToServer() {
            Log.Print(String.Format("Connecting to the server {0}:{1}", _ip, Settings.PORT));

            var _config = new NetPeerConfiguration(Settings.GAME_IDENTIFIER);

            Client = new NetClient(_config);
            Client.RegisterReceivedCallback(GotMessage);

            Client.Start();

            NetOutgoingMessage hail = Client.CreateMessage("Hail message");
            Log.Print("Before connect", LogType.NETWORK);
            Client.Connect(_ip, Settings.PORT, hail);

            // TODO: Сделать вразумительное ожидание завершения подключения...
            Thread.Sleep(400);

            Log.Print("NetStatus: " + Client.ConnectionStatus, LogType.NETWORK);
            NetCommand nc = new NetCommand(NetCommandType.CONNECT, "");
            Send(nc.Serialize());
        }

        public void Send(string data) {
            NetOutgoingMessage om = Client.CreateMessage(data);
            Client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
//            Log.Print("Sending '" + data + "'", LogType.NETWORK);
        }

        private void GotMessage(object data) {
//            Log.Print("Got data:" + data, LogType.NETWORK);
            NetIncomingMessage im;
            while ((im = Client.ReadMessage()) != null){
                string tmp = im.ReadString();
                lock (lockThis){
                    if (tmp.StartsWith("{\"")){
                        var nc = NetCommand.Deserialize(tmp);
                        Log.Print("Packet Time: " + (DateTime.Now.Ticks-nc.CreateTime.Ticks));
                        switch (nc.Type){
                                /** Ответ на запрос соединения */
                            case NetCommandType.CONNECT:
                                _scene.SetMyId(nc.Data);
                                break;
                            case NetCommandType.DISCONNECT:
                                break;
                            case NetCommandType.SAY:
                                Log.Print("Incoming message from server: " + nc.Data);
                                break;
                                /** Обновляем все необходимые данные об игровом состоянии */
                            case NetCommandType.UPDATE_GAMESTATE:
                                var gs = JsonConvert.DeserializeObject<GameState>(nc.Data);

                                foreach (var player in gs.players){
                                    _scene.UpdateEntity(player);
                                }
                                break;
                        }
                    }
                }
            }
        }
    }
}