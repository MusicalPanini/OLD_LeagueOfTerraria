using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TerraLeague.Buffs;
using TerraLeague.Items.Weapons;
using TerraLeague.Items.Weapons.Abilities;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Projectiles
{
    public class TerrorOfTheVoid_FeastTop : ModProjectile
    {
        float YDis = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Feast");
            ProjectileID.Sets.DontAttachHideToAlpha[projectile.type] = true;
        }

        public override void SetDefaults()
        {
            projectile.width = 64;
            projectile.height = 32;
            projectile.alpha = 255;
            projectile.timeLeft = 100;
            projectile.penetrate = -1;
            projectile.friendly = false;
            projectile.hostile = false;
            projectile.magic = true;
            projectile.tileCollide = false;
            projectile.ignoreWater = false;
            projectile.GetGlobalProjectile<PROJECTILEGLOBAL>().abilitySpell = true;
        }

        public override void AI()
        {
            Player player = Main.player[projectile.owner];

            projectile.scale = 3 + 2 * (int)projectile.ai[0];
            projectile.width = (int)(64 * projectile.scale);
            projectile.height = (int)(32 * projectile.scale);
            drawOriginOffsetX = -32;
            drawOriginOffsetY = 32 + 32 * (int)projectile.ai[0];

            Vector2 pos = Main.player[projectile.owner].MountedCenter;
            pos.Y += YDis;
            projectile.Center = pos;

            if ((int)projectile.ai[1] != 1)
            {
                if (projectile.timeLeft < 100 - 14)
                {
                    projectile.alpha = 0;
                    projectile.friendly = true;
                    projectile.timeLeft = 48;
                    projectile.extraUpdates = 1;
                    projectile.ai[1] = 1;
                    TerraLeague.PlaySoundWithPitch(Main.player[projectile.owner].MountedCenter, 3, 30, -1f);
                }
                else
                {
                    //YDis += -8f + -4 * (int)projectile.ai[0];
                    YDis = (4 / 7f) * (float)Math.Pow(100 - projectile.timeLeft - 14, 2) - (112);
                    YDis *= 1 + 0.5f * (int)projectile.ai[0];
                    projectile.alpha -= 10;
                }
            }
            if (projectile.timeLeft > 40 && projectile.timeLeft <= 42 && (int)projectile.ai[1] == 1)
                YDis += 64 + 32 * (int)projectile.ai[0];
            else if (projectile.timeLeft <= 40 && (int)projectile.ai[1] == 1)
            {
                projectile.extraUpdates = 0;
                projectile.friendly = false;

                if (projectile.timeLeft <= 30)
                    projectile.alpha += 255 / 30;
            }

            if (Main.rand.Next(0, 2) == 0)
            {
                Dust dustIndex = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 97, 0f, 0f, 100, default(Color), projectile.ai[0] + 1);
                dustIndex.noGravity = true;
                dustIndex.alpha = projectile.alpha;
            }
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (target.life <= Main.player[projectile.owner].GetModPlayer<PLAYERGLOBAL>().feastStacks && !target.immortal)
            {
                Main.player[projectile.owner].ApplyDamageToNPC(target, 99999, 0, 0, false);
            }

            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.life <= 0)
            {
                Player player = Main.player[projectile.owner];
                Feast feast = new Feast(GetModItem(ItemType<TerrorOfTheVoid>()));

                if (feast != null)
                {
                    feast.DoEfx(player, AbilityType.R);
                    player.GetModPlayer<PLAYERGLOBAL>().feastStacks += target.lifeMax;
                    CombatText.NewText(player.getRect(), new Color(89, 0, 77), "+" + target.lifeMax, true);
                }
            }

            base.OnHitNPC(target, damage, knockback, crit);
        }
    }
}
