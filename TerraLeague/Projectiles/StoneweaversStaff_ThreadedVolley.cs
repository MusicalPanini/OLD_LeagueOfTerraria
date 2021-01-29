using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    class StoneweaversStaff_ThreadedVolley : ModProjectile
    {
        Vector2[] stonePos = { new Vector2(-24,-24), new Vector2(24,-16), new Vector2(0,-8), new Vector2(16, -32), new Vector2(-8,-32) };

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Threaded Volley");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.timeLeft = 1000;
            projectile.penetrate = 1000;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.alpha = 150;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            if ((int)projectile.localAI[1] == 0)
            {
                if (projectile.alpha > 0)
                    projectile.alpha -= 10;
                if (projectile.alpha < 0)
                    projectile.alpha = 0;

                if (projectile.timeLeft == 1000)
                    projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

                projectile.velocity = Vector2.Zero;

                projectile.Center = new Vector2(Main.player[projectile.owner].MountedCenter.X + stonePos[(int)projectile.ai[0] % 5].X + projectile.ai[0], Main.player[projectile.owner].MountedCenter.Y + stonePos[(int)projectile.ai[0] % 5].Y + projectile.ai[0] /*+ (16 * (projectile.alpha / 255f))*/);

                if (projectile.timeLeft == 970 - ((int)projectile.ai[0] * (int)projectile.ai[1]))
                {
                    projectile.localAI[1] = 1;
                    projectile.velocity = new Vector2(0, -20).RotatedBy(projectile.rotation);
                    projectile.friendly = true;
                    projectile.tileCollide = true;
                    TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 19, -1f);
                }
            }
            else
            {
                Dust.NewDustDirect(projectile.position, 16, 16, 4, 0f, 0f, 100, new Color(255, 125, 0), 0.7f);
            }
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 4, 0f, 0f, 100, new Color(255, 125, 0), 0.7f);
                dust.velocity *= 1.5f;

                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 4, 0f, 0f, 100, new Color(255, 125, 0), 1f);
            }
            
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }

        public void Prime()
        {
            projectile.tileCollide = false;
            projectile.timeLeft = 3;
            projectile.velocity = Vector2.Zero;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 60;
            projectile.height = 60;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
        }
    }
}
