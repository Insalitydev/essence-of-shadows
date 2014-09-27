using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;

namespace EssenceShared.Entities.Projectiles {
    public class MysticProjectile:Entity {
        private CCPoint _target;
        private float _speed = 400;
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
