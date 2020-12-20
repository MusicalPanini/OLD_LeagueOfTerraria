﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class SpiritualLeggings : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Spiritual Leggings");
            Tooltip.SetDefault("Increases mana regeneration by 50%" +
                "\n7% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 18;
            item.value = 40000;
            item.rare = ItemRarityID.Orange;
            item.defense = 5;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.07f;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(GetInstance<ManaBar>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateArmorSet(Player player)
        {
        }

        
    }
}
