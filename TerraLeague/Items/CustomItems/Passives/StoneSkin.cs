using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class StoneSkin : Passive
    {
        int enemies;
        int armorResist;

        public StoneSkin(int EnemyAmount, int ArmorResistBonus)
        {
            enemies = EnemyAmount;
            armorResist = ArmorResistBonus;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: STONE SKIN -] [c/99e6ff:While near at least " + enemies + " enemies, gain " + armorResist + " armor and resist]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            int npcCount = 0;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                NPC target = Main.npc[i];

                if (player.Distance(target.Center) < 700 && target.active && !target.townNPC && target.lifeMax > 5 && !target.immortal)
                    npcCount++;
                if (npcCount >= enemies)
                {
                    player.AddBuff(BuffType<StonePlating>(), 5);
                    modPlayer.armor += armorResist;
                    modPlayer.resist += armorResist;
                    break;
                }
            }

            base.PostPlayerUpdate(player, modItem);
        }
    }
}
