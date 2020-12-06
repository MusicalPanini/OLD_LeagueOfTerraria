using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class VoidSharpened : Passive
    {
        int baseDamage;
        int minionScaling;

        public VoidSharpened(int BaseDamage, int MinionScaling)
        {
            baseDamage = BaseDamage;
            minionScaling = MinionScaling;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("Icathian Bite") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Melee attacks deal ") + baseDamage + " + " + TerraLeague.CreateScalingTooltip(DamageType.SUM, modPlayer.SUM, minionScaling) + TerraLeague.CreateColorString(PassiveSecondaryColor, " On Hit damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.meleeOnHit += baseDamage + (int)(modPlayer.SUM * minionScaling / 100d);

            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            //OnHitDamage += baseDamage + (int)(modPlayer.SUM * minionScaling / 100d);

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
