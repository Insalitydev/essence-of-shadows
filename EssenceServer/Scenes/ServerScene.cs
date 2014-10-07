using System.Collections.Generic;
using System.IO;
using CocosSharp;
using EssenceShared;
using EssenceShared.Entities;
using EssenceShared.Entities.Enemies;
using EssenceShared.Entities.Players;
using EssenceShared.Scenes;

namespace EssenceServer.Scenes {
    /// <summary>
    ///     Основная сцена на сервере. Запускает игровой слой и занимается управлением состояние сервера
    /// </summary>
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

        public override void OnEnter() {
            base.OnEnter();
            InitMap();

            // Adding test enemies:
            int mapW = GameLayer.currentMap[0].Length*Settings.TileSize*Settings.Scale;
            int mapH = GameLayer.currentMap.Count*Settings.TileSize*Settings.Scale;
            Log.Print("Map size: " + mapW + " " + mapH);

            for (int i = 0; i < 2; i++){
                GameLayer.AddEntity(new RangeEnemy(Resources.EnemyStinger, Util.GetUniqueId()) {
                    PositionX = CCRandom.Next(100, mapW - 100),
                    PositionY = CCRandom.Next(100, mapH - 100)
                });
            }
        }

        /// <summary>
        ///     Считывает карту и возвращает её как массив строк
        /// </summary>
        public List<string> ParseMap() {
            string s = File.ReadAllText("TestMap.txt");
            var tileMap = new List<string>(s.Split('\n'));

            for (int i = 0; i < tileMap.Count; i++){
                tileMap[i] = tileMap[i].TrimEnd('\r');
            }
            // Переворачиваем её сверху вниз
            tileMap.Reverse();
            return tileMap;
        }

        /// <summary>
        ///     Возвращает текущее игровое состояние для указанного игрока
        ///     В состояние помещаются сущности, находящиеся на определенном расстоянии от игрока
        /// </summary>
        public GameState GetGameState(string playerId) {
            var gs = new GameState();

            var pl = GameLayer.FindEntityById(playerId) as Player;

            if (pl != null){
                Entity[] entities = GameLayer.Entities.ToArray();
                foreach (Entity entity in entities){
                    if (pl.DistanceTo(entity.Position) < 800)
                        gs.Entities.Add(EntityState.ParseEntity(entity));
                }

                AccountState accState = Accounts.Find(x=>x.HeroId == playerId);
                if (accState != null){
                    gs.Account = accState;
                }
            }

            return gs;
        }

        /// <summary>
        ///     Обновляет состояние игрока, полученное от клиента
        ///     Обновляется только его позиция
        /// </summary>
        internal void AppendPlayerState(EntityState es) {
            Entity player = GameLayer.FindEntityById(es.Id);
            if (player != null){
                player.PositionX = es.PositionX;
                player.PositionY = es.PositionY;
            }
        }

        private void UpdateLogic(float dt) {
            GameLayer.Update(dt);
        }

        public override void Update(float dt) {
            base.Update(dt);

            Server.SendGameStateToAll();
        }

        private void InitMap() {
            GameLayer.CreateNewMap(ParseMap());
        }
    }
}