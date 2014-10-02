using CocosSharp;

namespace EssenceShared.Entities.Map.Tiles {
    internal class Tile : Entity {
        public Tile(string url, string id) : base(url, id) {
            Tag = Tags.MapTile;
        }

        protected override void AddedToScene() {
        }

        public override void Update(float dt) {
        }
    }
}