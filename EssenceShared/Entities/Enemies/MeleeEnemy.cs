using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared.Entities.Players;
using EssenceShared.Entities.Projectiles;
using EssenceShared.Scenes;

namespace EssenceShared.Entities.Enemies {
    public class MeleeEnemy: Enemy {
        public MeleeEnemy(string url, string id)
            : base(url, id) {
            Speed = 200;
            AttackDamage = 5;
            AttackCooldown = 1f;
            SightRadius = 500;
            AttackRadius = 100;
            Hp.Maximum = 150;
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

            // Если очень близко есть союзники, меняем угол движения
            var moveAngleChange = 0f;

            var enemies = Parent.Children.Where(x => ((x != null) && x.Tag == Tags.Enemy)).Cast<Entity>().OrderBy(DistanceTo).ToList();

            if (enemies.Count > 2){
                if (DistanceTo(enemies[2]) < 100){
                    moveAngleChange = -AngleTo(enemies[2].Position)/CCRandom.Next(1, 4);
                }
            }

            // Если в зоне атаки - атакуем
            if (Target != null && DistanceTo(Target) < AttackRadius && AttackCooldownCounter == 0){
                ActionState = ActionState.Attack;
            } // Если далеко - идем к цели
            else if (Target != null &&
                     DistanceTo(Target) < SightRadius*1.5f && DistanceTo(Target) > AttackRadius*0.7f){
                MoveByAngle(AngleTo(Target.Position) + moveAngleChange, Speed*dt);
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
                var projectile = new EnemyMeleeProjectile(AttackDamage, Resources.ParticleMeleeSweepAttack,
                    Util.GetUniqueId()) {
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