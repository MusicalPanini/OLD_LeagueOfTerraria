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
            item.height = 28;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);
        }

        public override void Tier1Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.05;
            base.Tier1Update(player);
        }

        public override void Tier2Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.07;
            base.Tier1Update(player);
        }

        public override void Tier3Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            base.Tier1Update(player);
        }

        public override void Tier4Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().extraSumCDRLastStep -= 0.05;
            base.Tier1Update(player);
        }

        public override void Tier5Update(Player player)
        {
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().extraSumCDRLastStep -= 0.1;
            base.Tier1Update(player);
        }

        public override string Tier1Tip()
        {
            return "Ability cooldown reduced by 5%";
        }

        public override string Tier2Tip()
        {
            return "Ability cooldown reduced by 7%";
        }

        public override string Tier3Tip()
        {
            return "Ability cooldown reduced by 10%";
        }

        public override string Tier4Tip()
        {
            return "Ability cooldown reduced by 10%" +
                "\nSummoner Spell cooldowns are reduced by an additional 5%";
        }

        public override string Tier5Tip()
        {
            return "Ability cooldown reduced by 10%" +
                "\nSummoner Spell cooldowns are reduced by an additional 10%";
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
