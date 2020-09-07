using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class Metallicize : Active
    {
        int cooldown;

        public Metallicize(int Cooldown)
        {
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("METALLICIZE") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Increase your health by 125% while lowering your attack for 10 seconds" +
                "\nIf STONE SKIN is active, also full heal") +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                player.statLifeMax2 += player.statLifeMax2 / 2;

                if (modPlayer.stonePlating)
                {
                    player.HealEffect(player.statLifeMax2);
                    player.statLife += (int)(player.statLifeMax2);
                }
                else
                {
                    player.HealEffect((int)(player.statLife * 0.5));
                    player.statLife += (int)(player.statLife * 0.5);
                }

                player.AddBuff(BuffType<GargoylesMight>(), 600);
                modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            AddStat(player, modItem, cooldown * 60, -1, true);
            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
            TerraLeague.DustRing(261, user, new Color(255, 255, 255, 0));
            Main.PlaySound(new LegacySoundStyle(3, 54).WithPitchVariance(-0.2f));
        }
    }
}

