using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class DarkIceTome_IceShardSmallB : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Shard");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 0;
            projectile.timeLeft = 35;
            projectile.penetrate = 3;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
        }

        public override void AI()
        {
            projectile.npcProj = false;
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }

            if (Main.rand.Next(0, 3) == 0)
            {
                Dust dustIndex = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 67, 0f, 0f, 100, default(Color), 1f);
                dustIndex.noGravity = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffType<Buffs.Slowed>(), 120);
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().slowed = true;
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().icebornSubjugation = true;
            target.GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().icebornSubjugationOwner = projectile.owner;
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27), projectile.position);

            return true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 67, projectile.velocity.X / 1.5f, projectile.velocity.Y / 1.5f, 100, default(Color), 1f);
                dustIndex.noGravity = true;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == projectile.ai[0])
                return false;
            else
                return base.CanHitNPC(target);
        }
    }
}
