using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class EchoingFlameCannon_CorrosiveCharge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corrosive Charge");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            base.SetDefaults();
        }
        public override void AI()
        {
            //Lighting.AddLight(projectile.position, 0f, 0f, 0.5f);
            //Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 67, 0f, 0f, 100, new Color(0, 255, 0));
            //dust.noLight = true;
            //dust.alpha = 0;
            //dust.noLight = false;
            //dust.noGravity = true;
            //dust.scale = 1.4f;


            if ((int)projectile.ai[0] == 0)
            {
                projectile.rotation = projectile.velocity.ToRotation() + (MathHelper.PiOver2 * 3);
                projectile.velocity.Y += 0.4f;
                if (projectile.velocity.Y > 16)
                    projectile.velocity.Y = 16;
            }
            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if ((int)projectile.ai[0] == 0)
            {
                projectile.timeLeft = 30;
                Main.PlaySound(SoundID.Dig, projectile.Center);
            }
            projectile.velocity *= 0;

            projectile.ai[0] = 1;
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Shatter, projectile.Center);
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 14), projectile.Center);
            if (projectile.owner == Main.myPlayer)
            {
                int spawnAmount = Main.rand.Next(20, 31);
                for (int i = 0; i < spawnAmount; i++)
                {
                    Vector2 vector14 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    vector14.Normalize();
                    vector14 *= (float)Main.rand.Next(10, 201) * 0.01f;
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector14.X, vector14.Y, ProjectileType<EchoingFlameCannon_CorrosiveCloud>(), projectile.damage, 1f, projectile.owner, 0f, (float)Main.rand.Next(-45, 1));
                }
            }

            for (int i = 0; i < 40; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 75, 0, 0, 0, default(Color), 0.5f);
                dust.noLight = true;
                dust.velocity *= 3;
                dust.velocity.X *= 2;
                dust.fadeIn = 1;
            }
           

            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }
    }
}
