using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Microsoft.Xna.Framework;

namespace TerraLeague.Buffs
{
    public class CrushingBlow : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Crushing Blow");
            Description.SetDefault("Your movement speed has been increased!" +
                "\nYour next attack will deal additional damage equal to you armor!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().crushingBlows = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
