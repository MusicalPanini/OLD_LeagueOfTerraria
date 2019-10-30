using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class GatheringFire : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Gathering Fire");
            Description.SetDefault("+7% magic damage and critical strike chance" +
                "\n+6 defence" +
                "\n+1 life regeneration" +
                "\n+10% cooldown reduction" +
                "\n+10% movement speed");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.magicDamage += 0.07f;
            player.magicCrit += 7;
            player.statDefense += 6;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().gathFire = true;
            player.moveSpeed += 0.1f;
            player.lifeRegen += 1;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
