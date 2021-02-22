using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Body, EquipType.Back)]
    public class SpiritualBreastplate : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Spiritual Gown");
            Tooltip.SetDefault("10% increased heal power");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 40000;
            item.rare = ItemRarityID.Orange;
            item.defense = 3;
            item.backSlot = (sbyte)mod.GetEquipSlot("SpiritualBreastplate", EquipType.Back);
        }

        public override void UpdateEquip(Player player)
        {
            player.back = item.backSlot;
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.1;
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            if (player.wings <= 0)
                player.back = item.backSlot;
            base.UpdateVanity(player, type);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(GetInstance<ManaBar>(), 18);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void UpdateArmorSet(Player player)
        {
        }


        public override void DrawHands(ref bool drawHands, ref bool drawArms)
        {
            drawHands = true;
            drawArms = false;
        }
    }
}
