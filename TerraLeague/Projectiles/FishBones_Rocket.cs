using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class FishBones_Rocket : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fish Bones");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 10;
            projectile.timeLeft = 300;
            projectile.penetrate = 100;
            projectile.friendly = true;
            projectile.ranged = true;
            projectile.knockBack = 2;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.position, 1f, 0.34f, 0.9f);
            projectile.rotation = projectile.velocity.ToRotation();
            if (projectile.velocity.X < 0)
            {
                projectile.rotation = projectile.velocity.ToRotation();
                projectile.scale = -1f;
                projectile.spriteDirection = -1;
            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 134, 0,0,0,default(Color), 1.2f);
                dust.noGravity = true;
                dust.velocity *= 0.3f;
            }
            base.AI();
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0)
            {
                Main.player[projectile.owner].AddBuff(BuffType<PowPowExcited>(), 300);
            }

            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool PreKill(int timeLeft)
        {
           
            return base.PreKill(timeLeft);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolume(1f), projectile.position);

            Dust dust;
            for (int i = 0; i < 50; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                dust.velocity *= 1.4f;
            }
            for (int i = 0; i < 80; i++)
            {
                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(255, 255, 0, 150), 2f);
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust.color = new Color(255,0,220);

                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 2f);
                dust.velocity *= 3f;
                dust.color = new Color(255, 0, 220);
            }
            for (int g = 0; g < 2; g++)
            {
                Gore gore;
                gore = Gore.NewGoreDirect(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X = gore.velocity.X + 1.5f;
                gore.velocity.Y = gore.velocity.Y + 1.5f;
                gore = Gore.NewGoreDirect(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X = gore.velocity.X - 1.5f;
                gore.velocity.Y = gore.velocity.Y + 1.5f;
                gore = Gore.NewGoreDirect(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 24f, projectile.position.Y + (float)(projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                gore.scale = 1.5f;
                gore.velocity.X = gore.velocity.X + 1.5f;
                gore.velocity.Y = gore.velocity.Y - 1.5f;
            }

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public void Prime()
        {
            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 115;
            projectile.height = 115;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 3;
        }
    }
}
