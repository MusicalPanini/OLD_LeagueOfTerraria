using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems
{
    abstract public class Passive
    {
        static internal PassivePacketHandler PacketHandler = new PassivePacketHandler(4);

        internal static string PassiveMainColor = "0099cc";
        internal static string PassiveSecondaryColor = "99e6ff";
        internal static string PassiveSubColor = "007399";

        public bool currentlyActive = true;
        public bool deactivateIfNotUnique = true;

        public float passiveStat = 0;
        public int cooldownCount = 0;
        public int passiveCooldown = 0;

        public abstract string Tooltip(Player player, ModItem modItem);

        public string TooltipName(string name)
        {
            return TerraLeague.CreateColorString(PassiveMainColor, "Passive: " + name.ToUpper() + " - ");
        }

        public virtual void UpdateAccessory(Player player, ModItem modItem)
        {
            
        }

        virtual public void PostPlayerUpdate(Player player, ModItem modItem)
        {
            if (passiveCooldown != 0)
            {
                if (cooldownCount > 0)
                    cooldownCount--;
            }
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

        public void SendEfx(Player player, ModItem modItem)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                PacketHandler.SendPassiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type, LeagueItem.GetPassivePositionInArray(modItem as LeagueItem, this));
        }

        public void SendEfx(Player player, NPC npc, ModItem modItem)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                PacketHandler.SendPassiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type, LeagueItem.GetPassivePositionInArray(modItem as LeagueItem, this), npc.whoAmI);
        }

        public void AddStat(Player player, float maxStat, float statToAdd, bool cannotGoNegative = false)
        {
            if (Main.LocalPlayer.whoAmI == player.whoAmI)
            {
                passiveStat += statToAdd;
                if (passiveStat > maxStat)
                {
                    passiveStat = maxStat;
                }

                if (passiveStat < 0 && cannotGoNegative)
                {
                    passiveStat = 0;
                }
            }
        }

        internal int GetScaledCooldown(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return (int)(passiveCooldown * modPlayer.ItemCdrLastStep);
        }

        internal void SetCooldown(Player player)
        {
            cooldownCount = GetScaledCooldown(player) * 60;
        }
    }
}
