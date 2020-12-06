﻿using TerraLeague.Buffs;
using TerraLeague.Items.AdvItems;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Executioner : Passive
    {
        int debuffDuration;

        public Executioner(int DebuffDuration)
        {
            debuffDuration = DebuffDuration;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("REND") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Melee and ranged attacks apply 'Greivous Wounds' for " + debuffDuration + " seconds");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
                    target.AddBuff(BuffType<GrievousWounds>(), debuffDuration * 60);

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
                if ((proj.melee || proj.ranged))
                    target.AddBuff(BuffType<GrievousWounds>(), debuffDuration * 60);

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
