using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class TideCallerStaff_AquaPrison : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aqua Prison");
        }

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 64;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            if (projectile.velocity.X > 12)
                projectile.velocity.X = 12;
            else if (projectile.velocity.X < -12)
                projectile.velocity.X = -12;

            Lighting.AddLight(projectile.position, 0f, 0f, 0.5f);
            Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 211);
            dust.alpha = 0;
            dust.noLight = false;
            dust.noGravity = true;
            dust.scale = 1.4f;

            dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustType<Dusts.BubbledBubble>(), 0f, 0, 100, default(Color), 2.5f);
            dust.noGravity = true;

            projectile.velocity.Y += 0.4f;

            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.netUpdate = true;

            if (!target.boss)
            {
                target.AddBuff(BuffType<TideCallerBubbled>(), 120);
                Projectile.NewProjectile(target.Center, Vector2.Zero, ProjectileType<TideCallerStaff_BubbleVisual>(), 0, 0, projectile.owner, target.whoAmI);
            }
            
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            if (Main.netMode != NetmodeID.Server)
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Sploosh").WithVolume(.7f), projectile.position);

            for (int i = 0; i < 30; i++)
            {
                Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 211, 0f, -3f, 0, default(Color), 2f);
            }
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustType<Dusts.BubbledBubble>(), -5 + i, 0, 100, default(Color), 4f);
                dust.noGravity = true;
            }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().bubbled)
                return false;
            else
                return base.CanHitNPC(target);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 20;
            return true;
        }
    }
}
