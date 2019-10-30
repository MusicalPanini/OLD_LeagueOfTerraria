using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Dusts
{
    public class BubbledBubble : ModDust
    {
        public override void SetDefaults()
        {
            updateType = -1;
        }

        public override void OnSpawn(Dust dust)
        {
        }

        public override bool Update(Dust dust)
        {
            dust.rotation = 0;
            dust.position.Y -= 1;
            return base.Update(dust);
        }
    }
}
