using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class AbyssalCurse : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: ABYSSAL CURSE -] [c/99e6ff:Near by enemies take 8% more magic damage]";
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

            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC DamTarget = Main.npc[i];

                float damtoX = DamTarget.position.X + (float)DamTarget.width * 0.5f - player.Center.X;
                float damtoY = DamTarget.position.Y + (float)DamTarget.height * 0.5f - player.Center.Y;
                float distance = (float)System.Math.Sqrt((double)(damtoX * damtoX + damtoY * damtoY));

                if (distance < 300 && !DamTarget.townNPC)
                {
                    Main.npc[i].AddBuff(BuffType<Buffs.AbyssalCurse>(), 5);
                }
            }
        }
    }
}
