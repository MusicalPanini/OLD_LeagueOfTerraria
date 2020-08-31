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
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC DamTarget = Main.npc[i];

                    float damtoX = DamTarget.position.X + (float)DamTarget.width * 0.5f - player.Center.X;
                    float damtoY = DamTarget.position.Y + (float)DamTarget.height * 0.5f - player.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(damtoX * damtoX + damtoY * damtoY));

                    if (distance < effectRadius && !DamTarget.townNPC && !DamTarget.immortal)
                    {
                        if (Main.netMode == NetmodeID.MultiplayerClient)
                            DamTarget.GetGlobalNPC<NPCsGLOBAL>().PacketHandler.SendAddBuff(-1, player.whoAmI, BuffType<Buffs.AbyssalCurse>(), 240, i);

                        DamTarget.AddBuff(BuffType<Buffs.AbyssalCurse>(), 240);
                    }
                }

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
