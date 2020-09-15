using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class HexCoreStaff_Storm : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chaos Storm");
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.timeLeft = 300;
            projectile.penetrate = 100;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.magic = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
                Prime();
            projectile.soundDelay = 100;
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool PreKill(int timeLeft)
        {
           
            return base.PreKill(timeLeft);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(3, 53), projectile.position);
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - projectile.width/4, projectile.Center.Y - projectile.height / 4), projectile.width / 2, projectile.height / 2, 261, 0, 0, 50, new Color(0, 255, 255), 1.5f);
                dust.velocity *= 7f;
                dust.noGravity = true;
                dust.noLight = true;
                dust.fadeIn = 2;
                dust = Dust.NewDustDirect(new Vector2(projectile.Center.X - projectile.width / 4, projectile.Center.Y - projectile.height /4), projectile.width / 2, projectile.height / 2, 226, 0, 0, 50, new Color(0, 255, 255), 0.5f);
                dust.velocity *= 5f;
            }
            

            TerraLeague.DustBorderRing(projectile.width / 2, projectile.Center, 261, new Color(0, 255, 255), 2);

            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public void Prime()
        {
            int size = 400;

            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = size;
            projectile.height = size;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 2;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
