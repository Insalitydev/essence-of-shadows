using EssenceShared.Entities.Players;
using EssenceShared.Scenes;

namespace EssenceShared.Entities.Enemies {
    public class Enemy: Entity {
        public Enemy(string id): base(Resources.ItemChest, id) {
            Scale = 4;
            Tag = Tags.Enemy;
        }

        public override void Collision(Entity other) {
            base.Collision(other);

            if (other.Tag == Tags.Projectile){
                var player = other.GetOwner() as Player;
                
                if (player != null){
                    player.accState.Gold += 40;
                }
            }
        }
    }
}