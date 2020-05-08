using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class Quicksilver : Active
    {
        int effectDuration;
        int cooldown;

        public Quicksilver(int EffectDuration, int Cooldown)
        {
            effectDuration = EffectDuration;
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return "[c/ff4d4d:Active: QUICKSILVER -] [c/ff8080:Become immune to most debuffs for " + effectDuration + " seconds]" +
                "\n[c/cc0000:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (modItem.GetStatOnPlayer(player) <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                player.AddBuff(BuffType<GeneralCleanse>(), effectDuration * 60);
                modPlayer.FindAndSetActiveStat(this, (int)(cooldown * modPlayer.Cdr * 60));

                Efx(player);
                if (Main.netMode == 1)
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
            Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.3f), user.Center);
            for (int j = 0; j < 18; j++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Main.rand.Next((int)user.position.X - 8, (int)user.position.X + 8), user.position.Y + 16), user.width, user.height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(255, 255, 255, 0), Main.rand.Next(Main.rand.Next(2, 3)));
                dust.noGravity = true;
            }
        }
    }
}

