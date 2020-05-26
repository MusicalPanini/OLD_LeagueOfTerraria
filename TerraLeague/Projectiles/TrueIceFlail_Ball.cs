using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class TrueIceFlail_Ball : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Ice Flail");
        }

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.alpha = 0;
            projectile.width = 22;
            projectile.height = 22;
            projectile.melee = true;
            projectile.penetrate = -1;
            projectile.scale = 1.25f;
            projectile.tileCollide = false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = mod.GetTexture("Projectiles/TrueIceFlail_Chain");

            Vector2 position = projectile.Bottom.RotatedBy(projectile.rotation, projectile.Center);
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1.35f, SpriteEffects.None, 0.0f);
                }
            }

            return true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 113, 0f, 0f, 100, default(Color));
            dust.velocity *= 0.2f;
            dust.scale *= 0.7f;

            if (!player.active || player.dead || Vector2.Distance(projectile.Center, player.Center) > 900f)
            {
                projectile.Kill();
            }
            else if (Main.myPlayer == projectile.owner && Main.mapFullscreen)
            {
                projectile.Kill();
            }
            else
            {
                Vector2 mountedCenter = player.MountedCenter;
                bool flag2 = false;
                int throwTimerMax = 14;
                float throwSpeed = 24;//24f;
                float ProjectileMaxDistance = 800f;
                float maxReturnSpeed = 3f;
                float returnMagnitude = 16f;
                int generalHitCooldown = 60;
                int swingHitCooldown = 60;
                int throwHitCooldown = 60;
                float swingDistance = 45f;
                float swingSpeed = 90f;

                float speedMultiplier = 1f / player.meleeSpeed;
                throwSpeed *= speedMultiplier;
                maxReturnSpeed *= speedMultiplier;
                returnMagnitude *= speedMultiplier;
                swingSpeed *= player.meleeSpeed;

                projectile.localNPCHitCooldown = generalHitCooldown;
                switch ((int)projectile.ai[0])
                {
                    case 0:
                        flag2 = true;
                        if (projectile.owner == Main.myPlayer)
                        {
                            Vector2 CursorPosition = Main.MouseWorld - mountedCenter;
                            CursorPosition = CursorPosition.SafeNormalize(Vector2.UnitX * (float)player.direction);
                            player.ChangeDir((CursorPosition.X > 0f) ? 1 : (-1));
                            if (!player.channel)
                            {
                                projectile.ai[0] = 1f;
                                projectile.ai[1] = 0f;
                                projectile.velocity = CursorPosition * throwSpeed + player.velocity;
                                projectile.Center = mountedCenter;
                                projectile.netUpdate = true;
                                projectile.tileCollide = true;
                                for (int i = 0; i < projectile.localNPCImmunity.Length; i++)
                                {
                                    projectile.localNPCImmunity[i] = 0;
                                }
                                projectile.localNPCHitCooldown = throwHitCooldown;
                                break;
                            }
                        }
                        projectile.localAI[1] += 1f;
                        Vector2 vector3 = new Vector2((float)player.direction).RotatedBy((double)(31.4159279f * (projectile.localAI[1] / swingSpeed) * (float)player.direction), default(Vector2));
                        vector3.Y *= 0.8f;
                        if (vector3.Y * player.gravDir > 0f)
                        {
                            vector3.Y *= 0.5f;
                        }
                        projectile.Center = mountedCenter + vector3 * swingDistance;
                        projectile.velocity = Vector2.Zero;
                        projectile.localNPCHitCooldown = swingHitCooldown;

                        break;
                    case 1:
                        projectile.ai[1]++;
                        bool PullProjectileBack = projectile.ai[1] >= (float)throwTimerMax;
                        PullProjectileBack |= (projectile.Distance(mountedCenter) >= ProjectileMaxDistance);

                        if (PullProjectileBack)
                        {
                            projectile.ai[0] = 2f;
                            projectile.ai[1] = 0f;
                            projectile.netUpdate = true;
                            projectile.velocity *= 0.3f;
                        }
                        player.ChangeDir((player.Center.X < projectile.Center.X) ? 1 : (-1));
                        projectile.localNPCHitCooldown = throwHitCooldown;

                        break;
                    case 2:
                        projectile.tileCollide = false;
                        Vector2 direction = projectile.DirectionTo(mountedCenter).SafeNormalize(Vector2.Zero);
                        if (projectile.Distance(mountedCenter) <= returnMagnitude)
                        {
                            projectile.Kill();
                            return;
                        }
                        projectile.velocity *= 0.98f;
                        projectile.velocity = MoveTowards(projectile.velocity, direction * returnMagnitude, maxReturnSpeed);
                        player.ChangeDir((player.Center.X < projectile.Center.X) ? 1 : (-1));

                        break;
                }
                projectile.spriteDirection = player.direction;
                projectile.ownerHitCheck = flag2;
                player.itemTime = 5;
                player.itemAnimation = 5;
                projectile.timeLeft = 2;
                player.heldProj = projectile.whoAmI;
                player.itemRotation = projectile.DirectionFrom(mountedCenter).ToRotation();
                if (projectile.Center.X < mountedCenter.X)
                {
                    player.itemRotation += 3.14159274f;
                }
                player.itemRotation = MathHelper.WrapAngle(player.itemRotation);

                projectile.rotation = (projectile.Center - mountedCenter).ToRotation() + (MathHelper.Pi * 0.5f);
                //projectile.AI_015_Flails_Dust(doFastThrowDust);
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if ((int)projectile.ai[0] == 0)
            {
                damage = (int)(damage * 0.25);
                knockback = (int)(knockback * 0.25);
            }

            if (Main.rand.Next(0, 4) == 0)
            {
                if (target.HasBuff(BuffType<Slowed>()))
                {
                    target.AddBuff(BuffType<Frozen>(), 180);
                    target.DelBuff(target.FindBuffIndex(BuffType<Slowed>()));
                }
                else
                {
                    target.AddBuff(BuffType<Slowed>(), 180);
                }
            }
            else if (target.HasBuff(BuffType<Slowed>()))
            {
                target.AddBuff(BuffType<Slowed>(), 180);
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            projectile.ai[0] = 2;

            if (oldVelocity.X != projectile.velocity.X)
                projectile.velocity.X *= 0.1f;
            if (oldVelocity.Y != projectile.velocity.Y)
                projectile.velocity.Y *= 0.1f;
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 16;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public Vector2 MoveTowards(Vector2 currentPosition, Vector2 targetPosition, float maxAmountAllowedToMove)
        {
            Vector2 v = targetPosition - currentPosition;
            if (v.Length() < maxAmountAllowedToMove)
            {
                return targetPosition;
            }
            return currentPosition + v.SafeNormalize(Vector2.Zero) * maxAmountAllowedToMove;
        }

        public Vector2 ClosestPointInRect(Rectangle r, Vector2 point)
        {
            Vector2 vector = point;
            if (vector.X < (float)r.Left)
            {
                vector.X = (float)r.Left;
            }
            if (vector.X > (float)r.Right)
            {
                vector.X = (float)r.Right;
            }
            if (vector.Y < (float)r.Top)
            {
                vector.Y = (float)r.Top;
            }
            if (vector.Y > (float)r.Bottom)
            {
                vector.Y = (float)r.Bottom;
            }
            return vector;
        }
    }
}
