using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class CriticalEdge : Passive
    {
        int percentIncrease;

        public CriticalEdge(int PercentCritDamageIncrease)
        {
            percentIncrease = PercentCritDamageIncrease;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("CRITICAL EDGE") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Ranged crits deal " + percentIncrease + "% more damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            player.GetModPlayer<PLAYERGLOBAL>().critEdge = true;
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (crit && proj.ranged)
                modPlayer.rangedModifer *= (0.01 * percentIncrease) + 1;

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
