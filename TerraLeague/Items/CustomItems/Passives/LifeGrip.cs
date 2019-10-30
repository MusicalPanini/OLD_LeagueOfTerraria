using TerraLeague.Items.CompleteItems;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class LifeGrip : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/99e6ff:Triggering LIFELINE grants LIFEGRIP]" +
                "\n[c/0099cc:Passive: LIFEGRIP -] [c/99e6ff:Grants 5% life steal, melee and ranged damage, and 5 resist]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {

            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<Maw>()) != -1)
            {
                if (modItem.item.type == ItemType<Maw>())
                {
                    player.AddBuff(BuffType<Buffs.LifeGrip>(), 720);
                }
            }

            base.PostPlayerUpdate(player, modItem);
        }
    }
}
