namespace KappaUtility.Misc
{
    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using SharpDX;

    internal class AutoReveal
    {
        public static readonly Item Sweeping_Lens_Trinket = new Item(ItemId.Sweeping_Lens_Trinket, 550f);

        public static readonly Item Oracle_Alteration = new Item(ItemId.Oracle_Alteration, 550f);

        public static readonly Item Vision_Ward = new Item(ItemId.Vision_Ward, 550f);

        public static readonly Item Warding_Totem_Trinket = new Item(ItemId.Warding_Totem_Trinket, 550f);

        public static readonly Item Greater_Stealth_Totem_Trinket = new Item(ItemId.Greater_Stealth_Totem_Trinket, 550f);

        public static readonly Item Greater_Vision_Totem_Trinket = new Item(ItemId.Greater_Vision_Totem_Trinket, 550f);

        public static readonly Item Sightstone = new Item((int)ItemId.Sightstone, 550f);

        public static readonly Item Ruby_Sightstone = new Item((int)ItemId.Ruby_Sightstone, 550f);

        public static readonly Item Farsight_Alteration = new Item((int)ItemId.Farsight_Alteration, 1250);

        internal class AkaliSmoke
        {
            public GameObject Object { get; set; }

            public float NetworkId { get; set; }

            public Vector3 SmokePos { get; set; }

            public double ExpireTime { get; set; }
        }

        internal class Shaco
        {
            public GameObject Object { get; set; }

            public float NetworkId { get; set; }

            public Vector3 SmokePos { get; set; }

            public double ExpireTime { get; set; }
        }
        private static readonly AkaliSmoke Akalismoke = new AkaliSmoke();

        private static readonly Shaco shaco = new Shaco();

        public static int LastTickTime;

        public static Menu BushMenu { get; private set; }

        private static int lastWard;

        private static bool DontWard, Changing = false;

        internal static void OnLoad()
        {
            BushMenu = Load.UtliMenu.AddSubMenu("Auto Revealer");
            BushMenu.AddGroupLabel("Auto Bush Reveal Settings");
            BushMenu.Add("enable", new CheckBox("Enable", false));
            BushMenu.Add("combo", new CheckBox("Only On Combo", false));
            BushMenu.AddSeparator();
            BushMenu.AddGroupLabel("Auto Stealth Reveal Settings");
            BushMenu.Add("enables", new CheckBox("Enable", false));
            BushMenu.Add("combos", new CheckBox("Only On Combo", false));
            
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var targets = EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.Distance(Player.Instance) < 1250);
            foreach (var target in targets)
            {
                if (target.BaseSkinName == "Shaco")
                {
                    if (shaco.Object != null)
                    {
                        Drawing.DrawText(
                            Drawing.WorldToScreen(target.Path.LastOrDefault()) - new Vector2(30, -30),
                            System.Drawing.Color.White,
                            "Shaco Here",
                            12);
                        Circle.Draw(Color.White, 400, target.Path.LastOrDefault());
                    }
                }

                if (target.BaseSkinName == "Akali")
                {
                    if (Akalismoke.Object != null)
                    {
                        Circle.Draw(Color.White, 400, target.Path.LastOrDefault());
                        Drawing.DrawText(
                            Drawing.WorldToScreen(target.Path.LastOrDefault()) - new Vector2(30, -30),
                            System.Drawing.Color.White,
                            "Akali Here",
                            2);
                    }
                }
            }
        }

        private static void GameObject_OnCreate(GameObject obj, EventArgs args)
        {
            if (obj != null && obj.Name.Contains("Akali_Base_smoke_bomb_tar"))
            {
                Akalismoke.Object = obj;
                Akalismoke.ExpireTime = Game.Time + 8;
                Akalismoke.NetworkId = obj.NetworkId;
                Akalismoke.SmokePos = obj.Position;
            }

            if (obj != null && obj.Name.Contains("JackintheboxPoof2"))
            {
                shaco.Object = obj;
                shaco.ExpireTime = Game.Time + 8;
                shaco.NetworkId = obj.NetworkId;
                shaco.SmokePos = obj.Position;
            }

            if (obj != null)
            {
            }
        }

        private static void GameObject_OnDelete(GameObject obj, EventArgs args)
        {
            if (obj != null && obj.Name.Contains("Akali_Base_smoke_bomb_tar"))
            {
                Akalismoke.Object = null;
                LastTickTime = 0;
            }

            if (obj != null && obj.Name.Contains("JackintheboxPoof2"))
            {
                shaco.Object = null;
                LastTickTime = 0;
            }

            if (obj != null)
            {
            }
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            var enemies = EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.Distance(Player.Instance) < 1250);
            if (BushMenu["enable"].Cast<CheckBox>().CurrentValue && !Player.Instance.IsDead)
            {
                var flags = Orbwalker.ActiveModesFlags;
                foreach (var target in enemies)
                {
                    if (!BushMenu["combo"].Cast<CheckBox>().CurrentValue)
                    {
                        if ((NavMesh.IsWallOfGrass(target.Path.LastOrDefault(), 1)
                                && target.Distance(target.Path.LastOrDefault()) < 200)
                               && (!NavMesh.IsWallOfGrass(Player.Instance.Position, 1)
                                   && Player.Instance.Distance(target.Path.LastOrDefault()) < 500))
                        {
                            var target1 = target;
                            var wards =
                                ObjectManager.Get<Obj_AI_Minion>()
                                    .Where(
                                        x =>
                                        x.Name.Contains("Ward") && x.IsAlly
                                        && x.Position.Distance(target1.Position) < 750);

                            foreach (var ward in wards)
                            {
                                if (NavMesh.IsWallOfGrass(ward.Position, 50) || target.IsHPBarRendered)
                                {
                                    DontWard = true;
                                }
                            }


                            if (DontWard == false)
                            {
                                WardCast();
                            }

                            if (DontWard && Changing == false)
                            {
                                Changing = true;
                                Core.DelayAction(
                                    delegate
                                        {
                                            DontWard = false;
                                            Changing = false;
                                        },
                                    500);
                            }
                        }
                    }

                    if (BushMenu["combo"].Cast<CheckBox>().CurrentValue && flags.HasFlag(Orbwalker.ActiveModes.Combo))
                    {
                        if ((NavMesh.IsWallOfGrass(target.Path.LastOrDefault(), 1)
                                && target.Distance(target.Path.LastOrDefault()) < 200)
                               && (!NavMesh.IsWallOfGrass(Player.Instance.Position, 1)
                                   && Player.Instance.Distance(target.Path.LastOrDefault()) < 500))
                        {
                            var target1 = target;
                            var wards =
                                ObjectManager.Get<Obj_AI_Minion>()
                                    .Where(
                                        x =>
                                        x.Name.Contains("Ward") && x.IsAlly
                                        && x.Position.Distance(target1.Position) < 750);

                            foreach (var ward in wards)
                            {
                                if (NavMesh.IsWallOfGrass(ward.Position, 50))
                                {
                                    DontWard = true;
                                }
                            }


                            if (DontWard == false)
                            {
                                WardCast();
                            }

                            if (DontWard && Changing == false)
                            {
                                Changing = true;
                                Core.DelayAction(
                                    delegate
                                    {
                                        DontWard = false;
                                        Changing = false;
                                    },
                                    500);
                            }
                        }
                    }
                }
            }
        }

        public static void WardCast()
        {
            var enemies = EntityManager.Heroes.Enemies.Where(x => !x.IsDead && x.Distance(Player.Instance) < 1250);

            foreach (var target in enemies)
            {
                if (Warding_Totem_Trinket.IsOwned() && Warding_Totem_Trinket.IsReady())
                {
                    Warding_Totem_Trinket.Cast(target.Path.LastOrDefault());
                    return;
                }

                if (Greater_Stealth_Totem_Trinket.IsOwned() && Greater_Stealth_Totem_Trinket.IsReady())
                {
                    Greater_Stealth_Totem_Trinket.Cast(target.Path.LastOrDefault());
                    return;
                }

                if (Greater_Vision_Totem_Trinket.IsOwned() && Greater_Vision_Totem_Trinket.IsReady())
                {
                    Greater_Vision_Totem_Trinket.Cast(target.Path.LastOrDefault());
                    return;
                }

                if (Sightstone.IsOwned() && Sightstone.IsReady())
                {
                    Sightstone.Cast(target.Path.LastOrDefault());
                    return;
                }

                if (Ruby_Sightstone.IsOwned() && Ruby_Sightstone.IsReady())
                {
                    Ruby_Sightstone.Cast(target.Path.LastOrDefault());
                    return;
                }

                if (Farsight_Alteration.IsOwned() && Farsight_Alteration.IsReady())
                {
                    Farsight_Alteration.Cast(target.Path.LastOrDefault());
                    return;
                }
            }
        }
    }
}