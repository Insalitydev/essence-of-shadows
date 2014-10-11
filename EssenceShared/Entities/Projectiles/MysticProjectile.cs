using CocosSharp;
using EssenceShared.Entities.Players;

namespace EssenceShared.Entities.Projectiles {
    public class MysticProjectile: Entity {
        public MysticProjectile(string id): base(Resources.ProjectileMystic, id) {
            Scale = Settings.Scale;
            Tag = Tags.PlayerProjectile;
            Speed = 700;
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            Schedule(Update);
            Schedule(Delete, 2);
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

        public void Delete(float dt) {
            RemoveAllChildren(true);
            Remove();
        }

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.Enemy){
                var player = GetOwner() as Player;
                if (player != null) player.accState.Exp.Current += 200;

                Remove();
            }
        }

        public void UpdateAnimation(float dt) {
            Color = new CCColor3B((byte) ((PositionX + PositionY)*2), (byte) ((PositionX + PositionY)*2),
                (byte) ((PositionX + PositionY)*2));
        }
    }
}