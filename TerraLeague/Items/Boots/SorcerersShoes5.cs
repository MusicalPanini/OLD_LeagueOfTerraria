using TerraLeague.Items.AdvItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class SorcerersShoes5 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sorcerer's Shoes");
            Tooltip.SetDefault("[c/F892F8:Tier 5: Fastest Sprint + Rocket Boots + Ice Skates]" +
                "\nIncreases magic armor penetration by 18" +
                       "\n8% increased movement speed");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes1>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes2>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes3>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes4>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes2>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes3>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<SorcerersShoes4>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 26;
            item.value = 350000;
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T5Boots = true;
            player.rocketBoots = 3;
            player.moveSpeed += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().magicArmorPen += 18;
            player.iceSkate = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IceSkates);
            recipe.AddIngredient(ItemType<SorcerersShoes4>());
            recipe.AddIngredient(ItemType<Orb>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
