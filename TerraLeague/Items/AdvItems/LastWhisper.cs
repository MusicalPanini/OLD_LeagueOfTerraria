using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class LastWhisper : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Last Whisper");
            Tooltip.SetDefault("3% increased melee and ranged damage");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;

            Passives = new Passive[]
            {
                new CustomItems.Passives.LastWhisper(15, true)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.03f;
            player.rangedDamage += 0.03f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<LongSword>(), 1);
            recipe.AddIngredient(ItemID.SharkToothNecklace, 1);
            recipe.AddIngredient(ItemID.DemonBow, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe recipe2 = new ModRecipe(mod);
            recipe2.AddIngredient(ItemType<LongSword>(), 1);
            recipe2.AddIngredient(ItemID.SharkToothNecklace, 1);
            recipe2.AddIngredient(ItemID.TendonBow, 1);
            recipe2.AddTile(TileID.Anvils);
            recipe2.SetResult(this);
            recipe2.AddRecipe();
        }
    }
}
