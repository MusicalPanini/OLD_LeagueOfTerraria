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
    public class TrueIceArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("True Ice Arrow");
        }

        public override void SetDefaults()
        {
            projectile.arrow = true;
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 0;
            projectile.timeLeft = 1200;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.aiStyle = 1;
            aiType = ProjectileID.WoodenArrowFriendly;
        }

        public override void AI()
        {
            //projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(90);
            int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 67, 0f, 0f, 100, default(Color));
            Main.dust[dustIndex].noGravity = true;
            Lighting.AddLight(projectile.position, 0f, 0f, 0.5f);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            crit = false;
            Console.WriteLine(projectile.extraUpdates = 1);
            target.AddBuff(BuffType<Slowed>(), 120);
            base.OnHitPlayer(target, damage, crit);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            crit = false;
            Console.WriteLine(projectile.extraUpdates = 1);
            target.AddBuff(BuffType<Slowed>(), 120);

            base.OnHitNPC(target, damage, knockback, false);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.Kill();
            Main.PlaySound(0, projectile.Center);
            return false;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 80, 0f, 0f, 100, default(Color), 0.7f);

            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            crit = false;
            if (target.GetModPlayer<PLAYERGLOBAL>().slowed)
            {
                crit = true;
                float multiplier = (Main.player[projectile.owner].rangedCrit + 75) * 0.01333f;
                float dam = damage * 0.5f * multiplier;
                damage = (int)dam;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            crit = false;
            if (target.GetGlobalNPC<NPCsGLOBAL>().slowed)
            {
                crit = true;
                float multiplier = (Main.player[projectile.owner].rangedCrit + 75) * 0.01333f;
                float dam = damage * 0.5f * multiplier;
                damage = (int)dam;
            }
            
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            // For going through platforms and such, javelins use a tad smaller size
            //width = height = 10; // notice we set the width to the height, the height to 10. so both are 10
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

    }
}
