using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;

namespace TerraLeague.Buffs
{
    public class FleetFoot : ModBuff
    {
        int damage = 0;

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Inspired");
            Description.SetDefault("Movement speed increased by 100%" +
                "\nYour attacks deal On Hit damage");
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.moveSpeed *= 2f;
            if (damage == 0)
            {
                damage = player.buffTime[buffIndex] + 1;
                player.buffTime[buffIndex] = 60 * 6;
            }

            if (player.buffTime[buffIndex] == 0)
                damage = 0;

            player.GetModPlayer<PLAYERGLOBAL>().meleeOnHit += damage;
            player.GetModPlayer<PLAYERGLOBAL>().rangedOnHit += damage;
            player.GetModPlayer<PLAYERGLOBAL>().magicOnHit += damage;
            player.GetModPlayer<PLAYERGLOBAL>().minionOnHit += damage;
        }
    }
}
