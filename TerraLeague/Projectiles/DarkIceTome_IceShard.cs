using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class DarkIceTome_IceShard : ModProjectile
    {
        bool split = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Shard");
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 22;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 50;
            projectile.magic = true;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Left, 0.09f, 0.40f, 0.60f);


            if (projectile.alpha > 0)
            {
                projectile.alpha -= 15;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }

            for (int i = 0; i < 1; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 67, 0f, 0f, 100, default(Color), 1.5f);
                dustIndex.noGravity = true;
                dustIndex.velocity *= 0.3f;
            }

            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Split(target.whoAmI);
            target.AddBuff(BuffType<Buffs.Slowed>(), 240);
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().slowed = true;
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().icebornSubjugation = true;
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().icebornSubjugationOwner = projectile.owner;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;

            return true;
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27), projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 67, projectile.velocity.X / 1.5f, projectile.velocity.Y / 1.5f, 100, default(Color), 1.5f);
                dustIndex.noGravity = true;
            }
            base.Kill(timeLeft);
        }

        public void Split(int num = -1)
        {
            if (!split && num != -1)
            {
                for (int j = Main.rand.Next(0, 2); j < 2; j++)
                {
                    Projectile proj3 = Projectile.NewProjectileDirect(projectile.Center, projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(10, 21))), ProjectileType<DarkIceTome_IceShardSmallA>(), projectile.damage, 0, projectile.owner, num == -1 ? 255 : num);
                }
                for (int j = Main.rand.Next(0, 2); j < 2; j++)
                {
                    Projectile proj2 = Projectile.NewProjectileDirect(projectile.Center, projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-5, 6))), ProjectileType<DarkIceTome_IceShardSmallB>(), projectile.damage, 0, projectile.owner, num == -1 ? 255 : num);
                }
                for (int j = Main.rand.Next(0, 2); j < 2; j++)
                {
                    Projectile proj = Projectile.NewProjectileDirect(projectile.Center, projectile.velocity.RotatedBy(MathHelper.ToRadians(Main.rand.Next(-20, -11))), ProjectileType<DarkIceTome_IceShardSmallC>(), projectile.damage, 0, projectile.owner, num == -1 ? 255 : num);
                }

                split = true;
            }
        }
    }
}
