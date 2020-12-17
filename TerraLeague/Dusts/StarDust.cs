using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Dusts
{
    public class StarDust : ModDust
    {
        public override void SetDefaults()
        {
            updateType = -1;
        }

        public override void OnSpawn(Dust dust)
        {
            dust.frame = new Rectangle(0, 0, 9, 9);
        }

        public override bool Update(Dust dust)
        {
            dust.rotation = 0;
            if (!dust.noLight)
                Lighting.AddLight(dust.position, dust.color.ToVector3() * (1 - (dust.alpha / 255f)));
            return base.Update(dust);
        }

        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return dust.color;
        }
    }
}
