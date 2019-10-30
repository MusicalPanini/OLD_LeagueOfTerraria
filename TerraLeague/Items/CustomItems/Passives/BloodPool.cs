using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class BloodPool : Passive
    {
        int BloodPoolMaxStack;

        public BloodPool(int bloodPoolMaxStacks)
        {
            BloodPoolMaxStack = bloodPoolMaxStacks;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: BLOOD POOL -] [c/99e6ff:Store 10% of magic and minion damage as charge up to " + BloodPoolMaxStack + "]" +
                "\n[c/99e6ff:Healing an ally will consume the charge and heal that much extra]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            player.GetModPlayer<PLAYERGLOBAL>().bloodPool = true;
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            if (proj.magic || TerraLeague.IsMinionDamage(proj))
            {
                AddStat(player, modItem, BloodPoolMaxStack, damage * 0.1);
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
