using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class DarksteelBattleaxe_Decimate : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 1;
            DisplayName.SetDefault("Darksteel Battleaxe");
        }

        public override void SetDefaults()
        {
            projectile.width = 66;
            projectile.height = 81;
            projectile.alpha = 0;
            projectile.timeLeft = 71;
            projectile.penetrate = 1000;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().channelProjectile = true;
            base.SetDefaults();
        }
        
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            //player.itemTime = 10;
            //player.noItems = true;

            if (projectile.soundDelay == 0)
            {
                if (projectile.ai[0] == 1)
                    projectile.rotation = -MathHelper.PiOver2;
                else
                    projectile.rotation = MathHelper.PiOver2;
            }
            projectile.soundDelay = 100;

            if (projectile.timeLeft == 26)
            {
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 71, -1f);
            }

            if (projectile.timeLeft <= 26)
            {
                projectile.friendly = true;
                if (projectile.ai[0] == 1)
                {
                    projectile.rotation += (2 * MathHelper.Pi) / 15;
                    projectile.Center = Main.player[projectile.owner].MountedCenter + new Vector2(33f, 40.5f).RotatedBy(projectile.rotation);
                }
                else
                {
                    projectile.spriteDirection = -1;
                    projectile.rotation -= (2 * MathHelper.Pi) / 15;
                    projectile.Center = Main.player[projectile.owner].MountedCenter - new Vector2(33f, -40.5f).RotatedBy(projectile.rotation);
                }
            }
            else
            {
                if (projectile.ai[0] == 1)
                {
                    projectile.rotation -= (2 * MathHelper.Pi) / 90;
                    projectile.Center = Main.player[projectile.owner].MountedCenter + new Vector2(33f, 40.5f).RotatedBy(projectile.rotation);
                }
                else
                {
                    projectile.spriteDirection = -1;
                    projectile.rotation += (2 * MathHelper.Pi) / 90;
                    projectile.Center = Main.player[projectile.owner].MountedCenter - new Vector2(33f, -40.5f).RotatedBy(projectile.rotation);
                }
            }

            player.direction = projectile.spriteDirection;
            
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            PLAYERGLOBAL player = Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>();

            player.lifeToHeal += 7;

            int stacks = target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().HemorrhageStacks;

            if (stacks < 4)
            {
                target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().HemorrhageStacks++;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 4, target.whoAmI, target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().HemorrhageStacks);
            }

            target.AddBuff(BuffType<Hemorrhage>(), 300);

            base.OnHitNPC(target, damage, knockback, crit);
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
