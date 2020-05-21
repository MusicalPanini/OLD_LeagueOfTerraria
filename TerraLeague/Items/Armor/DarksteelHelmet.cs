using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class DarksteelHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darksteel Helmet");
            Tooltip.SetDefault("3 armor" +
            "\nIncreases your max life by 25");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 26;
            item.value = 50000;
            item.rare = ItemRarityID.Orange;
            item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;
            player.statLifeMax2 += 25;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {

            if(body.type == ItemType<DarksteelBreastplate>() && legs.type == ItemType<DarksteelLeggings>())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "+12% melee damage";
            player.meleeDamage += 0.12f;
        }
    }
}
