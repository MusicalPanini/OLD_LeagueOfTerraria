using TerraLeague.Items.AdvItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CompleteItems
{
    public class RapidFire : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rapidfire Cannon");
            Tooltip.SetDefault("20% increased ranged attack speed" +
                "\n10% increased ranged critical strike chance" +
                "\n10% increased movement speed");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = 800000;
            item.rare = 10;
            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.rangedCrit += 10;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed *= 1.2;
            player.moveSpeed += 0.10f;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<KircheisShard>(), 1);
            recipe.AddIngredient(ItemType<Zeal>(), 1);
            recipe.AddIngredient(ItemID.Cannon, 1);
            recipe.AddIngredient(ItemID.RocketLauncher, 1);
            recipe.AddIngredient(ItemID.FragmentVortex, 10);
            recipe.AddTile(412);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override Passive GetPrimaryPassive()
        {
            return new Energized(70, 40, true);
        }

        public override Passive GetSecondaryPassive()
        {
            return new Detonate();
        }

        public override string GetStatText()
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);

            if (slot != -1 && Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[slot * 2])
                return ((int)GetStatOnPlayer(Main.LocalPlayer)).ToString() + "%";
            else
                return "";
        }

        public override bool OnCooldown(Player player)
        {
            int slot = TerraLeague.FindAccessorySlotOnPlayer(Main.LocalPlayer, this);
            if (slot != -1)
            {
                if (!Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().PassivesAreActive[slot * 2])
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
