using EssenceShared.Entities.Players;

namespace EssenceShared.Entities.Projectiles {
    public class EnemyProjectile: Entity {
        private const float Speed = 600;

        public EnemyProjectile(int damage, string url, string id)
            : base(url, id) {
            Scale = Settings.Scale;
            Tag = Tags.EnemyProjectile;
            AttackDamage = damage;
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            Schedule(Update);
            Schedule(Delete, 2);
            Rotation = 360 - Direction;
        }

        public override void Update(float dt) {
            base.Update(dt);

            MoveByAngle(Direction, Speed*dt);
        }

        public void Delete(float dt) {
            RemoveAllChildren(true);
            Remove();
        }

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.Player){
                if (other as Player != null){
                    (other as Player).Hp.Current -= AttackDamage;
                }
                Remove();
            }
        }
    }
}