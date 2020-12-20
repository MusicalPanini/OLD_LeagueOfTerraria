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
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 82, -0.7f);
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 45, -0.5f);
                projectile.ai[0] = -1;
            }

            if (!Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().frostHarbinger)
                projectile.Kill();

            projectile.Center = Main.player[projectile.owner].Center;

            if (projectile.timeLeft < 15)
            {
                projectile.alpha += 5;
            }

            if (projectile.timeLeft % 15 == 0)
            {
                TerraLeague.GiveNPCsInRangeABuff(projectile.Center, projectile.width / 2, BuffType<Buffs.Slowed>(), 15, true, true);
            }

            AnimateProjectile();
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().harbingersInferno)
            {
                if (target.townNPC)
                    return false;
                return TerraLeague.IsHitboxWithinRange(projectile.Center, target.Hitbox, projectile.width / 2f);
            }
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

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
