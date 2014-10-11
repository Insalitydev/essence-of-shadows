using CocosSharp;
using EssenceShared.Game;
using Lidgren.Network;

namespace EssenceShared.Entities.Players {
    public class Player: Entity {
        public AccountState accState;
        public float AttackCooldown;
        public float AttackCooldownCounter;

        public Player(string id, string type, AccountState account): base(type, id) {
            Scale = Settings.Scale;
            Tag = Tags.Player;
            accState = account;
            Hp = new Stat(200);
            AttackDamage = 20;
            Speed = 340;
            AttackCooldownCounter = 0;
            AttackCooldown = 0.2f;
        }

        public override void OnEnter() {
            base.OnEnter();
            Schedule(Update);
        }

        public override void Update(float dt) {
            base.Update(dt);
            UpdateAccState();

            AttackCooldownCounter -= dt;
            if (AttackCooldownCounter < 0){
                AttackCooldownCounter = 0;
            }
        }

        private void UpdateAccState() {
            if (accState != null){
                accState.Update();
            }
        }

        public void Control(float dt) {
            if (Input.IsKeyIn(CCKeys.Up) || Input.IsKeyIn(CCKeys.W)){
                PositionY += Speed*dt;
            }

            if (Input.IsKeyIn(CCKeys.Down) || Input.IsKeyIn(CCKeys.S)){
                PositionY -= Speed*dt;
            }

            if (Input.IsKeyIn(CCKeys.Right) || Input.IsKeyIn(CCKeys.D)){
                PositionX += Speed*dt;
            }

            if (Input.IsKeyIn(CCKeys.Left) || Input.IsKeyIn(CCKeys.A)){
                PositionX -= Speed*dt;
            }
        }

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.Enemy){
                Hp.Current -= 1;
            }
        }

        public void Attack(CCPoint target) {
        }
    }
}