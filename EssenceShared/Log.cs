using System;

namespace EssenceShared {
    public enum LogType {
        DEBUG,
        NETWORK,
        INFO
    }

    public class Log {
        public static void Print(string text, LogType type) {
            string curTime = DateTime.Now.ToString("[HH:mm:ss]: ");

            Console.WriteLine(curTime + text);

            if (Settings.IS_LOG_TO_FILE){
                PrintToLogFile(text);
            }
        }

        public static void Print(string text) {
            Print(text, LogType.DEBUG);
        }


        /** Logging to text file */

        private static void PrintToLogFile(string text) {
            throw new NotImplementedException();
        }
    }
}