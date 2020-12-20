using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class CrystalineVoidEnergy_VoidSeeker : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Seeker");
        }

        public override void SetDefaults()
        {
            projectile.width = 28;
            projectile.height = 28;
            projectile.alpha = 255;
            projectile.timeLeft = 300;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.extraUpdates = 1;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            projectile.rotation = projectile.velocity.ToRotation();
            Lighting.AddLight(projectile.position, 0.75f, 0f, 0.75f);

            double rad = MathHelper.ToRadians((projectile.frameCounter / 24f) * 180f);

            float float1 = (float)(System.Math.Sin(rad) * 12);
            float float2 = (float)(System.Math.Sin(rad + System.Math.PI) * 12);
            int offsetX = 22;
            int offsetY = 14;

            if (projectile.timeLeft == 300)
            {
                if ((int)projectile.ai[0] == 1)
                {
                    projectile.magic = true;
                }
                else
                {
                    projectile.ranged = true;
                }
            }

            if (projectile.ranged)
            {
                Color color = new Color(250, 114, 247);
                Vector2 pos1 = new Vector2(projectile.position.X + offsetX, (projectile.position.Y + offsetY) + float1).RotatedBy(projectile.rotation, projectile.Center);
                Dust dust = Dust.NewDustPerfect(pos1, 112, null, 0, color, 2);
                dust.noGravity = true;
                dust.velocity *= 0;
                dust.noLight = true;

                Vector2 pos2 = new Vector2(projectile.position.X + offsetX, (projectile.position.Y + offsetY) + float2).RotatedBy(projectile.rotation, projectile.Center);
                dust = Dust.NewDustPerfect(pos2, 112, null, 0, color, 2);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0;
            }
            else
            {
                Color color = new Color(59, 0, 255);
                Vector2 pos1 = new Vector2(projectile.position.X + offsetX, (projectile.position.Y + offsetY) + float1).RotatedBy(projectile.rotation, projectile.Center);
                Dust dust = Dust.NewDustPerfect(pos1, 112, null, 0, color, 2);
                dust.noGravity = true;
                dust.velocity *= 0;
                dust.noLight = true;

                Vector2 pos2 = new Vector2(projectile.position.X + offsetX, (projectile.position.Y + offsetY) + float2).RotatedBy(projectile.rotation, projectile.Center);
                dust = Dust.NewDustPerfect(pos2, 112, null, 0, color, 2);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0;

                Vector2 pos3 = new Vector2(projectile.position.X + offsetX, (projectile.position.Y + offsetY)).RotatedBy(projectile.rotation, projectile.Center);
                dust = Dust.NewDustPerfect(pos3, 112, null, 0, color, 1.5f);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 0;
            }

            if (projectile.frameCounter < 48)
                projectile.frameCounter++;
            else
                projectile.frameCounter = 0;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            TerraLeagueNPCsGLOBAL modNPC = target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>();
            PLAYERGLOBAL modPlayer = Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>();

            target.AddBuff(BuffType<CausticWounds>(), 240);
            modNPC.CausticWounds = true;
            modNPC.CausticStacks += projectile.ranged ? 2 : 3;

            int stacks = modNPC.CausticStacks;
            if (stacks - (projectile.ranged ? 2 : 3) < 5 && stacks >= 5)
            {
                projectile.magic = true;

                int damCap = (int)(modPlayer.MAG + 50);

                damage += (target.lifeMax - target.life) / 4 > damCap ? damCap : (target.lifeMax - target.life) / 4;

                projectile.netUpdate = true;
                projectile.ai[0] = 1;

                modPlayer.CausticWoundsEffect(target);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    modPlayer.PacketHandler.SendCausticEFX(-1, projectile.owner, target.whoAmI);
            }
            else if (stacks > 5)
            {
                modNPC.CausticStacks = projectile.ranged ? 2 : 3;
            }

            if (Main.netMode == NetmodeID.MultiplayerClient)
                modNPC.PacketHandler.SendSyncStats(-1, Main.LocalPlayer.whoAmI, 1, target.whoAmI, modNPC.CausticStacks);

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            Color color = projectile.ranged ? new Color(250, 114, 247) : new Color(59, 0, 255);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, 8, 8, 112, 0f, 0f, 255, color, 3.5f);
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
