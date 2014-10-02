using CocosSharp;
using EssenceShared.Entities.Players;
using EssenceShared.Game;
using EssenceShared.Scenes;

namespace EssenceShared.Entities.Enemies {
    public class Enemy: Entity {

       
        public Enemy(string id): base(Resources.ItemChest, id) {
            Scale = 4;
            Tag = Tags.Enemy;
            Hp = new Stat(100);
        }

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.Projectile){

                Hp.Current -= 10;
                var player = other.GetOwner() as Player;
                
                if (player != null){
                    player.accState.Gold += 40;
                }
            }
        }

        protected override void Draw() {
            base.Draw();
            drawHealthBar();
        }

        private void drawHealthBar() {
            CCDrawingPrimitives.Begin();

            CCDrawingPrimitives.DrawSolidRect(new CCPoint(0, 0), new CCPoint(Texture.PixelsWide, -2), CCColor4B.Black);
            CCDrawingPrimitives.DrawSolidRect(new CCPoint(0, 0), new CCPoint(Texture.PixelsWide * Hp.GetPerc(), -2), CCColor4B.Red);

            CCDrawingPrimitives.End();
            
        }
    }
}