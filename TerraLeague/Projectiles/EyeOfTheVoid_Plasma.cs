using Microsoft.Xna.Framework;
using System.IO;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
	public class EyeOfTheVoid_Plasma : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Plasma Fission");
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.alpha = 255;
            projectile.timeLeft = 50;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(projectile.owner);
            writer.Write(projectile.ai[0]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            projectile.owner = reader.ReadInt32();
            projectile.ai[0] = reader.ReadInt32();
        }

        public override void AI()
        {
            if (projectile.velocity.X < 0)
                projectile.spriteDirection = -1;

            projectile.rotation += (float)projectile.direction * 0.1f;
            Lighting.AddLight(projectile.position, 0.75f, 0f, 0.75f);
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 16, 16, 112, 0f, 0f, 255, new Color(59, 0, 255), 2f);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<OrganicDeconstruction>(), 240);
            projectile.position += projectile.velocity * 1;
            projectile.friendly = false;
            projectile.ai[0] = target.whoAmI;
        }

        public override void Kill(int timeLeft)
        {
            Projectile.NewProjectile(projectile.position, projectile.oldVelocity.RotatedBy(MathHelper.ToRadians(90)), ProjectileType<EyeOfTheVoid_PlasmaSplit>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0]);
            Projectile.NewProjectile(projectile.position, projectile.oldVelocity.RotatedBy(MathHelper.ToRadians(-90)), ProjectileType<EyeOfTheVoid_PlasmaSplit>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0]);

            TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 45, 0.25f);
            TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 91, 0.5f);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 8, 8, 112, 0f, 0f, 255, new Color(59, 0, 255), 3.5f);
                dust.noGravity = true;
                dust.noLight = true;
            }
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10; 
            return true;
        }
    }
}
