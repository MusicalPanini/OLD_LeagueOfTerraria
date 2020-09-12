using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class TravelerBlessing : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Blessing of The Traveler");
            Description.SetDefault("15% increased movement speed" +
                "\nIncreased jump height");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 0.15f;
            player.jumpBoost = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
