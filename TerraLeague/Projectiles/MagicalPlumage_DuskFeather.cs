using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class MagicalPlumage_DuskFeather : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dusk Feather");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.alpha = 0;
            projectile.timeLeft = 45;
            projectile.penetrate = 3;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.aiStyle = 1;
        }

        public override void AI()
        {
            Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 154, 0f, 0f, 147, Main.rand.Next(2) == 0 ? new Color(255,0,201) : new Color(0, 255, 255), 1.7f);
            dust.noGravity = true;

            if (projectile.timeLeft < 10)
            {
                projectile.alpha += 26;
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Dig, projectile.Center);
            return true;
        }

        public override void Kill(int timeLeft)
        {
            if (projectile.alpha < 255)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 154, 0f, 0f, 147, Main.rand.Next(2) == 0 ? new Color(255, 0, 201) : new Color(0, 255, 255), 1.7f);
                    dust.noGravity = true;
                }
            }
        }
    }
}
