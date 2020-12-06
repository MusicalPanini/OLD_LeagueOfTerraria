using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Dusts
{
    public class OneFrameDust : ModDust
    {
        Vector2 displacment = Vector2.Zero;

        public override void SetDefaults()
        {
            updateType = -1;
        }

        public override void OnSpawn(Dust dust)
        {
            displacment = dust.position;
        }

        public override bool Update(Dust dust)
        {
            Entity e = (Entity)dust.customData;

            if (e != null)
            {
                dust.position = displacment + e.Center;
            }

            return base.Update(dust);
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return dust.color;
        }
    }
}
