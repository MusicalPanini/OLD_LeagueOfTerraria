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
    public class EssenceReaver : LeagueItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Essence Reaver");
            Tooltip.SetDefault("4% increased melee and ranged damage" +
                "\n6% increased melee and ranged critical strike chance" +
                "\nIncreases ability haste by 20");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 25, 0, 0);
            item.rare = ItemRarityID.LightRed;
            item.accessory = true;

            Passives = new Passive[]
            {
                new SoulReave(7)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeCrit += 6;
            player.rangedCrit += 6;
            player.meleeDamage += 0.04f;
            player.rangedDamage += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().abilityHaste += 20;
            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Pickaxe>(), 1);
            recipe.AddIngredient(ItemType<Warhammer>(), 1);
            recipe.AddIngredient(ItemType<CloakofAgility>(), 1);
            recipe.AddIngredient(ItemID.Sickle, 1);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 20);
            recipe.AddIngredient(ItemID.Bone, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
