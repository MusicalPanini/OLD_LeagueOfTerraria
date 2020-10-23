using Microsoft.Xna.Framework;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class VoidCaller : Active
    {
        int baseDamage;
        int baseMinions;
        int sumScaling;

        public VoidCaller(int BaseDamage, int BaseMinions, int SumScaling, int Cooldown)
        {
            baseDamage = BaseDamage;
            baseMinions = BaseMinions;
            sumScaling = SumScaling;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("VOID CALLER") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Summon a Zz'Rot portal at your cursor" +
                "\nIt ejects ") + TerraLeague.CreateScalingTooltip(DamageType.NONE, 3, 100) + " + " + TerraLeague.CreateScalingTooltip(TerraLeague.MINIONMAXColor, "MINIONS", (int)modPlayer.maxMinionsLastStep, 100) +
                TerraLeague.CreateColorString(ActiveSecondaryColor, " Zz'Rots every second for 5 seconds. The Zz'Rots deal ") + TerraLeague.CreateScalingTooltip(DamageType.NONE, baseDamage, 100) + " + " + TerraLeague.CreateScalingTooltip(DamageType.SUM, modPlayer.SUM, sumScaling) +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (cooldownCount <= 0)
            {
                player.FindSentryRestingSpot(ProjectileType<Item_ZzrotPortal>(), out int xPos, out int yPos, out int yDis);
                Projectile.NewProjectile((float)xPos, (float)(yPos - yDis), 0f, 0f, ProjectileType<Item_ZzrotPortal>(), baseDamage + (int)(modPlayer.SUM * sumScaling * 0.01f), 2, player.whoAmI, baseMinions);
                SetCooldown(player);

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);
            }
        }

        public override void Efx(Player user)
        {
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 113), user.MountedCenter);
            base.Efx(user);
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }
    }
}

