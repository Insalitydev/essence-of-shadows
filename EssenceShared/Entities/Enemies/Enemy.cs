using CocosSharp;
using EssenceShared.Entities.Players;
using EssenceShared.Game;

namespace EssenceShared.Entities.Enemies {
    public abstract class Enemy: Entity {
        public Enemy(string url, string id): base(url, id) {
            Scale = Settings.Scale;
            Tag = Tags.Enemy;
            Hp = new Stat(100);
        }

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.PlayerProjectile){
                var player = other.GetOwner() as Player;
                if (player != null)
                    Damage(player.AttackDamage);
            }
        }

        private void Damage(int p) {
            Hp.Current -= p;
            if (Hp.Perc == 0){
                Schedule(Die, 0.01f);
            }
        }

        protected virtual void Die(float dt) {
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