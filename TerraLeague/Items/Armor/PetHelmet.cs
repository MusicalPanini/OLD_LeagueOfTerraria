using TerraLeague.Items.PetrifiedWood;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class PetHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Petrified Breastplate");
            Tooltip.SetDefault("Decreases maximum mana by 20");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = 0;
            item.rare = ItemRarityID.Blue;
            item.defense = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 -= 20;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(GetInstance<PetWood>(), 20);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            if(body.type == ItemType<PetBreastplate>() && legs.type == ItemType<PetLeggings>())
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
            player.setBonus = "3 resist";
            player.GetModPlayer<PLAYERGLOBAL>().resist += 3;
        }
    }
}
