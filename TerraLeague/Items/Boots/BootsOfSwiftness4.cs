using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class BootsOfSwiftness4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boots of Swiftness");
            Tooltip.SetDefault("[c/F49090:Tier 4: Faster Sprint + Rocket Boots]" +
                "\nGreatly increased sprint speed" +
                "\n8% increased movement speed");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness1>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness2>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness3>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness5>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness2>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness3>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness5>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 26;
            item.value = 300000;
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T4Boots = true;
            player.rocketBoots = 2;
            player.moveSpeed += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().swifties = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Aglet);
            recipe.AddIngredient(ItemID.AnkletoftheWind);
            recipe.AddIngredient(ItemType<BootsOfSwiftness3>());
            recipe.AddIngredient(ItemID.SwiftnessPotion, 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
