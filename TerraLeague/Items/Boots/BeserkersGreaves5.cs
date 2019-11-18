using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class BeserkersGreaves5 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beserker's Greaves");
            Tooltip.SetDefault("[c/F892F8:Tier 5: Fastest Sprint + Rocket Boots + Ice Skates]" +
                "\n18% increased melee and ranged attack speed" +
                       "\n8% increased movement speed");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves1>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves2>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves3>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves4>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves2>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves3>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves4>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 26;
            item.value = 350000;
            item.rare = 5;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T5Boots = true;
            player.rocketBoots = 3;
            player.moveSpeed += 0.08f;
            player.meleeSpeed += 0.18f;
            player.iceSkate = true;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed *= 1.18;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IceSkates);
            recipe.AddIngredient(ItemType<BeserkersGreaves4>());
            recipe.AddIngredient(ItemType<Dagger>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
