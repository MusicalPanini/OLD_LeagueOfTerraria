﻿using TerraLeague.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Banners
{
    public class MistwraithBanner : ModItem
    {
		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 24;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.rare = ItemRarityID.Blue;
			item.value = Item.buyPrice(0, 0, 10, 0);
			item.createTile = TileType<MonsterBanner>();
			item.placeStyle = (int)BannerType.Mistwraith;        //Place style means which frame(Horizontally, starting from 0) of the tile should be placed
		}
	}
}
