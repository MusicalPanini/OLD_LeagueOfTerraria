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

        public SolarisProtection(int PercentLifeShield, int EffectRadius, int ShieldDuration, int Cooldown)
        {
            percentLifeShield = PercentLifeShield;
            effectRadius = EffectRadius;
            shieldDuration = ShieldDuration;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("DEVOTION") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Give nearby allies a ") + TerraLeague.CreateScalingTooltip(UI.HealthbarUI.RedHealthColor.Hex3(), "LIFE", modPlayer.maxLifeLastStep, percentLifeShield, true) + TerraLeague.CreateColorString(ActiveSecondaryColor, " Shield") +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                int shieldAmount = modPlayer.ScaleValueWithHealPower(modPlayer.GetRealHeathWithoutShield(true) * percentLifeShield * 0.01f);

                Efx(player);
                player.AddBuff(BuffType<SolarisBlessing>(), shieldDuration * 60);
                modPlayer.AddShieldAttachedToBuff(shieldAmount, BuffType<SolarisBlessing>(), new Color(224, 113, 0), ShieldType.Basic);
                SetCooldown(player);

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
            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
            //TerraLeague.DustRing(261, user, new Color(255, 106, 0, 0));
            Main.PlaySound(new LegacySoundStyle(2, 28).WithPitchVariance(-0.3f), user.Center);
            TerraLeague.DustBorderRing(effectRadius, user.MountedCenter, 263, new Color(255, 106, 0, 0), 2);
            List<int> players = TerraLeague.GetAllPlayersInRange(user.MountedCenter, effectRadius, -1, user.team);

            for (int i = 0; i < players.Count; i++)
            {
                Player player = Main.player[players[i]];
                TerraLeague.DustElipce(32, 32, 0, player.MountedCenter, 174, default, 2, 36, true, 0f);

                if (user.whoAmI != players[i])
                {
                    Vector2 pointA = user.MountedCenter + TerraLeague.CalcVelocityToPoint(user.MountedCenter, player.MountedCenter, 32);
                    Vector2 pointB = player.MountedCenter + TerraLeague.CalcVelocityToPoint(player.MountedCenter, user.MountedCenter, 32);

                    TerraLeague.DustLine(pointA, pointB, 174, 0.5f, 2);
                }
            }
        }
    }
}

