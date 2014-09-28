namespace EssenceShared.Entities.Enemies {
    public class Enemy: Entity {
        public Enemy(string id): base(Resources.ItemChest, id) {
            Scale = 4;
            Tag = Tags.Enemy;
        }
    }
}