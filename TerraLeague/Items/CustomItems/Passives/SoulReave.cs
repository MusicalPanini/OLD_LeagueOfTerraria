using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Items.CompleteItems;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class SoulReave : Passive
    {
        int manaRestore;

        public SoulReave(int ManaReave)
        {
            manaRestore = ManaReave;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("SOUL REAVE") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Ranged critical hits will restore " + manaRestore + "% of your missing mana");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
           

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            if (proj.ranged && crit && player.statMana != player.statManaMax2)
            {
                int mana = (int)((player.statManaMax2 - player.statMana) * (manaRestore * 0.01));

                mana = mana == 0 ? 1 : mana;

                player.ManaEffect(mana);
                player.statMana += mana;
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }

        public void DoThing(Player player, ModItem modItem)
        {

        }
    }
}
