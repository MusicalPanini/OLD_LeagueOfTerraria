using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class Hexdrinker : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hexdrinker");
            Tooltip.SetDefault("3% increased melee and ranged damage" +
                "\nIncreases resist by 3");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;

            Passives = new Passive[]
            {
                new Lifeline(90)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.03f;
            player.rangedDamage += 0.03f;
            player.GetModPlayer<PLAYERGLOBAL>().resist += 3;
            base.UpdateAccessory(player, hideVisual);

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<LongSword>(), 1);
            recipe.AddIngredient(ItemType<NullMagic>(), 1);
            recipe.AddIngredient(ItemType<Petricite>(), 12);
            recipe.AddIngredient(ItemID.MeteoriteBar, 5);
            recipe.AddIngredient(ItemID.Amber, 4);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            if (Passives[0].currentlyActive)
            {
                if (modPlayer.lifeLineCooldown > 0)
                    return (modPlayer.lifeLineCooldown / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.lifeLineCooldown > 0 || !Passives[0].currentlyActive)
                return true;
            else
                return false;
        }
    }
}
