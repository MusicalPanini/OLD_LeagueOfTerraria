using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class GuinsoosRage : Passive
    {
        int meleeRangedAttackSpeed;

        public GuinsoosRage(int meleeAttacksSpeedAmmoConsumeChance)
        {
            meleeRangedAttackSpeed = meleeAttacksSpeedAmmoConsumeChance;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("Seething strikes") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Every second in combat will grant a stack up to 6" +
                "\nGain " + meleeRangedAttackSpeed + "% melee and ranged attack speed per stack" +
                "\nAt max stacks, melee and ranged On Hit damage will deal 1.5x damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.meleeSpeed += (float)(meleeRangedAttackSpeed * 0.01 * passiveStat);
            modPlayer.rangedAttackSpeed += meleeRangedAttackSpeed * 0.01 * passiveStat;

            if (passiveStat >= 6)
            {
                modPlayer.guinsoosRage = true;
            }

            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.CombatTimer >= 120)
            {
                modPlayer.rageTimer = 0;
                passiveStat = 0;
            }
            if (modPlayer.CombatTimer < 120 && passiveStat < 6)
            {
                modPlayer.rageTimer++;
                if (modPlayer.rageTimer >= 60)
                {
                    modPlayer.rageTimer = 0;
                    AddStat(player, 10, 1);
                }
            }

            base.PostPlayerUpdate(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            
            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
