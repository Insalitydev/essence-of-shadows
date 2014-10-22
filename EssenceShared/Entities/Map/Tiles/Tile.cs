using CocosSharp;

namespace EssenceShared.Entities.Map.Tiles {
    internal class Tile: CCSprite {
        public bool IsSolid = false;
        public Tile(string url): base(url) {
            Tag = Tags.MapTile;
            IsAntialiased = false;
            AnchorPoint = CCPoint.AnchorLowerLeft;
        }
    }
}