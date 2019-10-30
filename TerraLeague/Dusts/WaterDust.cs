using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Dusts
{
    public class WaterDust : ModDust
    {
        public override void SetDefaults()
        {
            updateType = 33;
        }

        public override void OnSpawn(Dust dust)
        {
            switch (Main.rand.Next(1, 4))
            {
                case 1:
                    dust.color = new Color(0, 0, 255);
                    break;
                case 2:
                    dust.color = new Color(0, 0, 255);
                    break;
                case 3:
                    dust.color = new Color(0, 150, 255);
                    break;

            }
        }
    }
}
