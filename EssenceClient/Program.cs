using System;
using CocosSharp;
using EssenceShared;
using OpenTK.Graphics.OpenGL;

namespace EssenceClient {
    internal class Program {
        private static void Main(string[] args) {
            Log.Print("Starting Essence Client");

            var application = new CCApplication(false, new CCSize(Settings.SCREEN_WIDTH, Settings.SCREEN_HEIGHT));
            application.ApplicationDelegate = new Client();
            application.StartGame();
        }
    }
}