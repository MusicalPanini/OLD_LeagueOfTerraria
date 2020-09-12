using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace TerraLeague.Backgrounds
{
    public class TargonSky : CustomSky
    {
        private bool _isActive;
        private float _opacity;
        private bool _isLeaving;


        public override void Update(GameTime gameTime)
        {
            if (!Main.gamePaused && Main.hasFocus)
            {
                System.TimeSpan elapsedGameTime;
                if (_isLeaving)
                {
                    float opacity = _opacity;
                    elapsedGameTime = gameTime.ElapsedGameTime;
                    _opacity = opacity - (float)elapsedGameTime.TotalSeconds;
                    if (_opacity < 0f)
                    {
                        _isActive = false;
                        _opacity = 0f;
                    }
                }
                else
                {
                    float opacity2 = _opacity;
                    elapsedGameTime = gameTime.ElapsedGameTime;
                    _opacity = opacity2 + (float)elapsedGameTime.TotalSeconds;
                    if (_opacity > 1f)
                    {
                        _opacity = 1f;
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
            float scale = System.Math.Min(1f,  1.5f);
            Color color = new Color(new Vector4(0, 0.5f, 1f, 0.3f) * 1f * Main.bgColor.ToVector4()) * _opacity;

            var texture = TerraLeague.instance.GetTexture("Backgrounds/Targon_Back");

            spriteBatch.Draw(texture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), null, new Color(1f * _opacity, 1f * _opacity, 1f * _opacity, 1 * _opacity), 0, Vector2.Zero, SpriteEffects.None, 1);
        }

        public override float GetCloudAlpha()
        {
            return 1f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            _isActive = true;
            _isLeaving = false;
        }

        public override void Deactivate(params object[] args)
        {
            _isLeaving = true;
        }

        public override void Reset()
        {
            _opacity = 0f;
            _isActive = false;
        }

        public override bool IsActive()
        {
            return _isActive;
        }
    }
}