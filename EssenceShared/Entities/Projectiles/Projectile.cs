using System.Collections.Generic;

namespace EssenceShared.Entities.Projectiles {
    public abstract class Projectile: Entity {
        protected HashSet<Entity> AlreadyDamaged;
        protected float DieAfter = 1f;

        protected Projectile(string url, string id): base(url, id) {
            AlreadyDamaged = new HashSet<Entity>();
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            Schedule(Update);
            Schedule(Delete, DieAfter);
            Rotation = 360 - Direction;
        }

        public virtual void Delete(float dt) {
            RemoveAllChildren(true);
            Remove();
        }
    }
}