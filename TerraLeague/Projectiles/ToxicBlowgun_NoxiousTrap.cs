using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class ToxicBlowgun_NoxiousTrap : ModProjectile
    {
        bool grounded = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Noxious Trap");
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 32;
            projectile.timeLeft = 18000;
            projectile.penetrate = 1;
            projectile.friendly = false;
            projectile.minion = true;
            projectile.scale = 1.2f;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            projectile.rotation = 0;

            if (projectile.alpha >= 60)
            {
                if (TerraLeague.IsThereAnNPCInRange(projectile.Center, 90))
                {
                    Prime();
                }
            }

            if (!grounded)
            {
                projectile.velocity.Y += 0.3f;
                if (projectile.velocity.Y > 0)
                    for (int i = 0; i < Main.projectile.Length; i++)
                    {
                        if (Main.projectile[i].active)
                            if (Main.projectile[i].type == projectile.type)
                                if (Main.projectile[i].velocity.Length() < 0.0001f)
                                    if (Main.projectile[i].Hitbox.Intersects(projectile.Hitbox))
                                    {
                                        projectile.velocity.Y = -6;
                                        if (projectile.velocity.X < 3 && projectile.velocity.X >= 0)
                                            projectile.velocity.X = 3;
                                        if (projectile.velocity.X > -3 && projectile.velocity.X < 0)
                                            projectile.velocity.X = -3;
                                    }
                    }
            }
            else
            {
                if (projectile.alpha < 100)
                    projectile.alpha++;

                projectile.velocity = Vector2.Zero;
            }

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 102, -1f);

            Dust dust;
            for (int i = 0; i < 50; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
                dust.velocity *= 1.4f;
            }
            for (int i = 0; i < 80; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0f, 0f, 100, default(Color), 2f);
                dust.noGravity = true;

                Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 186, 0f, 0f, 100, default(Color), 1f);
            }

            if (projectile.owner == Main.myPlayer)
            {
                int spawnAmount = Main.rand.Next(20, 31);
                for (int i = 0; i < spawnAmount; i++)
                {
                    Vector2 vector14 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    vector14.Normalize();
                    vector14 *= (float)Main.rand.Next(10, 201) * 0.01f;
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector14.X, vector14.Y, ProjectileType<ToxicBlowgun_NoxiousCloud>(), projectile.damage, 0, projectile.owner, 0f, (float)Main.rand.Next(-45, 1));
                }
            }

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            base.Kill(timeLeft);
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;

            width = height = 10;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (projectile.velocity.X == 0)
            {
                projectile.velocity.X = oldVelocity.X * -0.5f;

                if (projectile.velocity.X < 3 && projectile.velocity.X >= 0)
                    projectile.velocity.X = 3;
                if (projectile.velocity.X > -3 && projectile.velocity.X < 0)
                    projectile.velocity.X = -3;
            }
            else if (projectile.velocity.Y == 0)
                grounded = true;

            return false;
        }

        public void Prime()
        {
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 150;
            projectile.height = 150;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);

            projectile.Kill();
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
