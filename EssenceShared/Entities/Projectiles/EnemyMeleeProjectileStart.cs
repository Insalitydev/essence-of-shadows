using EssenceShared.Scenes;

namespace EssenceShared.Entities.Projectiles {
    public class EnemyMeleeProjectileStart: Projectile {
        public EnemyMeleeProjectileStart(int damage, string url, string id)
            : base(url, id) {
            Scale = Settings.Scale;
            Tag = Tags.EnemyProjectile;
            AttackDamage = damage;
            Speed = 50;
            DieAfter = 0.4f;
        }

        public override void Delete(float dt) {
            if (Parent.Tag == Tags.Server){
                var proj = new EnemyMeleeProjectile(AttackDamage, Resources.ParticleMeleeSweepAttack, Util.GetUniqueId()) {
                    Scale = ScaleX,
                    Direction = Direction,
                    Tag = Tags.EnemyProjectile,
                    Position = Position
                };
                (Parent as GameLayer).AddEntity(proj);
            }
            RemoveAllChildren(true);
            Remove();
        }
    }
}