using System.Collections.Generic;
using CocosSharp;

namespace EssenceShared {
    public static class Input {
        private static readonly HashSet<CCKeys> Inputs = new HashSet<CCKeys>();
        private static readonly HashSet<CCMouseButton> InputsMouse = new HashSet<CCMouseButton>();

        public static void OnKeyPress(CCKeys key) {
            Inputs.Add(key);
        }

        public static void OnKeyRelease(CCKeys key) {
            Inputs.Remove(key);
        }

        public static void OnMousePress(CCMouseButton mouse) {
            InputsMouse.Add(mouse);
        }

        public static void OnMouseRelease(CCMouseButton mouse) {
            InputsMouse.Remove(mouse);
        }

        public static bool IsKeyIn(CCKeys key) {
            return Inputs.Contains(key);
        }

        public static bool IsMousePressed(CCMouseButton mouse) {
            return InputsMouse.Contains(mouse);
        }
    }
}