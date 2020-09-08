using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    class FishBones_SMDR : ModProjectile
    {
        int baseDamage;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Super Mega Death Rocket");
        }

        public override void SetDefaults()
        {
            projectile.width = 26;
            projectile.height = 26;
            projectile.timeLeft = 1200;
            projectile.penetrate = 1000;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.scale = 1.5f;
            aiType = 0;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
                baseDamage = projectile.damage;
            projectile.soundDelay = 100;

            Lighting.AddLight(projectile.Center, 1f, 0.34f, 0.9f);
            projectile.damage = (int)(baseDamage * (projectile.velocity.Length() / 25));
            projectile.rotation = projectile.velocity.ToRotation();
            if (projectile.velocity.X < 0)
            {
                projectile.rotation = projectile.velocity.ToRotation();
                projectile.scale = -1.5f;
                projectile.spriteDirection = -1;
            }

            if (projectile.velocity.Length() < 25)
            {
                projectile.velocity.X *= 1.05f;
                projectile.velocity.Y *= 1.05f;

            }

            for (int i = 0; i < 3; i++)
            {
                Dust dust1 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height/2, 6);
                Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height/2, 6);
                dust1.scale = 2 * (projectile.velocity.Length() / 25);
                dust1.noGravity = true;
                dust2.scale = 2 * (projectile.velocity.Length() / 50);
                dust2.noGravity = true;
            }
            
            if (projectile.timeLeft < 30)
            {
                projectile.alpha += 9;
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode.WithVolume(1f), projectile.position);

            for (int i = 0; i < 80; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 2f);
                dust.velocity *= 3f;
                if (Main.rand.Next(2) == 0)
                {
                    dust.scale = 0.5f;
                    dust.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
                }
            }
            for (int i = 0; i < 120; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 4.5f);
                dust.noGravity = true;
                dust.velocity *= 5f;

                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 3f);
                dust.velocity *= 3f;
            }
            for (int i = 0; i < 3; i++)
            {
                float velScale = (i+1) * 1f;

                Gore gore = Gore.NewGoreDirect(projectile.Center, default(Vector2), Main.rand.Next(61, 64), 2f);
                gore.velocity.X += 1.5f;
                gore.velocity.Y += 1.5f;

                gore = Gore.NewGoreDirect(projectile.Center, default(Vector2), Main.rand.Next(61, 64), 2f);
                gore.velocity.X -= 1.5f;
                gore.velocity.Y -= 1.5f;

                gore = Gore.NewGoreDirect(projectile.Center, default(Vector2), Main.rand.Next(61, 64), 2f);
                gore.velocity.X += 1.5f;
                gore.velocity.Y -= 1.5f;

                gore = Gore.NewGoreDirect(projectile.Center, default(Vector2), Main.rand.Next(61, 64), 2f);
                gore.velocity.X -= 1.5f;
                gore.velocity.Y += 1.5f;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.boss || projectile.penetrate == 997)
                Prime();

            base.OnHitNPC(target, damage, knockback, crit);
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
            projectile.width = 350;
            projectile.height = 350;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 3;
        }
    }
}
