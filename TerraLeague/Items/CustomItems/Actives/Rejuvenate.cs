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
        int cooldown;

        public Rejuvenate(int BaseHeal, int EffectRadius, int Cooldown)
        {
            baseHeal = BaseHeal;
            effectRadius = EffectRadius;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return "[c/ff4d4d:Active: REJUVINATE -] [c/ff8080:Heal all players in a large area around you for " + (int)(baseHeal * (1+((Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().healPowerLastStep-1) * 2))) + " life]" +
                "\n[c/ff8080:Heal Power is 2 times as effective for this heal]" +
                "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                Efx(player);
                modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));

                if (player.whoAmI == Main.myPlayer)
                {
                    modPlayer.lifeToHeal += (int)(baseHeal * (1 + ((modPlayer.healPower - 1) * 2)));
                    modPlayer.HealLife();
                }

                // For Server
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);

                    for (int i = 0; i < Main.player.Length; i++)
                    {
                        Player healTarget = Main.player[i];

                        if (player.Distance(healTarget.Center) < effectRadius && healTarget.active && i != player.whoAmI)
                        {
                            int healz = (int)(baseHeal * (1 + ((modPlayer.healPower - 1) * 2)));

                            modPlayer.SendHealPacket(healz, i, -1, player.whoAmI);
                        }
                    }
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            AddStat(player, modItem, cooldown * 60, -1, true);

            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
            TerraLeague.DustRing(261, user, new Color(0, 255, 0, 0));
            Main.PlaySound(new LegacySoundStyle(2, 29), user.Center);

            PLAYERGLOBAL modPlayer = user.GetModPlayer<PLAYERGLOBAL>();

            for (int i = 0; i < effectRadius / 5; i++)
            {
                Vector2 pos = new Vector2(effectRadius, 0).RotatedBy(MathHelper.ToRadians(360 * (i / (effectRadius / 5f)))) + user.Center;

                Dust dustR = Dust.NewDustPerfect(pos, 267, Vector2.Zero, 0, new Color(0, 255, 0, 0), 2);
                dustR.noGravity = true;
            }
        }
    }
}

