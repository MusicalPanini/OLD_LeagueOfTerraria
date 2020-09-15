using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
    public class Gravitum_BindingEclipse : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Binding Eclipse");
        }

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 1.2f;
            projectile.timeLeft = 300;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            projectile.Center = Main.npc[(int)projectile.ai[0]].Center;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(ModContent.BuffType<Stunned>(), 60 * 6);
            target.DelBuff(target.FindBuffIndex(ModContent.BuffType<GravitumMark>()));
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new LegacySoundStyle(2, 74), projectile.Center);
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 111), projectile.Center);

            Dust dust;
            for (int i = 0; i < 20; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X + projectile.width / 4, projectile.position.Y + projectile.width / 4), projectile.width / 2, projectile.height / 2, 54, 0f, 0f, 100, new Color(0, 0, 0), 2f);
                dust.noGravity = true;
                dust.velocity = (dust.position - projectile.Center) * -0.1f;

                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 71, 0f, 0f, 100, default(Color), 1f);
                dust.noGravity = true;
                dust.velocity *= 3f;
                dust.velocity = (dust.position - projectile.Center) * -0.01f;

                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 71, 0f, 0f, 100, default(Color), 1f);
                dust.noGravity = true;
                dust.velocity = (dust.position - projectile.Center) * -0.05f;
            }

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if ((int)projectile.ai[0] == target.whoAmI)
                return true;
            else
                return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
