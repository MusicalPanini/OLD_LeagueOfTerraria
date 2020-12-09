using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Deathfire : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Deathfire Grasp");
            Tooltip.SetDefault("12% increased magic damage" +
               "\nIncreases ability haste by 15");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 60, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.accessory = true;

            Active = new Doom(25, 1000, 120);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicDamage += 0.12f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 15;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            // Add late game shadow isles material

            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<LargeRod>(), 1);
            recipe.AddIngredient(ItemType<Codex>(), 1);
            recipe.AddIngredient(ItemID.Ectoplasm, 12);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 50);
            recipe.AddTile(TileID.LihzahrdAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Active.currentlyActive)
            {
                if (Active.cooldownCount > 0)
                    return (Active.cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Active.cooldownCount > 0 || !Active.currentlyActive)
                return true;
            else
                return false;
        }
    }
}

