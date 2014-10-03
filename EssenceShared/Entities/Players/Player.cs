using CocosSharp;
using EssenceShared.Game;

namespace EssenceShared.Entities.Players {
    public class Player: Entity {
        private const float _moveSpeed = 300;
        // TODO: Временно кол-во золото находится здесь
        public int Gold;
        public AccountState accState;

        public Player(string id, string type, AccountState account): base(type, id) {
            Scale = Settings.Scale;
            Tag = Tags.Player;
            accState = account;
            Hp = new Stat(200);
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

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.Enemy) {
                Hp.Current -= 1;
            }
        }

        public void Attack(CCPoint target) {
            //            Parent.AddChild(new MysticProjectile("sdsdss", new CCPoint(0, 0) ));
        }
    }
}