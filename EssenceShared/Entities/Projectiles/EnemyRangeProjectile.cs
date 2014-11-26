using EssenceShared.Entities.Players;

namespace EssenceShared.Entities.Projectiles {
    public class EnemyRangeProjectile : Projectile {
        public EnemyRangeProjectile(int damage, string url, string id)
            : base(url, id) {
            Scale = Settings.Scale;
            Tag = Tags.EnemyProjectile;
            AttackDamage = damage;
            Speed = 600;
            DieAfter = 2;
        }


        public override void Update(float dt) {
            base.Update(dt);

            MoveByAngle(Direction, Speed*dt);
        }


        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.Player) {
                if (other as Player != null && !AlreadyDamaged.Contains(other)) {
                    (other as Player).Hp.Current -= AttackDamage;
                    AlreadyDamaged.Add(other);
                }
                Remove();
            }
        }
    }
}