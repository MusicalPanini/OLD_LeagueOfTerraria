using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class PetriciteLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Silver-Steel Greaves");
            Tooltip.SetDefault("4 resist" +
                "\nIncreased melee knockback" +
                "\nEnemies are more likely to target you");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 45000;
            item.rare = ItemRarityID.Orange;
            item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 4;
            player.aggro += 150;
            player.kbGlove = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SilversteelBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
