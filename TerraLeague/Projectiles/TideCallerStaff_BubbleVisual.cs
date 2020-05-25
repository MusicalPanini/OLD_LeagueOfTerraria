using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class TideCallerStaff_BubbleVisual : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Aqua Prison");
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.alpha = 0;
            projectile.timeLeft = 120;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override void AI()
        {
            if (projectile.timeLeft == 120)
            {
                 Main.PlaySound(new LegacySoundStyle(2, 86), projectile.Center);
            }

            projectile.scale = ((Main.npc[(int)projectile.ai[0]].width / 30) + (Main.npc[(int)projectile.ai[0]].height / 30) + 5) / 2;
            projectile.Center = Main.npc[(int)projectile.ai[0]].Center;
            if (Main.npc[(int)projectile.ai[0]].life <= 0)
            {
                projectile.Kill();
            }

            int trueWidth = (int)(projectile.width * projectile.scale);
            int trueHeight = (int)(projectile.height * projectile.scale);
            Vector2 truePosition = new Vector2(projectile.Center.X - (trueWidth/2), projectile.Center.Y - (trueHeight/2));

            Lighting.AddLight(projectile.Center, 0f, 0f, 0.5f);
            Dust dust = Dust.NewDustDirect(truePosition, trueWidth, trueHeight, 211, 0, 2, 150, default(Color), 1.5f);
            dust.noGravity = true;

            base.AI();
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(2, 54), projectile.Center);

            int trueWidth = (int)(projectile.width * projectile.scale);
            int trueHeight = (int)(projectile.height * projectile.scale);
            Vector2 truePosition = new Vector2(projectile.Center.X - (trueWidth / 2), projectile.Center.Y - (trueHeight / 2));

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(truePosition, trueWidth, trueHeight, DustType<Dusts.BubbledBubble>(), -5 + i, 0, 100, default(Color), 4f);
                dust.noGravity = true;

                dust = Dust.NewDustDirect(truePosition, trueWidth, trueHeight, 211, 0, 0, 150, default(Color), 2.5f);
                dust.noGravity = true;
                dust.velocity *= 2;
            }

            base.Kill(timeLeft);
        }
    }
}
