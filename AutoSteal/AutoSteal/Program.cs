namespace AutoSteal
{
    using System;
    using EloBuddy;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Menu.Values;
    using Modes;

    internal class Program
    {

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnLoad;
        }


        private static void OnLoad(EventArgs args)
        {
            Game.OnUpdate += OnUpdate;
            Start.Startload();
        }

        private static void OnUpdate(EventArgs args)
        {
            if (Start.KillSteal["EnableKS"].Cast<CheckBox>().CurrentValue)
            {
                KillSteal.KS();
            }

            if (Start.KillSteal["EnableJS"].Cast<CheckBox>().CurrentValue)
            {
                JungleSteal.JS();
            }
        }
        }
    }
