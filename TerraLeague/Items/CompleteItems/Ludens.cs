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
    public class Ludens : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Luden's Echo");
            Tooltip.SetDefault("9% increased magic damage" +
                "\nIncreases maximum mana by 20" +
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
            player.magicDamage += 0.09f;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.1;
            player.statManaMax2 += 20;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<LostChapter>(), 1);
            recipe.AddIngredient(ItemType<BlastingWand>(), 1);
            recipe.AddIngredient(ItemID.RainbowRod, 1);
            recipe.AddIngredient(ItemID.CrystalShard, 10);
            recipe.AddIngredient(ItemType<VoidFragment>(), 50);
            recipe.AddIngredient(ItemID.SoulofNight, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Echo(40, 10);
        }

        public override Passive GetSecondaryPassive()
        {
            return new Haste();
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
