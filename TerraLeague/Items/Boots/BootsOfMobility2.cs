using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class BootsOfMobility2 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boots of Mobility");
            Tooltip.SetDefault("[c/92F892:Tier 2: Fast Sprint]" +
                "\nWhile out of combat, gain a slight amount of mobility");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility1>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility3>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility4>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility5>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility3>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility4>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility5>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 26;
            item.height = 28;
            item.value = 50000;
            item.rare = 2;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T2Boots = true;
            if (player.GetModPlayer<PLAYERGLOBAL>().CombatTimer >= 240)
            {
                player.moveSpeed += 0.2f;
                player.runAcceleration += 0.02f;

                Dust dust = Dust.NewDustDirect(new Vector2(player.position.X, player.position.Y + 30), player.width, player.height - 30, DustID.Smoke, 0, 0, 0, new Color(223, 135, 29));
                dust.noGravity = true;
                dust.velocity *= 0;
            }

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HermesBoots);
            recipe.AddIngredient(ItemType<BootsOfMobility1>());
            recipe.AddIngredient(ItemID.SwiftnessPotion, 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.SailfishBoots);
            recipe.AddIngredient(ItemType<BootsOfMobility1>());
            recipe.AddIngredient(ItemID.SwiftnessPotion, 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FlurryBoots);
            recipe.AddIngredient(ItemType<BootsOfMobility1>());
            recipe.AddIngredient(ItemID.SwiftnessPotion, 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
