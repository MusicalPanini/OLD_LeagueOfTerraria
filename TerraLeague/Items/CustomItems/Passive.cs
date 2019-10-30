using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems
{
    abstract public class Passive
    {
        public abstract string Tooltip(Player player, ModItem modItem);
        static internal PassivePacketHandler PacketHandler = new PassivePacketHandler(4);
        

        public virtual void UpdateAccessory(Player player, ModItem modItem)
        {
            
        }

        virtual public void PostPlayerUpdate(Player player, ModItem modItem)
        {

        }

        virtual public void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {

        }

        virtual public void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {

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

        virtual public void OnKilledNPC(NPC npc, int damage, bool crit, Player player, ModItem modItem)
        {

        }

        virtual public int PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, Player player, ModItem modItem)
        {
            return -1;
        }

        virtual public void Efx(Player user)
        {

        }

        virtual public void Efx(Player user, NPC effectedNPC)
        {

        }

        public bool FindIfPassiveIsSecondary(ModItem modItem)
        {
            LeagueItem legItem = modItem as LeagueItem;

            if (legItem != null)
            {
                if (legItem.GetSecondaryPassive() != null)
                {
                    if (legItem.GetSecondaryPassive().GetType().Name == this.GetType().Name)
                        return true;
                }
            }

            return false;
        }

        public void AddStat(Player player, ModItem modItem, double maxStat, double statToAdd, bool cannotGoNegative = false)
        {
            if (Main.LocalPlayer.whoAmI == player.whoAmI)
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
        }
    }
}
