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
            return "[c/0099cc:Passive: THE VOW -] [c/99e6ff:Grant near by allies 'Iron Skin']";
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
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                for (int i = 0; i < Main.player.Length; i++)
                {
                    Player DefTarget = Main.player[i];

                    float damtoX = DefTarget.position.X + (float)DefTarget.width * 0.5f - player.Center.X;
                    float damtoY = DefTarget.position.Y + (float)DefTarget.height * 0.5f - player.Center.Y;
                    float distance = (float)System.Math.Sqrt((double)(damtoX * damtoX + damtoY * damtoY));

                    if (distance < effectRadius && i != player.whoAmI && DefTarget.active)
                    {
                        modPlayer.SendBuffPacket(BuffID.Ironskin, 180, i, -1, player.whoAmI);
                    }
                }
            }
        }
    }
}
