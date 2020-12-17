using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Shaders;

namespace TerraLeague.Shaders
{
    public class TargonShaderData : ScreenShaderData
    {
        private Vector2 _texturePosition = Vector2.Zero;

        float red
        {
            get
            {
                return 1;
            }
        }

        float green
        {
            get
            {
                return 1;
            }
        }

        float blue
        {
            get
            {
                return 1;
            }
        }

        public TargonShaderData(string passName)
            : base(passName)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _texturePosition.X %= 10f;
            _texturePosition.Y %= 10f;
            base.Update(gameTime);


            UseColor(0.5f * (float)Math.Sin(MathHelper.ToRadians((float)(Main.time * 0.2f))) + 0.5f, 0.1f, 0.5f * (float)Math.Sin(MathHelper.ToRadians((float)(Main.time * 0.4f))) + 1f);
            base.UseIntensity(1);
            base.UseOpacity(0.5f);
        }
        public override void Apply()
        {
            base.UseTargetPosition(this._texturePosition);
            base.Apply();
        }

    }
}
