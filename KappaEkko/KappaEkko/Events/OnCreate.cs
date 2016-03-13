namespace KappaEkko.Events
{
    using System;

    using EloBuddy;

    internal class OnCreate
    {
        public static void Create(GameObject sender, EventArgs args)
        {
            var particle = sender as Obj_GeneralParticleEmitter;
            if (particle != null)
            {
                if (particle.Name.Equals("Ekko_Base_R_TrailEnd.troy"))
                {
                    Spells.EkkoREmitter = particle;
                }
            }
        }
    }
}