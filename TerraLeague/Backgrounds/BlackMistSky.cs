using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace TerraLeague.Backgrounds
{
    public class BlackMistSky : CustomSky
    {
        private bool isActive;
        private bool _isActive;
        private float intensity;
        private float _opacity;
        private bool _isLeaving;


        public override void Update(GameTime gameTime)
        {
            if (!Main.gamePaused && Main.hasFocus)
            {
                System.TimeSpan elapsedGameTime;
                if (this._isLeaving)
                {
                    float opacity = this._opacity;
                    elapsedGameTime = gameTime.ElapsedGameTime;
                    this._opacity = opacity - (float)elapsedGameTime.TotalSeconds;
                    if (this._opacity < 0f)
                    {
                        this._isActive = false;
                        this._opacity = 0f;
                    }
                }
                else
                {
                    float opacity2 = this._opacity;
                    elapsedGameTime = gameTime.ElapsedGameTime;
                    this._opacity = opacity2 + (float)elapsedGameTime.TotalSeconds;
                    if (this._opacity > 1f)
                    {
                        this._opacity = 1f;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (!(minDepth < 1f) && maxDepth != 3.40282347E+38f)
            {
                return;
            }
            float quotient = (float)Main.time / 3600f;

            if (quotient > 1)
                quotient = 1;

            float scale = System.Math.Min(1f,  1.5f);
            Color color = new Color(new Vector4(0, 2.5f, 1f, 0.3f) * 1f * Main.bgColor.ToVector4()) * this._opacity * scale * quotient;


                spriteBatch.Draw(TerraLeague.instance.GetTexture("Backgrounds/Fog"), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), color);
        }

        public override float GetCloudAlpha()
        {
            return 0f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            this._isActive = true;
            this._isLeaving = false;
        }

        public override void Deactivate(params object[] args)
        {
            this._isLeaving = true;
        }

        public override void Reset()
        {
            this._opacity = 0f;
            this._isActive = false;
        }

        public override bool IsActive()
        {
            return this._isActive;
        }
    }
}