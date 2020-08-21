using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class VoidProphetPants : ModItem
    {

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Void Prophet's Pants");
            Tooltip.SetDefault("12% increased minion damage" +
                "\nIncreases your max number of sentries");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 250000;
            item.rare = ItemRarityID.Lime;
            item.defense = 12;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxTurrets++;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.12;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidBar>(), 18);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
