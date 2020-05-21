using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class MercuryTreads5 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mercury Treads");
            Tooltip.SetDefault("[c/F892F8:Tier 5: Fastest Sprint + Rocket Boots + Ice Skates]" +
                "\nIncreases resist by 13" +
                "\n8% increased movement speed");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads1>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads2>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads3>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads5>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads2>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads3>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<MercuryTreads5>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 22;
            item.value = 350000;
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T5Boots = true;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 10;
            player.rocketBoots = 3;
            player.moveSpeed += 0.08f;
            player.iceSkate = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.IceSkates);
            recipe.AddIngredient(ItemType<MercuryTreads4>());
            recipe.AddIngredient(ItemType<NullMagic>());
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
