using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    public class VoidProphetHood : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Void Prophet's Hood");
            Tooltip.SetDefault("Increases your max number of minions" +
            "\nIncreases your max life by 50");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 26;
            item.value = 300000;
            item.rare = ItemRarityID.Lime;
            item.defense = 15;
        }

        public override void UpdateEquip(Player player)
        {
            player.maxMinions += 1;
            player.statLifeMax2 += 50;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(GetInstance<VoidBar>(), 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return (body.type == ItemType<VoidProphetGarb>() && legs.type == ItemType<VoidProphetPants>());
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Periodically spawn " + TerraLeague.CreateScalingTooltip(TerraLeague.MINIONMAXColor, "MINIONS", player.maxMinions, 100) + " Zz'rots";
            player.armorEffectDrawShadowBasilisk = true;
            player.GetModPlayer<PLAYERGLOBAL>().prophetSet = true;
        }
    }
}
