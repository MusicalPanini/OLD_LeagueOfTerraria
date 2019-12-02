using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Strengthen : Passive
    {
        int maxStacks;
        int lifeperStack;

        public Strengthen(int MaxStacks, int LifePerStack)
        {
            maxStacks = MaxStacks;
            lifeperStack = LifePerStack;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return "[c/0099cc:Passive: STRENGTHEN -] [c/99e6ff:Kills grant stacks up to " + maxStacks + "]" +
                "\n[c/99e6ff:Gain " + lifeperStack + " health per stack]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.statLifeMax2 += (int)modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)];

            base.UpdateAccessory(player, modItem);
        }

        public override void OnKilledNPC(NPC npc, int damage, bool crit, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            AddStat(player, modItem, maxStacks, lifeperStack);

            base.OnKilledNPC(npc, damage, crit, player, modItem);
        }
    }
}
