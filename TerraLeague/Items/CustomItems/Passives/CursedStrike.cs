using System;
using System.Collections.Generic;
using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class CursedStrike : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("Affliction") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Magic attacks apply 'Greivous Wounds'");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            if (proj.magic)
                target.AddBuff(BuffType<GrievousWounds>(), 180);

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
