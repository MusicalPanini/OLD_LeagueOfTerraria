﻿using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Dissonance : Passive
    {
        int magicMinionDamage;
        int perMana;
        
        public Dissonance(int magicMinionDamageIncrease, int PerMana)
        {
            magicMinionDamage = magicMinionDamageIncrease;
            perMana = PerMana;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: DISSONANCE -] [c/99e6ff:Gain " + magicMinionDamage + "% magic and minion damage per " + perMana + " current mana]" +
                "\n[c/007399:Disables HARMONY passive]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.magicDamage += (player.statMana / perMana) * (magicMinionDamage * 0.01f);
            modPlayer.TrueMinionDamage += (player.statMana / 40) * 0.01;

            base.UpdateAccessory(player, modItem);
        }
    }
}
