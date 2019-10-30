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
            Description.SetDefault("Ammo consume chance and melee speed increased by 10%!" +
                "\nMelee and ranged attacks deal 20 On Hit damage");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PLAYERGLOBAL>().ConsumeAmmoChance += 0.1;
            player.meleeSpeed += 0.1f;
            player.GetModPlayer<PLAYERGLOBAL>().meleeOnHit += 20;
            player.GetModPlayer<PLAYERGLOBAL>().rangedOnHit += 20;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
