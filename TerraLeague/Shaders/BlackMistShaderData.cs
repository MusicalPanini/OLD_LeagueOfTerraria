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
    public class BlackMistShaderData : ScreenShaderData
    {
        private Vector2 _texturePosition = Vector2.Zero;

        public BlackMistShaderData(string passName)
            : base(passName)
        {
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 vector = new Vector2(0f - Main.windSpeed, -1f) * new Vector2(0.01f, 0.001f);
            vector.Normalize();
            vector *= new Vector2(0.025f, 0);
            if (!Main.gamePaused && Main.hasFocus)
            {
                _texturePosition += vector * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            _texturePosition.X %= 10f;
            _texturePosition.Y %= 10f;
            base.UseDirection(vector);
            base.Update(gameTime);
            Main.bgColor = Color.Green;
            //if (_passName == "FilterSandstormForeground")
            //{
            //    base.UseOpacity(0.2f);
            //    base.UseIntensity(3.5f);
            //    base.UseColor(0,2,1);
            //    base.UseSecondaryColor(0,0,0);
            //}
            //else
            //{
            //    base.UseColor(0,1,0);
            //    base.UseSecondaryColor(0,0,0);
            //    base.UseIntensity(5f);
            //    base.UseOpacity(1f);
            //}
        }

        public override void Apply()
        {
            base.UseTargetPosition(this._texturePosition);
            base.Apply();
        }

    }
}
