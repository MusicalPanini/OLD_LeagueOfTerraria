using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class SolariHeadPiece : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Solari Head-Piece");
            Tooltip.SetDefault("Increases your max life by 10" +
                "\nIncreases life regeneration by 2");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 26;
            item.value = 145000 * 5;
            item.rare = ItemRarityID.Yellow;
            item.defense = 10;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().armor += 3;
            player.statLifeMax2 += 25;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CelestialBar>(), 8);
            recipe.AddIngredient(ItemType<FragmentOfTheAspect>(), 1);
            recipe.AddIngredient(ItemID.LunarTabletFragment, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (body.type == ItemType<SolariBreastplate>() && legs.type == ItemType<SolariLeggings>());
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Gain improved stats during the day." +
                "\nCharge up solar energy during the day" +
                "\nAt full charge double tap " + Terraria.Localization.Language.GetTextValue(Main.ReversedUpDownArmorSetBonuses ? "Key.UP" : "Key.DOWN") + " to summon a Solar Flare Storm";
            player.GetModPlayer<PLAYERGLOBAL>().solariSet = true;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
        }
    }
}
