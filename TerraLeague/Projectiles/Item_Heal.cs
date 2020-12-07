using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class Item_Heal : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heal");
        }

        public override void SetDefaults()
        {
            projectile.width = 8;
            projectile.height = 8;
            projectile.alpha = 0;
            projectile.timeLeft = 1000;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.alpha = 255;
        }

        public override void AI()
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2 dustBoxPosition = new Vector2(projectile.position.X + 6, projectile.position.Y + 6);
                int dustBoxWidth = projectile.width - 12;
                int dustBoxHeight = projectile.height - 12;
                Dust dust = Dust.NewDustDirect(dustBoxPosition, dustBoxWidth, dustBoxHeight, 137, 0, 0, 50, new Color(0, 255, 100), 1.5f);
                dust.noGravity = true;
                dust.velocity *= 0.1f;
                dust.velocity += projectile.velocity * 0.1f;
                dust.position.X -= projectile.velocity.X / 3f * (float)i;
                dust.position.Y -= projectile.velocity.Y / 3f * (float)i;
            }

            Player targetPlayer = Main.player[(int)projectile.ai[0]];

            if (!targetPlayer.active)
            {
                projectile.Kill();
                return;
            }

            Vector2 move = targetPlayer.MountedCenter - projectile.Center;
            AdjustMagnitude(ref move);
            projectile.velocity = (10 * projectile.velocity + move) / 11f;
            AdjustMagnitude(ref projectile.velocity);

            if (projectile.Hitbox.Intersects(targetPlayer.Hitbox))
            {
                HitPlayer(targetPlayer);
                projectile.Kill();
                return;
            }
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 15f)
            {
                vector *= 15f / magnitude;
            }
        }

        public void HitPlayer(Player player)
        {
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 4, 0);
            //Main.PlaySound(new LegacySoundStyle(2, 21), player.Center);

            projectile.netUpdate = true;
            if (projectile.owner == Main.LocalPlayer.whoAmI)
            {
                if (player.whoAmI != projectile.owner)
                    Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().SendHealPacket(projectile.damage, player.whoAmI, -1, projectile.owner);
                if (player.whoAmI == Main.myPlayer)
                {
                    player.GetModPlayer<PLAYERGLOBAL>().lifeToHeal += projectile.damage;
                }
            }
            for (int i = 0; i < 12; i++)
            {
                Dust dust = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 137, 0, 0, 50, new Color(0, 255, 100), 1.2f);
                dust.noGravity = true;
            }

            projectile.velocity.Y = -8;
            projectile.timeLeft = 90;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.penetrate--;
        }

        public override void Kill(int timeLeft)
        {
            
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 16;
            return true;
        }
    }
}
