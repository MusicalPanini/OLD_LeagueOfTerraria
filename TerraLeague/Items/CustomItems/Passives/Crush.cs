using TerraLeague.Buffs;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Crush : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("CARVE") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Melee attacks will reduce enemy defence by 6% stacking 6 times");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            int stacks = target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().CleavedStacks;

            if (stacks < 5)
            {
                target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().CleavedStacks++;

                if (Main.netMode == NetmodeID.MultiplayerClient)
                    target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 2, target.whoAmI, target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().CleavedStacks);
            }

            target.AddBuff(BuffType<Cleaved>(), 360);

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public void DoThing(Player player, ModItem modItem)
        {

        }
    }
}
