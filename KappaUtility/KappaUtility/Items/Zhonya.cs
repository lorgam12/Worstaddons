namespace KappaUtility.Items
{
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;

    internal class Zhonya
    {
        private static readonly string[] DangerSpells =
            {
                "AzirR", "zedult", "ViR", "SyndraR", "CaitlynAceintheHole",
                "LissandraR", "GarenR", "DariusR", "BlindMonkRKick"
            };

        internal static void OnLoad()
        {
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!(args.Target is AIHeroClient))
            {
                return;
            }

            var caster = sender;
            var target = (AIHeroClient)args.Target;

            if (!Defensive.Zhonyas.IsOwned() || !Defensive.Zhonyas.IsReady()
                || (!(caster is AIHeroClient) || caster == null || target == null || !target.IsMe
                    || Defensive.DefMenu["Zhonyas"].Cast<CheckBox>().CurrentValue
                    || Defensive.DefMenu["ZhonyasD"].Cast<CheckBox>().CurrentValue))
            {
                return;
            }

            foreach (var spell in
                DangerSpells.Where(spell => args.SData.Name == spell && caster.IsEnemy)
                    .Where(spell => spell != null && args.SData.Name == spell))
            {
                Core.DelayAction(() => Defensive.Zhonyas.Cast(), 50);
            }
        }
    }
}