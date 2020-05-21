using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class IonianBootsOfLucidity3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ionian Boots of Lucidity");
            Tooltip.SetDefault("[c/E8B688:Tier 3: Fast Sprint + Rocket Boots]" +
                "\nAbility cooldown reduced by 10%");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity1>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity2>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity4>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity5>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<IonianBootsOfLucidity2>()) != -1)
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
            item.value = 100000;
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T3Boots = true;
            player.rocketBoots = 2;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RocketBoots);
            recipe.AddIngredient(ItemType<IonianBootsOfLucidity2>());
            recipe.AddIngredient(ItemType<ManaBar>(), 10);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
