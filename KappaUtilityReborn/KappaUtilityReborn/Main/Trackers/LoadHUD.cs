namespace KappaUtilityReborn.Main.Trackers
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;

    using SharpDX.Direct3D9;

    using Sprite = EloBuddy.SDK.Rendering.Sprite;

    internal class LoadHUD
    {
        public static void LoadImages()
        {
            Handler.SummonersSprites = new Dictionary<string, Sprite>();
            Handler.UltSprites = new Dictionary<string, Sprite>();
            Handler.ChampionSprites = new Dictionary<string, Sprite>();
            Handler.LoadedTexture = new Dictionary<string, Texture>();
            Handler.LoadedSummonerTexture = new Dictionary<string, Texture>();
            Handler.LoadedUltsTexture = new Dictionary<string, Texture>();
            Handler.UltSprites.Clear();
            Handler.ChampionSprites.Clear();
            Handler.LoadedTexture.Clear();
            Handler.LoadedUltsTexture.Clear();
            Handler.LoadedSummonerTexture.Clear();
            Handler.sprites.Clear();

            LoadHudImgs();
            LoadChampionImgs();
            LoadSpellsImgs();
            LoadSummonerSpellsImgs();
        }

        private static void LoadHudImgs()
        {
            Handler.TextureLoader.Load("RightHUD", Properties.Resources.RightHUD);
            Handler.RightHUD = new Sprite(() => Handler.TextureLoader["RightHUD"]);

            Handler.TextureLoader.Load("LeftHUD", Properties.Resources.LeftHud);
            Handler.LeftHUD = new Sprite(() => Handler.TextureLoader["LeftHUD"]);

            Handler.TextureLoader.Load("RightEmpty", Properties.Resources.RightEmpty);
            Handler.RightEmpty = new Sprite(() => Handler.TextureLoader["RightEmpty"]);

            Handler.TextureLoader.Load("LeftEmpty", Properties.Resources.LeftEmpty);
            Handler.LeftEmpty = new Sprite(() => Handler.TextureLoader["LeftEmpty"]);

            Handler.TextureLoader.Load("HP", Properties.Resources.HP);
            Handler.HP = new Sprite(() => Handler.TextureLoader["HP"]);

            Handler.TextureLoader.Load("MP", Properties.Resources.MP);
            Handler.MP = new Sprite(() => Handler.TextureLoader["MP"]);

            Handler.sprites.Add(Handler.RightEmpty);
            Handler.sprites.Add(Handler.LeftEmpty);
            Handler.sprites.Add(Handler.LeftHUD);
            Handler.sprites.Add(Handler.RightHUD);
            Handler.sprites.Add(Handler.HP);
            Handler.sprites.Add(Handler.MP);
        }

        private static void LoadChampionImgs()
        {
            foreach (var hero in EntityManager.Heroes.AllHeroes)
            {
                Handler.TextureLoader.Load(ReColor(Image.FromFile(Program.ChampionIcons + hero.ChampionName + "\\" + hero.ChampionName + ".png") as Bitmap, 69), out Handler.TextureName);
                if (Handler.LoadedTexture.All(t => t.Key != hero.Name + "Dead"))
                {
                    Handler.LoadedTexture.Add(hero.Name + "Dead", Handler.LoadTexture);
                }

                Handler.TextureLoader.Load(ResizeImage(Image.FromFile(Program.ChampionIcons + hero.ChampionName + "\\" + hero.ChampionName + ".png"), 64, 64), out Handler.TextureName);
                if (Handler.LoadedTexture.All(t => t.Key != hero.Name))
                {
                    Handler.LoadedTexture.Add(hero.Name, Handler.LoadTexture);
                }
            }

            foreach (var hero in Handler.LoadedTexture)
            {
                var hero1 = hero;
                if (Handler.ChampionSprites.All(s => s.Key != hero.Key + "Dead"))
                {
                    Handler.ChampionSprites.Add(hero.Key + "Dead", new Sprite(() => hero1.Value));
                }

                if (Handler.ChampionSprites.All(s => s.Key != hero.Key))
                {
                    Handler.ChampionSprites.Add(hero.Key, new Sprite(() => hero1.Value));
                }
            }

            foreach (var sprite in Handler.ChampionSprites)
            {
                Handler.sprites.Add(sprite.Value);
            }
        }

        private static void LoadSummonerSpellsImgs()
        {
            foreach (var hero in EntityManager.Heroes.AllHeroes)
            {
                foreach (var sum in hero.Spellbook.Spells)
                {
                    if (sum.Slot == SpellSlot.Summoner1)
                    {
                        Handler.TextureLoader.Load(ReColor(Image.FromFile(Program.SummonerSpellsIcons + sum.Name + ".png") as Bitmap, 38), out Handler.TextureName);
                        if (Handler.LoadedSummonerTexture.All(t => t.Key != hero.Name + 1 + "NR"))
                        {
                            Handler.LoadedSummonerTexture.Add(hero.Name + 1 + "NR", Handler.LoadTexture);
                        }

                        Handler.TextureLoader.Load(ResizeImage(Image.FromFile(Program.SummonerSpellsIcons + sum.Name + ".png"), 32, 32), out Handler.TextureName);
                        if (Handler.LoadedSummonerTexture.All(t => t.Key != hero.Name + 1 + "R"))
                        {
                            Handler.LoadedSummonerTexture.Add(hero.Name + 1 + "R", Handler.LoadTexture);
                        }
                    }

                    if (sum.Slot == SpellSlot.Summoner2)
                    {
                        Handler.TextureLoader.Load(ReColor(Image.FromFile(Program.SummonerSpellsIcons + sum.Name + ".png") as Bitmap, 38), out Handler.TextureName);
                        if (Handler.LoadedSummonerTexture.All(t => t.Key != hero.Name + 2 + "NR"))
                        {
                            Handler.LoadedSummonerTexture.Add(hero.Name + 2 + "NR", Handler.LoadTexture);
                        }

                        Handler.TextureLoader.Load(ResizeImage(Image.FromFile(Program.SummonerSpellsIcons + sum.Name + ".png"), 32, 32), out Handler.TextureName);
                        if (Handler.LoadedSummonerTexture.All(t => t.Key != hero.Name + 2 + "R"))
                        {
                            Handler.LoadedSummonerTexture.Add(hero.Name + 2 + "R", Handler.LoadTexture);
                        }
                    }
                }
            }

            foreach (var summ in Handler.LoadedSummonerTexture)
            {
                var summ1 = summ;
                Handler.SummonersSprites.Add(summ.Key, new Sprite(() => summ1.Value));
            }

            foreach (var sprite in Handler.SummonersSprites)
            {
                Handler.sprites.Add(sprite.Value);
            }
        }

        private static void LoadSpellsImgs()
        {
            foreach (var hero in EntityManager.Heroes.AllHeroes)
            {
                foreach (var spell in hero.Spellbook.Spells.Where(s => s.Slot == SpellSlot.R))
                {
                    Handler.TextureLoader.Load(
                        cropAtRect(Image.FromFile(Program.ChampionIcons + hero.ChampionName + "\\" + hero.ChampionName + spell.Slot + ".png") as Bitmap, 38, true), 
                        out Handler.TextureName);
                    if (Handler.LoadedUltsTexture.All(t => t.Value != Handler.LoadTexture && t.Key != hero.Name + "NR"))
                    {
                        Handler.LoadedUltsTexture.Add(hero.Name + "NR", Handler.LoadTexture);
                    }

                    Handler.TextureLoader.Load(
                        cropAtRect(ResizeImage(Image.FromFile(Program.ChampionIcons + hero.ChampionName + "\\" + hero.ChampionName + spell.Slot + ".png"), 90, 90), 36), 
                        out Handler.TextureName);
                    if (Handler.LoadedUltsTexture.All(t => t.Value != Handler.LoadTexture && t.Key != hero.Name))
                    {
                        Handler.LoadedUltsTexture.Add(hero.Name, Handler.LoadTexture);
                    }
                }
            }

            foreach (var spell in Handler.LoadedUltsTexture.Where(texture => Handler.UltSprites.All(s => s.Key != texture.Key)))
            {
                var spell1 = spell;
                if (Handler.UltSprites.All(s => s.Key != spell.Key + "NR"))
                {
                    Handler.UltSprites.Add(spell.Key + "NR", new Sprite(() => spell1.Value));
                }

                if (Handler.UltSprites.All(s => s.Key != spell.Key))
                {
                    Handler.UltSprites.Add(spell.Key, new Sprite(() => spell1.Value));
                }
            }

            foreach (var sprite in Handler.UltSprites)
            {
                Handler.sprites.Add(sprite.Value);
            }
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Bitmap cropAtRect(Bitmap bi, int size, bool c = false)
        {
            var btm = new Bitmap(bi.Width + 4, bi.Height + 4);
            var btm2 = new Bitmap(size + 5, size + 5);

            if (c)
            {
                using (var grf = Graphics.FromImage(bi))
                {
                    using (Brush brsh = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
                    {
                        grf.FillRectangle(brsh, new Rectangle(0, 0, bi.Width, bi.Height));
                    }
                }

                using (var grf = Graphics.FromImage(btm))
                {
                    using (Brush brsh = new TextureBrush(bi))
                    {
                        grf.FillEllipse(brsh, 6, 6, bi.Width - 12, bi.Height - 12);
                    }
                }

                using (var grf = Graphics.FromImage(btm2))
                {
                    grf.InterpolationMode = InterpolationMode.High;
                    grf.CompositingQuality = CompositingQuality.HighQuality;
                    grf.SmoothingMode = SmoothingMode.AntiAlias;
                    grf.DrawImage(btm, new Rectangle(0, 0, size, size));
                }
            }
            else
            {
                using (var grf = Graphics.FromImage(bi))
                {
                    using (Brush brsh = new SolidBrush(Color.FromArgb(0, 0, 0, 0)))
                    {
                        grf.FillRectangle(brsh, new Rectangle(0, 0, bi.Width, bi.Height));
                    }
                }

                using (var grf = Graphics.FromImage(btm))
                {
                    using (Brush brsh = new TextureBrush(bi))
                    {
                        grf.FillEllipse(brsh, 6, 6, bi.Width - 12, bi.Height - 12);
                    }
                }

                using (var grf = Graphics.FromImage(btm2))
                {
                    grf.InterpolationMode = InterpolationMode.High;
                    grf.CompositingQuality = CompositingQuality.HighQuality;
                    grf.SmoothingMode = SmoothingMode.AntiAlias;
                    grf.DrawImage(btm, new Rectangle(0, 0, size, size));
                }
            }

            return btm2;
        }

        public static Bitmap ReColor(Bitmap bi, int size)
        {
            var btm = new Bitmap(bi.Width + 4, bi.Height + 4);
            var btm2 = new Bitmap(size + 5, size + 5);

            using (var grf = Graphics.FromImage(bi))
            {
                using (Brush brsh = new SolidBrush(Color.FromArgb(120, 0, 0, 0)))
                {
                    grf.FillRectangle(brsh, new Rectangle(0, 0, bi.Width, bi.Height));
                }
            }

            using (var grf = Graphics.FromImage(btm))
            {
                using (Brush brsh = new TextureBrush(bi))
                {
                    grf.FillRectangle(brsh, 6, 6, bi.Width, bi.Height);
                }
            }

            using (var grf = Graphics.FromImage(btm2))
            {
                grf.InterpolationMode = InterpolationMode.High;
                grf.CompositingQuality = CompositingQuality.HighQuality;
                grf.SmoothingMode = SmoothingMode.AntiAlias;
                grf.DrawImage(btm, new Rectangle(0, 0, size, size));
            }

            return btm2;
        }
    }
}
