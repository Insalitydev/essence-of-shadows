using System.Security.Cryptography.X509Certificates;
using CocosSharp;
using EssenceShared;
using EssenceShared.Scenes;

namespace EssenceServer.Scenes {
    internal class ServerScene: CCScene {
        public readonly GameLayer _gameLayer;
        public GameState GameState { get; private set; }

        public ServerScene(CCWindow window): base(window) {
            _gameLayer = new GameLayer();
            AddChild(_gameLayer);
            GameState = new GameState();

            Log.Print("Game has started, waiting for players");
            Schedule(Update, 0.04f);
        }


        public override void Update(float dt) {
            base.Update(dt);

            Server.SendGameStateToAll();
        }

        internal void AddNewPlayer(ulong id, int x, int y) {
            GameState.playersCount++;
            PlayerState newPlayer = new PlayerState(id);
            newPlayer.PositionX = x;
            newPlayer.PositionY = y;

            GameState.players.Add(newPlayer);
            Log.Print("New player spawned");
        }

        internal void UpdateGameState(PlayerState ps) {
            var id = GameState.players.FindIndex(x => x.Id == ps.Id);
            GameState.players[id] = ps;
        }
    }
}