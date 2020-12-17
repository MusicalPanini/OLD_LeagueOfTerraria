using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace TerraLeague.Backgrounds
{
    public class TargonSky : CustomSky
    {
        private bool _isActive;
        private float _opacity;
        private bool _isLeaving;

        private UnifiedRandom _random = new UnifiedRandom();
        private Texture2D _bgTexture;
        private Texture2D[] _starTextures;
        private Star[] _stars;
        private float _fadeOpacity;

        private struct Star
        {
            public Vector2 Position;
            public float Depth;
            public int TextureIndex;
            public float SinOffset;
            public float AlphaFrequency;
            public float AlphaAmplitude;
        }

        public override void OnLoad()
        {
            _bgTexture = TerraLeague.instance.GetTexture("Backgrounds/Targon_Back");
            _starTextures = new Texture2D[7];
            for (int i = 1; i < _starTextures.Length + 1; i++)
            {
                this._starTextures[i - 1] = TerraLeague.instance.GetTexture("Gores/Star_" + i.ToString());
            }

            base.OnLoad();
        }

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

                if (this._isActive)
                {
                    this._fadeOpacity = Math.Min(1f, 0.01f + this._fadeOpacity);
                }
                else
                {
                    this._fadeOpacity = Math.Max(0f, this._fadeOpacity - 0.01f);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            if (!(minDepth < 1f) && maxDepth != 3.40282347E+38f)
            {
                return;
            }
            //spriteBatch.Draw(_bgTexture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), null, new Color(1f * _opacity, 1f * _opacity, 1f * _opacity, 1 * _opacity), 0, Vector2.Zero, SpriteEffects.None, 1);

            int num = -1;
            int num2 = 0;
            for (int i = 0; i < this._stars.Length; i++)
            {
                float depth = this._stars[i].Depth;
                if (num == -1 && depth < maxDepth)
                {
                    num = i;
                }
                if (depth <= minDepth)
                {
                    break;
                }
                num2 = i;
            }
            if (num != -1)
            {
                float scale = Math.Min(1f, (Main.screenPosition.Y - 1000f) / 1000f);
                Vector2 screenCenter = Main.screenPosition + new Vector2((float)(Main.screenWidth >> 1), (float)(Main.screenHeight >> 1));
                Rectangle rectangle = new Rectangle(-1000, -1000, 4000, 2000);
                for (int j = num; j < num2; j++)
                {
                    Vector2 vector = new Vector2(1f / this._stars[j].Depth, 2f / this._stars[j].Depth);
                    Vector2 vector2 = (this._stars[j].Position - screenCenter) * vector + screenCenter - Main.screenPosition;
                    vector2.Y -= 720;
                    if (rectangle.Contains((int)vector2.X, (int)vector2.Y))
                    {
                        float value4 = (float)Math.Sin((double)(this._stars[j].AlphaFrequency * Main.GlobalTime + this._stars[j].SinOffset)) * this._stars[j].AlphaAmplitude + this._stars[j].AlphaAmplitude;
                        float num3 = (float)Math.Sin((double)(this._stars[j].AlphaFrequency * Main.GlobalTime * 5f + this._stars[j].SinOffset)) * 0.1f - 0.1f;
                        value4 = MathHelper.Clamp(value4, 0f, 1f);
                        Texture2D texture2D = this._starTextures[this._stars[j].TextureIndex];
                        spriteBatch.Draw(
                            texture2D, 
                            vector2, 
                            null, 
                            new Color(1f * _opacity, 1f * _opacity, 1f * _opacity, 1 * _opacity) /*Color.White * scale * value4 * 0.8f * (1f - num3) * this._fadeOpacity*/, 
                            0f, 
                            new Vector2((float)(texture2D.Width >> 1), (float)(texture2D.Height >> 1)), 
                            (vector.X * 0.5f + 0.5f) * (value4 * 1.2f + 0.7f), 
                            SpriteEffects.None, 
                            0f);
                    }
                }
            }
        }

        public override float GetCloudAlpha()
        {
            return (1f - _fadeOpacity) * 0.3f + 0.7f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            _isActive = true;
            _isLeaving = false;

            this._fadeOpacity = 0.002f;
            this._isActive = true;
            int num = 200;
            int num2 = 10;
            this._stars = new Star[num * num2];
            int num3 = 0;
            for (int i = 0; i < num; i++)
            {
                float num4 = (float)i / (float)num;
                for (int j = 0; j < num2; j++)
                {
                    float num5 = (float)j / (float)num2;
                    this._stars[num3].Position.X = num4 * (float)Main.maxTilesX * 16f;
                    this._stars[num3].Position.Y = num5 * ((float)Main.worldSurface * 16f + 2000f);
                    this._stars[num3].Depth = this._random.NextFloat() * 24f + 1.5f;
                    this._stars[num3].TextureIndex = this._random.Next(this._starTextures.Length);
                    this._stars[num3].SinOffset = this._random.NextFloat() * 6.28f;
                    this._stars[num3].AlphaAmplitude = this._random.NextFloat() * 2f;
                    this._stars[num3].AlphaFrequency = this._random.NextFloat() + 1f;
                    num3++;
                }
            }
            Array.Sort(this._stars, this.SortMethod);
        }

        private int SortMethod(Star meteor1, Star meteor2)
        {
            return meteor2.Depth.CompareTo(meteor1.Depth);
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