using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria;

namespace TerraLeague.Projectiles.Minions
{
    public class Minion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[projectile.type] = Main.projFrames[ProjectileID.OneEyedPirate];
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
            ProjectileID.Sets.Homing[projectile.type] = true;
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            projectile.netImportant = true;
            projectile.aiStyle = 67;
            projectile.width = 52;
            projectile.height = 40;
            projectile.timeLeft = 18000;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.minion = true;
            projectile.minionSlots = 1;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1;
        }

        public override void AI()
        {
            projectile.penetrate = -1;
            Main.projPet[projectile.type] = true;
            Player player = Main.player[projectile.owner];
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (player.dead)
            {
                modPlayer.minions = false;
            }
            if (modPlayer.minions)
            {
                projectile.timeLeft = 2;
            }

            base.AI();
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

    }
}
