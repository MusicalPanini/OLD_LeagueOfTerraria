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
    public class RavenousHydra : LeagueItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ravenous Hydra");
            Tooltip.SetDefault("7% increased melee damage" +
                "\nIncreases life regeneration by 2" +
                "\n+1 melee life steal" +
                //"\n12% reduced maximum life" +
                //"\n12% increased damage taken" +
                "\nCan only have one Hydra item equiped at a time");
        }

        public override bool CanEquipAccessory(Player player, int slot)
        {
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<TitanicHydra>()) == slot - 3)
                return true;
            if (TerraLeague.FindAccessorySlotOnPlayer(player, GetInstance<TitanicHydra>()) != -1)
                return false;

            return base.CanEquipAccessory(player, slot);
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.value = Item.buyPrice(0, 45, 0, 0);
            item.rare = ItemRarityID.LightPurple;
            item.accessory = true;

            Passives = new Passive[]
            {
                new Cleave(50, CleaveType.Lifesteal)
            };
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.meleeDamage += 0.07f;
            player.lifeRegen += 2;

            player.GetModPlayer<PLAYERGLOBAL>().lifeStealMelee += 1;// 0.05;
           // player.GetModPlayer<PLAYERGLOBAL>().healthModifier -= 0.12;
           // player.GetModPlayer<PLAYERGLOBAL>().damageTakenModifier += 0.12;

            base.UpdateAccessory(player, hideVisual);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemType<Tiamat>(), 1);
            recipe.AddIngredient(ItemType<VampiricScepter>(), 1);
            recipe.AddIngredient(ItemType<Pickaxe>(), 1);
            recipe.AddIngredient(ItemID.Gungnir, 1);
            recipe.AddIngredient(ItemType<DarksteelBar>(), 10);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
