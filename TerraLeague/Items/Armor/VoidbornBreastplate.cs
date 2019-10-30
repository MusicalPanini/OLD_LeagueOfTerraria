using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class VoidbornBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Voidborn Chestpiece");
            Tooltip.SetDefault("12% increased minion damage" +
                "\nIncreases your max number of minions");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 250000;
            item.rare = 7;
            item.defense = 14;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions++;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.12;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<VoidBar>(), 24);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 24);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateArmorSet(Player player)
        {
            
        }
    }
}
