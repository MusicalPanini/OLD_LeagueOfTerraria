using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Back, EquipType.Body)]
    public class SolariBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Solari Breastplate");
            Tooltip.SetDefault("10 armor" +
                "\nIncreases your max life by 20");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 145000 * 5;
            item.rare = ItemRarityID.Yellow;
            item.defense = 30;
        }


        
        public override void UpdateEquip(Player player)
        {
            player.back = (sbyte)mod.GetEquipSlot("SolariBreastplate", EquipType.Back);
            player.GetModPlayer<PLAYERGLOBAL>().armor += 10;
            player.statLifeMax2 += 20;
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            if (player.wings <= 0)
                player.back = (sbyte)mod.GetEquipSlot("SolariBreastplate", EquipType.Back); ;
            base.UpdateVanity(player, type);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CelestialBar>(), 14);
            recipe.AddIngredient(ItemType<FragmentOfTheAspect>(), 1);
            recipe.AddIngredient(ItemID.LunarTabletFragment, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
