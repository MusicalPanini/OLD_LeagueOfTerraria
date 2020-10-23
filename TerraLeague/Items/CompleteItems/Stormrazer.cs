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
    public class Stormrazer : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stormrazer");
            Tooltip.SetDefault("5% increased melee and ranged damage" +
                "\n8% increased melee and ranged attack speed");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 25, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;

            Passives = new Passive[]
            {
                new Energized(15, 25),
                new Storm()
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.05f;
            player.rangedDamage += 0.05f;
            player.meleeSpeed += 0.08f;
            player.GetModPlayer<PLAYERGLOBAL>().rangedAttackSpeed += 0.08;
            player.moveSpeed += 0.05f;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<BFSword>(), 1);
            recipe.AddIngredient(ItemType<LongSword>(), 1);
            recipe.AddIngredient(ItemType<KircheisShard>(), 1);
            recipe.AddIngredient(ItemID.Muramasa, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();

            ModRecipe Muramasa = new ModRecipe(mod);
            Muramasa.AddIngredient(ItemType<ManaBar>(), 18);
            Muramasa.AddIngredient(ItemID.Bone, 50);
            Muramasa.AddTile(TileID.Anvils);
            Muramasa.SetResult(ItemID.Muramasa);
            Muramasa.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
                return ((int)Passives[0].passiveStat).ToString() + "%";
            else
                return "";
        }

        public override bool OnCooldown(Player player)
        {
            return !Passives[0].currentlyActive;
        }
    }
}
