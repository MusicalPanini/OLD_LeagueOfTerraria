using Terraria;
using Terraria.ModLoader;
using TerraLeague.NPCs;
using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons;
using TerraLeague.Projectiles;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Buffs
{
    public class TidecallersBlessing : ModBuff
    {
        int damage = 0;

        public override void SetDefaults()
        {
            DisplayName.SetDefault("Tidecaller's Blessing");
            Description.SetDefault("Movement speed increased by 10%" +
                "\nYour attacks slow and deal On Hit damage");
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;

        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (damage == 0)
            {
                damage = player.buffTime[buffIndex] + 1;
                player.buffTime[buffIndex] = 60 * 6;
            }

            if (player.buffTime[buffIndex] == 0)
                damage = 0;

            player.moveSpeed += 0.10f;
            player.GetModPlayer<PLAYERGLOBAL>().tidecallersBlessing = true;

            player.GetModPlayer<PLAYERGLOBAL>().meleeOnHit += damage;
            player.GetModPlayer<PLAYERGLOBAL>().rangedOnHit += damage;
            player.GetModPlayer<PLAYERGLOBAL>().magicOnHit += damage;
            player.GetModPlayer<PLAYERGLOBAL>().minionOnHit += damage;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
}
