using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class SolarisProtection : Active
    {
        int percentLifeShield;
        int effectRadius = 500;
        int shieldDuration;
        int cooldown;

        public SolarisProtection(int PercentLifeShield, int EffectRadius, int ShieldDuration, int Cooldown)
        {
            percentLifeShield = PercentLifeShield;
            effectRadius = EffectRadius;
            shieldDuration = ShieldDuration;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("SOLARI'S PROTECTION") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Give nearby allies a ") + TerraLeague.CreateScalingTooltip(UI.HealthbarUI.RedHealthColor.Hex3(), "LIFE", modPlayer.maxLifeLastStep, percentLifeShield, true) + TerraLeague.CreateColorString(ActiveSecondaryColor, " Shield") +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                int shieldAmount = modPlayer.ScaleValueWithHealPower(modPlayer.GetRealHeathWithoutShield(true) * percentLifeShield * 0.01f);

                Efx(player);
                player.AddBuff(BuffType<SolarisBlessing>(), shieldDuration * 60);
                modPlayer.AddShieldAttachedToBuff(shieldAmount, BuffType<SolarisBlessing>(), new Color(224, 113, 0), ShieldType.Basic);
                modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));

                // For Server
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);

                    var players = TerraLeague.GetAllPlayersInRange(player.MountedCenter, effectRadius, player.whoAmI, player.team);

                    for (int i = 0; i < players.Count; i++)
                    {
                        modPlayer.SendShieldPacket(shieldAmount, players[i], ShieldType.Basic, shieldDuration * 60, -1, player.whoAmI, new Color(224, 113, 0));
                        modPlayer.SendBuffPacket(BuffType<SolarisBlessing>(), shieldDuration * 60, players[i], -1, player.whoAmI);
                    }
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            AddStat(player, modItem, cooldown * 60, -1, true);
            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
            TerraLeague.DustRing(261, user, new Color(255, 106, 0, 0));
            Main.PlaySound(new LegacySoundStyle(2, 28).WithPitchVariance(-0.3f), user.Center);
            TerraLeague.DustBorderRing(effectRadius, user.MountedCenter, 267, new Color(255, 106, 0, 0), 2);
        }
    }
}

