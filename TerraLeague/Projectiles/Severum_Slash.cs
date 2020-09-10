using Microsoft.Xna.Framework;
using System;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class Severum_Slash : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Severum");
            Main.projFrames[projectile.type] = 28;
        }

        public override void SetDefaults()
        {
            projectile.width = 136;
            projectile.height = 128;
            projectile.friendly = false;
            projectile.penetrate = -1;
            projectile.alpha = 0;
            projectile.timeLeft = 60;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.localNPCHitCooldown = 14;
            projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 2;

            if (player.GetModPlayer<PLAYERGLOBAL>().severumAmmo < 2)
            {
                projectile.Kill();
            }

            projectile.localNPCHitCooldown = (int)(14 * player.meleeSpeed);
            //if (player.channel && !player.noItems && !player.CCed)
            //{
            //    player.itemAnimation = 5;
            //    player.itemTime = 5;
            //    projectile.rotation = player.itemRotation;
            //    projectile.Center = player.MountedCenter + new Vector2(100, 0).RotatedBy(projectile.rotation);
            //    projectile.timeLeft = 60;

            //    AnimateProjectile();
            //}
            //else
            //{
            //    projectile.Kill();
            //}

            float num;
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, false);
            float num3 = 0f;
            player.itemAnimation = 5;
            player.itemTime = 5;
            num = 0f;
            if (projectile.spriteDirection == -1)
            {
                num = 3.14159274f;
            }
            AnimateProjectile();
            if (Main.myPlayer == projectile.owner)
            {
                if (player.channel && !player.noItems && !player.CCed)
                {
                    float scaleFactor6 = 1f;
                    if (player.inventory[player.selectedItem].shoot == projectile.type)
                    {
                        scaleFactor6 = player.inventory[player.selectedItem].shootSpeed * projectile.scale;
                    }
                    Vector2 vector19 = Main.MouseWorld - vector;
                    vector19.Normalize();
                    if (vector19.HasNaNs())
                    {
                        vector19 = Vector2.UnitX * (float)player.direction;
                    }
                    vector19 *= scaleFactor6;
                    if (vector19.X != projectile.velocity.X || vector19.Y != projectile.velocity.Y)
                    {
                        projectile.netUpdate = true;
                    }
                    projectile.velocity = vector19;
                }
                else
                {
                    projectile.Kill();
                }
            }
            Vector2 vector20 = projectile.Center + projectile.velocity * 3f;

            projectile.position = player.RotatedRelativePoint(player.MountedCenter, false) - projectile.Size / 2f;
            projectile.rotation = projectile.velocity.ToRotation() + num;
            projectile.spriteDirection = projectile.direction;
            projectile.timeLeft = 2;
            player.ChangeDir(projectile.direction);
            player.heldProj = projectile.whoAmI;
            player.itemRotation = MathHelper.WrapAngle((float)Math.Atan2((double)(projectile.velocity.Y * (float)projectile.direction), (double)(projectile.velocity.X * (float)projectile.direction)) + num3);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().lifeToHeal++;

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
        }

        public override void Kill(int timeLeft)
        {

            base.Kill(timeLeft);
        }

        public void AnimateProjectile()
        {
            projectile.frameCounter++;
            int frameCounterMax = Main.player[projectile.owner].meleeSpeed < 0.5f ? 1 : 2;
            if (projectile.frameCounter >= frameCounterMax)
            {
                projectile.frame++;
                projectile.frame %= 28;
                projectile.frameCounter = 0;
            }

            if (projectile.frameCounter == 1 && projectile.frame % 7 == 0)
            {
                Main.PlaySound(SoundID.Item1, projectile.Center);
            }

            if (projectile.frameCounter == 1 && projectile.frame % 7 == 2)
            {
                projectile.friendly = true;
                if (Main.LocalPlayer.whoAmI == projectile.owner)
                    Projectile.NewProjectileDirect(projectile.Center, (Main.player[projectile.owner].MountedCenter - projectile.Center).RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f)) / -6, ProjectileID.DD2SquireSonicBoom, projectile.damage/2, projectile.knockBack, projectile.owner);
                Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().severumAmmo -= 2;
                Main.PlaySound(SoundID.Item10, projectile.Center);
            }
        }
    }
}
