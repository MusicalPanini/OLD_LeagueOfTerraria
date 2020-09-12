﻿using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class WarBlessing : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blessing of The Warrior");
            Description.SetDefault("10% increased damage");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.meleeDamage += 0.1f;
            player.rangedDamage += 0.1f;
            player.magicDamage += 0.1f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.1;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
