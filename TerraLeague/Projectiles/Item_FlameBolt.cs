using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Item_FlameBolt: ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Protobelt-01");
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 10;
            projectile.alpha = 0;
            projectile.timeLeft = 60;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.aiStyle = 0;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 132);
            dust.color = new Color(0, 125, 255);
            dust.noGravity = true;
            Lighting.AddLight(projectile.position, 0f, 0f, 0.5f);
        }

        public override void Kill(int timeLeft)
        {
            Dust dust;
            for (int i = 0; i < 24; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 132, 0f, 0f, 100, default(Color), 1);
                dust.noGravity = true;
                dust.velocity *= 2f;
            }
            for (int i = 0; i < 8; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 132, 0f, 0f, 100, default(Color), 1);
                dust.velocity *= 1f;
            }
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 14, Terraria.Audio.SoundType.Sound), projectile.position);
            base.Kill(timeLeft);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Prime();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public void Prime()
        {
            projectile.velocity = Vector2.Zero;
            projectile.tileCollide = false;
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
