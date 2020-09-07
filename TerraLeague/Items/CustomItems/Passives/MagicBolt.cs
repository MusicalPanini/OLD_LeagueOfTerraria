using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class MagicBolt : Passive
    {
        int extraDamage;
        int magicMinionScaling;
        int cooldown;

        public MagicBolt(int Damage, int MagicMinionScaling, int Cooldown)
        {
            extraDamage = Damage;
            magicMinionScaling = MagicMinionScaling;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            string scaleText;
            if (modPlayer.SUM > modPlayer.MAG)
                scaleText = TerraLeague.CreateScalingTooltip(DamageType.SUM, modPlayer.SUM, magicMinionScaling);
            else
                scaleText = TerraLeague.CreateScalingTooltip(DamageType.MAG, modPlayer.MAG, magicMinionScaling);

            return TooltipName("MAGIC BOLT") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Your next magic or minion attack will deal ") + extraDamage + " + " + scaleText + TerraLeague.CreateColorString(PassiveSecondaryColor, " extra damage") +
                "\n" + TerraLeague.CreateColorString(PassiveSubColor, (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown. Damage scales with the highest of either MAG or SUM");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] <= 0 && (proj.magic || TerraLeague.IsMinionDamage(proj)))
            {
                damage += extraDamage + (int)(Math.Max(modPlayer.SUM, modPlayer.MAG) * magicMinionScaling / 100d);
                Efx(player, target);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendPassiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type, FindIfPassiveIsSecondary(modItem), target.whoAmI);
                modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] = (int)(cooldown * modPlayer.Cdr * 60);
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            AddStat(player, modItem, cooldown * 60, -1, true);
            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player player, NPC HitNPC)
        {
            Main.PlaySound(new LegacySoundStyle(2, 30).WithPitchVariance(0.3f), HitNPC.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(HitNPC.position, HitNPC.width, HitNPC.height, 15, 0, 0, 0, new Color(0, 0, 255, 150), 1.5f);
            }
        }
    }
}
