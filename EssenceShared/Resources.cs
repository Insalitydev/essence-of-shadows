using CocosSharp;

namespace EssenceShared {
    public class Resources {
        public const string ClassReaper = "Reaper";
        public const string ClassSniper = "Sniper";
        public const string ClassMystic = "Mystic";

        public const string ProjectileMystic = "MysticProjectile";
        public const string ProjectileSniper = "SniperProjectile";
        public const string ProjectileLaser = "LaserProjectile";
        public const string ProjectileCardinalPulse = "CardinalPulseProjectile";
        public const string ProjectileCardinalRocket = "CardinalRocketProjectile";

        public const string EnemyStinger = "Stinger";
        public const string EnemyMeleeRobot = "MeleeRobot";
        public const string EnemyMagicMelee = "MagicMelee";
        public const string EnemyMagicRange = "MagicRange";
        public const string EnemyPirate = "Pirate";

        public const string BossEmperor = "Emperor";
        public const string BossCardinal = "Cardinal";
        public const string BossCardinalBlades = "CardinalBlades";
        public const string BossInteritus = "Interitus";
        public const string BossMossorus = "Mossorus";

        public const string ItemChest = "Chest";
        public const string ItemGold = "GoldStack";
        public const string ItemHealpot = "HealPot";
        public const string ItemKeyCave = "KeyCave";
        public const string ItemKeyUrban = "KeyUrban";
        public const string ItemKeyDesert = "KeyDesert";
        public const string ItemGate = "Gate";

        public const string ObjectSmith = "Smith";

        public const string GuiIncrease = "Increase";
        public const string GuiIncreaseActive = "IncreasePressed";
        public const string GuiIncreaseInactive = "IncreaseInactive";

        public const string GraphicShadow = "Shadow";

        public const string ParticleMysticProjectile = "MysticProjectileTrail";
        public const string ParticleLevelUp = "ExpParticle";
        public const string ParticleMeleeSweepStart = "MeleeAttackLine";
        public const string ParticleMeleeSweepAttack = "MeleeAttackLineActive";

        public const string MapTileError = "Error.png";
        public const string MapTileSand = "Sand.png";
        public const string MapTileGrass = "Grass.png";
        public const string MapTileStone = "Stone.png";
        public const string MapTileCityStone = "CityStone.png";
        public const string MapTileWater = "Water.png";
        public const string MapTileRoad = "CityRoad.png";
        public const string MapTileDirt = "CaveDirt.png";
        public const string MapTileCaveWall = "CaveWall.png";
        public const string MapTileTownCell = "TownCell.png";


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
            application.ContentSearchPaths.Add("Image\\Objects");
            application.ContentSearchPaths.Add("Image\\Item");
            application.ContentSearchPaths.Add("Image\\Projectile");
            application.ContentSearchPaths.Add("Image\\Tile");
            application.ContentSearchPaths.Add("Image\\Tile\\Cave");
            application.ContentSearchPaths.Add("Image\\Tile\\City");
            application.ContentSearchPaths.Add("Image\\Tile\\Desert");
            application.ContentSearchPaths.Add("Image\\Tile\\Town");
            application.ContentSearchPaths.Add("Image\\Enemy");
            application.ContentSearchPaths.Add("Music");
            application.ContentSearchPaths.Add("Sound");

            CCSpriteFontCache.FontScale = 1f;
            CCSpriteFontCache.RegisterFont("kongtext", 14, 24);
            CCSpriteFontCache.RegisterFont("arial", 24);
        }
    }
}