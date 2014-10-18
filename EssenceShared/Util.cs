using CocosSharp;

namespace EssenceShared {
    public class Util {
        private static long _lastId = 1;

        public static string GetUniqueId() {
            return (_lastId++).ToString();
        }

        public static CCLabelTtf GetSmallLabelHint(string text) {
            return new CCLabelTtf(text, "kongtext", 4) {
                Color = CCColor3B.White,
                IsAntialiased = false
            };
        }
    }
}