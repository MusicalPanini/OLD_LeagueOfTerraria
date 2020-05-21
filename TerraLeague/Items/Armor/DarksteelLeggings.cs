using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class DarksteelLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Darksteel Greaves");
            Tooltip.SetDefault("2 armor" +
                "\n4% increased melee critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 45000;
            item.rare = ItemRarityID.Orange;
            item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 2;
            player.meleeCrit += 4;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
