using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class AzirBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Sunstone Breastplate");
            Tooltip.SetDefault("5% increased minion damage" +
                "\nIncreases your max number of minions by 1");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 28;
            item.value = 6000 * 5;
            item.rare = ItemRarityID.Green;
            item.defense = 4;
        }


        
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.05;
            player.maxMinions += 1;
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            base.UpdateVanity(player, type);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Sunstone>(), 10);
            recipe.AddIngredient(ItemID.GoldBar, 20);
            recipe.AddIngredient(ItemID.Sapphire, 3);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
