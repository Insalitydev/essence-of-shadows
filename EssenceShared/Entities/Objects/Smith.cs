using System;
using CocosSharp;
using EssenceShared.Game;

namespace EssenceShared.Entities.Objects {
    public class Smith: Entity {
        public Smith(string id): base(Resources.ObjectSmith, id) {
            Tag = Tags.Object;
        }

        public override void OnEnter() {
            base.OnEnter();

            if (Parent.Tag == Tags.Client){
                var label = new CCLabelTtf("Store", "kongtext", 8) {
                    Color = CCColor3B.White,
                    PositionY = 30,
                    PositionX = -5,
                    IsAntialiased = false
                };

                AddChild(label, 10);
            }

            // adding shop GUI
            CCMenuItem upgradeHp = new CCMenuItemImage(Resources.GuiIncrease, Resources.GuiIncreaseActive,
                Resources.GuiIncreaseInactive, UpgradeHp);
            upgradeHp.AddChild(Util.GetSmallLabelHint("Vitality"));

            CCMenuItem upgradeAttack = new CCMenuItemImage(Resources.GuiIncrease, Resources.GuiIncreaseActive,
                Resources.GuiIncreaseInactive, UpgradeAttack);
            upgradeAttack.AddChild(Util.GetSmallLabelHint("Damage"));

            CCMenuItem upgradeSpeed = new CCMenuItemImage(Resources.GuiIncrease, Resources.GuiIncreaseActive,
                Resources.GuiIncreaseInactive, UpgradeSpeed);
            upgradeSpeed.AddChild(Util.GetSmallLabelHint("Speed"));

            CCMenuItem upgradeClass = new CCMenuItemImage(Resources.GuiIncrease, Resources.GuiIncreaseActive,
                Resources.GuiIncreaseInactive, UpgradeClass);
            upgradeClass.AddChild(Util.GetSmallLabelHint("Class"));

            upgradeSpeed.Enabled = false;

            var upgradeMenu = new CCMenu(upgradeHp, upgradeAttack, upgradeSpeed, upgradeClass) {
                Position = new CCPoint(70, 30),
            };
            upgradeMenu.AlignItemsVertically();

            AddChild(upgradeMenu);
        }

        private void UpgradeHp(object obj) {
            Log.Print("Grading HP");
            EosEvent.RaiseEvent(this, new UpgradeEventArgs(AcccountUpgrade.Hp), EventType.PlayerUpgrade);
        }

        private void UpgradeAttack(object obj) {
            Log.Print("Grading Attack");
            EosEvent.RaiseEvent(this, new UpgradeEventArgs(AcccountUpgrade.Attack), EventType.PlayerUpgrade);
        }

        private void UpgradeSpeed(object obj) {
            Log.Print("Grading Speed");
            EosEvent.RaiseEvent(this, new UpgradeEventArgs(AcccountUpgrade.Speed), EventType.PlayerUpgrade);
        }

        private void UpgradeClass(object obj) {
            Log.Print("Grading Class");
            EosEvent.RaiseEvent(this, new UpgradeEventArgs(AcccountUpgrade.Class), EventType.PlayerUpgrade);
        }
    }
}