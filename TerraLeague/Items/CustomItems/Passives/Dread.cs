using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Dread : Passive
    {
        int maxStacks;
        int lostStacks;
        double magicMinionDamage;

        public Dread(int MaxStacks, int LostStacks, double magicMinionDamageIncrease)
        {
            maxStacks = MaxStacks;
            lostStacks = LostStacks;
            magicMinionDamage = magicMinionDamageIncrease;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("DREAD") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Kills grant stacks up to " + maxStacks)
                + "\n" + TerraLeague.CreateColorString(PassiveSecondaryColor, "Lose " + lostStacks + " stacks when enemies damage you")
                 + "\n" + TooltipName("DO OR DIE") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Gain " + magicMinionDamage + "% magic and minion damage per stack of DREAD");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.magicDamage += (float)(magicMinionDamage * 0.01 * modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)]);
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += magicMinionDamage * 0.01 * modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)];

            base.UpdateAccessory(player, modItem);
        }

        public override void OnKilledNPC(NPC npc, int damage, bool crit, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            AddStat(player, modItem, maxStacks, 1);

            base.OnKilledNPC(npc, damage, crit, player, modItem);
        }

        public override void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            AddStat(player, modItem, maxStacks, -lostStacks, true);
            base.OnHitByNPC(npc, ref damage, ref crit, player, modItem);
        }

        public override void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            AddStat(player, modItem, maxStacks, -lostStacks, true);

            base.OnHitByProjectile(npc, ref damage, ref crit, player, modItem);
        }

        public override void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            AddStat(player, modItem, maxStacks, -lostStacks, true);

            base.OnHitByProjectile(proj, ref damage, ref crit, player, modItem);
        }
    }
}
