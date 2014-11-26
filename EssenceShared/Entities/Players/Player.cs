using CocosSharp;
using EssenceShared.Game;

namespace EssenceShared.Entities.Players {
    public class Player : Entity {
        public const int BaseHp = 200;
        public const int BaseAd = 20;
        public const int BaseSpeed = 270;
        public AccountState AccState;
        public float AttackCooldown;
        public float AttackCooldownCounter;

        public Player(string id, string type, AccountState account) : base(type, id) {
            Scale = Settings.Scale;
            Tag = Tags.Player;
            AccState = account;

            Hp = new Stat(BaseHp);
            AttackDamage = BaseAd;
            Speed = BaseSpeed;
            AttackCooldownCounter = 0;
            AttackCooldown = 0.2f;
        }

        public override void OnEnter() {
            base.OnEnter();


            if (Parent.Tag == Tags.Client) {
                var ss = new CCSpriteSheet("MysticAnim.plist");
                var walkAnim = new CCAnimation(ss.Frames, 0.3f);
                foreach (CCSpriteFrame sf in ss.Frames) {
                    sf.Texture.IsAntialiased = false;
                }

                AddAction(new CCRepeatForever(new CCAnimate(walkAnim)));
            }

            Schedule(Update);
        }

        public override void Update(float dt) {
            base.Update(dt);
            UpdateAccState();

            AttackCooldownCounter -= dt;
            if (AttackCooldownCounter < 0) {
                AttackCooldownCounter = 0;
            }
        }

        private void UpdateAccState() {
            if (AccState != null) {
                AccState.Update();
            }
        }

        public void Control(float dt) {
            if (Input.IsKeyIn(CCKeys.Up) || Input.IsKeyIn(CCKeys.W)) {
                MoveByAngle(90, Speed*dt);
            }

            if (Input.IsKeyIn(CCKeys.Down) || Input.IsKeyIn(CCKeys.S)) {
                MoveByAngle(270, Speed*dt);
            }

            if (Input.IsKeyIn(CCKeys.Right) || Input.IsKeyIn(CCKeys.D)) {
                MoveByAngle(0, Speed*dt);
            }

            if (Input.IsKeyIn(CCKeys.Left) || Input.IsKeyIn(CCKeys.A)) {
                MoveByAngle(180, Speed*dt);
            }
        }

        public void Attack(CCPoint target) {
        }
    }
}