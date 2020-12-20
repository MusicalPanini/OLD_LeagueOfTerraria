using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Pox : Passive
    {
        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("POX") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Magic attacks will apply stacks of 'Pox' stacking 5 times");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
           

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            if (proj.magic)
            {
                int stacks = target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().PoxStacks;

                if (stacks < 4)
                {
                    target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().PoxStacks++;

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().PacketHandler.SendSyncStats(Main.LocalPlayer.whoAmI, -1, 5, target.whoAmI, target.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().PoxStacks);
                }

                target.AddBuff(BuffType<Buffs.Pox>(), 600);
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }

        public void DoThing(Player player, ModItem modItem)
        {

        }
    }
}
