using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class GargoylesMight : ModBuff
    {
        public bool initial = true;
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Metallicize");
            Description.SetDefault("Your health has been increased and your damage has been decreased");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            Main.buffNoTimeDisplay[Type] = false;
            player.meleeDamage -= 0.6f;
            player.rangedDamage -= 0.6f;
            player.magicDamage -= 0.6f;
            player.minionDamage -= 0.6f;
            player.thrownDamage -= 0.6f;
            player.statLifeMax2 += player.statLifeMax2 / 2;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
