using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class BrassBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bilgewater Brass Bar");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 30;
            item.height = 24;
            item.rare = ItemRarityID.Blue;
            item.value = Item.buyPrice(0, 0, 30, 0);
            item.uniqueStack = false;
            item.createTile = TileType<Tiles.BrassBarTile>();
            item.useTurn = true;
            item.autoReuse = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe pirateHat = new ModRecipe(mod);
            pirateHat.AddIngredient(ItemType<BrassBar>(), 10);
            pirateHat.AddIngredient(ItemID.Silk, 4);
            pirateHat.AddTile(TileID.Anvils);
            pirateHat.SetResult(ItemID.PirateHat);
            pirateHat.AddRecipe();

            ModRecipe pirateShirt = new ModRecipe(mod);
            pirateShirt.AddIngredient(ItemType<BrassBar>(), 16);
            pirateShirt.AddIngredient(ItemID.Silk, 10);
            pirateShirt.AddTile(TileID.Anvils);
            pirateShirt.SetResult(ItemID.PirateShirt);
            pirateShirt.AddRecipe();

            ModRecipe piratePants = new ModRecipe(mod);
            piratePants.AddIngredient(ItemType<BrassBar>(), 12);
            piratePants.AddIngredient(ItemID.Silk, 6);
            piratePants.AddTile(TileID.Anvils);
            piratePants.SetResult(ItemID.PiratePants);
            piratePants.AddRecipe();

            ModRecipe cannonHat = new ModRecipe(mod);
            cannonHat.AddIngredient(ItemType<BrassBar>(), 10);
            cannonHat.AddIngredient(ItemID.HellstoneBar, 10);
            cannonHat.AddTile(TileID.MythrilAnvil);
            cannonHat.SetResult(ItemID.BuccaneerBandana);
            cannonHat.AddRecipe();

            ModRecipe cannonShirt = new ModRecipe(mod);
            cannonShirt.AddIngredient(ItemType<BrassBar>(), 16);
            cannonShirt.AddIngredient(ItemID.HellstoneBar, 16);
            cannonShirt.AddTile(TileID.MythrilAnvil);
            cannonShirt.SetResult(ItemID.BuccaneerShirt);
            cannonShirt.AddRecipe();

            ModRecipe cannonPants = new ModRecipe(mod);
            cannonPants.AddIngredient(ItemType<BrassBar>(), 12);
            cannonPants.AddIngredient(ItemID.HellstoneBar, 12);
            cannonPants.AddTile(TileID.MythrilAnvil);
            cannonPants.SetResult(ItemID.BuccaneerPants);
            cannonPants.AddRecipe();
        }
    }
}
