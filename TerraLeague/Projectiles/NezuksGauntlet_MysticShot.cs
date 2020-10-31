using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Items.Weapons.Abilities;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class NezuksGauntlet_MysticShot : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mystic Shot");
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;
            projectile.alpha = 255;
            projectile.timeLeft = 60;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            for (int i = 0; i < 5; i++)
            {
                Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                int dustBoxWidth = projectile.width - 12;
                int dustBoxHeight = projectile.height - 12;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 91, 0f, 0f, 100, default(Color), 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.2f;
                dust.position.X -= projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
            }

            Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 15, projectile.velocity.X, projectile.velocity.Y, 50, default(Color), 1f);
            dust2.noGravity = true;
            dust2.velocity *= 0.6f;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 91, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 50, default(Color), 2f);
                dust.noGravity = true;
                dust.velocity *= 0.6f;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return true;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (target.HasBuff(BuffType<Buffs.EssenceFluxDebuff>()))
            {
                modPlayer.magicFlatDamage += EssenceFlux.GetFluxDamage(modPlayer);

                TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 12, 0.5f);

                projectile.magic = true;
                player.ManaEffect(100);
                player.statMana += 100;
            }

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }

    public class MysticShotGlobalNPC : GlobalNPC
    {
        public override void OnHitByProjectile(NPC npc, Projectile projectile, int damage, float knockback, bool crit)
        {
            if (npc.HasBuff(BuffType<Buffs.EssenceFluxDebuff>()))
            {
                if (projectile.type == ProjectileType<NezuksGauntlet_MysticShot>())
                    npc.DelBuff(npc.FindBuffIndex(BuffType<Buffs.EssenceFluxDebuff>()));
            }
        }
    }
}
