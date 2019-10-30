using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
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

            return "[c/ff4d4d:Active: SOLARI'S PROTECTION -] [c/ff8080:Give all players in a large area around you a sheild]" +
                "\n[c/ff8080:Shield size is " + percentLifeShield + "% of your max life (" + (int)(modPlayer.maxLifeLastStep * percentLifeShield * 0.01 * modPlayer.healPower) + ")]" +
                "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                int shieldAmount = (int)(modPlayer.GetRealHeathWithoutShield(true) * percentLifeShield * 0.01 * modPlayer.healPower);

                Efx(player);
                player.AddBuff(BuffType<SolarisBlessing>(), shieldDuration * 60);
                modPlayer.AddShieldAttachedToBuff(shieldAmount, BuffType<SolarisBlessing>(), new Color(224, 113, 0), ShieldType.Basic);
                modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));

                // For Server
                if (Main.netMode == 1)
                {
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);

                    for (int i = 0; i < Main.player.Length; i++)
                    {
                        Player healTarget = Main.player[i];

                        if (player.Distance(healTarget.Center) < effectRadius && healTarget.active && i != player.whoAmI)
                        {
                            modPlayer.SendShieldPacket(shieldAmount, i, ShieldType.Basic, shieldDuration * 60, -1, player.whoAmI, new Color(224, 113, 0));
                            modPlayer.SendBuffPacket(BuffType<SolarisBlessing>(), shieldDuration * 60, i, -1, player.whoAmI);
                        }
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

            for (int i = 0; i < effectRadius / 5; i++)
            {
                Vector2 pos = new Vector2(effectRadius, 0).RotatedBy(MathHelper.ToRadians(360 * (i / (effectRadius / 5f)))) + user.Center;

                Dust dustR = Dust.NewDustPerfect(pos, 267, Vector2.Zero, 0, new Color(255, 106, 0, 0), 2);
                //dustR.velocity *= 0;
                dustR.noGravity = true;
            }
        }
    }
}

