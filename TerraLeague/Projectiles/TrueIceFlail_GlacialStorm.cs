using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class TrueIceFlail_GlacialStorm : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Glacial Storm");
        }

        public override void SetDefaults()
        {
            projectile.width = 512;
            projectile.height = 512;
            projectile.timeLeft = 160;
            projectile.penetrate = 1000;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void AI()
        {
            if (Main.rand.Next(0, 1) == 0)
            {
                Dust dust = Dust.NewDustDirect(projectile.Center - (Vector2.One * 64), 128, 128, 67, 0, 0, 50, default(Color), 1.5f);
                dust.velocity *= 5f;
                dust.noGravity = true;
                dust.noLight = true;
            }

            int displacement = Main.rand.Next(24);

            for (int i = 0; i < 18 * 3; i++)
            {
                Vector2 pos = new Vector2(256, 0).RotatedBy(MathHelper.ToRadians((20 * i) + displacement)) + projectile.Center;

                Dust dustR = Dust.NewDustPerfect(pos, 113, Vector2.Zero, 0, default(Color), 1);
                dustR.noGravity = true;
            }

            if (projectile.timeLeft % 15 == 0)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    if (!npc.friendly && !npc.immortal && !npc.townNPC && npc.active && npc.CanBeChasedBy())
                    {
                        if (npc.Hitbox.Intersects(projectile.Hitbox))
                        {
                            npc.AddBuff(BuffType<Buffs.Slowed>(), 15);
                        }
                    }
                }
            }

            if (projectile.timeLeft <= 2)
                projectile.friendly = true;
        }

        public override void Kill(int timeLeft)
        {
            projectile.friendly = true;

            TerraLeague.DustRing(67, projectile, default(Color));
            TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 82, -0.7f);

            base.Kill(timeLeft);
        }

        public void Prime()
        {
            projectile.velocity = Vector2.Zero;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 256;
            projectile.height = 256;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
