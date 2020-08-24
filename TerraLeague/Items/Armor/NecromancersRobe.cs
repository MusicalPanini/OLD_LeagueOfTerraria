using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class NecromancersRobe : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Necromancer's Robe");
            Tooltip.SetDefault("Increases your max number of sentries by 1");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 30;
            item.value = 4000 * 5;
            item.rare = ItemRarityID.Blue;
            item.defense = 4;
        }
        
        public override void UpdateEquip(Player player)
        {
            player.maxTurrets += 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 32);
            recipe.AddIngredient(ItemID.Silk, 12);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
            drawArms = false;
        }
    }
}
