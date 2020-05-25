using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Drakebane_Whip : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Drakebane");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 0;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 3;
            projectile.hide = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (projectile.ai[0] == 0f)
            {
                projectile.localAI[1] = player.itemAnimationMax;
                projectile.restrikeDelay = 10;
            }
            float speedIncrease = (float)player.HeldItem.useAnimation / projectile.localAI[1];

            AI_075(projectile, 12, (int)projectile.localAI[1], true, 2, 39);
        }

        private static Vector2 AI_075(Projectile projectile, float swingLength, int swingTime, bool ignoreTiles, int sndgroup, int sound)
        {
            Player player = Main.player[projectile.owner];
            float halfPi = 1.57079637f;
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);

            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = projectile.velocity.ToRotation();
            }
            float num33 = (float)((projectile.localAI[0].ToRotationVector2().X >= 0f) ? 1 : -1);
            if (projectile.ai[1] <= 0f)
            {
                num33 *= -1f;
            }
            Vector2 vector25 = (num33 * (projectile.ai[0] / swingTime * 6.28318548f - 1.57079637f)).ToRotationVector2();
            vector25.Y *= (float)Math.Sin((double)projectile.ai[1]);
            if (projectile.ai[1] <= 0f)
            {
                vector25.Y *= -1f;
            }
            vector25 = vector25.RotatedBy((double)projectile.localAI[0], default(Vector2));
            projectile.ai[0] += 1f / projectile.MaxUpdates;
            if (projectile.ai[0] < swingTime)
            {
                projectile.velocity += swingLength * vector25;
            }
            else
            {
                projectile.Kill();
            }

            projectile.position = player.RotatedRelativePoint(player.MountedCenter, true) - projectile.Size / 2f;
            projectile.rotation = projectile.velocity.ToRotation() + halfPi;
            projectile.spriteDirection = projectile.direction;
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemTime = Math.Max(player.itemTime, projectile.restrikeDelay);
            player.itemAnimation = Math.Max(3, projectile.restrikeDelay);
            player.itemRotation = (float)Math.Atan2((double)(projectile.velocity.Y * (float)projectile.direction), (double)(projectile.velocity.X * (float)projectile.direction));

            Vector2 vector34 = Main.OffsetsPlayerOnhand[player.bodyFrame.Y / 56] * 2f;
            if (player.direction != 1)
            {
                vector34.X = (float)player.bodyFrame.Width - vector34.X;
            }
            if (player.gravDir != 1f)
            {
                vector34.Y = (float)player.bodyFrame.Height - vector34.Y;
            }
            vector34 -= new Vector2((float)(player.bodyFrame.Width - player.width), (float)(player.bodyFrame.Height - 42)) / 2f;
            projectile.Center = player.RotatedRelativePoint(player.position + vector34, true) - projectile.velocity;

            Vector2 endPoint = projectile.position + projectile.velocity * 2f;

            if (projectile.ai[0] > 1 && !ignoreTiles)
            {
                Vector2 prevPoint = projectile.oldPosition + projectile.oldVelocity * 2f;
                if (!Collision.CanHit(endPoint, projectile.width, projectile.height, prevPoint, projectile.width, projectile.height))
                {
                    if (projectile.ai[0] * 2 < projectile.localAI[1])
                    {
                        projectile.restrikeDelay = player.itemAnimationMax - (int)projectile.ai[0] * 2;
                        projectile.ai[0] = Math.Max(1f, projectile.localAI[1] - projectile.ai[0] + 1);
                        projectile.ai[1] *= -0.9f; 
                        Main.PlaySound(sndgroup, endPoint, sound);
                        Collision.HitTiles(endPoint, endPoint - prevPoint, 8, 8);
                    }
                }
            }

            return endPoint;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            if (Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(), targetHitbox.Size(),
                projectile.Center, projectile.Center + projectile.velocity,
                projectile.width * projectile.scale, ref collisionPoint))
            {
                return true;
            }
            return false;
        }

        public override bool? CanCutTiles()
        {
            DelegateMethods.tilecut_0 = Terraria.Enums.TileCuttingContext.AttackProjectile;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity, (float)projectile.width * projectile.scale, new Utils.PerLinePoint(DelegateMethods.CutTiles));
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int handleHeight = 76;
            int chainHeight = 14;
            int partHeight = 14;
            int tipHeight = 54;
            int partCount = 8;


            Vector2 vector38 = projectile.position + new Vector2((float)projectile.width, (float)projectile.height) / 2f + Vector2.UnitY * projectile.gfxOffY - Main.screenPosition;
            Texture2D projectileTexture = Main.projectileTexture[projectile.type];
            Color alpha3 = projectile.GetAlpha(Lighting.GetColor((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16));

            alpha3 *= projectile.Opacity;

            Rectangle handle = new Rectangle(0, 0, projectileTexture.Width, handleHeight);
            Rectangle chain = new Rectangle(0, handleHeight, projectileTexture.Width, chainHeight);
            Rectangle part = new Rectangle(0, handleHeight + chainHeight, projectileTexture.Width, partHeight);
            Rectangle tip = new Rectangle(0, handleHeight + chainHeight + partHeight, projectileTexture.Width, tipHeight);


            if (projectile.velocity == Vector2.Zero)
            {
                return false;
            }
            SpriteEffects se = SpriteEffects.None;
            if (projectile.spriteDirection < 0)
            {
                se = SpriteEffects.FlipHorizontally;
            }
            float chainCount = projectile.velocity.Length() + 16f - tipHeight / 2;
            bool halfSize = chainCount < partHeight * 4.5f;
            Vector2 normalVel = Vector2.Normalize(projectile.velocity);
            Rectangle currentRect = handle;
            Vector2 gfxOffY = new Vector2(0f, Main.player[projectile.owner].gfxOffY);
            float rotation24 = projectile.rotation + 3.14159274f;
            spriteBatch.Draw(projectileTexture, projectile.Center.Floor() - Main.screenPosition + gfxOffY, new Microsoft.Xna.Framework.Rectangle?(currentRect), alpha3, rotation24, currentRect.Size() / 2f - Vector2.UnitY * 4f, projectile.scale, se, 0f);
            chainCount -= 40f * projectile.scale;
            Vector2 centre = projectile.Center.Floor();
            centre += normalVel * projectile.scale * handle.Height / 2;
            Vector2 centreOffY;
            currentRect = chain;
            if (chainCount > 0f)
            {
                float i = 0f;
                while (i + 1f < chainCount)
                {
                    if (chainCount - i < (float)currentRect.Height)
                    {
                        currentRect.Height = (int)(chainCount - i);
                    }
                    centreOffY = centre + gfxOffY;
                    alpha3 = projectile.GetAlpha(Lighting.GetColor((int)centreOffY.X / 16, (int)centreOffY.Y / 16));
                    spriteBatch.Draw(projectileTexture, centreOffY - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(currentRect), alpha3, rotation24, new Vector2((float)(currentRect.Width / 2), 0f), projectile.scale, se, 0f);
                    i += (float)currentRect.Height * projectile.scale;
                    centre += normalVel * (float)currentRect.Height * projectile.scale;
                }
            }
            Vector2 centre2 = centre;
            centre = projectile.Center.Floor();
            centre += normalVel * projectile.scale * chain.Height / 2;
            currentRect = part;
            if (halfSize)
            {
                partCount /= 2;
            }
            float num200 = chainCount;
            if (chainCount > 0f)
            {
                float num201 = 0f;
                float num202 = num200 / (float)partCount;
                num201 += num202 * 0.25f;
                //centre += normalVel * num202 * 0.25f;
                centre += normalVel * projectile.scale * handle.Height / 2;
                for (int i = 0; i < partCount; i++)
                {
                    float num204 = num202;
                    if (i == 0)
                    {
                        num204 *= 0.75f;
                    }
                    centreOffY = centre + gfxOffY;
                    alpha3 = projectile.GetAlpha(Lighting.GetColor((int)centreOffY.X / 16, (int)centreOffY.Y / 16));
                    spriteBatch.Draw(projectileTexture, centreOffY - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(currentRect), alpha3, rotation24, new Vector2((float)(currentRect.Width / 2), 0f), projectile.scale, se, 0f);
                    num201 += num204;
                    centre += normalVel * num204;
                }
            }
            currentRect = tip;
            Vector2 centreOffY2 = centre2 + gfxOffY;
            alpha3 = projectile.GetAlpha(Lighting.GetColor((int)centreOffY2.X / 16, (int)centreOffY2.Y / 16));

            spriteBatch.Draw(projectileTexture, centreOffY2 - Main.screenPosition, new Microsoft.Xna.Framework.Rectangle?(currentRect), alpha3, rotation24, projectileTexture.Frame(1, 1, 0, 0).Top(), projectile.scale, se, 0f);

            return false;
        }

        public override void Kill(int timeLeft)
        {
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }
    }
}



