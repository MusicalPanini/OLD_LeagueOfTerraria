using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using TerraLeague.Items.AdvItems;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Ardents : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ardent Censer");
            Tooltip.SetDefault("8% increased magic and minion damage" +
                "\n8% increased movement speed" +
                "\nIncreases mana regeneration by 20%" +
                "\n8% increased healing power" +
                "\nAbility cooldown reduced by 10%");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 180000;
            item.rare = ItemRarityID.Pink;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().healPower += 0.08;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().manaRegenModifer += 0.2;
            player.magicDamage += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.08;
            player.moveSpeed += 0.08f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<ForbiddenIdol>(), 1);
            recipe.AddIngredient(ItemType<AetherWisp>(), 1);
            recipe.AddIngredient(ItemID.HeartLantern, 1);
            recipe.AddRecipeGroup("TerraLeague:Tier3Bar", 8);
            recipe.AddIngredient(ItemID.SoulofMight, 6);
            recipe.AddIngredient(ItemID.SoulofSight, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new ArdentsFrenzy();
        }
    }
}
