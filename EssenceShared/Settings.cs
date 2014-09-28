using CocosSharp;

namespace EssenceShared {
    public class Settings {
        /** General settings */
        public const float ScreenWidth = 800f;
        public const float ScreenHeight = 600f;
        public static CCSize ScreenSize = new CCSize(ScreenWidth, ScreenHeight);
        public const string GameName = "Essence of Shadows";
        public const string GameIdentifier = "EssenceOfShadows";

        /** Log && Statistic settings */
        public const bool IsDebug = true;
        public const bool IsLogToFile = false;

        /** Network settings */
        public const int Port = 4075;
        public const int MaxConnections = 5;

    }
}