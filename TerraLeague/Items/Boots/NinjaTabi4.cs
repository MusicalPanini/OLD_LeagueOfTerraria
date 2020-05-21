using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class NinjaTabi4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ninja Tabi");
            Tooltip.SetDefault("[c/F49090:Tier 4: Faster Sprint + Rocket Boots]" +
                "\nIncreases armor by 12" +
                "\n8% increased movement speed");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<NinjaTabi1>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<NinjaTabi2>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<NinjaTabi3>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<NinjaTabi5>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<NinjaTabi1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<NinjaTabi2>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<NinjaTabi3>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<NinjaTabi5>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 22;
            item.value = 300000;
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;
            item.material = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T4Boots = true;
            player.rocketBoots = 2;
            player.GetModPlayer<PLAYERGLOBAL>().armor += 12;
            player.moveSpeed += 0.08f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Aglet);
            recipe.AddIngredient(ItemID.AnkletoftheWind);
            recipe.AddIngredient(ItemType<NinjaTabi3>());
            recipe.AddIngredient(ItemType<ClothArmor>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
