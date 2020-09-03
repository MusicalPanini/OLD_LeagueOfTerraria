using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Awe : Passive
    {
        int manaReduction;
        int manaToADConversion;
        int manaToAPConversion;

        public Awe(int ManaReduction, int ManaToADConversion, int ManaToAPConversion)
        {
            manaReduction = ManaReduction;
            manaToADConversion = ManaToADConversion;
            manaToAPConversion = ManaToAPConversion;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            string text = "[c/0099cc:Passive: AWE -] [c/99e6ff:Reduced mana usage by " + manaReduction + "%]";

            if (manaToADConversion > 0)
                text += "\n[c/99e6ff:Gain 1% melee and ranged damage per " + manaToADConversion + " max mana]";

            if (manaToAPConversion > 0)
                text += "\n[c/99e6ff:Gain 1% magic and minion damage per " + manaToAPConversion + " max mana]";

            return text;
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            player.manaCost -= manaReduction * 0.01f;
            player.GetModPlayer<PLAYERGLOBAL>().awe = true;

            if (manaToADConversion > 0)
            {
                player.meleeDamage += (player.statManaMax2 / manaToADConversion) * 0.01f;
                player.rangedDamage += (player.statManaMax2 / manaToADConversion) * 0.01f;
            }

            if (manaToAPConversion > 0)
            {
                player.magicDamage += (player.statManaMax2 / manaToAPConversion) * 0.01f;
                player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += (player.statManaMax2 / manaToAPConversion) * 0.01f;
            }

            base.UpdateAccessory(player, modItem);
        }
    }
}
