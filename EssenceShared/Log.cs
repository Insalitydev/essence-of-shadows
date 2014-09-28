using System;

namespace EssenceShared {
    public enum LogType {
        Debug,
        Network,
        Info,
        Error
    }

    public class Log {
        public static void Print(string text, LogType type, bool isShowTime) {
            string curTime = "";

            if (isShowTime){
                curTime = DateTime.Now.ToString("[HH:mm:ss]: ");
            }

            Console.WriteLine(curTime + text);

            if (Settings.IsLogToFile){
                PrintToLogFile(text);
            }
        }

        public static void Print(string text, LogType type) {
            Print(text, type, true);
        }

        public static void Print(string text, bool isShowTime) {
            Print(text, LogType.Debug, isShowTime);
        }

        public static void Print(string text) {
            Print(text, LogType.Debug, true);
        }


        /** Logging to text file */

        private static void PrintToLogFile(string text) {
            throw new NotImplementedException();
        }
    }
}