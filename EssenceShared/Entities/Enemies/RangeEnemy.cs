using CocosSharp;
using EssenceShared.Entities.Projectiles;
using EssenceShared.Scenes;

namespace EssenceShared.Entities.Enemies {
    public class RangeEnemy: Enemy {
        public RangeEnemy(string url, string id): base(url, id) {
            AttackDamage = 15;
        }

        public override void OnEnter() {
            base.OnEnter();
            Schedule(TryAttack, 3);
        }

        public void TryAttack(float dt) {
            if (Parent.Tag == Tags.Server){
                CCNode pl = Parent.GetChildByTag(Tags.Player);

                if (pl != null && DistanceTo(pl.Position) < Settings.ScreenWidth){
                    var projectile = new EnemyProjectile(AttackDamage, Resources.ProjectileLaser, Util.GetUniqueId()) {
                        PositionX = PositionX,
                        PositionY = PositionY,
                        Direction =
                            AngleBetweenPoints(new CCPoint(PositionX, PositionY),
                                new CCPoint(pl.PositionX, pl.PositionY)),
                        OwnerId = Id
                    };
                    (Parent as GameLayer).AddEntity(projectile);
                }
            }
        }
    }
}