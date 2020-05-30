using Microsoft.Xna.Framework.Audio;
using TerraLeague.NPCs;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class Item_FrostStorm : ModProjectile
    {
        int framecount2 = 29;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Frost Storm");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 400;
            projectile.height = 400;
            projectile.timeLeft = 600;
            projectile.penetrate = 1000;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1;
            projectile.alpha = 180;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if ((int)projectile.ai[0] >= 0)
            {
                SoundEffectInstance sound = Main.PlaySound(new LegacySoundStyle(2, 82, Terraria.Audio.SoundType.Sound), projectile.Center);
                if (sound != null)
                    sound.Pitch = -0.7f;

                sound = Main.PlaySound(new LegacySoundStyle(2, 45, Terraria.Audio.SoundType.Sound), Main.player[(int)projectile.ai[0]].Center);
                if (sound != null)
                    sound.Pitch = -0.5f;

                projectile.ai[0] = -1;
            }

            if (!Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().frostHarbinger)
                projectile.Kill();

            projectile.Center = Main.player[projectile.owner].Center;

            if (projectile.timeLeft < 15)
            {
                projectile.alpha += 5;
            }

            for (int i = 0; i < 200; i++)
            {
                if (Main.npc[i].Hitbox.Intersects(projectile.Hitbox))
                {
                    Main.npc[i].AddBuff(BuffType<Buffs.Slowed>(), 2);
                }
            }

            AnimateProjectile();
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<NPCsGLOBAL>().harbingersInferno)
                return base.CanHitNPC(target);
            else
                return false;
        }

        public void AnimateProjectile()
        {
            projectile.friendly = false;
            projectile.frameCounter++;
            framecount2++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frame %= 4;
                projectile.frameCounter = 0;
            }
            if (framecount2 >= 20)
            {
                projectile.friendly = true;
                framecount2 = 0;
            }
        }
    }
}
