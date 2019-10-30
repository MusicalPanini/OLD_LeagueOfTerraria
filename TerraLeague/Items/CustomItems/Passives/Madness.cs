using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Madness : Passive
    {
        int minionDamagePerStack;

        public Madness(int MinionDamagePerStack)
        {
            minionDamagePerStack = MinionDamagePerStack;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: MADNESS -] [c/99e6ff:Every second in combat will grant a stack]" +
                "\n[c/99e6ff:Gain " + minionDamagePerStack + "% minion damage per stack up to " + minionDamagePerStack * 10 + "%]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.CombatTimer >= 240)
            {
                modPlayer.madnessTimer = 0;
                modPlayer.FindAndSetPassiveStat(this, 0);
            }
            if (modPlayer.CombatTimer < 240 && modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] < 10)
            {
                modPlayer.madnessTimer++;
                if (modPlayer.madnessTimer >= 60)
                {
                    modPlayer.madnessTimer = 0;
                    AddStat(player, modItem, 10, 1);
                }
            }

            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += minionDamagePerStack * 0.01 * modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)];

            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
