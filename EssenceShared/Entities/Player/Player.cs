using System;
using System.Runtime.Remoting.Channels;
using CocosSharp;
using EssenceShared.Entities.Projectiles;

namespace EssenceShared.Entities.Player {
    public class Player: Entity {
        private float _moveSpeed = 200;
        public Player(string id): base("Mystic.png", id) {
            Scale = 4;
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            Schedule(Update);
        }

        public override void Update(float dt) {
            base.Update(dt);
        }

        public void Control(float dt) {
            if (Input.IsKeyIn(CCKeys.Up)) {
                PositionY += _moveSpeed * dt;
            }

            if (Input.IsKeyIn(CCKeys.Down)) {
                PositionY -= _moveSpeed * dt;
            }

            if (Input.IsKeyIn(CCKeys.Right)) {
                PositionX += _moveSpeed * dt;
            }

            if (Input.IsKeyIn(CCKeys.Left)) {
                PositionX -= _moveSpeed * dt;
            }
        }

        public void Attack(CCPoint target) {
            Parent.AddChild(new MysticProjectile("sdsdss", new CCPoint(0, 0) ));
        }

        internal void AppendState(EntityState es) {
            EntityState.AppendStateToEntity(this, es);
        }
    }
}