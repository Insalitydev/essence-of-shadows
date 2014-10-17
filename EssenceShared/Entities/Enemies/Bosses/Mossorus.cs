using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared.Entities.Players;
using EssenceShared.Entities.Projectiles;
using EssenceShared.Scenes;

namespace EssenceShared.Entities.Enemies.Bosses {
    public class Mossorus: Enemy {
        public Mossorus(string id)
            : base(Resources.BossMossorus, id) {
            Speed = 300;
            AttackDamage = 100;
            AttackCooldown = 0.9f;
            SightRadius = 600;
            AttackRadius = 300;
            Hp.Maximum = 3000;
        }

        protected override void IdleAction(float dt) {
            List<Player> players = GetPlayers();
            if (players.Any()){
                if (DistanceTo(players[0]) < SightRadius){
                    Target = players[0];
                    ActionState = ActionState.MoveToAttack;
                }
            }
        }

        protected override void MoveToAttackAction(float dt) {
            // Если в зоне атаки - атакуем
            if (Target != null && DistanceTo(Target) < AttackRadius && AttackCooldownCounter == 0){
                ActionState = ActionState.Attack;
            } // Если далеко - идем к цели
            else if (Target != null &&
                     DistanceTo(Target) < SightRadius*1.5f && DistanceTo(Target) > AttackRadius*0.7f &&
                     AttackCooldownCounter == 0){
                MoveByAngle(AngleTo(Target.Position), Speed*dt);
            }
            else{
                Target = null;
                ActionState = ActionState.Idle;
            }
        }

        protected override void TryAttackTarget(float dt) {
            if (AttackCooldownCounter == 0){
                if (Target != null && DistanceTo(Target) < AttackRadius){
                    SpawnProjectileToTarget();
                    // TODO: необходимо стоять на месте после атаки какое-то время
                    ActionState = ActionState.MoveToAttack;
                    AttackCooldownCounter = AttackCooldown;
                }
                else{
                    ActionState = ActionState.MoveToAttack;
                }
            }
            else{
                ActionState = ActionState.MoveToAttack;
            }
        }

        private void SpawnProjectileToTarget() {
            if (Target != null){
                var projectile = new EnemyRangeProjectile(AttackDamage, Resources.ProjectileCardinalPulse,
                    Util.GetUniqueId()) {
                        PositionX = PositionX,
                        PositionY = PositionY,
                        Direction =
                            AngleBetweenPoints(new CCPoint(PositionX, PositionY),
                                new CCPoint(Target.PositionX, Target.PositionY)),
                        OwnerId = Id,
                    };
                (Parent as GameLayer).AddEntity(projectile);
            }
        }

        protected override void Die(float dt) {
            List<Player> players = GetPlayers();

            foreach (Player pl in players){
                if (DistanceTo(pl.Position) < 800){
                    pl.AccState.Gold += 10000;
                }
            }
            base.Die(dt);
        }
    }
}