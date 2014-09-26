using CocosSharp;
using EssenceShared.Scenes;
using IniParser;
using IniParser.Model;

namespace EssenceClient.Scenes.Game {
    internal class GameScene: CCScene {
        private BackgroundLayer _backgroundLayer;
        private HudLayer _hudLayer;

        private NetGameClient netGameClient;

        public GameLayer _gameLayer { get; private set; }

        public GameScene(CCWindow window): base(window) {
            _backgroundLayer = new BackgroundLayer();
            AddChild(_backgroundLayer);

            _gameLayer = new GameLayer();
            AddChild(_gameLayer);

            _hudLayer = new HudLayer();
            AddChild(_hudLayer);


            var parser = new FileIniDataParser();
            var data = parser.ReadFile("Config.ini");
            netGameClient = new NetGameClient(data["Server"]["ip"]);
            netGameClient.ConnectToServer();
        }
    }
}