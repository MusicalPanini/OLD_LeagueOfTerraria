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
            Description.SetDefault("25% increased movement speed" +
                "\nIncreased jump height");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.persistentBuff[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.25f;
            player.jumpBoost = true;
            if (player.GetModPlayer<PLAYERGLOBAL>().bottleOfStardust)
                player.moveSpeed += 0.75f;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.bottleOfStardust)
                tip = "100% increased movement speed" +
                "\nIncreased jump height";
            else
                tip = "25% increased movement speed" +
                "\nIncreased jump height";
            base.ModifyBuffTip(ref tip, ref rare);
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
