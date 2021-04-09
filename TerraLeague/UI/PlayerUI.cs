using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria.ID;
using TerraLeague.Items.SummonerSpells;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.Boots;
using static Terraria.ModLoader.ModContent;
using TerraLeague.Items.Weapons;

namespace TerraLeague.UI
{
    internal class PlayerUI : UIState
    {
        public override void OnInitialize()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (player.HeldItem.type == ItemType<Whisper>() && Main.myPlayer == player.whoAmI)
            {
                Texture2D texture = null;
                int frame = 0;

                switch (modPlayer.WhisperShotsLeft)
                {
                    case 4:
                        texture = TerraLeague.instance.GetTexture("UI/Ammo1");
                        break;
                    case 3:
                        texture = TerraLeague.instance.GetTexture("UI/Ammo2");
                        break;
                    case 2:
                        texture = TerraLeague.instance.GetTexture("UI/Ammo3");
                        break;
                    case 1:
                        texture = TerraLeague.instance.GetTexture("UI/Ammo4");
                        frame = ((int)Main.time % 10);
                        frame = frame > 5 ? 0 : 1;
                        break;
                    case 0:
                        texture = TerraLeague.instance.GetTexture("UI/AmmoNone");
                        break;
                    default:
                        break;
                }

                if (texture != null)
                {
                    Main.spriteBatch.Draw
                       (
                           texture,
                           new Rectangle((int)(player.MountedCenter.X - Main.screenPosition.X - 32), (int)(player.MountedCenter.Y - Main.screenPosition.Y - (player.breathMax == player.breath && player.lavaMax == player.lavaTime && TerraLeague.UseModResourceBar ? 32 : 40) - 21), 64, 16),
                           new Rectangle(0, frame * 16, 64, 16),
                           Color.White,
                           0,
                           Vector2.Zero,
                           SpriteEffects.None,
                           0f
                       );
                }
            }

            if (player.HeldItem.type == ItemType<BrassShotgun>() && Main.myPlayer == player.whoAmI)
            {
                Texture2D texture = null;
                switch (modPlayer.DestinyShotsLeft)
                {
                    case 2:
                        texture = TerraLeague.instance.GetTexture("UI/ShotgunAmmo2");
                        break;
                    case 1:
                        texture = TerraLeague.instance.GetTexture("UI/ShotgunAmmo1");
                        break;
                    case 0:
                        texture = TerraLeague.instance.GetTexture("UI/ShotgunAmmoNone");
                        break;
                    default:
                        break;
                }

                if (texture != null)
                {
                    Main.spriteBatch.Draw
                       (
                           texture,
                           new Rectangle((int)(player.MountedCenter.X - Main.screenPosition.X - 32), (int)(player.MountedCenter.Y - Main.screenPosition.Y - (player.breathMax == player.breath && player.lavaMax == player.lavaTime && TerraLeague.UseModResourceBar ? 32 : 40) - 21), 64, 16),
                           new Rectangle(0, 0, 64, 16),
                           Color.White,
                           0,
                           Vector2.Zero,
                           SpriteEffects.None,
                           0f
                       );
                }
            }

            if (player.HeldItem.type == ItemType<EchoingFlameCannon>() && Main.myPlayer == player.whoAmI)
            {
                Texture2D textureLT = TerraLeague.instance.GetTexture("UI/EchoingFlames_LT");
                Texture2D textureLM = TerraLeague.instance.GetTexture("UI/EchoingFlames_LM"); ;
                Texture2D textureLB = TerraLeague.instance.GetTexture("UI/EchoingFlames_LB"); ;
                Texture2D textureRB = TerraLeague.instance.GetTexture("UI/EchoingFlames_RB"); ;
                Texture2D textureRM = TerraLeague.instance.GetTexture("UI/EchoingFlames_RM"); ;
                Texture2D textureRT = TerraLeague.instance.GetTexture("UI/EchoingFlames_RT"); ;


                if (modPlayer.echoingFlames_LT <= 0)
                    Main.spriteBatch.Draw
                       (
                           textureLT,
                           new Rectangle((int)(player.MountedCenter.X - Main.screenPosition.X - (textureLT.Width / 2)), (int)(player.MountedCenter.Y - Main.screenPosition.Y - (textureLT.Height / 2)), textureLT.Width, textureLT.Height),
                           new Rectangle(0, 0, textureLT.Width, textureLT.Height),
                           Color.White,
                           0,
                           Vector2.Zero,
                           SpriteEffects.None,
                           0f
                       );

                if (modPlayer.echoingFlames_LM <= 0)
                    Main.spriteBatch.Draw
                       (
                           textureLM,
                           new Rectangle((int)(player.MountedCenter.X - Main.screenPosition.X - (textureLM.Width / 2)), (int)(player.MountedCenter.Y - Main.screenPosition.Y - (textureLM.Height / 2)), textureLM.Width, textureLM.Height),
                           new Rectangle(0, 0, textureLM.Width, textureLM.Height),
                           Color.White,
                           0,
                           Vector2.Zero,
                           SpriteEffects.None,
                           0f
                       );

                if (modPlayer.echoingFlames_LB <= 0)
                    Main.spriteBatch.Draw
                       (
                           textureLB,
                           new Rectangle((int)(player.MountedCenter.X - Main.screenPosition.X - (textureLB.Width / 2)), (int)(player.MountedCenter.Y - Main.screenPosition.Y - (textureLB.Height / 2)), textureLB.Width, textureLB.Height),
                           new Rectangle(0, 0, textureLB.Width, textureLB.Height),
                           Color.White,
                           0,
                           Vector2.Zero,
                           SpriteEffects.None,
                           0f
                       );

                if (modPlayer.echoingFlames_RB <= 0)
                    Main.spriteBatch.Draw
                       (
                           textureRB,
                           new Rectangle((int)(player.MountedCenter.X - Main.screenPosition.X - (textureRB.Width / 2)), (int)(player.MountedCenter.Y - Main.screenPosition.Y - (textureRB.Height / 2)), textureRB.Width, textureRB.Height),
                           new Rectangle(0, 0, textureRB.Width, textureRB.Height),
                           Color.White,
                           0,
                           Vector2.Zero,
                           SpriteEffects.None,
                           0f
                       );

                if (modPlayer.echoingFlames_RM <= 0)
                    Main.spriteBatch.Draw
                       (
                           textureRM,
                           new Rectangle((int)(player.MountedCenter.X - Main.screenPosition.X - (textureRM.Width / 2)), (int)(player.MountedCenter.Y - Main.screenPosition.Y - (textureRM.Height / 2)), textureRM.Width, textureRM.Height),
                           new Rectangle(0, 0, textureRM.Width, textureRM.Height),
                           Color.White,
                           0,
                           Vector2.Zero,
                           SpriteEffects.None,
                           0f
                       );

                if (modPlayer.echoingFlames_RT <= 0)
                    Main.spriteBatch.Draw
                       (
                           textureRT,
                           new Rectangle((int)(player.MountedCenter.X - Main.screenPosition.X - (textureRT.Width / 2)), (int)(player.MountedCenter.Y - Main.screenPosition.Y - (textureRT.Height / 2)), textureRT.Width, textureRT.Height),
                           new Rectangle(0, 0, textureRT.Width, textureRT.Height),
                           Color.White,
                           0,
                           Vector2.Zero,
                           SpriteEffects.None,
                           0f
                       );
            }

            if (modPlayer.onslaught)
            {
                Texture2D texture = TerraLeague.instance.GetTexture("UI/OnslaughtRange");

                if (texture != null)
                {
                    Color c = Color.White;
                    c.A = 255;

                    Main.spriteBatch.Draw
                       (
                           texture,
                           new Rectangle((int)(player.MountedCenter.X - Main.screenPosition.X - 300), (int)(player.MountedCenter.Y - Main.screenPosition.Y - 300), 600, 600),
                           new Rectangle(0, 0, 600, 600),
                           c,
                           0,
                           Vector2.Zero,
                           SpriteEffects.None,
                           0f
                       );
                }
            }
        }
        
    }
}