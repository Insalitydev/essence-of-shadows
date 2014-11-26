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

            if (isShowTime) {
                curTime = DateTime.Now.ToString("[HH:mm:ss]: ");
            }

            Console.ForegroundColor = ConsoleColor.White;
            switch (type) {
                case LogType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogType.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogType.Network:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
            }


            Console.WriteLine(curTime + text);

            if (Settings.IsLogToFile) {
                PrintToLogFile(text);
            }
        }

        // TODO: заменить на один метод с параметрами по умолчанию...
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