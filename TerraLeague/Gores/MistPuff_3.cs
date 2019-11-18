using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Gores
{
    public class MistPuff_3 : ModGore
    {
        public override void OnSpawn(Gore gore)
        {
            gore.sticky = false;
            updateType = 99;
            gore.alpha = 100;
            base.OnSpawn(gore);
        }

        public override bool Update(Gore gore)
        {
            Lighting.AddLight(gore.position, new Color(5, 245, 150).ToVector3());

            gore.velocity.Y *= 0.98f;
            gore.velocity.X *= 0.98f;
            gore.scale -= 0.007f;
            if ((double)gore.scale < 0.1)
            {
                gore.scale = 0.1f;
                gore.alpha = 255;
            }

            return base.Update(gore);
        }
    }
}
