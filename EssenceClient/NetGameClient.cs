using System;
using System.Threading;
using EssenceShared;
using Lidgren.Network;

namespace EssenceClient {
    /** Обрабатывает всю сетевую часть клиента */

    internal class NetGameClient {
        public static NetClient Client;
        private readonly string _ip;

        public NetGameClient(string ip) {
            _ip = ip;
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

            Log.Print("NetStatus: " + Client.ConnectionStatus.ToString(), LogType.NETWORK);
            Send("Connect Me");
        }

        private void Send(string data) {
            NetOutgoingMessage om = Client.CreateMessage(data);
            Client.SendMessage(om, NetDeliveryMethod.ReliableOrdered);
            Log.Print("Sending '" + data + "'", LogType.NETWORK);
        }

        private void GotMessage(object data) {
            Log.Print("Got data:" + data, LogType.NETWORK);
            NetIncomingMessage im;
            while ((im = Client.ReadMessage()) != null){
                string tmp = im.ReadString();
                //                if (tmp.Split('.')[0] == "data") {
                //                    int x = Int32.Parse(tmp.Split('.')[1]);
                //                    int y = Int32.Parse(tmp.Split('.')[2]);
                //                    if (_gameScene._gameLayer.GetPlayer() != null) {
                //                        _gameScene._gameLayer.GetPlayer().PositionX = x;
                //                        _gameScene._gameLayer.GetPlayer().PositionY = y;
                //                    }
                //                }
                //                if (tmp.Split('.')[0] == "id") {
                //                    id = tmp.Split('.')[1];
                //                    Log.Print("Set uni id for me: " + id);
                //                }
                //
                //                lock (lockThis) {
                //                    if (tmp.StartsWith("{\"")) {
                //                        var gs = JsonConvert.DeserializeObject<GameState>(tmp);
                //                        Log.Print("Getted gs: " + tmp);
                //                        foreach (PlayerState hero in gs.players) {
                //                            _gameScene._gameLayer.UpdateEntity(hero, id);
                //                        }
                //                    }
                //                }
            }
        }
    }
}