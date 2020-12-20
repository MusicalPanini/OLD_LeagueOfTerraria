using Microsoft.Xna.Framework;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Terraria;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class DiseaseHarvest : Active
    {
        int stackDamage;
        int manaRestore;
        int magicScaling;

        public DiseaseHarvest(int StackDamage, int ManaRestore, int MagicScaling, int Cooldown)
        {
            stackDamage = StackDamage;
            manaRestore = ManaRestore;
            magicScaling = MagicScaling;
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("DISEASE HARVEST") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Deal ") + stackDamage + " + " + TerraLeague.CreateScalingTooltip(DamageType.MAG, modPlayer.MAG, magicScaling) + TerraLeague.CreateColorString(ActiveSecondaryColor, " magic damage per stack to near by enemies infected with 'Pox'" +
                "\nRestore " + manaRestore + " mana for each stack harvested")
                + "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                DoAction(player, modItem);
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        public void DoAction(Player player, LeagueItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            var npcs = TerraLeague.GetAllNPCsInRange(player.MountedCenter, 700, true);

            for (int i = 0; i < npcs.Count; i++)
            {
                NPC npc = Main.npc[npcs[i]];
                if (npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().pox)
                {
                    SetCooldown(player);
                    Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ProjectileType<Item_DiseaseHarvest>(), stackDamage + (int)(magicScaling * player.GetModPlayer<PLAYERGLOBAL>().MAG / 100d), 0, player.whoAmI, i, manaRestore);
                }
            }
        }
    }
}

