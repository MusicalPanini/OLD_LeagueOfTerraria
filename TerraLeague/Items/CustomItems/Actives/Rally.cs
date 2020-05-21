using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class Rally : Active
    {
        int duration;
        int minionScaling;
        int cooldown;

        public Rally(int Duration, int MinionScaling, int Cooldown)
        {
            duration = Duration;
            minionScaling = MinionScaling;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return "[c/ff4d4d:Active: RALLY -] [c/ff8080:Your minions deal] [c/" + TerraLeague.SUMColor + ":" + (int)(modPlayer.SUM * minionScaling / 100d) + "%] [c/ff8080:increased damage for " + duration + " seconds]" +
                 "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                player.AddBuff(BuffType<Buffs.Rally>(), duration * 60);
                modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);
            }
        }


        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            AddStat(player, modItem, cooldown * 60, -1, true);
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.rally)
                modPlayer.minionModifer += modPlayer.SUM * minionScaling / 100d;

            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
            //Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.3f), user.Center);
            //for (int j = 0; j < 18; j++)
            //{
            //    int num2 = Dust.NewDust(new Vector2(Main.rand.Next((int)player.position.X - 8, (int)player.position.X + 8), player.position.Y + 16), player.width, player.height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(255, 255, 255, 0), Main.rand.Next(Main.rand.Next(2, 3)));
            //    Main.dust[num2].noGravity = true;
            //}
        }
    }
}

