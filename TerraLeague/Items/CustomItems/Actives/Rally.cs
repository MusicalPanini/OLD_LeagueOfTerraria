using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class Rally : Active
    {
        int duration;
        int minionScaling;

        public Rally(int Duration, int MinionScaling, int Cooldown)
        {
            duration = Duration;
            minionScaling = MinionScaling;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("RALLY") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Your minions deal ") + TerraLeague.CreateScalingTooltip(DamageType.SUM, modPlayer.SUM, minionScaling, false, "%") + TerraLeague.CreateColorString(ActiveSecondaryColor, " increased damage for " + duration + " seconds") +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                player.AddBuff(BuffType<Buffs.Rally>(), duration * 60);
                SetCooldown(player);

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);
            }
        }


        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.rally)
                modPlayer.minionModifer += modPlayer.SUM * minionScaling / 10000d;

            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
            TerraLeague.PlaySoundWithPitch(user.Center, 2, 117, -1f);
            for (int j = 0; j < 18; j++)
            {
                int num2 = Dust.NewDust(user.position, user.width, user.height, 218, 0, -Main.rand.NextFloat(3, 5), 0, new Color(255, 0, 0),Main.rand.NextFloat(2, 3));
                Main.dust[num2].noGravity = true;
            }
        }
    }
}

