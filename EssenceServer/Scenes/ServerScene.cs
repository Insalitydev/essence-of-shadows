using System.Collections.Generic;
using System.IO;
using CocosSharp;
using EssenceShared;
using EssenceShared.Entities;
using EssenceShared.Entities.Enemies;
using EssenceShared.Entities.Players;
using EssenceShared.Game;
using EssenceShared.Scenes;
using Microsoft.Xna.Framework;

namespace EssenceServer.Scenes {
    /// <summary>
    ///     Основная сцена на сервере. Запускает игровой слой и занимается управлением состояние сервера
    /// </summary>
    internal class ServerScene: CCScene {

        public Dictionary<Locations, GameLayer> LocationDic;  

        public readonly GameLayer TownGameLayer;
        private readonly GameLayer GameLayer;
        public List<AccountState> Accounts = new List<AccountState>();

        public ServerScene(CCWindow window): base(window) {

            LocationDic = new Dictionary<Locations, GameLayer>();

            GameLayer = new GameLayer {Tag = Tags.Server, Location = Locations.Desert};
            AddChild(GameLayer);
            LocationDic.Add(Locations.Desert, GameLayer);

            TownGameLayer = new GameLayer { Tag = Tags.Server, Location = Locations.Town };
            AddChild(TownGameLayer);
            LocationDic.Add(Locations.Town, TownGameLayer);

            Log.Print("Game has started, waiting for players");
            Schedule(Update, 0.04f);
            Schedule(UpdateLogic);
        }

        public GameLayer GetGameLayer(Locations location) {
            return LocationDic[location];
        }

        public override void OnEnter() {
            base.OnEnter();
            InitMap();

            // Adding test enemies:
            int mapW = GameLayer.currentMap[0].Length*Settings.TileSize*Settings.Scale;
            int mapH = GameLayer.currentMap.Count*Settings.TileSize*Settings.Scale;
            Log.Print("Map size: " + mapW + " " + mapH);

            for (int i = 0; i < 50; i++){
                GameLayer.AddEntity(new RangeEnemy(Resources.EnemyStinger, Util.GetUniqueId()) {
                    PositionX = CCRandom.Next(100, mapW - 100),
                    PositionY = CCRandom.Next(100, mapH - 100)
                });
            }
            for (int i = 0; i < 50; i++) {
                GameLayer.AddEntity(new MeleeEnemy(Resources.EnemyMeleeRobot, Util.GetUniqueId()) {
                    PositionX = CCRandom.Next(100, mapW - 100),
                    PositionY = CCRandom.Next(100, mapH - 100)
                });
            }
        }

        /// <summary>
        ///     Считывает карту и возвращает её как массив строк
        /// </summary>
        public List<string> ParseMap(string map) {
            string s = File.ReadAllText(map);
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

            Player pl = GetPlayer(playerId);

            if (pl != null){
                AccountState accState = Accounts.Find(x => x.HeroId == playerId);
                if (accState != null){
                    gs.Account = accState;

                    Entity[] entities = GetGameLayer(accState.location).Entities.ToArray();
                    foreach (Entity entity in entities){
                        if (pl.DistanceTo(entity.Position) < 800)
                            gs.Entities.Add(EntityState.ParseEntity(entity));
                    }
                }
            }

            return gs;
        }

        /// <summary>
        ///     Обновляет состояние игрока, полученное от клиента
        ///     Обновляется только его позиция
        /// </summary>
        internal void AppendPlayerState(EntityState es) {
            Entity player = GetPlayer(es.Id);

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
            GameLayer.CreateNewMap(ParseMap("TestMap.txt"));
            TownGameLayer.CreateNewMap(ParseMap("TownMap.txt"));
        }

        internal Player GetPlayer(string id) {
            Player player = null;
            foreach (var gameLayer in LocationDic.Values) {
                player = gameLayer.FindEntityById(id) as Player;
                if (player != null)
                    break;
            }
            return player;
        }
    }
}