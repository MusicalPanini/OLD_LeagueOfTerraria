using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Boots
{
    public class BootsOfMobility3 : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Boots of Mobility");
            Tooltip.SetDefault("[c/E8B688:Tier 3: Fast Sprint + Rocket Boots]" +
                "\nWhile out of combat, gain a good amount of mobility");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility1>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility2>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility4>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility5>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility1>()) != -1)
                return false;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<BootsOfMobility2>()) != -1)
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
            item.value = 100000;
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;

        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PLAYERGLOBAL>().T3Boots = true;
            player.rocketBoots = 2;
            if (player.GetModPlayer<PLAYERGLOBAL>().CombatTimer >= 240)
            {
                player.moveSpeed += 0.4f;
                player.runAcceleration += 0.04f;

                Dust dust = Dust.NewDustDirect(new Microsoft.Xna.Framework.Vector2(player.position.X, player.position.Y + 30), player.width, player.height - 30, DustID.Smoke, 0, 0, 0, new Microsoft.Xna.Framework.Color(223, 135, 29));
                dust.noGravity = true;
                dust.velocity *= 0;
            }

        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.RocketBoots);
            recipe.AddIngredient(ItemType<BootsOfMobility2>());
            recipe.AddIngredient(ItemID.SwiftnessPotion, 5);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
