using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class SoulTaint : Passive
    {
        int percentMaxLifeDamage;
        int minDamage;
        int maxDamage;

        public SoulTaint(int PercentMaxLifeDamage, int MinDamage, int MaxDamage)
        {
            percentMaxLifeDamage = PercentMaxLifeDamage;
            minDamage = MinDamage;
            maxDamage = MaxDamage;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("Mist's edge") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Melee and ranged attack will deal " + percentMaxLifeDamage + "% of their max life as On Hit damage")
                + "\n" + TerraLeague.CreateColorString(PassiveSubColor, "(Max damage: " + maxDamage + ")");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            int SoulTaintDamage = (int)(target.lifeMax * percentMaxLifeDamage * 0.01);

            if (SoulTaintDamage > maxDamage)
                SoulTaintDamage = maxDamage;

            OnHitDamage += SoulTaintDamage;

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            if ((proj.melee || proj.ranged) /*&& !TerraLeague.DoNotCountRangedDamage(proj)*/)
            {
                int SoulTaintDamage = (int)(target.lifeMax * percentMaxLifeDamage * 0.01);

                if (SoulTaintDamage > maxDamage)
                    SoulTaintDamage = maxDamage;
                else if (SoulTaintDamage < minDamage)
                    SoulTaintDamage = minDamage;

                OnHitDamage += SoulTaintDamage;
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
