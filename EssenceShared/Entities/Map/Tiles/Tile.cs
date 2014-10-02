using CocosSharp;

namespace EssenceShared.Entities.Map.Tiles {
    internal class Tile : Entity {
        public Tile(string id) : base(Resources.MapTileSand, id) {
            Tag = Tags.MapTile;
        }
    }
}