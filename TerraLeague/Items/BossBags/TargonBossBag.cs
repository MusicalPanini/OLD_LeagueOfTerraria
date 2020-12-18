using TerraLeague.Items.Accessories;
using TerraLeague.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.BossBags
{
    public class TargonBossBag : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treasure Bag");
			Tooltip.SetDefault("{$CommonItemTooltip.RightClickToOpen}");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.consumable = true;
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Expert;
			item.expert = true;
		}

		public override bool CanRightClick()
		{
			return true;
		}

		public override void OpenBossBag(Player player)
		{
			player.TryGettingDevArmor();
			int choice = Main.rand.Next(5);
			if (choice == 0)
			{
				player.QuickSpawnItem(ModContent.ItemType<Placeable.TargonMonolith>());
			}
			player.QuickSpawnItem(ModContent.ItemType<CelestialBar>(), Main.rand.Next(4, 11));
			player.QuickSpawnItem(ModContent.ItemType<BottleOfStardust>());
		}

		public override int BossBagNPC => ModContent.NPCType<TargonSigil>();
	}
}