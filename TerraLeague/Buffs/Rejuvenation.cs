using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Rejuvenation : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Rejuvenation");
            Description.SetDefault("Life regen and movement speed increased!");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed += 0.3f;
            player.lifeRegen += 3;

            player.GetModPlayer<PLAYERGLOBAL>().rejuvenation = true;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
