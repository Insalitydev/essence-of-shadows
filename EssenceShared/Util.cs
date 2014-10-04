namespace EssenceShared {
    public class Util {
        private static long _lastId = 1;

        public static string GetUniqueId() {
            return (_lastId++).ToString();
        }
    }
}