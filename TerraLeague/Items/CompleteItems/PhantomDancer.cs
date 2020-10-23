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
    public class PhantomDancer : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Phantom Dancer");
            Tooltip.SetDefault("20% increased melee speed" +
                "\n15% increased melee critical strike chance" +
                "\n10% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 34;
            item.height = 32;
            item.value = Item.buyPrice(0, 50, 0, 0);
            item.rare = ItemRarityID.Yellow;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
                new Lifeline(90)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeSpeed += 0.20f;
            player.meleeCrit += 15;
            player.moveSpeed += 0.1f;
            player.accRunSpeed *= 1.1f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Dagger>(), 2);
            recipe.AddIngredient(ItemType<Zeal>(), 1);
            recipe.AddIngredient(ItemID.Sickle, 1);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 50);
            recipe.AddIngredient(ItemID.Ectoplasm, 5);
            recipe.AddIngredient(ItemID.SpectreBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
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
