using TerraLeague.Items.BasicItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class IonianBootsOfLucidity : LeagueBoot
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ionian Boots of Lucidity");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 22;
            item.accessory = true;
            base.SetDefaults();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
        }

        public override void Tier1Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 5;
            base.Tier1Update(player);
        }

        public override void Tier2Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 10;
            base.Tier1Update(player);
        }

        public override void Tier3Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 15;
            base.Tier1Update(player);
        }

        public override void Tier4Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 15;
            player.GetModPlayer<PLAYERGLOBAL>().summonerHaste += 10;
            base.Tier1Update(player);
        }

        public override void Tier5Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 15;
            player.GetModPlayer<PLAYERGLOBAL>().summonerHaste += 20;
            base.Tier1Update(player);
        }

        public override string Tier1Tip()
        {
            return "Increases ability haste by 5";
        }

        public override string Tier2Tip()
        {
            return "Increases ability haste by 10";
        }

        public override string Tier3Tip()
        {
            return "Increases ability haste by 15";
        }

        public override string Tier4Tip()
        {
            return "Increases ability haste by 15" +
                "\nIncreases summoner spell haste by 10";
        }

        public override string Tier5Tip()
        {
            return "Increases ability haste by 15" +
                "\nIncreases summoner spell haste by 20";
        }


        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BootsOfSpeed>());
            recipe.AddIngredient(ItemID.Moonglow, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
