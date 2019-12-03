using Terraria;
using Terraria.DataStructures;
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
                "\n[c/99e6ff:Gain " + magicPerStack + "% magic damage and " + armorPerStack + " armor per stack]" +
                 "\n[c/007399:Lose all stacks on death]";
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

        public override int PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] = 0;

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource, player, modItem);
        }
    }
}
