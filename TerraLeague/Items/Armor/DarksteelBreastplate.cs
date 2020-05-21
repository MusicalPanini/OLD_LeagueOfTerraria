using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Back, EquipType.Body)]
    public class DarksteelBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Darksteel Breastplate");
            Tooltip.SetDefault("4 armor" +
                "\n10% increased melee damage");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 45000;
            item.rare = ItemRarityID.Orange;
            item.defense = 7;
        }


        
        public override void UpdateEquip(Player player)
        {
            player.back = (sbyte)mod.GetEquipSlot("DarksteelBreastplate", EquipType.Back);
            player.GetModPlayer<PLAYERGLOBAL>().armor += 4;
            player.meleeDamage += 0.22f;
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            if (player.wings <= 0)
                player.back = (sbyte)mod.GetEquipSlot("DarksteelBreastplate", EquipType.Back); ;
            base.UpdateVanity(player, type);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 18);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
