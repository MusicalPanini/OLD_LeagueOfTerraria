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
    public class TempestEcho : Passive
    {
        int baseDamage;
        double magicScaling;

        public TempestEcho(int BaseDamage, int MagicScaling, int Cooldown)
        {
            baseDamage = BaseDamage;
            magicScaling = MagicScaling;
            passiveCooldown = Cooldown;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("Tempest") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Your next magic or minion attack will deal ") + baseDamage + " + " +  TerraLeague.CreateScalingTooltip(DamageType.MAG, modPlayer.MAG, (int)magicScaling) + TerraLeague.CreateColorString(PassiveSecondaryColor, " extra magic damage\nand create 11 bolts of lightning around the target")
            + "\n" + TerraLeague.CreateColorString(PassiveSubColor, GetScaledCooldown(player) + " second cooldown.");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (cooldownCount <= 0 && (proj.magic || TerraLeague.IsMinionDamage(proj)))
            {
                int bonusDamage = baseDamage + (int)(modPlayer.MAG * magicScaling / 100d);
                damage += bonusDamage;

                Vector2 pos = new Vector2(target.Center.X + (Main.rand.Next(-Item_Tempest.randSpread, Item_Tempest.randSpread)), target.Center.Y - 700);
                Vector2 vel = TerraLeague.CalcVelocityToPoint(pos, target.Center, 4);

                Projectile.NewProjectileDirect(pos, vel, ProjectileType<Item_Tempest>(), bonusDamage, 0, player.whoAmI, target.Center.X, target.Center.Y);

                Efx(player, target);
                SendEfx(player, target, modItem);
                SetCooldown(player);
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        public override void Efx(Player user, NPC effectedNPC)
        {
            Main.PlaySound(new LegacySoundStyle(2, 12).WithPitchVariance(-0.6f), effectedNPC.Center);
        }
    }
}
