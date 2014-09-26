using CocosSharp;

namespace EssenceShared {
    public class Settings {
        /** General settings */
        public const float SCREEN_WIDTH = 800f;
        public const float SCREEN_HEIGHT = 600f;
        public static CCSize SCREEN_SIZE = new CCSize(SCREEN_WIDTH, SCREEN_HEIGHT);
        public const string GAME_NAME = "Essence of Shadows";
        public const string GAME_IDENTIFIER = "EssenceOfShadows";

        /** Log && Statistic settings */
        public const bool IS_LOG_TO_FILE = false;

        /** Network settings */
        public const int PORT = 4075;
        public const int MAX_CONNECTIONS = 10;
    }
}