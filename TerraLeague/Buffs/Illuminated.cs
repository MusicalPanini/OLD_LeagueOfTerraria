﻿using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class Illuminated : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Illuminated");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.GetGlobalNPC<NPCsGLOBAL>().illuminated = true;
        }
    }
}
