using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class NetherBladeofHorok_RiftwalkHitbox : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Riftwalk");
        }

        public override void SetDefaults()
        {
            projectile.width = 256;
            projectile.height = 256;
            projectile.timeLeft = 1;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            projectile.Center = Main.player[projectile.owner].MountedCenter;
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.DustRing(112, projectile, default(Color));
            TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 82, -0.7f);

            base.Kill(timeLeft);
        }
    }
}
