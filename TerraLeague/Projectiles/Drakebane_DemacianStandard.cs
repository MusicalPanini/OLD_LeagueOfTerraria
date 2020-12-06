using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class Drakebane_DemacianStandard : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Demacian Standard");
            Main.projFrames[projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            projectile.width = 14;
            projectile.height = 64;
            projectile.timeLeft = 60*8;
            projectile.penetrate = 1000;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.scale = 1;
            projectile.alpha = 0;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            if (projectile.friendly && projectile.velocity.Length() < 0.1f && projectile.tileCollide)
            {
                for (int i = 0; i < 5; i++)
                {
                    Collision.HitTiles(projectile.Bottom, projectile.oldVelocity, projectile.width, 1);
                }
                Main.PlaySound(Terraria.ID.SoundID.Item10, projectile.position);
                projectile.extraUpdates = 0;
                projectile.timeLeft = 60 * 6;
                projectile.friendly = false;
            }

            Lighting.AddLight(projectile.Center, 0.75f, 0.75f, 0.75f);

            if (projectile.ai[1] == 0f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.ai[1] = 1f;
                projectile.netUpdate = true;
            }
            if (projectile.ai[1] != 0f)
            {
                projectile.tileCollide = true;
            }

            if (projectile.friendly)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 1);
                dust.velocity /= 3;
            }
            else
            {
                Dust dust = Dust.NewDustDirect(new Vector2(0, -2) + projectile.BottomLeft, projectile.width, 3, 133);
                dust.noGravity = true;
                dust.velocity.X *= 2;
                dust.velocity.Y = 0;
                dust.scale = 0.8f;

                TerraLeague.DustBorderRing(500, projectile.Center, 204, default, 2, true, true, 0.075f);
            }

            var players = TerraLeague.GetAllPlayersInRange(projectile.Center, 500, -1, Main.player[projectile.owner].team);
            for (int i = 0; i < players.Count; i++)
            {
                Player target = Main.player[players[i]];
                target.AddBuff(BuffType<ForDemacia>(), 2);
            }

            AnimateProjectile();
        }

        public void AnimateProjectile()
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 5)
            {
                projectile.frame++;
                projectile.frame %= 4; 
                projectile.frameCounter = 0;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
