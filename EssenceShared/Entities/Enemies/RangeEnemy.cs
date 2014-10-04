using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using EssenceShared.Entities.Projectiles;
using EssenceShared.Scenes;

namespace EssenceShared.Entities.Enemies {
    public class RangeEnemy: Enemy {
        public RangeEnemy(string url, string id): base(url, id) {
            AttackDamage = 10;
        }

        public override void OnEnter() {
            base.OnEnter();
            Schedule(TryAttack, 3);
        }

        public void TryAttack(float dt) {
            if (Parent.Tag == Tags.Server){
                var pl = Parent.GetChildByTag(Tags.Player);

                if (pl != null && DistanceTo(pl.Position) < Settings.ScreenWidth){
                    var projectile = new EnemyProjectile(AttackDamage, Resources.ProjectileLaser, Util.GetUniqueId()) {
                        PositionX = PositionX,
                        PositionY = PositionY,
                        Direction =
                            Entity.AngleBetweenPoints(new CCPoint(PositionX, PositionY),
                                new CCPoint(pl.PositionX, pl.PositionY)),
                        OwnerId = Id
                    };
                    (Parent as GameLayer).AddEntity(projectile);
                }
            }
        }
    }
}
