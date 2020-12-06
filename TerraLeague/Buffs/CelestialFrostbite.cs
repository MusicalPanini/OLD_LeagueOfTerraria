using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Terraria.ID;

namespace TerraLeague.Buffs
{
    public class CelestialFrostbite : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Celestial Frostbite");
            Description.SetDefault("The mountains peak bares down on you with an immense chill" +
                "\nYou are unable to fly, grapple, build, or use gravity potions");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().celestialFrostbite = true;
            
            //player.noItems = true;
            player.controlHook = false;
            player.noBuilding = true;
            player.wingTime = 0;
            player.slow = true;
            player.gravDir = 1;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
