using CocosSharp;

namespace EssenceShared.Entities.Projectiles {
    public class MysticProjectile: Entity {
        private const float _speed = 400;
        private CCPoint _target;

        public MysticProjectile(string id, CCPoint target): base("MysticProjectile.png", id) {
            Scale = 4;
            _target = target;
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            Schedule(Update);
        }

        public override void Update(float dt) {
            base.Update(dt);

            PositionX += _speed*dt;
        }
    }
}