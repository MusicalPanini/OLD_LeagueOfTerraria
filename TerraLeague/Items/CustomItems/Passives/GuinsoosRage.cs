using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class GuinsoosRage : Passive
    {
        int meleeSpeedAmmoConsume;

        public GuinsoosRage(int meleeAttacksSpeedAmmoConsumeChance)
        {
            meleeSpeedAmmoConsume = meleeAttacksSpeedAmmoConsumeChance;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: GUINSOO'S RAGE -] [c/99e6ff:Every second in combat will grant a stack up to 6]" +
                "\n[c/99e6ff:Gain " + meleeSpeedAmmoConsume + "% melee attack speed and chance to not consume ammo]" +
                "\n[c/99e6ff:At max stacks, melee and ranged On Hit damage will be doubled]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.meleeSpeed += (float)(meleeSpeedAmmoConsume * 0.01 * modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)]);
            modPlayer.ConsumeAmmoChance += meleeSpeedAmmoConsume * 0.01 * modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)];

            if (modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] >= 6)
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
                modPlayer.FindAndSetPassiveStat(this, 0);
            }
            if (modPlayer.CombatTimer < 120 && modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] < 6)
            {
                modPlayer.rageTimer++;
                if (modPlayer.rageTimer >= 60)
                {
                    modPlayer.rageTimer = 0;
                    AddStat(player, modItem, 10, 1);
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
