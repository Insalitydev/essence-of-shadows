using CocosSharp;
using EssenceShared.Game;

namespace EssenceShared.Entities.Players {
    public class Player: Entity {
        public float AttackCooldown;
        public float AttackCooldownCounter;
        public AccountState accState;

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
                MoveByAngle(90, Speed*dt);
            }

            if (Input.IsKeyIn(CCKeys.Down) || Input.IsKeyIn(CCKeys.S)){
                MoveByAngle(270, Speed*dt);
            }

            if (Input.IsKeyIn(CCKeys.Right) || Input.IsKeyIn(CCKeys.D)){
                MoveByAngle(0, Speed*dt);
            }

            if (Input.IsKeyIn(CCKeys.Left) || Input.IsKeyIn(CCKeys.A)){
                MoveByAngle(180, Speed*dt);
            }
        }

        public void Attack(CCPoint target) {
        }
    }
}