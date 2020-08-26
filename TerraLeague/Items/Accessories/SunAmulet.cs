using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Items.CustomItems;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class SunAmulet : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sun Amulet");
            Tooltip.SetDefault("Gain damage based on the light level your in");
            base.SetStaticDefaults();
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");
            Player player = Main.LocalPlayer;
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (tt != null)
            {
                tt.text = "Gain damage based on the light level your in (Current damage: " + modPlayer.sunAmuletDamage + "%)";
            }
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 32;
            item.rare = ItemRarityID.LightRed;
            item.value = 6400 * 5;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.sunAmulet = true;
        }

        public override void AddRecipes()
        {
            //ModRecipe recipe = new ModRecipe(mod);
            //recipe.AddIngredient(ItemID.CrossNecklace, 1);
            //recipe.AddIngredient(ItemType<Nightbloom>(), 1);
            //recipe.AddTile(TileID.TinkerersWorkbench);
            //recipe.SetResult(this);
            //recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);
            Player player = Main.LocalPlayer;
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (slot != -1)
                return modPlayer.sunAmuletDamage + "%";
            else
                return "";
        }
    }

    public class GraymarkNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            float rnd = Main.rand.NextFloat();
            if ((npc.netID == NPCID.Antlion || npc.netID == NPCID.FlyingAntlion || npc.netID == NPCID.WalkingAntlion || npc.netID == NPCType<ShadowArtilery>()) && (rnd <= 0.0133 || (Main.expertMode && rnd <= 0.0266f)) && !npc.SpawnedFromStatue)
            {
                Item.NewItem(npc.getRect(), ItemType<SunAmulet>(), 1);
            }

            base.NPCLoot(npc);
        }
    }
}
