using CocosSharp;

namespace EssenceShared.Entities.Map.Tiles {
    internal class Tile:CCSprite {
        public Tile(string url) : base(url) {
            Tag = Tags.MapTile;
        }
    }
}