using CocosSharp;

namespace EssenceShared {
    public class Resources {
        public const string ClassReaper = "Reaper";
        public const string ClassSniper = "Sniper";
        public const string ClassMystic = "Mystic";
        public const string ProjectileMystic = "MysticProjectile";
        public const string ProjectileSniper = "SniperProjectile";

        public const string BossEmperor = "Emperor";
        public const string BossCardinal = "Cardinal";
        public const string BossInteritus = "Interitus";
        public const string BossMossorus = "Mossorus";

        public const string ItemChest = "Chest";
        public const string ItemGold = "GoldStack";
        public const string ItemHealpot = "HealPot";
        public const string ItemKeyCave = "KeyCave";
        public const string ItemKeyUrban = "KeyUrban";
        public const string ItemKeyDesert = "KeyDesert";

        public const string ParticleMysticProjectile = "MysticProjectileTrail";

        public const string MapTileSand = "Sand.png";
        public const string MapTileGrass = "Grass.png";
        public const string MapTileStone = "Stone.png";
        public const string MapTileWater = "Water.png";
        public const string MapTileRoad = "Road.png";
        public const string MapTileDirt = "Dirt.png";


        /** Добавляет в пути поиска ресурсов необходимые папки */

        public static void LoadContent(CCApplication application) {
            Log.Print("Loading resources");
            application.ContentRootDirectory = "Resource";

            CCSpriteFontCache.DefaultFont = "kongtext";

            application.ContentSearchPaths.Add("Font");
            application.ContentSearchPaths.Add("Icon");
            application.ContentSearchPaths.Add("Image");
            application.ContentSearchPaths.Add("Image\\Boss");
            application.ContentSearchPaths.Add("Image\\Class");
            application.ContentSearchPaths.Add("Image\\Effects");
            application.ContentSearchPaths.Add("Image\\GUI");
            application.ContentSearchPaths.Add("Image\\GUI\\Menu");
            application.ContentSearchPaths.Add("Image\\Item");
            application.ContentSearchPaths.Add("Image\\Projectile");
            application.ContentSearchPaths.Add("Image\\Tile");
            application.ContentSearchPaths.Add("Image\\Tile\\Cave");
            application.ContentSearchPaths.Add("Image\\Tile\\City");
            application.ContentSearchPaths.Add("Image\\Tile\\Desert");
            application.ContentSearchPaths.Add("Music");
            application.ContentSearchPaths.Add("Sound");

            CCSpriteFontCache.FontScale = 1f;
            CCSpriteFontCache.RegisterFont("kongtext", 14, 24);
            CCSpriteFontCache.RegisterFont("arial", 24);
        }
    }
}