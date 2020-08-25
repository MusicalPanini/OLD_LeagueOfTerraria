using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class ObsidianPossessedSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Obsidian Possessed Skull");
            Tooltip.SetDefault("Increase max number of minions by 1" +
                "\nGrants immunity to fire blocks");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 30;
            item.rare = ItemRarityID.LightRed;
            item.value = 100000;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 1;
            player.fireWalk = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ObsidianSkull, 1);
            recipe.AddIngredient(ItemType<PossessedSkull>(), 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
