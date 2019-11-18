using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class BeserkersGreaves4 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Beserker's Greaves");
            Tooltip.SetDefault("[c/F49090:Tier 4: Faster Sprint + Rocket Boots]" +
                "\n15% increased melee and ranged attack speed" +
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
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves5>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves2>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves3>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BeserkersGreaves5>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 26;
            item.value = 300000;
            item.rare = 4;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

            player.GetModPlayer<PLAYERGLOBAL>().T4Boots = true;
            player.rocketBoots = 2;
            player.moveSpeed += 0.08f;
            player.meleeSpeed += 0.15f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed *= 1.15;

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Aglet);
            recipe.AddIngredient(ItemID.AnkletoftheWind);
            recipe.AddIngredient(ItemType<BeserkersGreaves3>());
            recipe.AddIngredient(ItemType<Dagger>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
