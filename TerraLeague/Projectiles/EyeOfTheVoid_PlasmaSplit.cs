using Microsoft.Xna.Framework;
using System.IO;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
	public class EyeOfTheVoid_PlasmaSplit : ModProjectile
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
            projectile.tileCollide = false;
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

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            target.AddBuff(BuffType<OrganicDeconstruction>(), 240);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.position, 0.75f, 0f, 0.75f);
            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 16, 16, 112, 0f, 0f, 255, new Color(59, 0, 255), 2f);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0;
            }

            if (projectile.timeLeft < 10)
                projectile.alpha += 26;
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.whoAmI == projectile.ai[0])
                return false;
            else
                return base.CanHitNPC(target);
        }

        public override void Kill(int timeLeft)
        {
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
