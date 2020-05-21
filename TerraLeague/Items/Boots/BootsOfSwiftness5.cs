using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class BootsOfSwiftness5 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boots of Swiftness");
            Tooltip.SetDefault("[c/F892F8:Tier 5: Fastest Sprint + Rocket Boots + Ice Skates]" +
                "\nImmensely increased sprint speed" +
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
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness4>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness2>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness3>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness4>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 26;
            item.value = 350000;
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T5Boots = true;
            player.moveSpeed += 0.08f;
            player.rocketBoots = 3;
            player.GetModPlayer<PLAYERGLOBAL>().swifties = true;
            player.iceSkate = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IceSkates);
            recipe.AddIngredient(ItemType<BootsOfSwiftness4>());
            recipe.AddIngredient(ItemID.SwiftnessPotion, 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
