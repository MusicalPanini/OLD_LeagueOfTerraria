using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class EyeOfTheVoid_Lazer : ModProjectile
    {
        private const float MaxChargeValue = 50f;
        private const float MoveDistance = 60f;

        public float Distance
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }

        public float Charge
        {
            get { return projectile.localAI[0]; }
            set { projectile.localAI[0] = value; }
        }

        public bool IsAtMaxCharge => Charge == MaxChargeValue;

        public bool AtMaxCharge { get { return Charge >= MaxChargeValue; } }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.hide = true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (AtMaxCharge)
            {
                DrawLaser(spriteBatch, Main.projectileTexture[projectile.type], Main.player[projectile.owner].Center,
                    projectile.velocity, 10, projectile.damage, -1.57f, 1f, 1000f, Color.White, (int)MoveDistance);
            }
            return false;
        }

        public void DrawLaser(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 unit, float step, int damage, float rotation = 0f, float scale = 1f, float maxDist = 2000f, Color color = default(Color), int transDist = 50)
        {
            Vector2 origin = start;
            float r = unit.ToRotation() + rotation;

            #region Draw laser body
            for (float i = transDist; i <= Distance; i += step)
            {
                Color c = Color.White;
                origin = start + i * unit;
                spriteBatch.Draw(texture, origin - Main.screenPosition,
                    new Rectangle(0, 26, 28, 26), i < transDist ? Color.Transparent : c, r,
                    new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            }
            #endregion

            #region Draw laser tail
            spriteBatch.Draw(texture, start + unit * (transDist - step) - Main.screenPosition,
                new Rectangle(0, 0, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            #endregion

            #region Draw laser head
            spriteBatch.Draw(texture, start + (Distance + step) * unit - Main.screenPosition,
                new Rectangle(0, 52, 28, 26), Color.White, r, new Vector2(28 * .5f, 26 * .5f), scale, 0, 0);
            #endregion
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (AtMaxCharge)
            {
                Player player = Main.player[projectile.owner];
                Vector2 unit = projectile.velocity;
                float point = 0f;
                return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), player.Center,
                    player.Center + unit * Distance, 6, ref point);
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.immune[projectile.owner] = 10;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0 && AtMaxCharge)
            {
                projectile.soundDelay = 25;
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 15), projectile.position);
            }

            Vector2 mousePos = Main.MouseWorld;
            Player player = Main.player[projectile.owner];

            #region Set projectile position
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 diff = mousePos - player.Center;
                diff.Normalize();
                projectile.velocity = diff;
                projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                projectile.netUpdate = true;
            }
            projectile.position = player.Center + projectile.velocity * MoveDistance;
            projectile.timeLeft = 2;
            int dir = projectile.direction;
            player.ChangeDir(dir);
            player.heldProj = projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
            #endregion

            #region Charging process
            if (!player.channel)
            {
                projectile.Kill();
            }
            else
            {
                if (Main.time % 10 < 1)
                {
                    if (!player.CheckMana(player.inventory[player.selectedItem].mana))
                    {
                        projectile.Kill();
                    }
                    else if (AtMaxCharge)
                    {
                        player.CheckMana(player.inventory[player.selectedItem].mana, true);
                    }
                }
                Vector2 offset = projectile.velocity;
                offset *= MoveDistance - 20;
                Vector2 pos = player.Center + offset - new Vector2(10, 10);
                if (Charge < 60)
                {
                    Charge++;
                }
                int chargeFact = (int)(Charge / 20f);
                Vector2 dustVelocity = Vector2.UnitX * 18f;
                dustVelocity = dustVelocity.RotatedBy(projectile.rotation - 1.57f, default(Vector2));
                Vector2 spawnPos = projectile.Center + dustVelocity;
                for (int k = 0; k < chargeFact + 1; k++)
                {
                    Vector2 spawn = spawnPos + ((float)Main.rand.NextDouble() * 6.28f).ToRotationVector2() * (12f - chargeFact * 2);
                    Dust dust = Dust.NewDustDirect(pos, 20, 20, 112, projectile.velocity.X / 2f, projectile.velocity.Y / 2f, 0, default(Color), 1f);
                    dust.velocity = Vector2.Normalize(spawnPos - spawn) * 1.5f * (10f - chargeFact * 2f) / 10f;
                    dust.noGravity = true;
                    dust.scale = Main.rand.Next(10, 20) * 0.05f;
                }
            }
            #endregion

            #region Set laser tail position and dusts
            if (!AtMaxCharge)
            {
                return;
            }

            Vector2 start = player.Center;
            Vector2 unit = projectile.velocity;
            unit *= -1;
            for (Distance = MoveDistance; Distance <= 300; Distance += 4f)
            {
                start = player.Center + projectile.velocity * Distance;
                if (!Collision.CanHitLine(player.Center, 0, 0, start,0, 0))
                {
                    Distance -= 4f;
                    break;
                }
            }

            Vector2 dustPos = player.Center + projectile.velocity * (Distance + 16);
            //Imported dust code from source because I'm lazy
            for (int i = 0; i < 2; ++i)
            {
                float num1 = projectile.velocity.ToRotation() + (Main.rand.Next(2) == 1 ? -1.0f : 1.0f) * 1.57f;
                float num2 = (float)(Main.rand.NextDouble() * 0.8f + 1.0f);
                Vector2 dustVel = new Vector2((float)Math.Cos(num1) * num2, (float)Math.Sin(num1) * num2);
                Dust dust = Dust.NewDustDirect(dustPos, 0, 0, 112, dustVel.X, dustVel.Y, 0, new Color(59, 0, 255), 3f);
                dust.noGravity = true;
            }
            if (Main.rand.NextBool(5))
            {
                Vector2 offset = projectile.velocity.RotatedBy(1.57f, new Vector2()) * ((float)Main.rand.NextDouble() - 0.5f) * projectile.width;
                Dust dust = Dust.NewDustDirect(dustPos + offset - Vector2.One * 4f, 8, 8, 112, 0.0f, 0.0f, 100, new Color(), 1.5f);
                dust.velocity = dust.velocity * 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);

                unit = dustPos - Main.player[projectile.owner].Center;
                unit.Normalize();
                dust = Dust.NewDustDirect(Main.player[projectile.owner].Center + 55 * unit, 8, 8, 112, 0.0f, 0.0f, 100, new Color(), 1.5f);
                dust.velocity = dust.velocity * 0.5f;
                dust.velocity.Y = -Math.Abs(dust.velocity.Y);
            }
            #endregion

            DelegateMethods.v3_1 = new Vector3(0.8f, 0.8f, 1f);
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - MoveDistance), 26,
                DelegateMethods.CastLight);
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void CutTiles()
        {
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 unit = projectile.velocity;
            Utils.PlotTileLine(projectile.Center, projectile.Center + unit * Distance, (projectile.width + 16) * projectile.scale, DelegateMethods.CutTiles);
        }
    }
}
