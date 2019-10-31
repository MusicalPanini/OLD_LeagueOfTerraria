using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class FleetFoot : Active
    {
        int effectRadius = 500;
        int effectDuration;
        int cooldown;

        public FleetFoot(int EffectRadius, int EffectDuration, int Cooldown)
        {
            effectRadius = EffectRadius;
            effectDuration = EffectDuration;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return "[c/ff4d4d:Active: FLEET FOOT -] [c/ff8080:Give all players in a large area around you a speed boost]" +
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
                    player.AddBuff(BuffType<Buffs.FleetFoot>(), effectDuration * 60);
                }

                // For Server
                if (Main.netMode == 1)
                {
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);

                    for (int i = 0; i < Main.player.Length; i++)
                    {
                        Player healTarget = Main.player[i];
                        int distance = effectRadius;

                        if (player.Distance(healTarget.Center) < distance && healTarget.active && i != player.whoAmI)
                        {
                            modPlayer.SendBuffPacket(BuffType<Buffs.FleetFoot>(), effectDuration * 60, i, -1, player.whoAmI);
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
            TerraLeague.DustRing(261, user, new Color(255, 255, 0, 0));
            Main.PlaySound(new LegacySoundStyle(2, 29), user.Center);

            for (int i = 0; i < effectRadius / 5; i++)
            {
                Vector2 pos = new Vector2(effectRadius, 0).RotatedBy(MathHelper.ToRadians(360 * (i / (effectRadius / 5f)))) + user.Center;

                Dust dustR = Dust.NewDustPerfect(pos, 267, Vector2.Zero, 0, new Color(255, 255, 0, 0), 2);
                dustR.noGravity = true;
            }
        }
    }
}

