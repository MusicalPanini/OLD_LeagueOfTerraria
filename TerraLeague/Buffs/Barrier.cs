﻿using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Barrier : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Barrier");
            Description.SetDefault("You have a cool shield!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            Main.buffNoTimeDisplay[Type] = false;

            if (Main.LocalPlayer.whoAmI == player.whoAmI)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                for (int i = 0; i < modPlayer.Shields.Count; i++)
                {
                    if (modPlayer.Shields[i].AssociatedBuff == Type)
                    {
                        break;
                    }

                    if (i == modPlayer.Shields.Count - 1)
                    {
                        player.ClearBuff(Type);
                    }
                }

                if (modPlayer.Shields.Count == 0)
                {
                    player.ClearBuff(Type);
                }
            }
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
