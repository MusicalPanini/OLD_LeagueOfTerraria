using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class CelestialShirt : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Celestial Garb");
            Tooltip.SetDefault("MEL, RNG, MAG, and SUM increased by 25");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 26;
            item.value = 9000 * 5;
            item.rare = ItemRarityID.Green;
            item.defense = 4;
        }


        
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().BonusMEL += 25;
            player.GetModPlayer<PLAYERGLOBAL>().BonusRNG += 25;
            player.GetModPlayer<PLAYERGLOBAL>().BonusMAG += 25;
            player.GetModPlayer<PLAYERGLOBAL>().BonusSUM += 25;
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            base.UpdateVanity(player, type);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CelestialBar>(), 20);
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.AddIngredient(ItemID.Leather, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
