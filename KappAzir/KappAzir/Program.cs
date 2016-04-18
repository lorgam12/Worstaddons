namespace KappAzir
{
    using System;

    using EloBuddy;
    using EloBuddy.SDK.Events;

    using Modes;

    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        /// <summary>
        /// The firs thing that runs on the template
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        /// <summary>
        /// This event is triggered when the game loads
        /// </summary>
        /// <param name="args"></param>
        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            try
            {
                //Put the name of the champion here
                if (Player.Instance.ChampionName != "Azir")
                {
                    return;
                }

                SpellsManager.InitializeSpells();
                Menus.CreateMenu();
                ModeManager.InitializeModes();
                DrawingsManager.InitializeDrawings();
            }
            catch (Exception e)
            {
                if (e.ToString().Contains("Mario"))
                {
                    Chat.Print("[KappAzir ERROR] Failed to Load addon Please Make sure you have Mario's Lib installed");
                    Console.Write("[KappAzir ERROR] Failed to Load addon Please Make sure you have Mario's Lib installed");
                }
            }
        }
    }
}