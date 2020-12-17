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
    public class Star_1 : ModGore
    {
        public override void OnSpawn(Gore gore)
        {
            gore.sticky = false;
            gore.drawOffset = new Vector2(-9, -9);
            gore.alpha = 255;
            base.OnSpawn(gore);
        }

        public override bool Update(Gore gore)
        {
            Lighting.AddLight(gore.position, new Color(255, 0, 0).ToVector3() * (1 - (gore.alpha/255f)));
            gore.velocity = new Vector2(0, -0.2f);

            if (gore.timeLeft <= 0)
            {
                gore.alpha += 3;
            }
            else if (gore.alpha > 10)
            {
                gore.alpha -= 25;
            }

            //gore.scale = trueScale + ((float)Math.Sin(gore.timeLeft * 0.05) * 0.5f);

            return base.Update(gore);
        }

        public override Color? GetAlpha(Gore gore, Color lightColor)
        {
            return null;
        }
    }
}
