using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Attunement : Passive
    {
        int maxStacks;
        decimal magicPerStack;
        decimal armorPerStack;

        public Attunement(int MaxStacks, decimal MagicPerStack, decimal ArmorPerStack)
        {
            maxStacks = MaxStacks;
            magicPerStack = MagicPerStack;
            armorPerStack = ArmorPerStack;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return "[c/0099cc:Passive: ATTUNEMENT -] [c/99e6ff:Kills grant stacks up to " + maxStacks + "]" +
                "\n[c/99e6ff:Gain " + magicPerStack + "% magic damage and " + armorPerStack + " armor per stack]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.magicDamage += (float)magicPerStack * (int)modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] * 0.01f;
            modPlayer.armor += (int)(armorPerStack * (int)modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)]);
            base.UpdateAccessory(player, modItem);
        }

        public override void OnKilledNPC(NPC npc, int damage, bool crit, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            AddStat(player, modItem, maxStacks, 1);

            base.OnKilledNPC(npc, damage, crit, player, modItem);
        }
    }
}
