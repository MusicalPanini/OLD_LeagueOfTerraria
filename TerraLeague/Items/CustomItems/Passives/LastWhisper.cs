using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class LastWhisper : Passive
    {
        int percentArmorReduction;
        bool includeMeleeDamage;

        public LastWhisper(int PercentArmorReduction, bool IncludeMeleeDamage)
        {
            percentArmorReduction = PercentArmorReduction;
            includeMeleeDamage = IncludeMeleeDamage;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            string text = "";

            if (includeMeleeDamage)
                text = "Melee and ranged ";
            else
                text = "Ranged ";

                text += "attacks ignore " + percentArmorReduction + "% of enemy armor";

            return TooltipName("LAST WHISPER") + TerraLeague.CreateColorString(PassiveSecondaryColor, text);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (includeMeleeDamage)
                modPlayer.meleeArmorPen += (int)(target.defense * percentArmorReduction * 0.01);

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (includeMeleeDamage && proj.melee)
                modPlayer.meleeArmorPen += (int)(target.defense * percentArmorReduction * 0.01);
            if (proj.ranged)
                modPlayer.rangedArmorPen += (int)(target.defense * percentArmorReduction * 0.01);

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
