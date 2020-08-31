using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class VoidBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Void Warped Breastplate");
            Tooltip.SetDefault("5% increased minion damage" +
                "\n5% increased melee damage");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 40000;
            item.rare = ItemRarityID.Orange;
            item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.05;
            player.meleeDamage += 0.05f;
        }

        public override void AddRecipes()
        {
        }

        public override void UpdateArmorSet(Player player)
        {
        }
    }
}
