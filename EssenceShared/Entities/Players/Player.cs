using CocosSharp;

namespace EssenceShared.Entities.Players {
    public class Player: Entity {
        private const float _moveSpeed = 200;
        // TODO: Временно кол-во золото находится здесь
        public int Gold;

        public Player(string id): base("Mystic.png", id) {
            Scale = 4;
            Tag = Tags.Player;
            Gold = 0;
        }

        public Player(string id, string type): base(type, id) {
            Scale = 4;
            Tag = Tags.Player;
            Gold = 0;
        }

        public void Control(float dt) {
            if (Input.IsKeyIn(CCKeys.Up)){
                PositionY += _moveSpeed*dt;
            }

            if (Input.IsKeyIn(CCKeys.Down)){
                PositionY -= _moveSpeed*dt;
            }

            if (Input.IsKeyIn(CCKeys.Right)){
                PositionX += _moveSpeed*dt;
            }

            if (Input.IsKeyIn(CCKeys.Left)){
                PositionX -= _moveSpeed*dt;
            }
        }

        public void Attack(CCPoint target) {
            //            Parent.AddChild(new MysticProjectile("sdsdss", new CCPoint(0, 0) ));
        }
    }
}