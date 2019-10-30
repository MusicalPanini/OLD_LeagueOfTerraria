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
    class DarksteelDecimate : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
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
            base.SetDefaults();
        }
        
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.itemTime = 1;
            player.noItems = true;

            if (projectile.timeLeft == 71)
            {
                if (projectile.ai[0] == 1)
                    projectile.rotation = -MathHelper.PiOver2;
                else
                    projectile.rotation = MathHelper.PiOver2;
            }

            if (projectile.timeLeft == 26)
            {
                SoundEffectInstance sound = Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 71).WithPitchVariance(-0.2f), projectile.Center);
                if (sound != null)
                    sound.Pitch = -1f;
            }

            if (projectile.timeLeft <= 26)
            {
                projectile.friendly = true;
                if (projectile.ai[0] == 1)
                {
                    projectile.rotation += (2 * MathHelper.Pi) / 15;
                    projectile.Center = Main.player[projectile.owner].Center + new Vector2(33f, 40.5f).RotatedBy(projectile.rotation);
                }
                else
                {
                    projectile.spriteDirection = -1;
                    projectile.rotation -= (2 * MathHelper.Pi) / 15;
                    projectile.Center = Main.player[projectile.owner].Center - new Vector2(33f, -40.5f).RotatedBy(projectile.rotation);
                }
            }
            else
            {
                if (projectile.ai[0] == 1)
                {
                    projectile.rotation -= (2 * MathHelper.Pi) / 90;
                    projectile.Center = Main.player[projectile.owner].Center + new Vector2(33f, 40.5f).RotatedBy(projectile.rotation);
                }
                else
                {
                    projectile.spriteDirection = -1;
                    projectile.rotation += (2 * MathHelper.Pi) / 90;
                    projectile.Center = Main.player[projectile.owner].Center - new Vector2(33f, -40.5f).RotatedBy(projectile.rotation);
                }
            }

            player.direction = projectile.spriteDirection;
            

            //Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 211);
            //dust.alpha = 0;
            //dust.noLight = false;
            //dust.noGravity = true;
            //dust.scale = 1.4f;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            PLAYERGLOBAL player = Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>();

            player.lifeToHeal += (int)(7 * player.healPower);

            int stacks = target.GetGlobalNPC<NPCsGLOBAL>().HemorrhageStacks;

            if (stacks < 4)
            {
                target.GetGlobalNPC<NPCsGLOBAL>().HemorrhageStacks++;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    target.GetGlobalNPC<NPCsGLOBAL>().PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 4, target.whoAmI, target.GetGlobalNPC<NPCsGLOBAL>().HemorrhageStacks);
            }

            target.AddBuff(BuffType<Hemorrhage>(), 300);

            //target.AddBuff(BuffType("Hemorrhage"), 300);
            //if (target.HasBuff(BuffType("HemorrhageStacks")))
            //{
            //    int time = target.buffTime[target.FindBuffIndex(BuffType("HemorrhageStacks"))];

            //    target.AddBuff(BuffType("HemorrhageStacks"), time + 100);
            //}
            //else
            //{
            //    target.AddBuff(BuffType("HemorrhageStacks"), 100);
            //}
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            // For going through platforms and such, javelins use a tad smaller size
            width = height = 10; // notice we set the width to the height, the height to 10. so both are 10
            return true;
        }
    }
}
