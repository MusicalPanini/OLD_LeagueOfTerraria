using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Dissolve : Passive
    {
        int armorReduction;

        public Dissolve(int percentArmorPen)
        {
            armorReduction = percentArmorPen;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("DISSOLVE") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Magic attacks ignore " + armorReduction + "% of enemy armor");
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.magicArmorPen += (int)(target.defense * armorReduction * 0.01);

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
