using System.Collections.Generic;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Actives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.BasicItems
{
    public class Stopwatch : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magic Stopwatch");
            Tooltip.SetDefault("\"Seems fragile\"");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 80000;
            item.rare = 4;
            item.accessory = true;
            item.material = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            base.ModifyTooltips(tooltips);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Stopwatch, 1);
            recipe.AddIngredient(ItemType<HextechCore>(), 2);
            recipe.AddIngredient(ItemID.SoulofLight, 6);
            recipe.AddIngredient(ItemID.SoulofNight, 6);
            recipe.AddTile(TileID.CrystalBall);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Active GetActive()
        {
            return new Stasis(2, 120, true);
        }

        public override bool OnCooldown(Player player)
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
            {
                if (!Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().stopWatchActive)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
