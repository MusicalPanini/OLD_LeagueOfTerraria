using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class BeserkersGreaves2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beserker's Greaves");
            Tooltip.SetDefault("[c/92F892:Tier 2: Fast Sprint]" +
                "\n10% increased melee speed" +
                "\n10% chance to not consume ammo");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves1>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves3>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves4>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves5>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves3>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves4>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves5>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }


        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 26;
            item.value = 50000;
            item.rare = 2;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T2Boots = true;

            player.meleeSpeed += 0.1f;
            player.GetModPlayer<PLAYERGLOBAL>().ConsumeAmmoChance += 0.1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HermesBoots);
            recipe.AddIngredient(ItemType<BeserkersGreaves1>());
            recipe.AddIngredient(ItemType<Dagger>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SailfishBoots);
            recipe.AddIngredient(ItemType<BeserkersGreaves1>());
            recipe.AddIngredient(ItemType<Dagger>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FlurryBoots);
            recipe.AddIngredient(ItemType<BeserkersGreaves1>());
            recipe.AddIngredient(ItemType<Dagger>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
