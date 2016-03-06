using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

#region Credits
//=====================================================================
//+ Massive thanks to the entire community of EB for making this
//+ Spell library possible. Special thanks to: Coman3, MarioGK, 
//+ KarmaPanda, Bloodimir, Hellsing, iRaxe, plebsot, Chaos, 
//+ zpitty and many others!
//+
//+ This spell database was last updated 2/29/2016
//======================================================================
#endregion

namespace GenesisSpellLibrary.Spells
{

    public class Aatrox : SpellBase // Quality Tested, Genesis Approved
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Aatrox()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 650, SkillShotType.Circular, 250, 450, 285) { AllowedCollisionCount = int.MaxValue }; 
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, 1200, 100) { AllowedCollisionCount = int.MaxValue };
            R = new Spell.Active(SpellSlot.R);
            QisCC = true;
            QisDash = true;
            WisToggle = true;
            EisCC = true;
            LogicDictionary = new Dictionary<string, Func<AIHeroClient,Obj_AI_Base, bool>>();
            LogicDictionary.Add("RLogic", RLogic);
        }

        public static bool RLogic(AIHeroClient player, object _)
        {
            if (player == null) return false;
            return EntityManager.Heroes.Enemies.Count(e => e.Distance(player) < 500) >= 1;
        }

    }
    public class Ahri : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Ahri()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1750, 100);
            W = new Spell.Active(SpellSlot.W, 550);
            E = new Spell.Skillshot(SpellSlot.E, 950, SkillShotType.Linear, 250, 1550, 60);
            R = new Spell.Active(SpellSlot.R, 600);
            Options.Clear();
            Options.Add("EisCC", true);
            Options.Add("RisDash", true);

        }
    }
    public class Akali : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }

        public Akali()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Circular);
            E = new Spell.Active(SpellSlot.E, 325);
            R = new Spell.Targeted(SpellSlot.R, 700);
            WisCC = true;
            RisDash = true;

        }
    }
    public class Alistar : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Alistar()
        {
            Q = new Spell.Active(SpellSlot.Q, 315);
            W = new Spell.Targeted(SpellSlot.W, 625);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Active(SpellSlot.R);
            QisCC = true;
            WisDash = true;
            WisCC = true;
            LogicDictionary = new Dictionary<string, Func<AIHeroClient, Obj_AI_Base, bool>>();
            LogicDictionary.Add("RLogic", RLogic);
        }

        public static bool RLogic(AIHeroClient player, object _)
        {
            if (player == null) return false;
            int x = EntityManager.Heroes.Enemies.Count(e => e.Distance(player) < 1000);
                if((
                player.HasBuffOfType(BuffType.Fear) || 
                player.HasBuffOfType(BuffType.Silence) ||
                player.HasBuffOfType(BuffType.Snare) ||
                player.HasBuffOfType(BuffType.Stun) || 
                player.HasBuffOfType(BuffType.Charm) ||
                player.HasBuffOfType(BuffType.Blind) ||
                player.HasBuffOfType(BuffType.Taunt))
                 || (x > 3)
                ) return true;
            return false;
        }
    }
    public class Amumu : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Amumu()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 2000, 80);
            W = new Spell.Active(SpellSlot.W, 300);
            E = new Spell.Active(SpellSlot.E, 350);
            R = new Spell.Active(SpellSlot.R, 550);
            QisCC = true;
            QisDash = true;
            RisCC = true;
        }
    }
    public class Anivia : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Anivia()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 850, 100);
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Circular, 0, int.MaxValue, 20);
            E = new Spell.Targeted(SpellSlot.E, 650);
            R = new Spell.Skillshot(SpellSlot.R, 600, SkillShotType.Circular, 0, int.MaxValue, 200);
            QisCC = true;
        }
    }
    public class Annie : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Annie()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Skillshot(SpellSlot.W, 500, SkillShotType.Cone, 250, 100, 80);
            E = new Spell.Active(SpellSlot.E, 0);
            R = new Spell.Skillshot(SpellSlot.R, 600, SkillShotType.Circular, 250, 0, 290);
            LogicDictionary = new Dictionary<string, Func<AIHeroClient, Obj_AI_Base, bool>>();
            LogicDictionary.Add("RLogic", RLogic);
        }

        public bool RLogic(AIHeroClient player, Obj_AI_Base target)
        {
            if (player == null) return false;
            
            
            return EntityManager.Heroes.Enemies.Count(e => e.Distance(target) < 300) >= 1;
        }
    }
    public class Ashe : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Ashe()
        {
            Q = new Spell.Active(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 1200, SkillShotType.Cone);
            E = new Spell.Active(SpellSlot.E, 1000);
            R = new Spell.Skillshot(SpellSlot.R, 10000, SkillShotType.Linear, 250, 1600, 100);
            RisCC = true;
        }
    }
    public class Azir : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Azir()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Linear, 250, 1000, 70);
            W = new Spell.Skillshot(SpellSlot.W, 450, SkillShotType.Circular);
            E = new Spell.Skillshot(SpellSlot.E, 1200, SkillShotType.Linear, 250, 1600, 100);
            R = new Spell.Skillshot(SpellSlot.R, 300, SkillShotType.Linear, 500, 1000, 532);
            RisCC = true;
            EisDash = true;
        }
    }
    public class Bard : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Bard()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 860, SkillShotType.Linear, 250, 1600, 65);
            //Q2 = new Spell.Skillshot(SpellSlot.Q, 1310, SkillShotType.Linear, 250, 1600, 65);
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Circular);
            E = new Spell.Skillshot(SpellSlot.E, int.MaxValue, SkillShotType.Linear);
            R = new Spell.Skillshot(SpellSlot.R, 3400, SkillShotType.Circular, 250, int.MaxValue, 650);
        }
    }
    public class Blitzcrank : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Blitzcrank()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 980, SkillShotType.Linear, 250, 1800, 70);
            W = new Spell.Active(SpellSlot.W, 0);
            E = new Spell.Active(SpellSlot.E, 150);
            R = new Spell.Active(SpellSlot.R, 550);
        }
    }
    public class Brand : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Brand()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1600, 120);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 850, int.MaxValue, 250);
            E = new Spell.Targeted(SpellSlot.E, 640);
            R = new Spell.Targeted(SpellSlot.R, 750);
        }
    }
    public class Braum : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Braum()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1700, 60);
            W = new Spell.Targeted(SpellSlot.W, 650);
            E = new Spell.Skillshot(SpellSlot.E, 500, SkillShotType.Cone, 250, 2000, 250);
            R = new Spell.Skillshot(SpellSlot.R, 1300, SkillShotType.Linear, 250, 1300, 115);
        }
    }
    public class Caitlyn : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Caitlyn()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1240, SkillShotType.Linear, 250, 2000, 60);
            W = new Spell.Skillshot(SpellSlot.W, 820, SkillShotType.Circular, 500, int.MaxValue, 80);
            E = new Spell.Skillshot(SpellSlot.E, 800, SkillShotType.Linear, 250, 1600, 80);
            R = new Spell.Targeted(SpellSlot.R, 2000);
        }
    }
    public class Cassiopeia : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Cassiopeia()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Circular, castDelay: 400, spellWidth: 75);
            W = new Spell.Skillshot(SpellSlot.W, 850, SkillShotType.Circular, spellWidth: 125);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Skillshot(SpellSlot.R, 825, SkillShotType.Cone, spellWidth: 80);
        }
    }
    public class Chogath : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Chogath()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Circular, 750, int.MaxValue, 175);
            W = new Spell.Skillshot(SpellSlot.W, 575, SkillShotType.Cone, 250, 1750, 100);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Targeted(SpellSlot.R, 500);
        }
    }
    public class Corki : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Corki()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Circular, 300, 1000, 250);
            W = new Spell.Skillshot(SpellSlot.W, 600, SkillShotType.Linear);
            //W2 = new Spell.Skillshot(SpellSlot.W, 1800, SkillShotType.Linear);
            E = new Spell.Active(SpellSlot.E, 600);
            R = new Spell.Skillshot(SpellSlot.R, 1300, SkillShotType.Linear, 200, 1950, 40);
        }
    }
    public class Darius : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Darius()
        {
            Q = new Spell.Active(SpellSlot.Q, 400);
            W = new Spell.Active(SpellSlot.W, 145);
            E = new Spell.Skillshot(SpellSlot.E, 540, SkillShotType.Cone, 250, 100, 120);
            R = new Spell.Targeted(SpellSlot.R, 460);
        }
    }
    public class Diana : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Diana()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 830, SkillShotType.Cone, 500, 1600, 195);
            W = new Spell.Active(SpellSlot.W, 350);
            E = new Spell.Active(SpellSlot.E, 200);
            R = new Spell.Targeted(SpellSlot.R, 825);
        }
    }
    public class DrMundo : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public DrMundo()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 50, 2000, 60);
            W = new Spell.Active(SpellSlot.W, 162);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class Draven : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Draven()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 1050, SkillShotType.Linear);
            R = new Spell.Skillshot(SpellSlot.R, 2000, SkillShotType.Linear);
        }
    }
    public class Ekko : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Ekko()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 750, SkillShotType.Linear, 250, 2200, 60);
            W = new Spell.Skillshot(SpellSlot.W, 1620, SkillShotType.Circular, 500, 1000, 500);
            E = new Spell.Skillshot(SpellSlot.E, 400, SkillShotType.Linear, 250, int.MaxValue, 1);
            R = new Spell.Active(SpellSlot.R, 400);
            
        }
    }
    /*public class Elise : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Elise()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular);
            E = new Spell.Skillshot(SpellSlot.E, 1075, SkillShotType.Linear, 250, 1600, 80) { AllowedCollisionCount = 0 };
            R = new Spell.Active(SpellSlot.R); // TODO: Support Elise
        }
    }*/

        
    public class Evelynn : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Evelynn()
        {
            Q = new Spell.Active(SpellSlot.Q, 475);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 225);
            R = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Circular, 250, 1200, 150);
        }
    }
    public class Ezreal : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Ezreal()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 250, 2000, 60) { AllowedCollisionCount = 0 };
            W = new Spell.Skillshot(SpellSlot.W, 1050, SkillShotType.Linear, 250, 1600, 80);
            E = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Linear, 250, 2000, 80);
            R = new Spell.Skillshot(SpellSlot.R, 5000, SkillShotType.Linear, 1000, 2000, 160);
        }
    }
    public class FiddleSticks : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public FiddleSticks()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 575);
            W = new Spell.Targeted(SpellSlot.W, 575);
            E = new Spell.Targeted(SpellSlot.E, 750);
            R = new Spell.Skillshot(SpellSlot.R, 800, SkillShotType.Circular, 1750, Int32.MaxValue, 600);
        }
    }
    public class Fiora : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Fiora()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 750, SkillShotType.Linear);
            W = new Spell.Skillshot(SpellSlot.W, 750, SkillShotType.Linear, 500, 3200, 70);
            E = new Spell.Active(SpellSlot.E, 200);
            R = new Spell.Targeted(SpellSlot.R, 500);
        }
    }
    public class Fizz : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Fizz()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 550);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 400, SkillShotType.Circular, 250, int.MaxValue, 330);
            R = new Spell.Skillshot(SpellSlot.R, 1300, SkillShotType.Linear, 250, 1200, 80);
        }
    }
    public class Galio : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Galio()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 940, SkillShotType.Circular, 250, 1300, 120);
            W = new Spell.Targeted(SpellSlot.W, 830);
            E = new Spell.Skillshot(SpellSlot.E, 1180, SkillShotType.Linear, 250, 1200, 140);
            R = new Spell.Active(SpellSlot.R, 560);
        }
    }
    public class Gangplank : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Gangplank()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 1150, SkillShotType.Circular, 450, 2000, 390);
            R = new Spell.Skillshot(SpellSlot.R, int.MaxValue, SkillShotType.Circular, 250, int.MaxValue, 600);
        }
    }
    public class Garen : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Garen()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 300);
            R = new Spell.Targeted(SpellSlot.R, 400);
        }
    }
    /*public class Gnar : SpellBase 
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Gnar()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1200, 55);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Circular, 500, int.MaxValue, 150);
            R = new Spell.Active(SpellSlot.R);
        }
    }*/ // TODO: Same boat as Elise
    public class Gragas : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Gragas()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 775, SkillShotType.Circular, 1, 1000, 110);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 675, SkillShotType.Linear, 0, 1000, 50);
            R = new Spell.Skillshot(SpellSlot.R, 1100, SkillShotType.Circular, 1, 1000, 700);
        }
    }
    public class Graves : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Graves()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear, 0, 3000, 40) { AllowedCollisionCount = 0 };
            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular, 500, 1500, 120);
            E = new Spell.Skillshot(SpellSlot.E, 425, SkillShotType.Linear, 500, 0, 50);
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Linear, 500, 2100, 100);
        }
    }
    public class Hecarim : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Hecarim()
        {
            Q = new Spell.Active(SpellSlot.Q, 350);
            W = new Spell.Active(SpellSlot.W, 525);
            E = new Spell.Active(SpellSlot.E, 450);
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Linear, 250, 800, 200);
        }
    }
    public class Heimerdinger : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }

        public Heimerdinger()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 350, SkillShotType.Linear, (int)0.5f, 1450, (int)40f);
            W = new Spell.Skillshot(SpellSlot.W, 1325, SkillShotType.Cone, (int)0.5f, 902, 200);
            E = new Spell.Skillshot(SpellSlot.E, 970, SkillShotType.Circular, (int)0.5f, 2500, 120);
            R = new Spell.Active(SpellSlot.R, 350);
        }
    }
    public class Illaoi : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Illaoi()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Linear, 750, int.MaxValue, 100);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 950, SkillShotType.Linear, 250, 1900, 50);
            R = new Spell.Active(SpellSlot.R, 450);
        }
    }
    public class Irelia : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Irelia()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 425);
            R = new Spell.Skillshot(SpellSlot.R, 900, SkillShotType.Linear, 250, 1600, 120);
        }
    }
    public class Janna : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Janna()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 300);
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Targeted(SpellSlot.E, 800);
            R = new Spell.Active(SpellSlot.R, 725);
        }
    }
    public class JarvanIV : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public JarvanIV()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 830, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W, 520);
            E = new Spell.Skillshot(SpellSlot.E, 860, SkillShotType.Circular);
            R = new Spell.Targeted(SpellSlot.R, 650);
        }
    }
    public class Jax : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Jax()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 700);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 187);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    /*public class Jayce : SpellBase //todo
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Jayce()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Circular);
            E = new Spell.Active(SpellSlot.E, 325);
            R = new Spell.Targeted(SpellSlot.R, 700);
        }
    } // TODO: FUCK THERE ARE 3 OF YOU?!
    public class Jhin : SpellBase // todo
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Jhin()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Circular);
            E = new Spell.Active(SpellSlot.E, 325);
            R = new Spell.Targeted(SpellSlot.R, 700);
        }
    }*/ // Just fuck you jhin.
    public class Jinx : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Jinx()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 1450, SkillShotType.Linear, 500, 1500, 60) { AllowedCollisionCount = 0 };
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Circular, 1200, 1750, 100);
            R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 700, 1500, 140);
        }
    }
    public class Kalista : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Kalista()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1150, SkillShotType.Linear, 250, 1200, 40);
            W = new Spell.Targeted(SpellSlot.W, 5000);
            E = new Spell.Active(SpellSlot.E, 1000);
            R = new Spell.Active(SpellSlot.R, 1500); //You are gonna suck until you get logic
        }
    }
    public class Karma : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Karma()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear, 250, 1500, 100);
            W = new Spell.Targeted(SpellSlot.W, 675);
            E = new Spell.Targeted(SpellSlot.E, 800);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class Karthus : SpellBase //Want to try
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Karthus()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Circular, 1000, int.MaxValue, 160);
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Circular, 500, int.MaxValue, 70);
            E = new Spell.Active(SpellSlot.E, 505);
            R = new Spell.Skillshot(SpellSlot.R, 25000, SkillShotType.Circular, 3000, int.MaxValue, int.MaxValue);
        }
    }
    public class Kassadin : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Kassadin()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 650);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 400, SkillShotType.Cone, (int)0.5f, int.MaxValue, 10);
            R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Circular, (int)0.5f, int.MaxValue, 150);
        }
    }
    public class Katarina : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Katarina()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 675);
            W = new Spell.Active(SpellSlot.W, 375);
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Active(SpellSlot.R, 550);
        }
    }
    public class Kayle : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Kayle()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 650);
            W = new Spell.Targeted(SpellSlot.W, 900);
            E = new Spell.Skillshot(SpellSlot.E, 650, SkillShotType.Circular, 1, 50, 400);
            R = new Spell.Targeted(SpellSlot.R, 900);
        }
    }
    public class Kennen : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Kennen()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 125, 1700, 50);
            W = new Spell.Active(SpellSlot.W, 900);
            E = new Spell.Active(SpellSlot.E, 500);//Kappa ;)
            R = new Spell.Active(SpellSlot.R, 500);
        }
    }
    public class Khazix : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Khazix()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 325);
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Linear, 225, 828, 80);
            E = new Spell.Skillshot(SpellSlot.E, 600, SkillShotType.Circular, 25, 1000, 100);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class Kindred : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Kindred()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1125, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 500);
            R = new Spell.Targeted(SpellSlot.R, 500);
        }
    }
    public class KogMaw : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public KogMaw()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 980, SkillShotType.Linear, 250, 2000, 50) { AllowedCollisionCount = int.MaxValue };
            W = new Spell.Active(SpellSlot.W, 700);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, 1530, 60);
            R = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Circular, 250, 1200, 30);
        }
    }
    public class Leblanc : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Leblanc()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 700);
            W = new Spell.Skillshot(SpellSlot.W, 600, SkillShotType.Circular, 250, 1450, 250);
            E = new Spell.Skillshot(SpellSlot.E, 950, SkillShotType.Linear, 250, 1550, 55) { AllowedCollisionCount = 0 };
            R = new Spell.Targeted(SpellSlot.R, 950);
        }
    }
    public class LeeSin : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public LeeSin()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear, 250, 1800, 60) { AllowedCollisionCount = 0 };
            //Q2 = new Spell.Active(SpellSlot.Q, 1300);
            W = new Spell.Skillshot(SpellSlot.W, 1200, SkillShotType.Linear, 50, 1500, 100);
            //W2 = new Spell.Active(SpellSlot.W, 700);
            E = new Spell.Skillshot(SpellSlot.E, 350, SkillShotType.Linear, 250, 2500, 100);
            //E2 = new Spell.Skillshot(SpellSlot.E, 675, SkillShotType.Linear, 250, 2500, 100)
            R = new Spell.Targeted(SpellSlot.R, 375);
        }
    }
    public class Leona : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Leona()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W, 275);
            E = new Spell.Skillshot(SpellSlot.E, 875, SkillShotType.Linear, 250, 2000, 70);
            R = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Circular, 1000, int.MaxValue, 250);
        }
    }
    public class Lissandra : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Lissandra()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 725, SkillShotType.Linear);
            //Q1 = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W, 450);
            E = new Spell.Skillshot(SpellSlot.E, 1050, SkillShotType.Linear);
            //E1 = new Spell.Active(SpellSlot.E);
            R = new Spell.Targeted(SpellSlot.R, 550);
        }
    }
    public class Lucian : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Lucian()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 675);
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Linear, 250, 1600, 80);
            E = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Linear);
            R = new Spell.Skillshot(SpellSlot.R, 1400, SkillShotType.Linear, 500, 2800, 110);
        }
    }
    public class Lulu : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Lulu()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear, 250, 1450, 60);
            W = new Spell.Targeted(SpellSlot.W, 650);
            E = new Spell.Targeted(SpellSlot.E, 650);
            R = new Spell.Targeted(SpellSlot.R, 900);
        }
    }
    public class Lux : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Lux()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1175, SkillShotType.Linear, 250, 1200, 65) { AllowedCollisionCount = 1 };
            W = new Spell.Skillshot(SpellSlot.W, 1075, SkillShotType.Linear, 0, 1400, 85);
            E = new Spell.Skillshot(SpellSlot.E, 1050, SkillShotType.Circular, 250, 1300, 330);
            R = new Spell.Skillshot(SpellSlot.R, 3200, SkillShotType.Circular, 500, int.MaxValue, 160);
        }
    }
    public class Malphite : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Malphite()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 400);
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Circular, 250, 700, 270);

        }
    }
    public class Malzahar : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Malzahar()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 500, int.MaxValue, 100) { MinimumHitChance = HitChance.High };
            W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Circular, 500, int.MaxValue, 250);
            E = new Spell.Targeted(SpellSlot.E, 650);
            R = new Spell.Targeted(SpellSlot.R, 700);
        }
    }
    public class Maokai : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Maokai()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 600, SkillShotType.Linear, 500, 1200, 110);
            W = new Spell.Targeted(SpellSlot.W, 525);
            E = new Spell.Skillshot(SpellSlot.E, 1075, SkillShotType.Circular, 1000, 1500, 225);
            R = new Spell.Active(SpellSlot.R, 475);
        }
    }
    public class MasterYi : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public MasterYi()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class MissFortune : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public MissFortune()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 650);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Circular, 500, int.MaxValue, 200);
            R = new Spell.Skillshot(SpellSlot.R, 1400, SkillShotType.Cone, 0, int.MaxValue);
        }
    }
    public class Mordekaiser : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Mordekaiser()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Targeted(SpellSlot.W, 1000);
            E = new Spell.Skillshot(SpellSlot.E, 670, SkillShotType.Cone, (int)0.25f, 2000, 12 * 2 * (int)Math.PI / 180);
            R = new Spell.Targeted(SpellSlot.R, 1500);
        }
    }
    public class Morgana : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Morgana()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 250, 1200, 80);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 250, 2200, 400);
            E = new Spell.Targeted(SpellSlot.E, 750);
            R = new Spell.Active(SpellSlot.R, 600);
        }
    }
    public class Nami : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Nami()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 875, SkillShotType.Circular, 1, int.MaxValue, 150);
            W = new Spell.Targeted(SpellSlot.W, 725);
            E = new Spell.Targeted(SpellSlot.E, 800);
            R = new Spell.Skillshot(SpellSlot.R, 2750, SkillShotType.Linear, 250, 500, 160);

        }
    }
    public class Nasus : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Nasus()
        {
            Q = new Spell.Active(SpellSlot.Q, 150);
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Skillshot(SpellSlot.E, 650, SkillShotType.Circular, 250, 190, int.MaxValue);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class Nautilus : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Nautilus()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1100, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, (uint)ObjectManager.Player.Spellbook.GetSpell(SpellSlot.E).SData.CastRange);
            R = new Spell.Targeted(SpellSlot.R, (uint)ObjectManager.Player.Spellbook.GetSpell(SpellSlot.R).SData.CastRange);
        }
    }
    /*public class Nidalee : SpellBase // todo
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Nidalee()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Circular);
            E = new Spell.Active(SpellSlot.E, 325);
            R = new Spell.Targeted(SpellSlot.R, 700);
        }
    }*/
    public class Nocturne : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Nocturne()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1125, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 425);
            R = new Spell.Active(SpellSlot.R, 2500);
            // R1 = new Spell.Targeted(SpellSlot.R, R.Range);
        }
    }
    public class Nunu : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Nunu()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 350);
            W = new Spell.Targeted(SpellSlot.W, 700);
            E = new Spell.Targeted(SpellSlot.E, 550);
            R = new Spell.Active(SpellSlot.R, 650);
        }
    }
    public class Olaf : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Olaf()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1550, 75);
            //Q2 = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1550, 75)     
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 325);
            R = new Spell.Active(SpellSlot.R);

        }
    }
  /*  public class Orianna : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Orianna()
        {
            {
                Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 250, 1550, 75);
                //Q2 = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1550, 75)     
                W = new Spell.Active(SpellSlot.W);
                E = new Spell.Targeted(SpellSlot.E, 325);
                R = new Spell.Active(SpellSlot.R);
            }
    } */ //Todo
    public class Pantheon : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Pantheon()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Skillshot(SpellSlot.E, 600, SkillShotType.Cone, 250, 2000, 70);
            R = new Spell.Skillshot(SpellSlot.R, 2000, SkillShotType.Circular);
        }
    }
    public class Poppy : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Poppy()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 430, SkillShotType.Linear, 250, null, 100);
            W = new Spell.Active(SpellSlot.W, 400);
            E = new Spell.Targeted(SpellSlot.E, 525);
            R = new Spell.Chargeable(SpellSlot.R, 500, 1200, 4000, 250, int.MaxValue, 90);
        }
    }
    public class Quinn : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Quinn()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1025, SkillShotType.Linear, 0, 750, 210);
            W = new Spell.Active(SpellSlot.W, 2100);
            E = new Spell.Targeted(SpellSlot.E, 675);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class Rammus : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Rammus()
        {
            Q = new Spell.Active(SpellSlot.Q, 200);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 325);
            R = new Spell.Active(SpellSlot.R, 300);
        }
    }
    public class RekSai : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public RekSai()
        {
            Q = new Spell.Active(SpellSlot.Q, 325);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 250);
            R = new Spell.Targeted(SpellSlot.R,0);
        }
    }

    public class Renekton : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }

        public Renekton()
        {
            Q = new Spell.Active(SpellSlot.Q, 225);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 450, SkillShotType.Linear);
            R = new Spell.Active(SpellSlot.R);
        }
    }

    public class Rengar : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Rengar()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 500, SkillShotType.Circular, 250, 2000, 100);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Linear, 250, 1500, 140);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class Riven : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Riven()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Circular);
            E = new Spell.Active(SpellSlot.E, 325);
            W = new Spell.Active(SpellSlot.W, (uint)(70 + ObjectManager.Player.BoundingRadius + 120));
        }
    }
    public class Rumble : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Rumble()
        {
            Q = new Spell.Active(SpellSlot.Q, 600);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 840, SkillShotType.Linear, 250, 2000, 70);
            R = new Spell.Skillshot(SpellSlot.R, 1700, SkillShotType.Linear, 400, 2500, 120);
        }
    }
    public class Ryze : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Ryze()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1700, 100);
            //Q2 = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Linear, 250, 1700, 100);
            W = new Spell.Targeted(SpellSlot.W, 600);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class Sejuani : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Sejuani()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 650, SkillShotType.Linear, 0, 1600, 70);
            W = new Spell.Active(SpellSlot.W, 350);
            E = new Spell.Active(SpellSlot.E, 1000);
            R = new Spell.Skillshot(SpellSlot.R, 1175, SkillShotType.Linear, 250, 1600, 110);
        }
    }
    public class Shaco : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Shaco()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 400);
            //Q2 = new Spell.Targeted(SpellSlot.Q, 1100);
            W = new Spell.Targeted(SpellSlot.W, 425);
            E = new Spell.Targeted(SpellSlot.E, 625);
            R = new Spell.Targeted(SpellSlot.R, 200);
        }
    }
    public class Shen : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Shen()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 2000, SkillShotType.Linear, 500, 2500, 150);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 610, SkillShotType.Linear, 500, 1600, 50);
            R = new Spell.Targeted(SpellSlot.R, 31000);
        }
    }
    public class Shyvana : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Shyvana()
        {
            Q = new Spell.Active(SpellSlot.Q, (uint)Player.Instance.GetAutoAttackRange());
            W = new Spell.Active(SpellSlot.W, 425);
            E = new Spell.Skillshot(SpellSlot.E, 925, SkillShotType.Linear, 250, 1500, 60);
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Circular, 250, 1500, 150);
        }
    }
    public class Singed : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Singed()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Circular, 500, 700, 350);
            E = new Spell.Targeted(SpellSlot.E, 125);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class Sion : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Sion()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 740, SkillShotType.Cone, 250, 100, 500);
            //Q2 = new Spell.Active(SpellSlot.Q, 680);
            W = new Spell.Active(SpellSlot.W, 490);
            E = new Spell.Skillshot(SpellSlot.E, 755, SkillShotType.Linear);
            R = new Spell.Active(SpellSlot.R, 800);
        }
    }
    public class Sivir : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Sivir()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1245, SkillShotType.Linear, (int)0.25, 1030, 90);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Active(SpellSlot.R, 1000);
        }
    }
    public class Skarner : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Skarner()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Circular);
            E = new Spell.Active(SpellSlot.E, 325);
            R = new Spell.Targeted(SpellSlot.R, 700);
        }
    }
    public class Sona : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Sona()
        {
            Q = new Spell.Active(SpellSlot.Q, 850);
            W = new Spell.Active(SpellSlot.W, 1000);
            E = new Spell.Active(SpellSlot.E, 350);
            R = new Spell.Skillshot(SpellSlot.R, 1000, SkillShotType.Circular, 250, 2400, 140);
        }
    }
    public class Soraka : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Soraka()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Circular, (int)0.283f, 1100, (int)210f);
            W = new Spell.Targeted(SpellSlot.W, 550);
            E = new Spell.Skillshot(SpellSlot.E, 925, SkillShotType.Circular, (int)0.5f, 1750, (int)70f);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class Swain : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Swain()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 625);
            W = new Spell.Skillshot(SpellSlot.W, 820, SkillShotType.Circular, 500, 1250, 275);
            E = new Spell.Targeted(SpellSlot.E, 625);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class Syndra : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Syndra()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Circular, 600, int.MaxValue, 125);
            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular, 250, 1600, 140);
            E = new Spell.Skillshot(SpellSlot.E, 700, SkillShotType.Cone, 250, 2500, 22);
            R = new Spell.Targeted(SpellSlot.R, 675);
            //EQ = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 500, 2500, 55);
        }
    }
    public class TahmKench : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public TahmKench()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Circular);
            E = new Spell.Active(SpellSlot.E, 325);
            R = new Spell.Targeted(SpellSlot.R, 700);
        }
    }
    public class Talon : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Talon()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 600, SkillShotType.Cone, 1, 2300, 75) { AllowedCollisionCount = int.MaxValue };
            E = new Spell.Targeted(SpellSlot.E, 700);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class Taric : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Taric()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 750);
            W = new Spell.Active(SpellSlot.W, 400);
            E = new Spell.Targeted(SpellSlot.E, 625);
            R = new Spell.Active(SpellSlot.R, 400);
        }
    }
    public class Teemo : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Teemo()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 680);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Skillshot(SpellSlot.R, 300, SkillShotType.Circular, 500, 1000, 120);
        }
    }
    public class Thresh : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Thresh()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1040, SkillShotType.Linear, 500, 1900, 60) { AllowedCollisionCount = 0 };
            W = new Spell.Skillshot(SpellSlot.W, 950, SkillShotType.Circular, 250, 1800, 300) { AllowedCollisionCount = int.MaxValue };
            E = new Spell.Skillshot(SpellSlot.E, 480, SkillShotType.Linear, 0, 2000, 110) { AllowedCollisionCount = int.MaxValue };
            R = new Spell.Active(SpellSlot.R, 450);
        }
    }
    public class Tristana : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Tristana()
        {
            Q = new Spell.Active(SpellSlot.Q, 550);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 450, int.MaxValue, 180);
            E = new Spell.Targeted(SpellSlot.E, 550);
            R = new Spell.Targeted(SpellSlot.R, 550);
        }
    }
    public class Trundle : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Trundle()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 0, int.MaxValue, 1000);
            E = new Spell.Skillshot(SpellSlot.E, 1000, SkillShotType.Circular, 250, int.MaxValue, 225);
            R = new Spell.Targeted(SpellSlot.R, 700);
        }
    }
    public class Tryndamere : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Tryndamere()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W, 400);
            E = new Spell.Skillshot(SpellSlot.E, 660, SkillShotType.Linear, 250, 700, (int)92.5);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class TwistedFate : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public TwistedFate()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1450, SkillShotType.Linear, 0, 1000, 40);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E);
            R = new Spell.Active(SpellSlot.R, 5500);
        }
    }
    public class Twitch : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Twitch()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Skillshot(SpellSlot.W, 925, SkillShotType.Circular, 250, 1400, 275) { AllowedCollisionCount = int.MaxValue };
            E = new Spell.Active(SpellSlot.E, 1200);
            R = new Spell.Active(SpellSlot.R, 900);
            //R2 = new Spell.Skillshot(SpellSlot.R, 1200, SkillShotType.Linear, 0, 3000, 100)
        }
    }
    public class Udyr : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Udyr()
        {
            Q = new Spell.Active(SpellSlot.Q, 250);
            W = new Spell.Active(SpellSlot.W, 250);
            E = new Spell.Active(SpellSlot.E, 250);
            R = new Spell.Active(SpellSlot.R, 500);
        }
    }
    public class Urgot : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Urgot()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1000, SkillShotType.Linear, 125, 1600, 60);
            //Q2 = new Spell.Targeted(SpellSlot.Q, 1200);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Circular, 250, 1500, 210);
            R = new Spell.Targeted(SpellSlot.R, 850);
        }
    }
    public class Varus : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Varus()
        {
            //Q2 = new Spell.Skillshot(SpellSlot.Q, 925, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 0, 1900, 100);
            //Q2.AllowedCollisionCount = int.MaxValue;
            Q = new Spell.Chargeable(SpellSlot.Q, 925, 1625, 2000, 0, 1900, 100);
            E = new Spell.Skillshot(SpellSlot.E, 925, SkillShotType.Circular, 500, int.MaxValue, 750);
            R = new Spell.Skillshot(SpellSlot.R, 1075, SkillShotType.Linear, 0, 1200, 120);
        }
    }
    public class Vayne : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Vayne()
        {
            Q = new Spell.Active(SpellSlot.Q, 300);
            //Q2 = new Spell.Skillshot(SpellSlot.Q, 300, SkillShotType.Linear);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 590);
            //E2 = new Spell.Skillshot(SpellSlot.E, 590, SkillShotType.Linear, 250, 1250);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class Veigar : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Veigar()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 950, SkillShotType.Linear, 250, 2000, 70);
            W = new Spell.Skillshot(SpellSlot.W, 900, SkillShotType.Circular, 1350, 0, 225);
            E = new Spell.Skillshot(SpellSlot.E, 700, SkillShotType.Circular, 500, 0, 425);
            R = new Spell.Targeted(SpellSlot.R, 650);
        }
    }
    public class Velkoz : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Velkoz()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 1050, SkillShotType.Linear, 250, 1300, 50)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = 0
            };
            W = new Spell.Skillshot(SpellSlot.W, 1050, SkillShotType.Linear, 250, 1700, 80)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            E = new Spell.Skillshot(SpellSlot.E, 850, SkillShotType.Circular, 500, 1500, 120)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
            R = new Spell.Skillshot(SpellSlot.R, 1550, SkillShotType.Linear)
            {
                MinimumHitChance = HitChance.High,
                AllowedCollisionCount = int.MaxValue
            };
        }
    }
    public class Vi : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Vi()
        {
            Q = new Spell.Chargeable(SpellSlot.Q, 250, 875, 1250, 0, 1400, 55);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 600);
            //E2 = new Spell.Skillshot(SpellSlot.E, 600, SkillShotType.Cone);
            R = new Spell.Targeted(SpellSlot.R, 800);
        }
    }
    public class Viktor : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
       
        public Viktor()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Circular, 500, int.MaxValue, 300){ AllowedCollisionCount = int.MaxValue };
            E = new Spell.Skillshot(SpellSlot.E, 525, SkillShotType.Linear, 250, int.MaxValue, 100) { AllowedCollisionCount = int.MaxValue };
            R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Circular, 250, int.MaxValue, 450){ AllowedCollisionCount = int.MaxValue };
    }
    }
    public class Vladimir : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Vladimir()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Active(SpellSlot.W, 150);
            E = new Spell.Active(SpellSlot.E, 600);
            R = new Spell.Skillshot(SpellSlot.R, 750, SkillShotType.Circular, 250, int.MaxValue, 170);
        }
    }
    public class Volibear : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Volibear()
        {
            Q = new Spell.Active(SpellSlot.Q, 750);
            W = new Spell.Targeted(SpellSlot.W, 395);
            E = new Spell.Active(SpellSlot.E, 415);
            R = new Spell.Active(SpellSlot.R);
        }
    }
    public class Warwick : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Warwick()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 400);
            W = new Spell.Active(SpellSlot.W, 1250);
            E = new Spell.Active(SpellSlot.E, 1500);
            R = new Spell.Targeted(SpellSlot.R, 700);
        }
    }
    public class MonkeyKing : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public MonkeyKing()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Circular);
            E = new Spell.Active(SpellSlot.E, 325);
            R = new Spell.Targeted(SpellSlot.R, 700);
        }
    }
    public class Xerath : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Xerath()
        {
            Q = new Spell.Chargeable(SpellSlot.Q, 750, 1500, 1500, 500, int.MaxValue, 100);
            W = new Spell.Skillshot(SpellSlot.W, 1100, SkillShotType.Circular, 250, int.MaxValue, 100);
            E = new Spell.Skillshot(SpellSlot.E, 1050, SkillShotType.Linear, 250, 1600, 70);
            R = new Spell.Skillshot(SpellSlot.R, 3200, SkillShotType.Circular, 500, int.MaxValue, 120);
        }
    }
    public class XinZhao : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public XinZhao()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 650);
            R = new Spell.Active(SpellSlot.R, 500);
        }
    }
    public class Yasuo : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Yasuo()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 475, SkillShotType.Linear);
            W = new Spell.Skillshot(SpellSlot.W, 400, SkillShotType.Linear);
            E = new Spell.Targeted(SpellSlot.E, 475);
            R = new Spell.Active(SpellSlot.R, 1200);
        }
    }
    public class Yorick : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Yorick()
        {
            Q = new Spell.Active(SpellSlot.Q, 125);
            W = new Spell.Skillshot(SpellSlot.W, 585, SkillShotType.Circular, 250, int.MaxValue, 200);
            E = new Spell.Targeted(SpellSlot.E, 540);
            R = new Spell.Targeted(SpellSlot.R, 835);
        }
    }
    public class Zac : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Zac()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 550, SkillShotType.Linear, 500, int.MaxValue, 120);
            W = new Spell.Active(SpellSlot.W, 350);
            E = new Spell.Chargeable(SpellSlot.E, 0, 1750, 1500, 500, 1500, 250);
            R = new Spell.Active(SpellSlot.R, 300);
        }
    }
    public class Zed : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Zed()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 600);
            W = new Spell.Skillshot(SpellSlot.W, 700, SkillShotType.Circular);
            E = new Spell.Active(SpellSlot.E, 325);
            R = new Spell.Targeted(SpellSlot.R, 700);
        }
    }
    public class Ziggs : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Ziggs()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 850, SkillShotType.Linear, 300, 1700, 130);
            W = new Spell.Active(SpellSlot.W, 1000);
            E = new Spell.Skillshot(SpellSlot.E, 900, SkillShotType.Linear, 250, 1530, 60);
            R = new Spell.Skillshot(SpellSlot.R, 5300, SkillShotType.Circular, 1000, 2800, 500);
        }
    }
    public class Zilean : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Zilean()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Circular, 300, 2000, 150);
            W = new Spell.Active(SpellSlot.W, 700);
            E = new Spell.Targeted(SpellSlot.E, 1000);
            R = new Spell.Targeted(SpellSlot.R, 410);
        }
    }
    public class Zyra : SpellBase
    {
        public sealed override Spell.SpellBase Q { get; set; }
        public sealed override Spell.SpellBase W { get; set; }
        public sealed override Spell.SpellBase E { get; set; }
        public sealed override Spell.SpellBase R { get; set; }
        public Zyra()
        {
            Q = new Spell.Skillshot(SpellSlot.Q, 800, SkillShotType.Linear, 250, int.MaxValue, 85);
            W = new Spell.Skillshot(SpellSlot.W, 825, SkillShotType.Circular, 250, int.MaxValue, 20);
            E = new Spell.Skillshot(SpellSlot.E, 1100, SkillShotType.Linear, 250, 1150, 70);
            R = new Spell.Skillshot(SpellSlot.R, 700, SkillShotType.Circular, 250, 1200, 500);

            QisCC = true;
            EisCC = true;
            RisCC = true;

        }
    }
}
