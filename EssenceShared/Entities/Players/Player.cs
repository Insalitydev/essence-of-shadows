using CocosSharp;
using EssenceShared.Game;

namespace EssenceShared.Entities.Players {
    public class Player: Entity {
        private const float _moveSpeed = 300;
        public AccountState accState;

        public Player(string id, string type, AccountState account): base(type, id) {
            Scale = Settings.Scale;
            Tag = Tags.Player;
            accState = account;
            Hp = new Stat(200);
            AttackDamage = 25;
        }

        public override void OnEnter() {
            base.OnEnter();
            Schedule(Update);
        }

        public override void Update(float dt) {
            base.Update(dt);
            UpdateAccState();
        }

        private void UpdateAccState() {
            if (accState != null) {
                accState.Update();
            }
        }

        public void Control(float dt) {
            if (Input.IsKeyIn(CCKeys.Up) || Input.IsKeyIn(CCKeys.W)){
                PositionY += _moveSpeed*dt;
            }

            if (Input.IsKeyIn(CCKeys.Down) || Input.IsKeyIn(CCKeys.S)) {
                PositionY -= _moveSpeed*dt;
            }

            if (Input.IsKeyIn(CCKeys.Right) || Input.IsKeyIn(CCKeys.D)) {
                PositionX += _moveSpeed*dt;
            }

            if (Input.IsKeyIn(CCKeys.Left) || Input.IsKeyIn(CCKeys.A)) {
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