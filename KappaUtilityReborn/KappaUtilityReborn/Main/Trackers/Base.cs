namespace KappaUtilityReborn.Main.Trackers
{
    using System;
    using System.Drawing;

    using EloBuddy;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using KappaUtilityReborn.Main.Common.Utility;

    public abstract class Base : Main.Base
    {
        private static Menu Tracker;

        public Menu TrackMenu;

        public static Text WorldText;

        public static Text MinimapText;

        public static Text Team;

        public override void Initialize()
        {
            Tracker = Program.MenuIni.AddSubMenu("Trackers");
            Tracker.AddGroupLabel("World Drawings");
            var world = Tracker.Add("textsize", new Slider("(World) Text Size", 10, 1, 20));
            world.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args) { WorldText = new Text(string.Empty, new Font("Tahoma", args.NewValue, FontStyle.Bold)); };
            WorldText = new Text(string.Empty, new Font("Tahoma", world.CurrentValue, FontStyle.Bold)) { Color = Color.White };

            Tracker.AddSeparator(0);
            Tracker.AddGroupLabel("Minimap Drawings");
            var minimap = Tracker.Add("textsizemini", new Slider("(Minimap) Text Size", 7, 1, 15));
            minimap.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args) { MinimapText = new Text(string.Empty, new Font("Tahoma", args.NewValue)); };
            MinimapText = new Text(string.Empty, new Font("Tahoma", minimap.CurrentValue)) { Color = Color.White };

            foreach (var turret in ObjectsManager.AllTurrets)
            {
                ObjectsManager.AllObjects.Add(turret);
            }

            foreach (var inhb in ObjectsManager.AllInhb)
            {
                ObjectsManager.AllObjects.Add(inhb);
            }

            foreach (var nexues in ObjectsManager.AllNexues)
            {
                ObjectsManager.AllObjects.Add(nexues);
            }

            GameObject.OnCreate += delegate(GameObject sender, EventArgs args)
                {
                    if (!ObjectsManager.AllObjects.Contains(sender))
                    {
                        ObjectsManager.AllObjects.Add(sender);
                    }
                };

            GameObject.OnDelete += delegate(GameObject sender, EventArgs args)
                {
                    if (ObjectsManager.AllObjects.Contains(sender))
                    {
                        ObjectsManager.AllObjects.Remove(sender);
                    }
                };
        }
    }
}
