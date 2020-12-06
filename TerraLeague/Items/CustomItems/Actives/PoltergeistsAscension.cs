using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ID;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class PoltergeistsAscension : Active
    {
        int effectDuration;

        public PoltergeistsAscension(int EffectDuration, int Cooldown)
        {
            effectDuration = EffectDuration;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            return TooltipName("Wraith step") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Gain a burst of movement speed for " + effectDuration + " seconds") +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendActiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type);

                player.AddBuff(BuffID.Swiftness, effectDuration * 60);
                SetCooldown(player);
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        public override void Efx(Player user)
        {
            TerraLeague.PlaySoundWithPitch(user.MountedCenter, 2, 117, 0.5f);
        }
    }
}

