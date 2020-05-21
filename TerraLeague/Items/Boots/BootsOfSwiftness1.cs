using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class BootsOfSwiftness1 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boots of Swiftness");
            Tooltip.SetDefault("[c/8686E5:Tier 1: Slow Sprint]" +
                "\nUnremarkably increased sprint speed");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness2>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness3>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness4>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness5>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness2>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness3>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness4>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfSwiftness5>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 26;
            item.value = 20000;
            item.rare = ItemRarityID.Blue;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T1Boots = true;
            player.GetModPlayer<PLAYERGLOBAL>().swifties = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BootsOfSpeed>());
            recipe.AddIngredient(ItemID.SwiftnessPotion, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
