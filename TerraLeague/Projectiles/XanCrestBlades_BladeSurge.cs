using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Projectiles
{
	public class XanCrestBlades_BladeSurge : ModProjectile
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blade Surge");
        }

        public override void SetDefaults()
        {
            projectile.width = 60;
            projectile.height = 60;
            projectile.timeLeft = 15;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.melee = true;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.tileCollide = true;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            //if (!Main.npc[(int)projectile.ai[0]].active)
            //{
            //    projectile.Kill();
            //}
            //else
            //{
            //    projectile.timeLeft = 300;

            //    if (projectile.localAI[0] == 0f)
            //    {
            //        AdjustMagnitude(ref projectile.velocity);
            //        projectile.localAI[0] = 1f;
            //    }
            //    Vector2 move = Vector2.Zero;

            //    NPC npc = Main.npc[(int)projectile.ai[0]];

            //    Vector2 newMove = npc.Center - projectile.Center;
            //    float distanceTo = (float)Math.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
            //    move = newMove;
            //    AdjustMagnitude(ref move);
            //    projectile.velocity = (10 * projectile.velocity + move) / 20f;
            //    AdjustMagnitude(ref projectile.velocity);

            //    projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;

            //    Main.player[projectile.owner].MountedCenter = projectile.Center;
            //    Main.player[projectile.owner].fullRotationOrigin = new Vector2(16, 32);
            //    Main.player[projectile.owner].fullRotation = projectile.rotation;
            //    Main.player[projectile.owner].itemTime = 5;
            //}

            projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Main.player[projectile.owner].MountedCenter = projectile.Center;
            Main.player[projectile.owner].fullRotationOrigin = new Vector2(8, 24);
            Main.player[projectile.owner].fullRotation = projectile.rotation;
            Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().contactDodge = true;
            Main.player[projectile.owner].itemTime = 5;
        }

        private void AdjustMagnitude(ref Vector2 vector)
        {
            float magnitude = (float)Math.Sqrt(vector.X * vector.X + vector.Y * vector.Y);
            if (magnitude > 8)
            {
                vector *= 24f / magnitude;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0)
            {
                Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().AbilityCooldowns[(int)AbilityType.Q] = 0;
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void Kill(int timeLeft)
        {
            Main.player[projectile.owner].fullRotation = 0;
            Main.player[projectile.owner].velocity = projectile.oldVelocity / 4;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return base.PreDraw(spriteBatch, lightColor);
        }

        public override bool? CanHitNPC(NPC target)
        {
            //if ((int)projectile.ai[0] == target.whoAmI)
            //    return true;
            //else
            //    return false;
            return base.CanHitNPC(target);
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            width = height = 10;
            return base.TileCollideStyle(ref width, ref height, ref fallThrough);
        }
    }
}
