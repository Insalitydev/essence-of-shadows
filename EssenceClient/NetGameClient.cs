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
        private static GameScene _scene;
        private readonly string _ip;
        private readonly Object lockThis = new Object();

        public NetGameClient(string ip, GameScene scene) {
            _ip = ip;
            _scene = scene;
        }

        public void ConnectToServer() {
            Log.Print(String.Format("Connecting to the server {0}:{1}", _ip, Settings.Port));

            var _config = new NetPeerConfiguration(Settings.GameIdentifier);

            Client = new NetClient(_config);
            Client.RegisterReceivedCallback(GotMessage);

            Client.Start();

            NetOutgoingMessage hail = Client.CreateMessage("Hail message");
            Log.Print("Before connect", LogType.Network);
            Client.Connect(_ip, Settings.Port, hail);

            // TODO: Сделать вразумительное ожидание завершения подключения...
            Thread.Sleep(400);

            Log.Print("NetStatus: " + Client.ConnectionStatus, LogType.Network);
            var nc = new NetCommand(NetCommandType.Connect, "");
            Send(nc, NetDeliveryMethod.ReliableOrdered);
        }

        public void Send(NetCommand command, NetDeliveryMethod method) {
            NetOutgoingMessage om = Client.CreateMessage(command.Serialize());
            Client.SendMessage(om, method);
//            Log.Print("Sending '" + command.Data + "'", LogType.NETWORK);
        }

        public void SendChatMessage(string text) {
            var nc = new NetCommand(NetCommandType.Say, text);
            Send(nc, NetDeliveryMethod.ReliableUnordered);
        }

        private void GotMessage(object data) {

            NetIncomingMessage im;
            while ((im = Client.ReadMessage()) != null){
                string tmp = im.ReadString();
                lock (lockThis){
                    if (tmp.StartsWith("{\"")){
                        NetCommand nc = NetCommand.Deserialize(tmp);
//                        Log.Print("Got data" + nc.Data, LogType.NETWORK);
                        switch (nc.Type){
                                /** Ответ на запрос соединения */
                            case NetCommandType.Connect:
                                _scene.SetMyId(nc.Data);
                                break;
                            case NetCommandType.Disconnect:
                                break;
                            case NetCommandType.Say:
                                Log.Print("Incoming message from server: " + nc.Data);
                                _scene.GetChatMessage(nc.Data);
                                break;
                                /** Обновляем все необходимые данные об игровом состоянии */
                            case NetCommandType.UpdateGamestate:
                                var gs = JsonConvert.DeserializeObject<GameState>(nc.Data);

                                _scene.GameLayer.AppendGameState(gs, _scene.Id);
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