﻿using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class ImmortalFireBlessing : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blessing of The Immortal Fire");
            Description.SetDefault("Reduced cooldown of healing potions" +
                "\nIncreased length of invincibility after taking damage");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            player.longInvince = true;
            player.pStone = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
