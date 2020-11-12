using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class CelestialStaff_Starcall : ModProjectile
    {
        bool droppedStar = false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starcall");
        }

        public override void SetDefaults()
        {
            projectile.width = 22;
            projectile.height = 24;
            projectile.timeLeft = 300;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.magic = true;
            projectile.tileCollide = false;
            //projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            projectile.rotation += projectile.direction * 0.1f;

            if (projectile.soundDelay == 0)
            {
                projectile.soundDelay = Main.rand.Next(50, 71);
                Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 9), projectile.position);
            }

            if (projectile.ai[1] == 0f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.ai[1] = 1f;
                projectile.netUpdate = true;
            }
            if (projectile.ai[1] != 0f)
            {
                projectile.tileCollide = true;
            }

            for (int i = 0; i < 2; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 263, 0, 0, 0, new Color(248, 137, 89), 1.5f);
                dust.velocity *= 0.3f;
                dust.noGravity = true;
                dust.noLight = true;

                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 263, 0, 0, 0, new Color(237, 137, 164), 1.5f);
                dust.velocity *= 0.3f;
                dust.noGravity = true;
                dust.noLight = true;
            }
            
            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.width == 22)
                Prime();
            if (!droppedStar)
            {
                if (!target.immortal)
                {
                    float chance = Items.Weapons.CelestialStaff.RejuvDropChance(Main.player[projectile.owner]);

                    if (Main.rand.NextFloat() < chance)
                    {
                        droppedStar = true;
                        Item.NewItem(projectile.Hitbox, ItemType<Items.RegenHeart>());
                    }
                }
            }
                //Projectile.NewProjectileDirect(target.Center, Vector2.Zero, ProjectileType<CelestialStaff_StarcallRejuv>(), 0, 0, projectile.owner);

            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            hitDirection = projectile.Center.X > target.Center.X ? -1 : 1;

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void Kill(int timeLeft)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(3, 3), projectile.position);
            TerraLeague.PlaySoundWithPitch(projectile.Center, 2, 4, -1f);

            Dust dust;
            for (int i = 0; i < 40; i++)
            {
                dust = Dust.NewDustDirect(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 263, 0, 0, 0, new Color(237, 137, 164), 2f);
                dust.noGravity = true;
                dust.velocity *= 2f;

                dust = Dust.NewDustDirect(projectile.Center, 1,1, 263, 0, 0, 0, new Color(248, 137, 89), 2f);
                dust.noGravity = true;
                dust.velocity *= 4f;
            }

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            
            base.Kill(timeLeft);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = 10;
            height = 10;
            return true;
        }

        public void Prime()
        {
            projectile.velocity = Vector2.Zero;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 115;
            projectile.height = 115;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 1;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D texture = Main.projectileTexture[projectile.type];
            spriteBatch.Draw
            (
                texture,
                new Vector2
                (
                    projectile.position.X - Main.screenPosition.X + projectile.width * 0.5f,
                    projectile.position.Y - Main.screenPosition.Y + projectile.height - texture.Height * 0.5f
                ),
                new Rectangle(0, 0, texture.Width, texture.Height),
                Color.White,
                projectile.rotation,
                texture.Size() * 0.5f,
                projectile.scale,
                SpriteEffects.None,
                0f
            );
        }
    }
}
