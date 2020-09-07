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

        public Cleave(int AdditionMeleeDamage)
        {
            baseMeleeDamage = AdditionMeleeDamage;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            string text = "";

            if (modItem.item.type == ItemType<RavenousHydra>())
                text = TerraLeague.CreateColorString(PassiveSecondaryColor, "deal ") + TerraLeague.CreateScalingTooltip(DamageType.MEL, modPlayer.MEL, baseMeleeDamage) + TerraLeague.CreateColorString(PassiveSecondaryColor, " melee damage to near by enemies\nHeal for 10% of the damage");
            else if (modItem.item.type == ItemType<TitanicHydra>())
                text = TerraLeague.CreateColorString(PassiveSecondaryColor, "deal ") + TerraLeague.CreateScalingTooltip(DamageType.MEL, modPlayer.MEL, baseMeleeDamage) + " + "
                     + TerraLeague.CreateScalingTooltip(UI.HealthbarUI.RedHealthColor.Hex3(), "LIFE", Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep, 5)
                     + TerraLeague.CreateColorString(PassiveSecondaryColor, " melee damage to near by enemies");
            else
                text = TerraLeague.CreateColorString(PassiveSecondaryColor, "deal") + " " + TerraLeague.CreateScalingTooltip(DamageType.MEL, modPlayer.MEL, baseMeleeDamage) + TerraLeague.CreateColorString(PassiveSecondaryColor, " melee damage to near by enemies");

            return TooltipName("CLEAVE") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Your melee attacks will periodically ") + text;
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modItem.item.type == ItemType<RavenousHydra>())
            {
                modPlayer.ravenous = true;
                if (modPlayer.cleaveCooldown != 0)
                    modPlayer.cleaveCooldown--;
            }
            else if (modItem.item.type == ItemType<TitanicHydra>())
            {
                modPlayer.titanic = true;
                if (modPlayer.cleaveCooldown != 0)
                    modPlayer.cleaveCooldown--;
            }
            else
            {
                modPlayer.tiamat = true;
                if (modPlayer.cleaveCooldown != 0)
                    modPlayer.cleaveCooldown--;
            }

            base.UpdateAccessory(player, modItem);
        }

        static public void Efx(int user, int type)
        {
            Player player = Main.player[user];

            if (type == 1)
            {
                Main.PlaySound(new LegacySoundStyle(2, 71).WithPitchVariance(-0.2f), player.Center);
                TerraLeague.DustRing(261, player, new Color(255, 170, 0, 0));
                TerraLeague.DustBorderRing(200, player.MountedCenter, 261, new Color(255, 170, 0, 0), 2);
            }
            else if (type == 2)
            {
                Main.PlaySound(new LegacySoundStyle(2, 71).WithPitchVariance(-0.2f), player.Center);
                TerraLeague.DustRing(261, player, new Color(255, 170, 0, 0));
                TerraLeague.DustBorderRing(200, player.MountedCenter, 261, new Color(255, 170, 0, 0), 2);
            }
            else
            {
                Main.PlaySound(new LegacySoundStyle(2, 71).WithPitchVariance(-0.2f), player.Center);
                TerraLeague.DustRing(261, player, new Color(255, 255, 0, 0));
                TerraLeague.DustBorderRing(200, player.MountedCenter, 261, new Color(255, 255, 0, 0), 2);
            }

        }
    }
}
