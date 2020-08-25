using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class PossessedSkull : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Possessed Skull");
            Tooltip.SetDefault("Increase max number of minions by 1");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 30;
            item.rare = ItemRarityID.Green;
            item.value = 5400 * 5;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.maxMinions += 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Bone, 100);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 20);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
