using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EssenceShared.Entities.Projectiles {
    public abstract class Projectile: Entity {
        protected Projectile(string url, string id): base(url, id) {
        }
    }
}
