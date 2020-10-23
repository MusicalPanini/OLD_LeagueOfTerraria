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

            return TooltipName("ATTUNEMENT") +
                TerraLeague.CreateColorString(PassiveSecondaryColor, "Kills grant stacks up to " + maxStacks +
                "\nGain " + magicPerStack + "% magic damage and " + armorPerStack + " armor per stack") +
                "\n" + TerraLeague.CreateColorString(PassiveSubColor, "Lose all stacks on death");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.magicDamage += (float)magicPerStack * passiveStat * 0.01f;
            modPlayer.armor += (int)(armorPerStack * (int)passiveStat);
            base.UpdateAccessory(player, modItem);
        }

        public override void OnKilledNPC(NPC npc, int damage, bool crit, Player player, ModItem modItem)
        {
            AddStat(player, maxStacks, 1);

            base.OnKilledNPC(npc, damage, crit, player, modItem);
        }

        public override int PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, Player player, ModItem modItem)
        {
            passiveStat = 0;

            return base.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource, player, modItem);
        }
    }
}
