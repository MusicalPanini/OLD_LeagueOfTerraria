using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class NecromancersHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Necromancer's Hood");
            Tooltip.SetDefault("Increases your max number of minions by 1");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 26;
            item.value = 4000 * 5;
            item.rare = ItemRarityID.Blue;
            item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 20);
            recipe.AddIngredient(ItemID.Silk, 8);
            recipe.AddTile(TileID.Loom);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {

            if(body.type == ItemType<NecromancersRobe>())
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
            player.setBonus = "+10% minion damage";
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.1f;
        }
    }
}
