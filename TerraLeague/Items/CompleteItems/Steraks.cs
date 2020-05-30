using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class Steraks : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sterak's Gage");
            Tooltip.SetDefault("Increases maximum life by 40" +
                "\nIncreases melee knockback");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 800000;
            item.rare = ItemRarityID.Red;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statLifeMax2 += 40;
            player.kbGlove = true;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Jaurim>(), 1);
            recipe.AddIngredient(ItemType<Pickaxe>(), 1);
            recipe.AddIngredient(ItemType<RubyCrystal>(), 1);
            recipe.AddIngredient(ItemID.FragmentSolar, 10);
            recipe.AddIngredient(ItemType<BrassBar>(), 20);
            recipe.AddIngredient(ItemID.MechanicalGlove, 1);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Lifeline(90);
        }

        public override Passive GetSecondaryPassive()
        {
            return new GiantStrength(25);
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (slot != -1)
            {
                if (modPlayer.lifeLineCooldown > 0 && Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[slot * 2])
                    return ((int)GetStatOnPlayer(Main.LocalPlayer) / 60).ToString();
                else
                    return "";
            }
            else
                return "";
        }

        public override bool OnCooldown(Player player)
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (slot != -1)
            {
                if (modPlayer.lifeLineCooldown > 0 || !Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[slot * 2])
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
