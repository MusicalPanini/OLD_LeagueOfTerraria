using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items.CompleteItems;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Echo : Passive
    {
        int baseDamage;
        double magicScaling;

        public Echo(int BaseDamage, int MagicScaling)
        {
            baseDamage = BaseDamage;
            magicScaling = MagicScaling;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TerraLeague.CreateColorString(PassiveSubColor, "Moving will generate charge")
                + "\n" + TooltipName("ECHO") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Your next magic attack will deal ") + baseDamage + " + " +  TerraLeague.CreateScalingTooltip(DamageType.MAG, modPlayer.MAG, (int)magicScaling) + TerraLeague.CreateColorString(PassiveSecondaryColor, " extra magic damage and launch 8 homing projectiles");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (passiveStat >= 100 && proj.magic)
            {
                int bonusDamage = baseDamage + (int)(modPlayer.MAG * magicScaling / 100d);
                damage += bonusDamage;

                Efx(player, target);
                SendEfx(player, target, modItem);

                for (int i = 0; i < 8; i++)
                {
                    Projectile.NewProjectileDirect(target.position, new Vector2(14, 0).RotatedBy(MathHelper.ToRadians(45 * i)), ProjectileType<Item_Echo>(), bonusDamage, 0, player.whoAmI, target.whoAmI);
                }
                passiveStat = 0;
                modPlayer.echo = false;
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            float stat;
            if (player.velocity.X < 0)
                stat = -player.velocity.X * 0.05f;
            else
                stat = player.velocity.X * 0.05f;

            AddStat(player, 100, stat);

            if (modPlayer.echo)
            {
                player.AddBuff(BuffType<Buffs.Echo>(), 1);
                if (Main.rand.Next(0, 6) == 0)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 261, 0, 0, 0, new Color(255, 0, 255, 150));
                    dust.noGravity = true;
                }
            }
            else
                player.ClearBuff(BuffType<Buffs.Echo>());

            base.PostPlayerUpdate(player, modItem);
        }

        public override void Efx(Player user, NPC effectedNPC)
        {
            Main.PlaySound(new LegacySoundStyle(2, 12).WithPitchVariance(-0.6f), effectedNPC.Center);
        }
    }
}
