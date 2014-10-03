using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EssenceShared {
    public class Util {
        private static long _lastId = 1;

        public static string GetUniqueId() {
            return (_lastId++).ToString();
        }
    }
}
