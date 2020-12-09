using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class HextechEvolutionMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Evolution Mask");
            Tooltip.SetDefault("15% increased magic damage" +
                "\nIncreases ability haste by 10");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 24;
            item.value = 250000;
            item.rare = ItemRarityID.Pink;
            item.defense = 4;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicDamage += 0.15f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddRecipeGroup("TerraLeague:Tier3Bar", 10);
            recipe.AddIngredient(ItemType<PerfectHexCore>());
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            if(body.type == ItemType<HextechEvolutionBreastplate>() && legs.type == ItemType<HextechEvolutionLeggings>())
                return true;
            else
                return false;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Periodically fire a lazer at near by enemies";
            player.GetModPlayer<PLAYERGLOBAL>().hextechEvolutionSet = true;
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
            //drawAltHair = true;
        }
    }
}
