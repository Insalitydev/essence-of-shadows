using CocosSharp;

namespace EssenceShared.Entities.Player {
    public class Player: Entity {
        private float _moveSpeed = 200;
        public Player(ulong id): base("Mystic.png", id) {
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
    }
}