using System.Collections.Generic;
using System.IO;
using CocosSharp;
using EssenceShared;
using EssenceShared.Entities;
using EssenceShared.Entities.Enemies;
using EssenceShared.Scenes;

namespace EssenceServer.Scenes {
    internal class ServerScene: CCScene {
        public readonly GameLayer GameLayer;
        public List<AccountState> Accounts = new List<AccountState>();

        public ServerScene(CCWindow window): base(window) {
            GameLayer = new GameLayer {Tag = Tags.Server};
            AddChild(GameLayer);

            Log.Print("Game has started, waiting for players");
            Schedule(Update, 0.04f);
            Schedule(UpdateLogic);
        }

        /** формируем игровое состояние и возвращаем его */

        public override void OnEnter() {
            base.OnEnter();
            InitMap();

            // Adding test enemies:
            int mapW = GameLayer.currentMap[0].Length*Settings.TileSize*Settings.Scale;
            int mapH = GameLayer.currentMap.Count*Settings.TileSize*Settings.Scale;
            Log.Print(mapW + " " + mapH);
            for (int i = 0; i < 45; i++){
                GameLayer.AddEntity(new RangeEnemy(Resources.EnemyStinger, Util.GetUniqueId()) {
                    PositionX = CCRandom.Next(100, mapW - 100),
                    PositionY = CCRandom.Next(100, mapH - 100)
                });
            }
        }

        private void InitMap() {
            GameLayer.CreateNewMap(ParseMap());
        }

        public List<string> ParseMap() {
            string s = File.ReadAllText("TestMap.txt");
            var tileMap = new List<string>(s.Split('\n'));

            for (int i = 0; i < tileMap.Count; i++){
                tileMap[i] = tileMap[i].TrimEnd('\r');
            }
            Log.Print("Map parsed");
            // Переворачиваем её сверху вниз
            tileMap.Reverse();
            return tileMap;
        }

        public GameState GetGameState(string playerId) {
            var gs = new GameState();

            Entity[] entities = GameLayer.Entities.ToArray();
            foreach (Entity entity in entities){
                gs.Entities.Add(EntityState.ParseEntity(entity));
            }

            AccountState accState = Accounts.Find(x=>x.HeroId == playerId);
            if (accState != null){
                gs.Account = accState;
            }

            return gs;
        }

        private void UpdateLogic(float dt) {
            GameLayer.Update(dt);
        }

        public override void Update(float dt) {
            base.Update(dt);

            Server.SendGameStateToAll();
        }

        internal void AppendPlayerState(EntityState es) {
            //            GameLayer.UpdateEntity(es);
            // От игрока принимаем только его позицию
            Entity player = GameLayer.FindEntityById(es.Id);
            if (player != null){
                player.PositionX = es.PositionX;
                player.PositionY = es.PositionY;
            }
        }
    }
}