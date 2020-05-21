using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PetriciteHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver-Steel Helmet");
            Tooltip.SetDefault("4 resist" +
            "\nIncreases your max life by 10" +
            "\nEnemies are more likely to target you");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 26;
            item.value = 50000;
            item.rare = ItemRarityID.Orange;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 5;
            player.statLifeMax2 += 10;
            player.aggro += 150;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SilversteelBar>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            if(body.type == ItemType<PetriciteBreastplate>() && legs.type == ItemType<PetriciteLeggings>())
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
            player.setBonus = "+5% melee damage" +
                "\nGain 10 magic shield every 5 seconds (up tp 50)";
            player.meleeDamage += 0.05f;
        }
    }
}
