namespace KappaUtilityReborn
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using KappaUtilityReborn.Main;
    using KappaUtilityReborn.Main.Common;

    using Newtonsoft.Json.Linq;

    internal class Program
    {
        public static Menu MenuIni;

        private static int i;

        private static bool Loaded;

        private static readonly List<SpellSlot> spellSlots = new List<SpellSlot>() { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R };

        private static readonly List<SpellSlot> SummonerSpells = new List<SpellSlot>() { SpellSlot.Summoner1, SpellSlot.Summoner2 };

        public static string KappaUtilityFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\EloBuddy\\KappaUtility";

        public static string ChampionIcons;

        public static string SummonerSpellsIcons;

        private const string VersionUrl = "http://ddragon.leagueoflegends.com/api/versions.json";

        private static string LiveVersionString;

        private static System.Version LiveVersion;

        public const string ChampionsIconsUrl = "http://ddragon.leagueoflegends.com/cdn/{0}/img/champion/";

        public const string AbilitiesIconsUrl = "http://ddragon.leagueoflegends.com/cdn/{0}/img/spell/";

        private static void Main(string[] args)
        {
            try
            {
                var WebClient = new WebClient();
                WebClient.DownloadStringCompleted += WebOnDownloadStringCompleted;
                WebClient.DownloadStringTaskAsync(VersionUrl);
                Logger.Info("Checking Game Version");
            }
            catch (Exception a)
            {
                Logger.Error(a.ToString());
            }
        }

        private static void WebOnDownloadStringCompleted(object sender, DownloadStringCompletedEventArgs downloadStringCompletedEventArgs)
        {
            try
            {
                var versionJson = JArray.Parse(downloadStringCompletedEventArgs.Result);
                LiveVersionString = versionJson.First.ToObject<string>();
                LiveVersion = new System.Version(LiveVersionString);
                Logger.Info("LiveVersion = " + LiveVersion);
                ChampionIcons = KappaUtilityFolder + "\\" + LiveVersionString + "\\ChampionIcons\\";
                SummonerSpellsIcons = KappaUtilityFolder + "\\" + LiveVersionString + "\\SummonerSpellsIcons\\";
                Init();
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
        }

        public static bool Load()
        {
            try
            {
                if (!Loaded)
                {
                    MenuIni = MainMenu.AddMenu("KUtilityReborn", "KUtilityReborn");
                    MenuIni.AddGroupLabel("Global Settigs [F5 To Take Effect]");
                    var activator = MenuIni.Add("Activator", new CheckBox("Load Activator"));
                    var Tracker = MenuIni.Add("Trackers", new CheckBox("Load Trackers"));
                    if (activator.CurrentValue)
                    {
                        var a = (Base)Activator.CreateInstance(null, "KappaUtilityReborn.Main.Activator.Handler").Unwrap();
                    }

                    if (Tracker.CurrentValue)
                    {
                        var b = (Base)Activator.CreateInstance(null, "KappaUtilityReborn.Main.Trackers.Handler").Unwrap();
                    }
                }
            }
            catch (Exception a)
            {
                Logger.Error(a.ToString());
            }

            return true;
        }

        public static void Init()
        {
            Loading.OnLoadingComplete += delegate
                {
                    try
                    {
                        var champiconurl = String.Format(ChampionsIconsUrl, LiveVersion);
                        var abilityiconurl = String.Format(AbilitiesIconsUrl, LiveVersion);
                        foreach (var hero in EntityManager.Heroes.AllHeroes)
                        {
                            var chmpdirc = ChampionIcons + hero.ChampionName + "\\";
                            if (!Directory.Exists(chmpdirc))
                            {
                                Logger.Info("Creating Directory For " + hero.ChampionName);
                                Directory.CreateDirectory(chmpdirc);
                            }

                            if (!Directory.GetFiles(chmpdirc).Contains(chmpdirc + hero.ChampionName + ".png"))
                            {
                                Logger.Info("Downloading " + hero.ChampionName + " Icon !");
                                var webClient = new WebClient();
                                i++;
                                webClient.DownloadFileAsync(new Uri(champiconurl + hero.ChampionName + ".png"), chmpdirc + hero.ChampionName + ".png");
                                webClient.DownloadFileCompleted += delegate { i--; };
                            }

                            foreach (var slot in spellSlots)
                            {
                                var spell = hero.Spellbook.GetSpell(slot);
                                var filename = spell.Name + ".png";
                                if (!Directory.GetFiles(chmpdirc).Contains(chmpdirc + hero.ChampionName + slot + ".png"))
                                {
                                    Logger.Info("Downloading " + filename);
                                    var webClient = new WebClient();
                                    i++;
                                    webClient.DownloadFileAsync(new Uri(abilityiconurl + filename), chmpdirc + hero.ChampionName + slot + ".png");
                                    webClient.DownloadFileCompleted += delegate { i--; };
                                }
                            }

                            var hero1 = hero;
                            foreach (var slotname in SummonerSpells.Select(spell => hero1.Spellbook.GetSpell(spell).Name + ".png"))
                            {
                                if (!Directory.Exists(SummonerSpellsIcons))
                                {
                                    Logger.Info("Creating SummonerSpellsIcons Directory !");
                                    Directory.CreateDirectory(SummonerSpellsIcons);
                                }

                                if (!Directory.GetFiles(SummonerSpellsIcons).Contains(SummonerSpellsIcons + slotname))
                                {
                                    Logger.Info("Downloading " + slotname);
                                    var webClient = new WebClient();
                                    i++;
                                    webClient.DownloadFileAsync(new Uri(abilityiconurl + slotname), SummonerSpellsIcons + slotname);
                                    webClient.DownloadFileCompleted += delegate { i--; };
                                }
                            }
                        }

                        Game.OnTick += delegate
                            {
                                if (Loaded)
                                {
                                    return;
                                }

                                if (i == 0 && !Loaded)
                                {
                                    Logger.Info("Your Champion Icons Are Updated !");
                                    Load();
                                    Loaded = true;
                                }
                            };
                    }
                    catch (Exception a)
                    {
                        Logger.Error(a.ToString());
                    }
                };
        }
    }
}
