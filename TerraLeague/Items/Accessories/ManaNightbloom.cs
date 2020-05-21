using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Accessories
{
    public class ManaNightbloom : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mana Nightbloom");
            Tooltip.SetDefault("Increases mana regeneration by 1" +
                "\nWhile below 50% life increase mana regeneration by 2" +
                "\n6% reduced mana usage" +
                "\nAutomatically use mana potions when needed");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 44;
            item.rare = ItemRarityID.Pink;
            item.value = 100000;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.manaRegen += 1;
            player.manaCost -= 0.06f;
            player.manaFlower = true;
            if (modPlayer.GetRealHeathWithoutShield(true)/2 > modPlayer.GetRealHeathWithoutShield())
            {
                modPlayer.manaRegen += 2;
            }
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ManaFlower, 1);
            recipe.AddIngredient(ItemType<Nightbloom>(), 1);
            recipe.AddIngredient(ItemID.GreaterManaPotion, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
