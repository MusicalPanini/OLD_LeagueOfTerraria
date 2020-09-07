using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Afterburn : Passive
    {
        int minionScaling;
        int meleeScaling;
        int rangedScaling;
        int magicScaling;

        public Afterburn(int MinionScaling, int MeleeScaling, int RangedScaling, int MagicScaling)
        {
            minionScaling = MinionScaling;
            meleeScaling = MeleeScaling;
            rangedScaling = RangedScaling;
            magicScaling = MagicScaling;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("Afterburn") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Melee and ranged damage deal ") +
                TerraLeague.CreateScalingTooltip(DamageType.MEL, modPlayer.MEL, meleeScaling) + " + " +
                TerraLeague.CreateScalingTooltip(DamageType.RNG, modPlayer.RNG, rangedScaling) + " + " +
                TerraLeague.CreateScalingTooltip(DamageType.MAG, modPlayer.MAG, magicScaling) + " + " +
                TerraLeague.CreateScalingTooltip(DamageType.SUM, modPlayer.SUM, minionScaling) + " + " + TerraLeague.CreateColorString(PassiveSecondaryColor, "On Hit damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            OnHitDamage += (int)(meleeScaling * modPlayer.MEL / 100d) + (int)(rangedScaling * modPlayer.RNG / 100d) + (int)(magicScaling * modPlayer.MAG / 100d) + (int)(minionScaling * modPlayer.SUM / 100d);

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (proj.melee || proj.ranged)
            {
                OnHitDamage += (int)(meleeScaling * modPlayer.MEL / 100d) + (int)(rangedScaling * modPlayer.RNG / 100d) + (int)(magicScaling * modPlayer.MAG / 100d) + (int)(minionScaling * modPlayer.SUM / 100d);
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
