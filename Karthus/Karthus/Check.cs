using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Karthus
{
    internal class TI
    {
        public int timeCheck;

        public TI(AIHeroClient player)
        {
            this.Player = player;
        }

        public AIHeroClient Player { get; set; }
    }

    internal class Check
    {
        public IEnumerable<AIHeroClient> ETeam;
        public IEnumerable<AIHeroClient> ATeam;
        public List<TI> TI = new List<TI>();

        public Check()
        {
            var champs = ObjectManager.Get<AIHeroClient>().ToList();

            ATeam = champs.Where(x => x.IsAlly);
            ETeam = champs.Where(x => x.IsEnemy);

            TI = ETeam.Select(x => new TI(x)).ToList();

            Game.OnUpdate += Game_OnUpdate;
        }

        void Game_OnUpdate(EventArgs args)
        {
            var time = Game.Time;

            foreach (TI ti in TI.Where(x => x.Player.IsVisible && !x.Player.IsRecalling()))
                ti.timeCheck = (int)time;
        }

        public TI GetEI(AIHeroClient E)
        {
            return Program.Check.TI.Find(x => x.Player.NetworkId == E.NetworkId);
        }

        public float GetTargetHealth(TI ti, int addTime)
        {
            if (ti.Player.IsVisible)
                return ti.Player.Health;

            var predhealth = ti.Player.Health + ti.Player.HPRegenRate * ((Game.Time - ti.timeCheck + addTime) / 1000f);

            return predhealth > ti.Player.MaxHealth ? ti.Player.MaxHealth : predhealth;
        }

        public bool recalltc(TI ti)
        {
            if (ti.Player.HasBuff("exaltedwithbaronnashor"))
            {
                if ((Game.Time - ti.timeCheck + 3000f) < GetRecallTime(ti.Player))
                {
                    return true;
                }
                return false;
            }
            if ((Game.Time - ti.timeCheck + 3000f) < GetRecallTime(ti.Player))
            {
                return true;
            }
            return false;
        }

        public static int GetRecallTime(AIHeroClient obj)
        {
            return GetRecallTime(obj.Spellbook.GetSpell(SpellSlot.Recall).Name);
        }

        public static int GetRecallTime(string recallName)
        {
            var duration = 0;

            switch (recallName.ToLower())
            {
                case "recall":
                    duration = 8000;
                    break;
                case "recallimproved":
                    duration = 7000;
                    break;
                case "odinrecall":
                    duration = 4500;
                    break;
                case "odinrecallimproved":
                    duration = 4000;
                    break;
                case "superrecall":
                    duration = 4000;
                    break;
                case "superrecallimproved":
                    duration = 4000;
                    break;
            }
            return duration;
        }
    }
}
