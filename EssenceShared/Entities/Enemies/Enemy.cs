using CocosSharp;
using EssenceShared.Entities.Players;
using EssenceShared.Game;

namespace EssenceShared.Entities.Enemies {
    public class Enemy: Entity {
        public Enemy(string id): base(Resources.ItemChest, id) {
            Scale = Settings.Scale;
            Tag = Tags.Enemy;
            Hp = new Stat(100);
        }

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.Projectile){
                var player = other.GetOwner() as Player;

                if (player != null){
                    player.accState.Gold += 10;
                    player.accState.Exp.Current += 5;
                }

                Damage(10);
            }
        }

        private void Damage(int p) {
            Hp.Current -= 20;
            if (Hp.Perc == 0){
                Schedule(Die, 0.1f);
            }
        }

        private void Die(float dt) {
            Remove();
        }

        protected override void Draw() {
            base.Draw();
            drawHealthBar();
        }

        private void drawHealthBar() {
            CCDrawingPrimitives.Begin();

            CCDrawingPrimitives.DrawSolidRect(new CCPoint(0, 0), new CCPoint(Texture.PixelsWide, -2), CCColor4B.Black);
            CCDrawingPrimitives.DrawSolidRect(new CCPoint(0, 0), new CCPoint(Texture.PixelsWide*Hp.Perc, -2),
                CCColor4B.Red);

            CCDrawingPrimitives.End();
        }
    }
}