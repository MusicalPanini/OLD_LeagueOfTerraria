using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class IonianBootsOfLucidity2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ionian Boots of Lucidity");
            Tooltip.SetDefault("[c/92F892:Tier 2: Fast Sprint]" +
                "\nAbility cooldown reduced by 7%");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity1>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity3>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity4>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity5>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity3>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity4>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity5>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.value = 50000;
            item.rare = ItemRarityID.Green;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T2Boots = true;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.07;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HermesBoots);
            recipe.AddIngredient(ItemType<IonianBootsOfLucidity1>());
            recipe.AddIngredient(ItemType<ManaBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SailfishBoots);
            recipe.AddIngredient(ItemType<IonianBootsOfLucidity1>());
            recipe.AddIngredient(ItemType<ManaBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FlurryBoots);
            recipe.AddIngredient(ItemType<IonianBootsOfLucidity1>());
            recipe.AddIngredient(ItemType<ManaBar>(), 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
