using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class Purity : Active
    {
        int effectDuration;

        public Purity(int EffectDuration, int Cooldown)
        {
            effectDuration = EffectDuration;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            return TooltipName("PURITY") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Target yourself or an ally and 'Cleanse' them for " + effectDuration + " seconds") +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                int target = TerraLeague.PlayerMouseIsHovering(30);
                if (target != -1)
                {
                    DoAction(target, player, modItem);
                }
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        public void DoAction(int target, Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (target == player.whoAmI)
                player.AddBuff(BuffType<GeneralCleanse>(), effectDuration * 60);
            else
                modPlayer.SendBuffPacket(BuffType<GeneralCleanse>(), effectDuration * 60, target, -1, player.whoAmI);

            Efx(Main.player[target]);
            if (Main.netMode == NetmodeID.MultiplayerClient)
                PacketHandler.SendActiveEfx(-1, player.whoAmI, target, modItem.item.type);

            SetCooldown(player);
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

