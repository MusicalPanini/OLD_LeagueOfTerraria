using Microsoft.Xna.Framework;
using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Cleave : Passive
    {
        int baseMeleeDamage;
        CleaveType type;

        public Cleave(int AdditionMeleeDamage, CleaveType Type)
        {
            baseMeleeDamage = AdditionMeleeDamage;
            type = Type;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            string text = "";

            switch (type)
            {
                case CleaveType.Basic:
                    text = TerraLeague.CreateColorString(PassiveSecondaryColor, "deal") + " " + TerraLeague.CreateScalingTooltip(DamageType.MEL, modPlayer.MEL, baseMeleeDamage) + TerraLeague.CreateColorString(PassiveSecondaryColor, " melee damage to near by enemies");
                    break;
                case CleaveType.MaxLife:
                    text = TerraLeague.CreateColorString(PassiveSecondaryColor, "deal ") + TerraLeague.CreateScalingTooltip(DamageType.MEL, modPlayer.MEL, baseMeleeDamage) + " + "
                     + TerraLeague.CreateScalingTooltip(UI.HealthbarUI.RedHealthColor.Hex3(), "LIFE", Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep, 5)
                     + TerraLeague.CreateColorString(PassiveSecondaryColor, " melee damage to near by enemies");
                    break;
                case CleaveType.Lifesteal:
                    text = TerraLeague.CreateColorString(PassiveSecondaryColor, "deal ") + TerraLeague.CreateScalingTooltip(DamageType.MEL, modPlayer.MEL, baseMeleeDamage) 
                        + TerraLeague.CreateColorString(PassiveSecondaryColor, " melee damage to near by enemies\nHeal for 10% of the damage");
                    break;
                default:
                    break;
            }

            return TooltipName("CLEAVE") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Your melee attacks will periodically ") + text;
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            switch (type)
            {
                case CleaveType.Basic:
                    modPlayer.cleaveBasic = true;
                    if (modPlayer.cleaveCooldown != 0)
                        modPlayer.cleaveCooldown--;
                    break;
                case CleaveType.MaxLife:
                    modPlayer.cleaveMaxLife = true;
                    if (modPlayer.cleaveCooldown != 0)
                        modPlayer.cleaveCooldown--;
                    break;
                case CleaveType.Lifesteal:
                    modPlayer.cleaveLifesteal = true;
                    if (modPlayer.cleaveCooldown != 0)
                        modPlayer.cleaveCooldown--;
                    break;
                default:
                    break;
            }

            base.UpdateAccessory(player, modItem);
        }

        static public void Efx(int user, CleaveType type)
        {
            Player player = Main.player[user];

            if (type == CleaveType.MaxLife)
            {
                Main.PlaySound(new LegacySoundStyle(2, 71).WithPitchVariance(-0.2f), player.Center);
                TerraLeague.DustRing(261, player, new Color(255, 170, 0, 0));
                TerraLeague.DustBorderRing(200, player.MountedCenter, 261, new Color(255, 170, 0, 0), 2);
            }
            else if (type == CleaveType.Lifesteal)
            {
                Main.PlaySound(new LegacySoundStyle(2, 71).WithPitchVariance(-0.2f), player.Center);
                TerraLeague.DustRing(261, player, new Color(255, 170, 0, 0));
                TerraLeague.DustBorderRing(200, player.MountedCenter, 261, new Color(255, 170, 0, 0), 2);
            }
            else if(type == CleaveType.Basic)
            {
                Main.PlaySound(new LegacySoundStyle(2, 71).WithPitchVariance(-0.2f), player.Center);
                TerraLeague.DustRing(261, player, new Color(255, 255, 0, 0));
                TerraLeague.DustBorderRing(200, player.MountedCenter, 261, new Color(255, 255, 0, 0), 2);
            }

        }
    }

    public enum CleaveType
    {
        Basic,
        MaxLife,
        Lifesteal
    }
}
