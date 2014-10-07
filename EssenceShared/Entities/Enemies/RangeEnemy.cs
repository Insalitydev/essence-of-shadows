using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared.Entities.Players;
using EssenceShared.Entities.Projectiles;
using EssenceShared.Scenes;

namespace EssenceShared.Entities.Enemies {
    public class RangeEnemy: Enemy {
        public RangeEnemy(string url, string id): base(url, id) {
            AttackDamage = 15;
            SightRadius = 300;
        }

        public override void OnEnter() {
            base.OnEnter();
        }

        protected override void Action(float dt) {
            base.Action(dt);

            var players = GetPlayers();

            switch (ActionState){
                case ActionState.Idle:
                    if (players.Any()){
                        if (DistanceTo(players[0].Position) < SightRadius){
                            Target = players[0];
                            ActionState = ActionState.MoveToAttack;
                        }
                    }
                    break;
                case ActionState.MoveToAttack:
                    
                    if (Target != null && DistanceTo(Target.Position) < SightRadius*2){
                        MoveToTarget(Target.Position, Speed*dt);
                    }
                    else{
                        Target = null;
                        ActionState = ActionState.Idle;
                    }
                    break;
                case ActionState.Attack:
                    break;
            }
        }

        protected override void Die(float dt) {
            var players = GetPlayers();

            foreach (Player pl in players){
                if (DistanceTo(pl.Position) < 800){
                    pl.accState.Gold += 130;
                }
            }

            base.Die(dt);
        }

        public void TryAttack(float dt) {
            if (Parent.Tag == Tags.Server){
                var players = Parent.Children.Where(x=>x.Tag == Tags.Player).OrderBy(x=>DistanceTo(x.Position));

                if (players.Any()){
                    var pl = players.First();

                    if (DistanceTo(pl.Position) < Settings.ScreenWidth){
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
}