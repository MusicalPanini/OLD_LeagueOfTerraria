using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class CelestialHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            DisplayName.SetDefault("Celestial Hood");
            Tooltip.SetDefault("Increases maximum mana by 30" +
                "\nIncreases ability haste by 20" +
                "\nIncreases item and summoner spell haste by 15");
        }

        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 6000 * 5;
            item.rare = ItemRarityID.Green;
            item.defense = 3;
        }


        
        public override void UpdateEquip(Player player)
        {
            player.statManaMax2 += 30;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 20;
            player.GetModPlayer<PLAYERGLOBAL>().itemHaste += 15;
        }

        public override void UpdateVanity(Player player, EquipType type)
        {
            base.UpdateVanity(player, type);
        }

        public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawAltHair = true;
            base.DrawHair(ref drawHair, ref drawAltHair);
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            if (body.type == ItemType<CelestialShirt>() && legs.type == ItemType<CelestialBoots>())
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
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            player.setBonus = "Gain " + TerraLeague.CreateScalingTooltip(TerraLeague.CDRColor, "HASTE", modPlayer.abilityHasteLastStep, 25, false, "%") + " increased critical strike chance";
            player.meleeCrit += modPlayer.abilityHasteLastStep / 4;
            player.rangedCrit += modPlayer.abilityHasteLastStep / 4;
            player.magicCrit += modPlayer.abilityHasteLastStep / 4;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<CelestialBar>(), 10);
            recipe.AddIngredient(ItemID.Silk, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
