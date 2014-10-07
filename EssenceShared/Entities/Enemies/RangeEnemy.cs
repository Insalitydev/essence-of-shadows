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
            AttackCooldown = 3;
            SightRadius = 500;
            AttackRadius = 250;
        }

        protected override void Action(float dt) {
            base.Action(dt);

            List<Player> players = GetPlayers();

            switch (ActionState){
                case ActionState.Idle:
                    if (players.Any()){
                        if (DistanceTo(players[0]) < SightRadius){
                            Target = players[0];
                            ActionState = ActionState.MoveToAttack;
                        }
                    }
                    break;
                case ActionState.MoveToAttack:
                    Log.Print("Move state");
                    if (Target != null && DistanceTo(Target) < AttackRadius){
                        ActionState = ActionState.Attack;
                    }

                    if (Target != null && DistanceTo(Target) < SightRadius*2){
                        MoveToTarget(Target.Position, Speed*dt);
                    }
                    else{
                        Target = null;
                        ActionState = ActionState.Idle;
                    }
                    break;
                case ActionState.Attack:
                    Log.Print("Attack state");
                    TryAttackTarget();
                    break;
            }
        }

        public void TryAttackTarget() {
            if (AttackCooldownCounter == 0){
                if (Target != null && DistanceTo(Target) < AttackRadius){
                    SpawnProjectileToTarget();
                    ActionState = ActionState.MoveToAttack;
                    AttackCooldownCounter = AttackCooldown;
                }
            }
            else{
                ActionState = ActionState.MoveToAttack;
            }
        }

        private void SpawnProjectileToTarget() {
            if (Target != null){
                var projectile = new EnemyProjectile(AttackDamage, Resources.ProjectileLaser, Util.GetUniqueId()) {
                    PositionX = PositionX,
                    PositionY = PositionY,
                    Direction =
                        AngleBetweenPoints(new CCPoint(PositionX, PositionY),
                            new CCPoint(Target.PositionX, Target.PositionY)),
                    OwnerId = Id
                };
                (Parent as GameLayer).AddEntity(projectile);
            }
        }

        protected override void Die(float dt) {
            List<Player> players = GetPlayers();

            foreach (Player pl in players){
                if (DistanceTo(pl.Position) < 800){
                    pl.accState.Gold += 130;
                }
            }

            base.Die(dt);
        }
    }
}