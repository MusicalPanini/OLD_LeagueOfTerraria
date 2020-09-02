using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class TrueIceFlail_GlacialPrison : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("GlacialPrison");
        }

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.alpha = 255;
            projectile.scale = 0.75f;
            projectile.timeLeft = 45;
            projectile.magic = true;
            projectile.aiStyle = 0;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 25;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            for (int i = 0; i < 1; i++)
            {
                Dust dustIndex = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 67, 0f, 0f, 100, default(Color), 1.5f);
                dustIndex.noGravity = true;
                dustIndex.velocity *= 0.3f;
            }
            projectile.rotation += 0.4f * (float)projectile.direction;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Main.PlaySound(SoundID.Item10, projectile.position);
            return true;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.netUpdate = true;
            target.buffImmune[BuffType<Frozen>()] = false;
            TerraLeague.RemoveBuffFromNPC(BuffType<FrozenCooldown>(), target.whoAmI);
            target.AddBuff(BuffType<Frozen>(), 120);
            projectile.Center = target.Center;
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 27), projectile.position);
                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 14, 1f);
            }
            if (Main.LocalPlayer.whoAmI == projectile.owner)
            {
                Projectile proj = Projectile.NewProjectileDirect(projectile.Center, Vector2.Zero, ProjectileType<TrueIceFlail_GlacialStorm>(), projectile.damage, 0, projectile.owner);
                //proj.Center = projectile.Center;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;

            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
    }
}
