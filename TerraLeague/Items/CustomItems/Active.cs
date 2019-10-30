using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems
{
    public abstract class Active
    {
        public abstract string Tooltip(Player player, LeagueItem modItem);
        internal ActivePacketHandler PacketHandler = new ActivePacketHandler(5);
        public abstract void DoActive(Player player, LeagueItem modItem);

        virtual public void PostPlayerUpdate(Player player, LeagueItem modItem)
        {

        }

        virtual public void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int HitDamage, Player player, ModItem modItem)
        {

        }

        virtual public void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {

        }

        public void AddStat(Player player, ModItem modItem, double maxStat, double statToAdd, bool cannotGoNegative = false)
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(player, modItem);

            if (slot != -1)
            {
                player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot] += statToAdd;
                if (player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot] > maxStat)
                {
                    player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot] = maxStat;
                }
                if (player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot] < 0 && cannotGoNegative)
                {
                    player.GetModPlayer<PLAYERGLOBAL>().accessoryStat[slot] = 0;
                }
            }
        }

        virtual public void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {

        }

        virtual public void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player, ModItem modItem)
        {

        }

        virtual public void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {

        }

        virtual public void Efx(Player user)
        {

        }
    }
}
