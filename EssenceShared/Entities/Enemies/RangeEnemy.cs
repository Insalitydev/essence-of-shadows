using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared.Entities.Players;
using EssenceShared.Entities.Projectiles;
using EssenceShared.Scenes;

namespace EssenceShared.Entities.Enemies {
    public class RangeEnemy: Enemy {
        // Показывает, в какую сторону крутится враг, когда атакует, выбирается случайно. 1- вправо, -1 влево, 0 - не крутится
        private readonly int _isTurnRight;

        public RangeEnemy(string url, string id): base(url, id) {
            Speed = 150;
            AttackDamage = 15;
            AttackCooldown = 2;
            SightRadius = 500;
            AttackRadius = 650;
            Hp.Maximum = 100;
            Height = 8;

            _isTurnRight = CCRandom.Next(0, 3) - 1;
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
                     DistanceTo(Target) < SightRadius*1.5f && DistanceTo(Target) > AttackRadius*0.7f){
                MoveToTarget(Target.Position, Speed*dt);
            } // если слишком близко, отходим от врага
            else if (Target != null && DistanceTo(Target) < AttackRadius*0.5f){
                MoveFromTarget(Target.Position, Speed*dt);
            }
            else if (Target != null && DistanceTo(Target) < AttackRadius && AttackCooldownCounter != 0){
                if (_isTurnRight != 0)
                    MoveByAngle(AngleTo(Target.Position) + _isTurnRight*85, Speed*dt);
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
            }
            else{
                ActionState = ActionState.MoveToAttack;
            }
        }


        private void SpawnProjectileToTarget() {
            if (Target != null){
                var projectile = new EnemyRangeProjectile(AttackDamage, Resources.ProjectileLaser, Util.GetUniqueId()) {
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
                    pl.AccState.Gold += 130;
                }
            }

            base.Die(dt);
        }
    }
}