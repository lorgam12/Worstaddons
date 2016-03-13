namespace KappaEkko.Events
{
    using System;

    using EloBuddy;

    internal class OnDelete
    {
        public static void Delete(GameObject sender, EventArgs args)
        {
            var particle = sender as Obj_GeneralParticleEmitter;
            if (particle != null)
            {
                if (particle.Name.Equals("Ekko_Base_R_TrailEnd.troy"))
                {
                    Spells.EkkoREmitter = null;
                }
            }
        }
    }
}