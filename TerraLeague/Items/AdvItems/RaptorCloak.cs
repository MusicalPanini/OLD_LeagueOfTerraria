using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class RaptorCloak : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Raptor Cloak");
            Tooltip.SetDefault("4% increased minion damage" +
                "\nIncreases armor by 3" +
                "\nIncreases your max number of sentries" +
                "\nIncreases life regeneration by 2");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.04;
            player.lifeRegen += 2;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;
            player.maxTurrets += 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ClothArmor>(), 1);
            recipe.AddIngredient(ItemType<RejuvBead>(), 1);
            recipe.AddIngredient(ItemID.Feather, 10);
            recipe.AddIngredient(ItemID.SunplateBlock, 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
