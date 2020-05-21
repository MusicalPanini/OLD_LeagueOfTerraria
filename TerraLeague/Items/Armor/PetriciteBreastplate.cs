using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body, EquipType.Back)]
    public class PetriciteBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Silver-Steel Breastplate");
            Tooltip.SetDefault("5 resist" +
            "\n10% increased melee damage" +
            "\nEnemies are more likely to target you");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 26;
            item.value = 45000;
            item.rare = ItemRarityID.Orange;
            item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().resist += 5;
            player.meleeDamage += 0.07f;
            player.aggro += 150;
            player.back = (sbyte)mod.GetEquipSlot("PetriciteBreastplate", EquipType.Back);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(GetInstance<SilversteelBar>(), 18);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            if (player.wings <= 0)
                player.back = (sbyte)mod.GetEquipSlot("PetriciteBreastplate", EquipType.Back);
            base.UpdateVanity(player, type);
        }
    }
}
