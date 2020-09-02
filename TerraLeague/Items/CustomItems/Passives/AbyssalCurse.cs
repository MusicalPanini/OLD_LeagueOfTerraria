using Microsoft.Xna.Framework;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class AbyssalCurse : Passive
    {
        int effectRadius = 400;

        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: ABYSSAL CURSE -] [c/99e6ff:Debuff nearby enemies to make them take 8% more magic damage]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

       

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            DoThing(player, modItem);

            base.PostPlayerUpdate(player, modItem);
        }

        public void DoThing(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (Main.time % 240 == 120)
            {
                TerraLeague.GiveNPCsInRangeABuff(player.MountedCenter, effectRadius, BuffType<Buffs.AbyssalCurse>(), 240, true);

                Efx(player);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    PacketHandler.SendPassiveEfx(-1, player.whoAmI, player.whoAmI, modItem.item.type, FindIfPassiveIsSecondary(modItem));
            }
        }

        public override void Efx(Player user)
        {
            TerraLeague.DustRing(14, user, new Color(255, 0, 255));
            TerraLeague.DustBorderRing(effectRadius, user.MountedCenter, 14, new Color(255, 0, 255), 3);
            base.Efx(user);
        }
    }
}
