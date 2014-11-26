using CocosSharp;

namespace EssenceShared {
    public class Settings {
        /** General settings */
        public const float ScreenWidth = 1024f;
        public const float ScreenHeight = 740f;
        public const string GameName = "Essence of Shadows";
        public const string GameIdentifier = "EssenceOfShadows";
        public const int Scale = 4;

        /** Gameplay settings */
        public const int StartExp = 1000;
        public const float ExpMultiplier = 1.3f;

        /** Log && Statistic settings */
        public const bool IsDebug = true;
        public const bool IsLogToFile = false;

        /** Map settings */
        public const int TileSize = 32;

        /** Network settings */
        public const int Port = 4075;
        public const int MaxConnections = 8;
        public const float NetworkFreqUpdate = 0.014f;
        public static CCSize ScreenSize = new CCSize(ScreenWidth, ScreenHeight);
    }
}