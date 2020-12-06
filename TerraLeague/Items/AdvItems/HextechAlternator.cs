using TerraLeague.Items.BasicItems;
using TerraLeague.Items.CustomItems;
using TerraLeague.Items.CustomItems.Passives;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.AdvItems
{
    public class HextechAlternator : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Alternator");
            Tooltip.SetDefault("4% increased magic and minion damage");
        }

        public override void SetDefaults()
        {
            item.width = 52;
            item.height = 20;
            item.value = Item.buyPrice(0, 15, 0, 0);
            item.rare = ItemRarityID.Orange;
            item.accessory = true;
            item.material = true;

            Passives = new Passive[]
            {
                new MagicBolt(20, 20, 30)
            };
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.magicDamage += 0.04f;
            player.GetModPlayer<PLAYERGLOBAL>().TrueMinionDamage += 0.04;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<AmpTome>(), 2);
            //recipe.AddIngredient(ItemID.Revolver, 1);
            //recipe.AddIngredient(ItemID.SpaceGun, 1);
            recipe.AddIngredient(ItemID.MeteoriteBar, 10);
            recipe.AddIngredient(ItemID.Wire, 10);
            recipe.AddIngredient(ItemID.FallenStar, 4);
            recipe.AddIngredient(ItemType<PrototypeHexCore>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override string GetStatText()
        {
            if (Passives[0].currentlyActive)
            {
                if (Passives[0].cooldownCount > 0)
                    return (Passives[0].cooldownCount / 60).ToString();
            }
            return "";
        }

        public override bool OnCooldown(Player player)
        {
            if (Passives[0].cooldownCount > 0 || !Passives[0].currentlyActive)
                return true;
            else
                return false;
        }
    }
}
