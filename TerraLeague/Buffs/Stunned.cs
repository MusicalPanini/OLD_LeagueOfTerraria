﻿using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Stunned : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Stunned");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
        }

        public override void Update(NPC npc, ref int buffIndex)
        {

            if(!npc.boss)
            {
                npc.GetGlobalNPC<NPCsGLOBAL>().stunned = true;
                    
                    //npc.position.X = npc.oldPosition.X;
                    //npc.position.Y = npc.oldPosition.Y;
                    //if (npc.oldVelocity.X < 0)
                    //    npc.velocity.X = -0.000001f;
                    //else
                    //    npc.velocity.X = 0.000001f;
            }
        }
    }
}
