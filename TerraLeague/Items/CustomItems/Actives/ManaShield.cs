using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class ManaShield : Active
    {
        int duration;
        int percentMana;
        int manaScaling;
        int baseShield;
        int cooldown;

        public ManaShield(int Duration, int PercentMana, int ManaScaling, int BaseShield, int Cooldown)
        {
            duration = Duration;
            percentMana = PercentMana;
            manaScaling = ManaScaling;
            baseShield = BaseShield;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return "[c/ff4d4d:Active: MANA SHIELD -] [c/ff8080:Consume " + percentMana + "% of your current mana to shield yourself for " + manaScaling + "% of that + " + baseShield + " for " + duration +  " seconds]" +
                 "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                int manaUsed = (int)(player.statMana * percentMana * 0.01);
                player.CheckMana(manaUsed, true);

                modPlayer.AddShield(manaUsed + baseShield,duration * 60, Color.SkyBlue, ShieldType.Basic);
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
            Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f), user.Center);
        }
    }
}

