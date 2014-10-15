using CocosSharp;
using EssenceShared.Entities.Enemies;
using EssenceShared.Entities.Players;

namespace EssenceShared.Entities.Projectiles {
    public class MysticProjectile: Projectile {
        public MysticProjectile(int AttackDamage, string id): base(Resources.ProjectileMystic, id) {
            this.AttackDamage = AttackDamage;
            Scale = Settings.Scale;
            Tag = Tags.PlayerProjectile;
            Speed = 700;
            DieAfter = 2;
        }

        public override void OnEnter() {
            base.OnEnter();


            if (Parent != null && Parent.Tag == Tags.Client){
                CCParticleSystem emiter = Particle.GetEmiter(Resources.ParticleMysticProjectile,
                    ParticleType.ProjectileTrail, 3,
                    new CCPoint(0, 0), this);

                AddChild(emiter, -10);
            }
        }

        public override void Update(float dt) {
            base.Update(dt);

            MoveByAngle(Direction, Speed*dt);

            if (Parent != null && Parent.Tag == Tags.Client){
                UpdateAnimation(dt);
            }
        }

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.Enemy && !AlreadyDamaged.Contains(other)){
                var player = GetOwner() as Player;
                if (player != null) player.AccState.Exp.Current += 200;

                (other as Enemy).Damage(AttackDamage);
                AlreadyDamaged.Add(other);
            }
        }

        public void UpdateAnimation(float dt) {
            Color = new CCColor3B((byte) ((PositionX + PositionY)*2), (byte) ((PositionX + PositionY)*2),
                (byte) ((PositionX + PositionY)*2));
        }
    }
}