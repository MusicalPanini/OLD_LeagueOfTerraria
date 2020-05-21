using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class MercuryTreads3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mercury Treads");
            Tooltip.SetDefault("[c/E8B688:Tier 3: Fast Sprint + Rocket Boots]" +
                "\nIncreases resist by 7");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads1>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads2>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads4>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads5>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads2>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads4>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads5>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 22;
            item.value = 100000;
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T3Boots = true;
            player.rocketBoots = 2;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 7;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RocketBoots);
            recipe.AddIngredient(ItemType<MercuryTreads2>());
            recipe.AddIngredient(ItemType<NullMagic>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
