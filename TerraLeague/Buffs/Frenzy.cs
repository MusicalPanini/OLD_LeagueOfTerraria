using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class Frenzy : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Frenzy");
            Description.SetDefault("Melee and ranged attack speed increased by 20%!" +
                "\nMelee and ranged attacks deal 20 On Hit damage");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.2;
            player.meleeSpeed += 0.2f;
            player.GetModPlayer<PLAYERGLOBAL>().meleeOnHit += 30;
            player.GetModPlayer<PLAYERGLOBAL>().rangedOnHit += 30;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
