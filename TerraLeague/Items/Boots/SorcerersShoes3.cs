using TerraLeague.Items.AdvItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class SorcerersShoes3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sorcerer's Shoes");

            Tooltip.SetDefault("[c/E8B688:Tier 3: Fast Sprint + Rocket Boots]" +
                "\nIncreases magic armor penetration by 12");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes1>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes2>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes5>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes4>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes2>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes5>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes4>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 26;
            item.value = 100000;
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T3Boots = true;
            player.rocketBoots = 2;
            player.GetModPlayer<PLAYERGLOBAL>().magicArmorPen += 12;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RocketBoots);
            recipe.AddIngredient(ItemType<SorcerersShoes2>());
            recipe.AddIngredient(ItemType<Orb>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
