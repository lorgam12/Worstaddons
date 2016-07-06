namespace KappaUtilityReborn.Main.Trackers
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Rendering;

    using KappaUtilityReborn.Main.Common.Utility;

    using SharpDX;
    using SharpDX.Direct3D9;

    using Font = System.Drawing.Font;
    using Sprite = EloBuddy.SDK.Rendering.Sprite;

    internal class Handler : Base
    {
        public static readonly TextureLoader TextureLoader = new TextureLoader();

        private static Menu HealthTracker { get; }

        public static string TextureName;

        public static Texture LoadTexture
        {
            get
            {
                return TextureLoader[TextureName];
            }
        }

        private static readonly Text DeathText;

        private static readonly Text CDText;

        private static readonly Text hudText;

        public static Dictionary<string, Texture> LoadedTexture;

        public static Dictionary<string, Texture> LoadedSummonerTexture;

        public static Dictionary<string, Texture> LoadedUltsTexture;

        public static Dictionary<string, Sprite> ChampionSprites;

        public static Dictionary<string, Sprite> SummonersSprites;

        public static Dictionary<string, Sprite> UltSprites;

        public static Dictionary<string, int> DeathTimers;

        public static Sprite RightHUD;

        public static Sprite LeftHUD;

        public static Sprite HP;

        public static Sprite MP;

        public static Sprite RightEmpty;

        public static Sprite LeftEmpty;

        public static readonly List<Sprite> sprites = new List<Sprite>();

        static Handler()
        {
            LoadHUD.LoadImages();
            DeathTimers = new Dictionary<string, int>();
            DeathTimers.Clear();

            HealthTracker = Program.MenuIni.AddSubMenu("HealthTracker");

            HealthTracker.AddGroupLabel("HUD");
            HealthTracker.Add("ally", new CheckBox("Draw Ally HUD"));
            HealthTracker.Add("enemy", new CheckBox("Draw Enemy HUD"));
            HealthTracker.Add("RX", new Slider("Right Side X", 170, -200, 200));
            HealthTracker.Add("RY", new Slider("Right Side Y", 10, -200, 200));
            HealthTracker.Add("LX", new Slider("Left Side X", 0, -200, 200));
            HealthTracker.Add("LY", new Slider("Left Side Y", 10, -200, 200));
            HealthTracker.Add("space", new Slider("Spacing Between Huds", 11, 0, 30));
            /*
            HealthTracker.AddSeparator(0);
            HealthTracker.AddGroupLabel("Buildings");
            HealthTracker.Add("World", new ComboBox("World Health Drawings", 1, "Percent", "Current Health"));
            HealthTracker.Add("Minimap", new ComboBox("MiniMap Health Drawings", 0, "Percent", "Current Health"));
            HealthTracker.Add("team", new ComboBox("Tracking Team", 0, "Both", "Enemy", "Ally"));
            HealthTracker.Add("obj_AI_Turret", new CheckBox("Track Turrets Health"));
            HealthTracker.Add("obj_HQ", new CheckBox("Track Nexues Health"));
            HealthTracker.Add("obj_BarracksDampener", new CheckBox("Track Inhibitors Health"));
            */
            DeathText = new Text(string.Empty, new Font("Tahoma", 18, FontStyle.Bold)) { Color = System.Drawing.Color.White };
            CDText = new Text(string.Empty, new Font("Tahoma", 9, FontStyle.Bold)) { Color = System.Drawing.Color.White };
            hudText = new Text(string.Empty, new Font("Tahoma", 14, FontStyle.Bold)) { Color = System.Drawing.Color.White };

            foreach (var sprite in sprites)
            {
                sprite.Scale = new Vector2(1, 1);
            }

            Obj_AI_Base.OnPlayAnimation += delegate(Obj_AI_Base sender, GameObjectPlayAnimationEventArgs args)
                {
                    var hero = sender as AIHeroClient;
                    if (hero == null)
                    {
                        return;
                    }

                    if (args.Animation.ToLower().Contains("death"))
                    {
                        DeathTimers.Add(sender.Name, (int)(hero.DeathTimer() + Game.Time));
                    }
                };
        }

        public override void OnTick()
        {
            foreach (var hero in EntityManager.Heroes.AllHeroes.Where(h => !h.IsDead).Where(hero => DeathTimers.ContainsKey(hero.Name)))
            {
                DeathTimers.Remove(hero.Name);
            }
        }

        private static void Health(AIHeroClient hero, Vector2 vector2)
        {
            HP.Scale = new Vector2(1 * (hero.HealthPercent / 100), 1);
            HP.Draw(vector2);
        }

        private static void Mana(AIHeroClient hero, Vector2 vector2)
        {
            MP.Scale = new Vector2(1 * (hero.ManaPercent / 100), 1);
            MP.Draw(vector2);
        }

        public override void Draw()
        {
            try
            {
                var ia = 0;
                var ie = 0;
                var lefty = Camera.ScreenPosition.Y + HealthTracker["LY"].Cast<Slider>().CurrentValue;
                var leftx = (Camera.ScreenPosition.X + HealthTracker["LX"].Cast<Slider>().CurrentValue) * 10;
                var righty = Camera.ScreenPosition.Y + HealthTracker["RY"].Cast<Slider>().CurrentValue;
                var rightx = (Camera.ScreenPosition.X + 10 + HealthTracker["RX"].Cast<Slider>().CurrentValue) * 10;
                foreach (var hero in
                    EntityManager.Heroes.AllHeroes.Where(a => (a.IsAlly && HealthTracker["ally"].Cast<CheckBox>().CurrentValue) || (a.IsEnemy && HealthTracker["enemy"].Cast<CheckBox>().CurrentValue)))
                {
                    var spell1 = hero.SlotToSpell(SpellSlot.Summoner1);
                    var spell2 = hero.SlotToSpell(SpellSlot.Summoner2);
                    var i = hero.Team == GameObjectTeam.Chaos ? ia : ie;
                    var righthud = new Vector2(rightx, (righty + i) * 10);
                    var lefthud = new Vector2(leftx, (lefty + i) * 10);
                    var hero1 = hero;
                    if (hero.Team == GameObjectTeam.Chaos)
                    {
                        RightEmpty.Draw(righthud);
                        Health(hero, new Vector2(rightx + 17, ((righty + i) * 10) + 79));
                        Mana(hero, new Vector2(rightx + 17, ((righty + i) * 10) + 90));

                        foreach (var sprite in ChampionSprites)
                        {
                            if (hero.IsDead || !hero.IsHPBarRendered)
                            {
                                if (sprite.Key.Equals(hero.Name + "Dead"))
                                {
                                    sprite.Value.Draw(new Vector2(rightx + 11, ((righty + i) * 10) + 4));
                                    if (hero.IsDead)
                                    {
                                        if (DeathTimers.FirstOrDefault(a => a.Key == hero.Name).Value - Game.Time > 0)
                                        {
                                            var time = (int)(DeathTimers.FirstOrDefault(a => a.Key == hero.Name).Value - Game.Time);
                                            DeathText.Position = new Vector2(rightx + 30, ((righty + i) * 10) + 22);
                                            DeathText.TextValue = time.ToString();
                                            DeathText.Draw();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (sprite.Key.Equals(hero.Name))
                                {
                                    sprite.Value.Draw(new Vector2(rightx + 13, ((righty + i) * 10) + 6));
                                }
                            }
                        }

                        foreach (var sprite in SummonersSprites)
                        {
                            if (sprite.Key.Contains(hero1.Name + 1))
                            {
                                if (spell1.Timer() > 0)
                                {
                                    if (sprite.Key.Equals(hero1.Name + 1 + "NR"))
                                    {
                                        sprite.Value.Draw(new Vector2(rightx + 80, ((righty + i) * 10) + 5));
                                        CDText.Position = new Vector2(rightx + 84, ((righty + i) * 10) + 15);
                                        CDText.TextValue = spell1.CoolDown();
                                        CDText.Draw();
                                    }
                                }
                                else if (sprite.Key.Equals(hero1.Name + 1 + "R"))
                                {
                                    sprite.Value.Draw(new Vector2(rightx + 82, ((righty + i) * 10) + 7));
                                }
                            }

                            if (sprite.Key.Contains(hero1.Name + 2))
                            {
                                if (spell2.Timer() > 0)
                                {
                                    if (sprite.Key.Equals(hero1.Name + 2 + "NR"))
                                    {
                                        sprite.Value.Draw(new Vector2(rightx + 79, ((righty + i) * 10) + 39));
                                        CDText.Position = new Vector2(rightx + 84, ((righty + i) * 10) + 50);
                                        CDText.TextValue = spell2.CoolDown();
                                        CDText.Draw();
                                    }
                                }
                                else if (sprite.Key.Equals(hero1.Name + 2 + "R"))
                                {
                                    sprite.Value.Draw(new Vector2(rightx + 82, ((righty + i) * 10) + 40));
                                }
                            }
                        }

                        foreach (var sprite in UltSprites)
                        {
                            if (hero.SlotToSpell(SpellSlot.R).Timer() > 0 || !hero.SlotToSpell(SpellSlot.R).IsLearned)
                            {
                                if (sprite.Key.Equals(hero.Name + "NR"))
                                {
                                    sprite.Value.Draw(new Vector2(rightx, ((righty + i) * 10) + 1));
                                    if (hero.SlotToSpell(SpellSlot.R).IsLearned)
                                    {
                                        CDText.Position = new Vector2(rightx + 4, ((righty + i) * 10) + 10);
                                        CDText.TextValue = hero.SlotToSpell(SpellSlot.R).CoolDown();
                                        CDText.Draw();
                                    }
                                }
                            }
                            else if (sprite.Key.Equals(hero1.Name))
                            {
                                sprite.Value.Draw(new Vector2(rightx, ((righty + i) * 10) + 1));
                            }
                        }

                        RightHUD.Draw(righthud);
                        hudText.Position = new Vector2(rightx + 20, ((righty + i) * 10) + 45);
                        hudText.TextValue = hero.Level.ToString();
                        hudText.Draw();
                        ia += HealthTracker["space"].Cast<Slider>().CurrentValue;
                    }
                    else
                    {
                        LeftEmpty.Draw(lefthud);
                        Health(hero, new Vector2(leftx + 9, ((lefty + i) * 10) + 79));
                        Mana(hero, new Vector2(leftx + 9, ((lefty + i) * 10) + 90));
                        foreach (var sprite in ChampionSprites)
                        {
                            if (hero.IsDead || !hero.IsHPBarRendered)
                            {
                                if (sprite.Key.Equals(hero.Name + "Dead"))
                                {
                                    sprite.Value.Draw(new Vector2(leftx + 40, ((lefty + i) * 10) + 4));
                                    if (hero.IsDead)
                                    {
                                        if (DeathTimers.FirstOrDefault(a => a.Key == hero.Name).Value - Game.Time > 0)
                                        {
                                            var time = (int)(DeathTimers.FirstOrDefault(a => a.Key == hero.Name).Value - Game.Time);
                                            DeathText.Position = new Vector2(leftx + 60, ((lefty + i) * 10) + 25);
                                            DeathText.TextValue = time.ToString();
                                            DeathText.Draw();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (sprite.Key.Equals(hero.Name))
                                {
                                    sprite.Value.Draw(new Vector2(leftx + 43, ((lefty + i) * 10) + 6));
                                }
                            }
                        }

                        foreach (var sprite in SummonersSprites)
                        {
                            if (sprite.Key.Contains(hero1.Name + 1))
                            {
                                if (spell1.Timer() > 0)
                                {
                                    if (sprite.Key.Equals(hero1.Name + 1 + "NR"))
                                    {
                                        sprite.Value.Draw(new Vector2(leftx + 5, ((lefty + i) * 10) + 5));
                                        CDText.Position = new Vector2(leftx + 10, ((lefty + i) * 10) + 15);
                                        CDText.TextValue = spell1.CoolDown();
                                        CDText.Draw();
                                    }
                                }
                                else if (sprite.Key.Equals(hero1.Name + 1 + "R"))
                                {
                                    sprite.Value.Draw(new Vector2(leftx + 6, ((lefty + i) * 10) + 6));
                                }
                            }

                            if (sprite.Key.Contains(hero1.Name + 2))
                            {
                                if (spell2.Timer() > 0)
                                {
                                    if (sprite.Key.Equals(hero1.Name + 2 + "NR"))
                                    {
                                        sprite.Value.Draw(new Vector2(leftx + 5, ((lefty + i) * 10) + 38));
                                        CDText.Position = new Vector2(leftx + 10, ((lefty + i) * 10) + 48);
                                        CDText.TextValue = spell2.CoolDown();
                                        CDText.Draw();
                                    }
                                }
                                else if (sprite.Key.Equals(hero1.Name + 2 + "R"))
                                {
                                    sprite.Value.Draw(new Vector2(leftx + 6, ((lefty + i) * 10) + 40));
                                }
                            }
                        }

                        foreach (var sprite in UltSprites)
                        {
                            if (hero.SlotToSpell(SpellSlot.R).Timer() > 0 || !hero.SlotToSpell(SpellSlot.R).IsLearned)
                            {
                                if (sprite.Key.Equals(hero.Name + "NR"))
                                {
                                    sprite.Value.Draw(new Vector2(leftx + 86, ((lefty + i) * 10) + 1));
                                    if (hero.SlotToSpell(SpellSlot.R).IsLearned)
                                    {
                                        CDText.Position = new Vector2(leftx + 91, ((lefty + i) * 10) + 10);
                                        CDText.TextValue = hero.SlotToSpell(SpellSlot.R).CoolDown();
                                        CDText.Draw();
                                    }
                                }
                            }
                            else if (sprite.Key.Equals(hero1.Name))
                            {
                                sprite.Value.Draw(new Vector2(leftx + 86, ((lefty + i) * 10) + 1));
                            }
                        }

                        LeftHUD.Draw(lefthud);
                        hudText.Position = new Vector2(leftx + 80, ((lefty + i) * 10) + 45);
                        hudText.TextValue = hero.Level.ToString();
                        hudText.Draw();
                        ie += HealthTracker["space"].Cast<Slider>().CurrentValue;
                    }
                }

                /*
                var AX = HealthTracker["AX"].Cast<Slider>().CurrentValue * 10;
                var AY = HealthTracker["AY"].Cast<Slider>().CurrentValue * 10;
                var EX = HealthTracker["EX"].Cast<Slider>().CurrentValue * 10;
                var EY = HealthTracker["EY"].Cast<Slider>().CurrentValue * 10;
                float iq = 0;
                const float ic = 0;
                var objpos = new Vector2();
                var objmini = new Vector2();

                foreach (
                    var obj in
                        ObjectsManager.AllObjects.OrderBy(o => o.Distance(ObjectsManager.Fturret))
                            .Where(obj => obj.Team(HealthTracker["team"].Cast<ComboBox>().CurrentValue) && HealthTracker[obj.Type.ToString()].Cast<CheckBox>().CurrentValue && obj.Health() > 0))
                {
                    objpos = obj.Position.WorldToScreen();
                    objmini = obj.Position.WorldToMinimap();

                    // World Drawings
                    WorldText.TextValue = obj.Health(HealthTracker.DrawPercent("World")).ToString();
                    WorldText.Position = new Vector2(objpos.X - 20 + iq, objpos.Y - 80);
                    WorldText.Draw();

                    // MiniMap Drawings
                    MinimapText.TextValue = obj.Health(HealthTracker.DrawPercent("Minimap")).ToString();
                    MinimapText.Position = new Vector2(objmini.X - iq, objmini.Y - iq);
                    MinimapText.Draw();
                    iq += 0.2f;
                }
                /*
                Team.TextValue = "Enemy";
                Team.Position = new Vector2(Camera.ScreenPosition.X + EX, Camera.ScreenPosition.Y + EY + ic - 20f);
                Team.Draw();

                Team.TextValue = "Ally";
                Team.Position = new Vector2(Camera.ScreenPosition.X + AX, Camera.ScreenPosition.Y + AY - 20f);
                Team.Draw();
                */
            }
            catch (Exception ex)
            {
                Common.Logger.Error("Please report this to Definitely not Kappa.");
                Common.Logger.Error("There was an issue while trying to Draw in Trackers.Handler.");
                Common.Logger.Error(ex.ToString());
            }
        }
    }
}
