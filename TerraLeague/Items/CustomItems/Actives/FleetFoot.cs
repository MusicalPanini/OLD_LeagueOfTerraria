using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
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

            return TooltipName("FLEET FOOT") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Give all nearby allies a speed boost for " + effectDuration + " seconds") +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown");
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
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);

                    var players = TerraLeague.GetAllPlayersInRange(player.MountedCenter, effectRadius, player.whoAmI, player.team);

                    for (int i = 0; i < players.Count; i++)
                    {
                        Player healTarget = Main.player[players[i]];
                        modPlayer.SendBuffPacket(BuffType<Buffs.FleetFoot>(), effectDuration * 60, healTarget.whoAmI, -1, player.whoAmI);
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
            TerraLeague.DustBorderRing(effectRadius, user.MountedCenter, 267, new Color(255, 255, 0, 0), 2);
        }
    }
}

