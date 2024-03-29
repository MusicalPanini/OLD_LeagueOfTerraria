﻿using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class VoidEnergy : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            
            DisplayName.SetDefault("Void Energy");
        }

        public override void SetDefaults()
        {
            projectile.width = 4;
            projectile.height = 4;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 900;
            projectile.ranged = true;
            projectile.extraUpdates = 12;
        }

        public override void AI()
        {
            if (projectile.timeLeft < 896)
            {
                Dust dust = Dust.NewDustPerfect(projectile.position, 112, Vector2.Zero, 0, new Color(59, 0, 255), 1f);
                dust.noGravity = true;
                dust.alpha = 100;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            NPCsGLOBAL modNPC = target.GetGlobalNPC<NPCsGLOBAL>();
            PLAYERGLOBAL modPlayer = Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>();


            target.AddBuff(BuffType<CausticWounds>(), 240);
            modNPC.CausticWounds = true;
            modNPC.CausticStacks++;

            int stacks = target.GetGlobalNPC<NPCsGLOBAL>().CausticStacks;
            if (stacks == 5)
            {
                projectile.magic = true;

                int damCap = (int)(modPlayer.MAG + 50);

                damage += (target.lifeMax - target.life) / 4 > damCap ? damCap : (target.lifeMax - target.life) / 4;

                projectile.netUpdate = true;
                projectile.ai[0] = 1;
            }
            if (stacks > 5)
            {
                modNPC.CausticStacks = 1;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    modNPC.PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 1, target.whoAmI, modNPC.CausticStacks);
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
                modNPC.PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 1, target.whoAmI, modNPC.CausticStacks);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.ai[0] == 1)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 122), projectile.Center);
                for (int i = 0; i < 8; i++)
                {
                    Dust dust = Dust.NewDustDirect(projectile.position, 8, 8, 112, 0, 0, 0, new Color(59, 0, 255), 1f);

                }
            }

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }
    }
}
