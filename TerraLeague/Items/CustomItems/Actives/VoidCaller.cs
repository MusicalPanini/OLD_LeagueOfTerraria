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
        int cooldown;
        int sumScaling;

        public VoidCaller(int BaseDamage, int BaseMinions, int SumScaling, int Cooldown)
        {
            baseDamage = BaseDamage;
            baseMinions = BaseMinions;
            sumScaling = SumScaling;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return "[c/ff4d4d:Active: VOID CALLER -] [c/ff8080:Summon a Zz'Rot portal at your cursor.]" +
                "\n[c/ff8080:It ejects 3 + Max Minions (" + player.maxMinions + ") Zz'Rots every second for 5 seconds. The Zz'rots deal] " + baseDamage + " + [c/" + TerraLeague.SUMColor + ":" + (int)(modPlayer.SUM * sumScaling / 100d) + "] [c/ff8080:damage]" +
                "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                player.FindSentryRestingSpot(ProjectileType<Item_ZzrotPortal>(), out int xPos, out int yPos, out int yDis);
                Projectile.NewProjectile((float)xPos, (float)(yPos - yDis), 0f, 0f, ProjectileType<Item_ZzrotPortal>(), baseDamage + (int)(modPlayer.SUM * sumScaling * 0.01f), 2, player.whoAmI, baseMinions);
                modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));

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
            AddStat(player, modItem, cooldown * 60, -1, true);
            base.PostPlayerUpdate(player, modItem);
        }
    }
}

