using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class DuskBlade : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Duskblade of Draktharr");
            Tooltip.SetDefault("10% increased melee damage" +
                "\nAbility cooldown reduced by 10%" +
                "\nIncreases melee armor penetration by 7");
        }

        public override void SetDefaults()
        {
            item.width = 24;
            item.height = 30;
            item.value = 180000;
            item.rare = 5;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.1f;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().meleeArmorPen += 7;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<SerratedDirk>(), 1);
            recipe.AddIngredient(ItemType<Warhammer>(), 1);
            recipe.AddIngredient(ItemID.WarriorEmblem, 1);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 8);
            recipe.AddRecipeGroup("TerraLeague:DemonGroup", 5);
            recipe.AddIngredient(ItemID.SoulofSight, 6);
            recipe.AddIngredient(ItemID.SoulofFright, 6);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Nightstalker(3, 20);
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1)
                return ((int)GetStatOnPlayer(Main.LocalPlayer)).ToString() + "%";
            else
                return "";
        }
    }
}
