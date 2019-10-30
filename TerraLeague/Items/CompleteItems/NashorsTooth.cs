using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class NashorsTooth : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nashor's Tooth");
            Tooltip.SetDefault("10% increased minion damage" +
                "\n25% increased melee speed" +
                "\nAbility cooldown reduced by 20%");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 180000;
            item.rare = 6;
            item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.1;
            player.GetModPlayer<PLAYERGLOBAL>().Cdr -= 0.2;
            player.meleeSpeed += 0.25f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Stinger>(), 1);
            recipe.AddIngredient(ItemType<Codex>(), 1);
            recipe.AddIngredient(ItemID.Excalibur, 1);
            recipe.AddIngredient(ItemType<VoidBar>(), 8);
            recipe.AddIngredient(ItemID.SoulofFright, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new VoidSharpened(15, 25);
        }
    }
}
