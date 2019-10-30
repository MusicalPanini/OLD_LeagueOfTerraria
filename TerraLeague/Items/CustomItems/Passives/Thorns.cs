using TerraLeague.Buffs;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Thorns : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return "[c/0099cc:Passive: THORNS -] [c/99e6ff:Enemies who melee attack you will take 100% of your armor and defence (" + (modPlayer.armorLastStep + modPlayer.defenceLastStep) + ") as damage and gains 'Grievous Wounds']";
        }

        public override void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.ApplyDamageToNPC(npc, (modPlayer.armor + player.statDefense), 0, 0, false);
            npc.AddBuff(BuffType<GrievousWounds>(), 180);

            base.OnHitByNPC(npc, ref damage, ref crit, player, modItem);
        }
    }
}
