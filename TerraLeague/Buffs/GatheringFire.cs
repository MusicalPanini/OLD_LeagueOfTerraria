using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class GatheringFire : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Gathering Fire");
            Description.SetDefault("+10 increased MEL, RNG, MAG, and SUM" +
                "\n+5 defence" +
                "\n+1 life regeneration" +
                "\n+4 mana regeneration" +
                "\n+20 Ability Haste" +
                "\n+10% movement speed" +
                "\n+10% heal power");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 5;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 20;
            player.GetModPlayer<PLAYERGLOBAL>().gathFire = true;
            player.moveSpeed += 0.1f;
            player.lifeRegen += 1;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegen += 4;
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().BonusMEL += 10;
            player.GetModPlayer<PLAYERGLOBAL>().BonusRNG += 10;
            player.GetModPlayer<PLAYERGLOBAL>().BonusMAG += 10;
            player.GetModPlayer<PLAYERGLOBAL>().BonusSUM += 10;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
