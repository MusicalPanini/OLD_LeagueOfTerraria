using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class DarkinScythe_ReapingSlash : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Reaping Slash");
        }

        public override void SetDefaults()
        {
            projectile.width = 54;
            projectile.height = 60;
            projectile.alpha = 0;
            projectile.timeLeft = 27;
            projectile.penetrate = 1000;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().channelProjectile = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().playerInvincible = true;
            base.SetDefaults();
        }
        
        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            player.GetModPlayer<PLAYERGLOBAL>().invincible = true;

            if (projectile.soundDelay == 0)
            {
                player.ChangeDir(player.velocity.X > 0 ? 1 : -1);
                projectile.spriteDirection = player.direction;

                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 71, -1f);
            }
            projectile.soundDelay = 100;

            player.direction = projectile.spriteDirection;

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

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            PLAYERGLOBAL player = Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>();

            target.AddBuff(BuffType<UmbralTrespass>(), 300);

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
