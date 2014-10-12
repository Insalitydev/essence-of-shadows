using System.Collections.Generic;
using System.IO;
using System.Linq;
using CocosSharp;
using EssenceShared;
using EssenceShared.Entities;
using EssenceShared.Entities.Enemies;
using EssenceShared.Entities.Enemies.Bosses;
using EssenceShared.Entities.Players;
using EssenceShared.Game;
using EssenceShared.Scenes;
using Microsoft.Xna.Framework;

namespace EssenceServer.Scenes {
    /// <summary>
    ///     Основная сцена на сервере. Запускает игровой слой и занимается управлением состояние сервера
    /// </summary>
    internal class ServerScene: CCScene {
        private readonly GameLayer GameLayer;
        public readonly GameLayer TownGameLayer;
        public readonly GameLayer CityGameLayer;
        public readonly GameLayer CaveGameLayer;
        public List<AccountState> Accounts = new List<AccountState>();
        public Dictionary<Locations, GameLayer> LocationDic;

        public ServerScene(CCWindow window): base(window) {
            LocationDic = new Dictionary<Locations, GameLayer>();

            GameLayer = new GameLayer {Tag = Tags.Server, Location = Locations.Desert};
            AddChild(GameLayer);
            LocationDic.Add(Locations.Desert, GameLayer);

            TownGameLayer = new GameLayer {Tag = Tags.Server, Location = Locations.Town};
            AddChild(TownGameLayer);
            LocationDic.Add(Locations.Town, TownGameLayer);

            CityGameLayer = new GameLayer { Tag = Tags.Server, Location = Locations.City };
            AddChild(CityGameLayer);
            LocationDic.Add(Locations.City, CityGameLayer);

            CaveGameLayer = new GameLayer { Tag = Tags.Server, Location = Locations.Cave };
            AddChild(CaveGameLayer);
            LocationDic.Add(Locations.Cave, CaveGameLayer);

            Log.Print("Game has started, waiting for players");
            Schedule(UpdateNetwork, 0.04f);
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

//            for (int i = 0; i < 50; i++){
//                GameLayer.AddEntity(new RangeEnemy(Resources.EnemyStinger, Util.GetUniqueId()) {
//                    PositionX = CCRandom.Next(100, mapW - 100),
//                    PositionY = CCRandom.Next(100, mapH - 100)
//                });
//            }
            for (int i = 0; i < 20; i++){
                GameLayer.AddEntity(new MeleeEnemy(Resources.EnemyMeleeRobot, Util.GetUniqueId()) {
                    PositionX = CCRandom.Next(100, mapW - 100),
                    PositionY = CCRandom.Next(100, mapH - 100)
                });
            }
            GameLayer.AddEntity(new Emperor(Util.GetUniqueId()) {
                PositionX = -100,
                PositionY = -100
            });
            GameLayer.AddEntity(new Gate(Util.GetUniqueId()) {
                PositionX = -10,
                PositionY = -10,
                TeleportTo = Locations.Town
            });

            for (int i = 0; i < 10; i++)
                TownGameLayer.AddEntity(new GoldStack(Util.GetUniqueId()) {
                    PositionX = CCRandom.Next(100, 1400),
                    PositionY = CCRandom.Next(100, 1400)
                });
            TownGameLayer.AddEntity(new Gate(Util.GetUniqueId()) {
                PositionX = 500, PositionY = 500, TeleportTo = Locations.Desert
            });
            TownGameLayer.AddEntity(new Gate(Util.GetUniqueId()) {
                PositionX = 700,
                PositionY = 600,
                TeleportTo = Locations.City
            }); 
            TownGameLayer.AddEntity(new Gate(Util.GetUniqueId()) {
                PositionX = 900,
                PositionY = 500,
                TeleportTo = Locations.Cave
            });


            CityGameLayer.AddEntity(new Gate(Util.GetUniqueId()) {
                PositionX = -10,
                PositionY = -10,
                TeleportTo = Locations.Town
            });
            CaveGameLayer.AddEntity(new Gate(Util.GetUniqueId()) {
                PositionX = -10,
                PositionY = -10,
                TeleportTo = Locations.Town
            });

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
                AccountState accState = Accounts.Find(x=>x.HeroId == playerId);
                if (accState != null){
                    gs.Account = accState;

                    Entity[] entities = GetGameLayer(accState.Location).Entities.ToArray();
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
                player.FlipX = es.FlipX;
            }
        }

        private void UpdateLogic(float dt) {
            foreach (var gameLayer in LocationDic.Values){
                gameLayer.Update(dt);
            }

            //TODO: временное решение? когда меняется локация, пересылаем карту
            foreach (var accountState in Accounts.Where(accountState=>accountState.PrevLocation != accountState.Location)){
                accountState.PrevLocation = accountState.Location;
                Server.SendMap(accountState.HeroId, accountState.Location);
            }
        }

        public void UpdateNetwork(float dt) {
            Server.SendGameStateToAll();
        }

        private void InitMap() {
            GameLayer.CreateNewMap(ParseMap("DesertMap.txt"));
            TownGameLayer.CreateNewMap(ParseMap("TownMap.txt"));
            CityGameLayer.CreateNewMap(ParseMap("CityMap.txt"));
            CaveGameLayer.CreateNewMap(ParseMap("CaveMap.txt"));
        }

        internal Player GetPlayer(string id) {
            Player player = null;
            foreach (GameLayer gameLayer in LocationDic.Values){
                player = gameLayer.FindEntityById(id) as Player;
                if (player != null)
                    break;
            }
            return player;
        }
    }
}