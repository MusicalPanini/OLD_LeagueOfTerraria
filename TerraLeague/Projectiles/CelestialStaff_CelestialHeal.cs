using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class CelestialStaff_CelestialHeal : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("No one expects the Banana");
        }

        public override void SetDefaults()
        {
            
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 255;
            projectile.timeLeft = 90;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;

        }

        public override void AI()
        {
            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 74, projectile.velocity.X, projectile.velocity.Y, 50, default(Color), 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }

            for (int i = 0; i < Main.player.Length; i++)
            {
                if (projectile.Hitbox.Intersects(Main.player[i].Hitbox) && i != projectile.owner && Main.myPlayer == projectile.owner)
                {
                    if (Main.LocalPlayer.whoAmI == projectile.owner)
                        Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().SendHealPacket(projectile.damage, i, -1, projectile.owner);

                    projectile.Kill();
                }
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
