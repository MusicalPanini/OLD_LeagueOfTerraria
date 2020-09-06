using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Buffs;
using TerraLeague.Items;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    class World_CelestialMeteorite : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Celestial Meteorite");
        }

        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 36;
            projectile.alpha = 0;
            projectile.penetrate = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = false;
            projectile.aiStyle = 0;
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, Color.OrangeRed.ToVector3());

            if (projectile.soundDelay == 0)
            {
                projectile.soundDelay = 20 + Main.rand.Next(40);

                Vector2 position = projectile.Center;
                //if (projectile.Distance(Main.LocalPlayer.MountedCenter) > 2000)
                //{
                //    Vector2 dis = (projectile.Center - Main.LocalPlayer.MountedCenter).SafeNormalize(Vector2.Zero);
                //    dis *= 2000;
                //    position += dis;
                //}
                SoundEffectInstance sound = TerraLeague.PlaySoundWithPitch(position, 2, 9, -1f);

                if (sound != null)
                {
                    if (sound.Volume * 3 > 1)
                        sound.Volume = 1;
                    else
                        sound.Volume *= 3;
                }
            }

            projectile.rotation += projectile.velocity.X * 0.05f;

            for (int i = 0; i < 3; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, 0, 0, 0, default(Color), 4f);
                dust.noGravity = true;

                dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 6, 0, 3, 0, default(Color), 1f);
            }


            base.AI();
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Prime();
            return false;
        }


        public override void Kill(int timeLeft)
        {
            Vector2 position = Main.LocalPlayer.MountedCenter;
            if (projectile.Distance(Main.LocalPlayer.MountedCenter) > 1000)
            {
                Vector2 dis = (projectile.Center - Main.LocalPlayer.MountedCenter).SafeNormalize(Vector2.Zero);
                dis *= 1000;
                position += dis;
            }
            TerraLeague.PlaySoundWithPitch(position, 2, 89, -1f);
            SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_ExplosiveTrapExplode, position);
            Main.NewText("A Celestial Comet has landed", new Color(0, 200, 255));
            if (sound != null)
            {
                sound.Pitch = -1;

                if (sound.Volume * 10 > 1)
                    sound.Volume = 1;
                else
                    sound.Volume *= 10;
            }

            Dust dust;
            for (int i = 0; i < 75; i++)
            {
                dust = Dust.NewDustDirect(projectile.position + new Vector2(projectile.width * 0.4f, projectile.height * 0.4f), (int)(projectile.width * 0.2), (int)(projectile.height * 0.2), 31, 0f, 0f, 100, default(Color), 2f);
                dust.velocity.X *= Main.rand.NextFloat(3f);
                dust.fadeIn = Main.rand.NextFloat(2, 4);
                dust = Dust.NewDustDirect(projectile.position + new Vector2(projectile.width * 0.4f, projectile.height * 0.4f), (int)(projectile.width * 0.2), (int)(projectile.height * 0.2), 31, 0f, 0f, 100, default(Color), 2f);
                dust.velocity.X *= Main.rand.NextFloat(6, 10);
                dust.velocity.Y *= 0.5f;
                dust.fadeIn = Main.rand.NextFloat(2, 4);
                dust = Dust.NewDustDirect(projectile.position + new Vector2(projectile.width * 0.4f, projectile.height * 0.4f), (int)(projectile.width * 0.2), (int)(projectile.height * 0.2), 31, 0f, 0f, 100, default(Color), 2f);
                dust.velocity.Y *= -Main.rand.NextFloat(6, 10);
                ///dust.velocity.Y *= 0.5f;
                dust.fadeIn = Main.rand.NextFloat(2, 4);
                dust = Dust.NewDustDirect(projectile.position + new Vector2(projectile.width * 0.4f, projectile.height * 0.4f), (int)(projectile.width * 0.2), (int)(projectile.height * 0.2), 59, 0f, -3f, 100, default(Color), 3f);
                dust.velocity.Y = -Main.rand.NextFloat(1, 4);
                dust = Dust.NewDustDirect(projectile.position + new Vector2(projectile.width * 0.4f, projectile.height * 0.4f), (int)(projectile.width * 0.2), (int)(projectile.height * 0.2), 174, 0f, 0f, 100, default(Color), 2f);
                dust.noGravity = true;
                dust.fadeIn = 3;
            }
            for (int i = 0; i < 500; i++)
            {
                dust = Dust.NewDustDirect(projectile.position + new Vector2(projectile.width * 0.4f, projectile.height * 0.4f), (int)(projectile.width * 0.2), (int)(projectile.height * 0.2), 174, 0f, Main.rand.NextFloat(-10f, -3f), 100, default(Color), 1f);
                //dust.noGravity = true;
                dust.velocity.X *= Main.rand.NextFloat(3f);
                dust.velocity *= 1.5f;
                dust.color = new Color(255, 0, 220);
                dust.fadeIn = Main.rand.NextFloat(1, 3);
            }

            //TerraLeague.DustBorderRing(projectile.width / 2, projectile.Center, 174, new Color(255, 0, 220), 3);
            Item.NewItem(projectile.Hitbox, ItemType<FragmentOfTheAspect>());

            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 10;
            projectile.height = 10;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);


            base.Kill(timeLeft);
        }

        public void Prime()
        {
            projectile.velocity = Vector2.Zero;
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.position.X = projectile.position.X + (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y + (float)(projectile.height / 2);
            projectile.width = 1000;
            projectile.height = 1000;
            projectile.position.X = projectile.position.X - (float)(projectile.width / 2);
            projectile.position.Y = projectile.position.Y - (float)(projectile.height / 2);
            projectile.timeLeft = 1;
        }

    }
}
