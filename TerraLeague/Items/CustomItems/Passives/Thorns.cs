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

            return TooltipName("THORNS") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Enemies who melee attack you will take ") +  TerraLeague.CreateScalingTooltip(TerraLeague.ARMORColor, "ARM", modPlayer.armorLastStep, 100) + TerraLeague.CreateColorString(PassiveSecondaryColor, " damage and gain 'Grievous Wounds'");
        }

        public override void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.ApplyDamageToNPC(npc, (modPlayer.armor), 0, 0, false);
            npc.AddBuff(BuffType<GrievousWounds>(), 180);

            base.OnHitByNPC(npc, ref damage, ref crit, player, modItem);
        }
    }
}
