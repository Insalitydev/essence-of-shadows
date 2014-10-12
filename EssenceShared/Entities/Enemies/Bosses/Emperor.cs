using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using EssenceShared.Entities.Players;
using EssenceShared.Entities.Projectiles;
using EssenceShared.Scenes;

namespace EssenceShared.Entities.Enemies.Bosses {
    public class Emperor: Enemy {
        public Emperor(string id): base(Resources.BossEmperor, id) {
            Speed = 320;
            AttackDamage = 40;
            AttackCooldown = 1.5f;
            SightRadius = 800;
            AttackRadius = 120;
            Hp.Maximum = 4000;
        }

        protected override void IdleAction(float dt) {
            List<Player> players = GetPlayers();
            if (players.Any()) {
                if (DistanceTo(players[0]) < SightRadius) {
                    Target = players[0];
                    ActionState = ActionState.MoveToAttack;
                }
            }
        }

        protected override void MoveToAttackAction(float dt) {
            // Если в зоне атаки - атакуем
            if (Target != null && DistanceTo(Target) < AttackRadius && AttackCooldownCounter == 0) {
                ActionState = ActionState.Attack;
            } // Если далеко - идем к цели
            else if (Target != null &&
                     DistanceTo(Target) < SightRadius * 1.5f && DistanceTo(Target) > AttackRadius * 0.7f &&
                     AttackCooldownCounter == 0) {
                MoveByAngle(AngleTo(Target.Position), Speed * dt);
            } else {
                Target = null;
                ActionState = ActionState.Idle;
            }
        }

        protected override void TryAttackTarget(float dt) {
            if (AttackCooldownCounter == 0) {
                if (Target != null && DistanceTo(Target) < AttackRadius) {
                    SpawnProjectileToTarget();
                    // TODO: необходимо стоять на месте после атаки какое-то время
                    ActionState = ActionState.MoveToAttack;
                    AttackCooldownCounter = AttackCooldown;
                } else {
                    ActionState = ActionState.MoveToAttack;
                }
            } else {
                ActionState = ActionState.MoveToAttack;
            }
        }

        private void SpawnProjectileToTarget() {
            if (Target != null) {
                var projectile = new EnemyMeleeProjectileStart(AttackDamage, Resources.ParticleMeleeSweepStart,
                    Util.GetUniqueId()) {
                        PositionX = PositionX,
                        PositionY = PositionY,
                        Direction =
                            AngleBetweenPoints(new CCPoint(PositionX, PositionY),
                                new CCPoint(Target.PositionX, Target.PositionY)),
                        OwnerId = Id,
                        Scale = 8
                    };
                projectile.Position += Entity.GetNormalPointByDirection(projectile.Direction)*Texture.PixelsWide/2*ScaleX;
                (Parent as GameLayer).AddEntity(projectile);
            }
        }

        protected override void Die(float dt) {
            List<Player> players = GetPlayers();

            foreach (Player pl in players) {
                if (DistanceTo(pl.Position) < 800) {
                    pl.accState.Gold += 5000;
                }
            }
            base.Die(dt);
        }

    }
}
