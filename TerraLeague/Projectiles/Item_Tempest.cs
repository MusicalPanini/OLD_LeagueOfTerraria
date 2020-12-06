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
    public class Item_Tempest : ModProjectile
    {
        int spacing = 48;
        int number = 0;
        int numberOfExtraStrikes = 6;
        public static int randSpread = 64;

        bool leftBlocks = true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tempest");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = -1;
            projectile.alpha = 255;
            projectile.timeLeft = 301;
            projectile.extraUpdates = 35;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;

            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (projectile.soundDelay == 0)
            {
                number = (int)projectile.knockBack;
                projectile.knockBack = 0;

                for (int i = 0; i < 3; i++)
                {
                    Gore gore = Gore.NewGoreDirect(projectile.Center, default(Vector2), Main.rand.Next(61, 64), 1f);
                    gore.velocity.Y = gore.velocity.Y + 1.5f;
                }
            }
            projectile.soundDelay = 10;

            if (leftBlocks && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                leftBlocks = false;
                projectile.netUpdate = true;
            }
            if (!leftBlocks)
            {
                projectile.tileCollide = true;
            }

            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                int dustBoxWidth = projectile.width - 12;
                int dustBoxHeight = projectile.height - 12;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 261, 0f, 0f, 100, new Color(0, 0, 255, 150), 1f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.1f;
                dust.position.X -= projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
            }

            Dust dust2 = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(255, 106, 0, 150), 1f);
            dust2.noGravity = true;
            dust2.velocity *= 3f;
            Lighting.AddLight(projectile.position, 0f, 0f, 1f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();

            return false;
        }

        public override void Kill(int timeLeft)
        {
            if (number < numberOfExtraStrikes && number > -numberOfExtraStrikes)
            {
                if (number == 0)
                {
                    Vector2 pos = new Vector2(projectile.ai[0] + (spacing) + (Main.rand.Next(-randSpread, randSpread)), projectile.ai[1] - 700);
                    Vector2 targetPos = new Vector2(projectile.ai[0] + (spacing), projectile.ai[1]);
                    Vector2 vel = TerraLeague.CalcVelocityToPoint(pos, targetPos, 4);
                    Projectile.NewProjectileDirect(pos, vel, projectile.type, projectile.damage, 1, projectile.owner, targetPos.X, targetPos.Y);

                    pos = new Vector2(projectile.ai[0] + (-spacing) + (Main.rand.Next(-randSpread, randSpread)), projectile.ai[1] - 700);
                    targetPos = new Vector2(projectile.ai[0] + (-spacing), projectile.ai[1]);
                    vel = TerraLeague.CalcVelocityToPoint(pos, targetPos, 4);
                    Projectile.NewProjectileDirect(pos, vel, projectile.type, projectile.damage, -1, projectile.owner, targetPos.X, targetPos.Y);
                }
                else
                {
                    Vector2 pos = new Vector2(projectile.ai[0] + (spacing * number/Math.Abs(number)) + (Main.rand.Next(-randSpread, randSpread)), projectile.ai[1] - 700);
                    Vector2 targetPos = new Vector2(projectile.ai[0] + (spacing * number / Math.Abs(number)), projectile.ai[1]);

                    Vector2 vel = TerraLeague.CalcVelocityToPoint(pos, targetPos, 4);

                    Projectile.NewProjectileDirect(pos, vel, projectile.type, projectile.damage, number > 0 ? number + 1 : number - 1, projectile.owner, targetPos.X, targetPos.Y);
                }
            }

            Main.PlaySound(new LegacySoundStyle(3, 53), projectile.position);
            for (int i = 0; i < 50; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 261, 0, 0, 0, new Color(0, 0, 255, 150), 2f);
                dust.velocity *= 3f;
                dust.noGravity = true;
            }

            base.Kill(timeLeft);
        }

        public override bool? CanHitNPC(NPC target)
        {
            return base.CanHitNPC(target);
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public void Prime()
        {
            projectile.tileCollide = false;
            projectile.velocity = Vector2.Zero;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 64;
            projectile.height = 64;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 2;
        }
    }
}
