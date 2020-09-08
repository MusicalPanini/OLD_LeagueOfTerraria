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

            float quotient;
            if (Main.time <= 3600)
                quotient = (float)Main.time / 3600f;
            else if (Main.time >= 28800)
                quotient = (float)(32400 - Main.time) / 3600f;
            else
                quotient = 1;

            if (_passName == "FilterSandstormForeground")
            {
                base.UseOpacity(0.2f * quotient * TerraLeague.fogIntensity);
                base.UseIntensity(3.5f * quotient * TerraLeague.fogIntensity);
            }
            else
            {
                base.UseIntensity(5 * quotient * TerraLeague.fogIntensity);
                base.UseOpacity(1 * quotient * TerraLeague.fogIntensity);
            }
        }
        public override void Apply()
        {
            base.UseTargetPosition(this._texturePosition);
            base.Apply();
        }

    }
}
