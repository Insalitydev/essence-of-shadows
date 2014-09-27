using System.Collections.Generic;
using System.Linq;
using CocosSharp;
using EssenceShared;

namespace EssenceClient.Scenes.Game {
    internal class ChatLayer: CCLayerColor {
        public List<string> messages;
        private CCLabel _label;

        public ChatLayer() {
            Layer.Scale = 0.3f;
            messages = new List<string>();

            AnchorPoint = CCPoint.AnchorLowerLeft;
            PositionX = 200;
            PositionY = 0;

            Log.Print("cht init");
        }

        protected override void AddedToScene() {
            base.AddedToScene();

            _label = new CCLabel("Chat", "kongtext", 28);
            _label.Color = CCColor3B.White;
            _label.AnchorPoint = CCPoint.AnchorLowerLeft;
            _label.Position = Layer.Position;

            AddChild(_label);

            Schedule(Update);
        }

        public override void Update(float dt) {
            base.Update(dt);

            if (messages.Count > 0)
                _label.Text = "Message: " + messages.Last();
        }
    }
}