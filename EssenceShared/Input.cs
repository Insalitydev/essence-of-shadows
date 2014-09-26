using System.Collections.Generic;
using CocosSharp;

namespace EssenceShared {
    public static class Input {
        private static readonly HashSet<CCKeys> Inputs = new HashSet<CCKeys>();

        public static void OnKeyPress(CCKeys key) {
            Inputs.Add(key);
        }

        public static void OnKeyRelease(CCKeys key) {
            Inputs.Remove(key);
        }

        public static bool IsKeyIn(CCKeys key) {
            return Inputs.Contains(key);
        }
    }
}