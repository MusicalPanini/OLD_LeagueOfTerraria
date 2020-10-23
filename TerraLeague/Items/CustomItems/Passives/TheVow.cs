using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class TheVow : Passive
    {
        int effectRadius;

        public TheVow(int EffectRadius)
        {
            effectRadius = EffectRadius;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("THE VOW") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Periodically Grant nearby allies 'Iron Skin'");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

       

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            DoThing(player, modItem);
            base.PostPlayerUpdate(player, modItem);
        }

        public void DoThing(Player player, ModItem modItem)
        {
            if (Main.time % 240 == 180)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                Efx(player);
                SendEfx(player, modItem);

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    var players = TerraLeague.GetAllPlayersInRange(player.MountedCenter, effectRadius, player.whoAmI, player.team);

                    for (int i = 0; i < players.Count; i++)
                    {
                        modPlayer.SendBuffPacket(BuffID.Ironskin, 240, players[i], -1, player.whoAmI);
                    }
                }
            }
        }

        public override void Efx(Player user)
        {
            TerraLeague.DustRing(261, user, new Color(150, 150, 150, 0));
            TerraLeague.DustBorderRing(effectRadius, user.MountedCenter, 263, new Color(150, 150, 150, 0), 2);

            base.Efx(user);
        }
    }
}
