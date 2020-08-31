using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class VoidHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Warped Helmet");
            Tooltip.SetDefault("Increases your max number of minions by 2" +
            "\n5% increased minion damage");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = 45000;
            item.rare = ItemRarityID.Orange;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 2;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.05;
        }

        public override void AddRecipes()
        {
            
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {

            if(body.type == ItemType<VoidBreastplate>() && legs.type == ItemType<VoidLeggings>())
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
            player.setBonus = "Increases your max number of minions" +
                "\n5% increased melee damage";
            player.maxMinions += 1;
            player.meleeDamage += 0.05f;
        }
    }
}
