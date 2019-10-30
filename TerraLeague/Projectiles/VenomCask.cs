using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class VenomCask : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Venom Cask");
        }

        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 26;
            projectile.alpha = 0;
            projectile.timeLeft = 600;
            projectile.penetrate = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.aiStyle = 2;
            base.SetDefaults();
        }

        

        public override void AI()
        {
            Lighting.AddLight(projectile.position, 0f, 0f, 0.5f);
            Dust dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 67, 0f, 0f, 100, new Color(0, 255, 0));
            dust.alpha = 0;
            dust.noLight = false;
            dust.noGravity = true;
            dust.scale = 1.4f;

            base.AI();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int stacks = target.GetGlobalNPC<NPCsGLOBAL>().DeadlyVenomStacks;

            if (stacks < 4)
            {
                target.GetGlobalNPC<NPCsGLOBAL>().DeadlyVenomStacks++;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    target.GetGlobalNPC<NPCsGLOBAL>().PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 3, target.whoAmI, target.GetGlobalNPC<NPCsGLOBAL>().DeadlyVenomStacks);
            }

            target.AddBuff(BuffType<DeadlyVenom>(), 300);

            Prime();
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            //Projectile.NewProjectile(projectile.Center, new Vector2(0, 0), ProjectileType("ExplosionCollision"), (int)(projectile.damage), 10, Main.player[projectile.owner].whoAmI);
            // Play explosion sound
            Main.PlaySound(SoundID.Shatter, projectile.position);
            // Smoke Dust spawn
            for (int i = 0; i < 50; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, new Color(0, 255, 0), 1f);
                Main.dust[dustIndex].velocity *= 1.4f;
            }
            // Fire Dust spawn
            for (int i = 0; i < 80; i++)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 67, 0f, 0f, 100, new Color(0, 255, 0), 2f);
                Main.dust[dustIndex].noGravity = true;

                dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 67, 0f, 0f, 100, new Color(0, 255, 0),1f);
            }


            if (projectile.owner == Main.myPlayer)
            {
                int spawnAmount = Main.rand.Next(12, 21);
                for (int i = 0; i < spawnAmount; i++)
                {
                    Vector2 vector14 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
                    vector14.Normalize();
                    vector14 *= (float)Main.rand.Next(10, 201) * 0.01f;
                    Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, vector14.X, vector14.Y, ProjectileType<VenomCloud>(), 1, 1f, projectile.owner, 0f, (float)Main.rand.Next(-45, 1));
                }
            }

            // reset size to normal width and height.
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            base.Kill(timeLeft);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            // For going through platforms and such, javelins use a tad smaller size
            width = height = 10; // notice we set the width to the height, the height to 10. so both are 10
            return true;
        }

        public void Prime()
        {
            projectile.velocity = Vector2.Zero;
            projectile.tileCollide = false;
            // Set to transparent. This projectile technically lives as  transparent for about 3 frames
            projectile.alpha = 255;
            // change the hitbox size, centered about the original projectile center. This makes the projectile damage enemies during the explosion.
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 150;
            projectile.height = 150;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 3;
        }
    }
}
