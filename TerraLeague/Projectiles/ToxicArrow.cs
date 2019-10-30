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
    public class ToxicArrow : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Toxic Arrow");
        }

        public override void SetDefaults()
        {
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
            projectile.arrow = true;
        }

        public override void AI()
        {
            int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 67, 0f, 0f, 100, new Color(0,255,0));
            Main.dust[dustIndex].noGravity = true;
            Main.dust[dustIndex].noLight = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int stacks = target.GetGlobalNPC<NPCsGLOBAL>().DeadlyVenomStacks;

            if (stacks < 4)
            {
                target.GetGlobalNPC<NPCsGLOBAL>().DeadlyVenomStacks++;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    target.GetGlobalNPC<NPCsGLOBAL>().PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 3, target.whoAmI, target.GetGlobalNPC<NPCsGLOBAL>().DeadlyVenomStacks);
            }

            target.AddBuff(BuffType<DeadlyVenom>(), 300);

            //target.AddBuff(BuffType("DeadlyVenom"), 300);
            //if (target.HasBuff(BuffType("DeadlyVenomStacks")))
            //{
            //    int time = target.buffTime[target.FindBuffIndex(BuffType("DeadlyVenomStacks"))];

            //    target.AddBuff(BuffType("DeadlyVenomStacks"), time + 100);

            //}
            //else
            //{
            //    target.AddBuff(BuffType("DeadlyVenomStacks"), 100);
            //}

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
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 80, 0f, 0f, 100, new Color(0, 255, 0), 0.7f);

            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            // For going through platforms and such, javelins use a tad smaller size
            width = height = 10; // notice we set the width to the height, the height to 10. so both are 10
            return true;
        }

    }
}
