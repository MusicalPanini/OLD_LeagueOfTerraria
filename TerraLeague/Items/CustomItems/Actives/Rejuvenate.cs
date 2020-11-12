using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class Rejuvenate : Active
    {
        int baseHeal;
        int effectRadius = 500;

        public Rejuvenate(int BaseHeal, int EffectRadius, int Cooldown)
        {
            baseHeal = BaseHeal;
            effectRadius = EffectRadius;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            float modifiedhealPower = (float)(1 + ((Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().healPowerLastStep - 1) * 2));
            string statis = "";

            int value = (int)(baseHeal * modifiedhealPower);

            if (UI.ItemUI.extraStats)
            {
                statis += TerraLeague.CreateColorString(Color.White.Hex3(), value + "") + " + " + TerraLeague.CreateColorString(TerraLeague.HEALColor, "HEAL(" + (((int)(value * modifiedhealPower)) - (value)) + ")");
            }
            else
            {
                value = (int)(value * modifiedhealPower);
                statis += TerraLeague.CreateColorString(Color.White.Hex3(), value + "");
            }

            return TooltipName("REJUVINATE") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Heal nearby allies for ") + statis + TerraLeague.CreateColorString(ActiveSecondaryColor, " life\nHeal Power is 2 times as effective for this heal") +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                Efx(player);
                SetCooldown(player);

                if (player.whoAmI == Main.myPlayer)
                {
                    modPlayer.lifeToHeal += (int)(baseHeal * (1 + ((modPlayer.healPower - 1) * 2)));
                    modPlayer.HealLife();
                }

                // For Server
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);

                    var players = TerraLeague.GetAllPlayersInRange(player.MountedCenter, effectRadius, player.whoAmI, player.team);

                    for (int i = 0; i < players.Count; i++)
                    {
                        int healz = (int)(baseHeal * (1 + ((modPlayer.healPower - 1) * 2)));
                        modPlayer.SendHealPacket(healz, players[i], -1, player.whoAmI);
                    }
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
            TerraLeague.DustRing(261, user, new Color(0, 255, 0, 0));
            Main.PlaySound(new LegacySoundStyle(2, 29), user.Center);

            PLAYERGLOBAL modPlayer = user.GetModPlayer<PLAYERGLOBAL>();
            TerraLeague.DustBorderRing(effectRadius, user.MountedCenter, 267, new Color(0, 255, 0, 0), 2);
        }
    }
}

