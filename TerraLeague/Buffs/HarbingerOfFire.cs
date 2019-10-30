using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class HarbingerOfFire : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Harbinger of Fire");
            Description.SetDefault("Heat courses through your soul!" +
                "\nAll attacks deal 40 On Hit damage and apply 'Harbingers Inferno'");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.meleeOnHit += 40;
            modPlayer.rangedOnHit += 40;
            modPlayer.magicOnHit += 40;
            modPlayer.minionOnHit += 40;
            modPlayer.flameHarbinger = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
