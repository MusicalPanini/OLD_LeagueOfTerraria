using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SpiritualHelmet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spiritual Head Piece");
            Tooltip.SetDefault("Increases maximum mana by 30" +
                "\nAbility cooldown reduced by 10%");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 16;
            item.value = 45000;
            item.rare = ItemRarityID.Orange;
            item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ManaBar>(), 15);
            recipe.AddIngredient(ItemID.Emerald, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            if(body.type == ItemType<SpiritualBreastplate>() && legs.type == ItemType<SpiritualLeggings>())
                return true;
            else
                return false;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Gain a bunch of stats while below 50% life";
            player.armorEffectDrawShadowLokis = true;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
        }
    }
}
