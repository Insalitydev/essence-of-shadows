using CocosSharp;
using EssenceShared;
using EssenceShared.Entities;
using EssenceShared.Entities.Enemies;
using EssenceShared.Entities.Enemies.Bosses;
using EssenceShared.Game;
using EssenceShared.Scenes;

namespace EssenceServer {
    /// <summary>
    ///     Прикрепляется к игрому слою на сервере и управляет этим слоём по части
    ///     создания врагов и завершения текущей сессии этого мира
    /// </summary>
    internal class EnemyManager {
        private const int MinEnemies = 40;
        private readonly GameLayer _gameLayer;
        private Entity _boss;


        public EnemyManager(GameLayer gameLayer) {
            _gameLayer = gameLayer;
            SpawnBoss();
        }

        public void Update() {
            if (EnemiesCount() < MinEnemies){
                if (_gameLayer != null)
                    _gameLayer.AddEntity(GetRandomEnemy());
            }
        }

        private int EnemiesCount() {
            return _gameLayer != null ? _gameLayer.Children.Count : 0;
        }

        /// <summary>
        ///     Возвращает случайного врага (ближнего или дальнего боя)
        ///     для текущей локации
        /// </summary>
        private Entity GetRandomEnemy() {
            bool isRange = CCRandom.Next(0, 2) == 0;

            Entity enemy = null;

            switch (_gameLayer.Location){
                case Locations.Desert:
                    if (isRange)
                        enemy = new RangeEnemy(Resources.EnemyStinger, Util.GetUniqueId());
                    else
                        enemy = new MeleeEnemy(Resources.EnemyPirate, Util.GetUniqueId());
                    break;
                case Locations.Cave:
                    if (isRange)
                        enemy = new RangeEnemy(Resources.EnemyMagicRange, Util.GetUniqueId());
                    else
                        enemy = new MeleeEnemy(Resources.EnemyMagicMelee, Util.GetUniqueId());
                    break;
                case Locations.City:
                    if (isRange)
                        enemy = new RangeEnemy(Resources.EnemyStinger, Util.GetUniqueId());
                    else
                        enemy = new MeleeEnemy(Resources.EnemyMeleeRobot, Util.GetUniqueId());
                    break;
            }

            enemy.Position = new CCPoint(CCRandom.Next(0, (int) _gameLayer.MapSize().Width),
                CCRandom.Next(0, (int) _gameLayer.MapSize().Height));

            return enemy;
        }

        private void SpawnBoss() {
            Entity boss = null;
            switch (_gameLayer.Location){
                case Locations.Desert:
                    boss = new Emperor(Util.GetUniqueId());
                    break;
                case Locations.City:
                    boss = new Cardinal(Util.GetUniqueId());
                    break;
                default:
                    Log.Print("DO nothing at SpawnBoss");
                    break;
            }

            if (boss != null){
                boss.Position = new CCPoint(_gameLayer.MapSize().Width, _gameLayer.MapSize().Height);
                _gameLayer.AddEntity(boss);
                _boss = boss;
            }
        }
    }
}