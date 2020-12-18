using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Items.Banners;
using TerraLeague.NPCs;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Tiles
{
    public class MonsterBanner : ModTile
    {
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide | AnchorType.SolidBottom, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.addTile(Type);
			dustType = -1;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Banner");
			AddMapEntry(new Color(13, 88, 130), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			BannerType style = (BannerType)(frameX / 18);
			int item = 0;
			switch (style)
			{
				case BannerType.MarbleSlime:
					item = ItemType<MarbleSlimeBanner>();
					break;
				case BannerType.MountainSlime:
					item = ItemType<MountainSlimeBanner>();
					break;
				case BannerType.SoulBoundSlime:
					item = ItemType<SoulBoundSlimeBanner>();
					break;
				case BannerType.Undying:
					item = ItemType<UndyingBanner>();
					break;
				case BannerType.UndyingArcher:
					item = ItemType<UndyingArcherBanner>();
					break;
				case BannerType.UndyingNecro:
					item = ItemType<UndyingNecromancerBanner>();
					break;
				case BannerType.UnleashedSpirit:
					item = ItemType<UnleashedSpiritBanner>();
					break;
				case BannerType.MistEater:
					item = ItemType<MistEaterBanner>();
					break;
				case BannerType.MistDevor:
					item = ItemType<MistDevourerBanner>();
					break;
				case BannerType.FallenCrimera:
					item = ItemType<FallenCrimeraBanner>();
					break;
				case BannerType.HMCrimson_UNUSED:
					break;
				case BannerType.SpectralBiter:
					item = ItemType<MistBitterBanner>();
					break;
				case BannerType.ShelledHorror:
					item = ItemType<ShelledHorrorBanner>();
					break;
				case BannerType.PHMOcean_UNUSED:
					break;
				case BannerType.SpectralShark:
					item = ItemType<SpectralSharkBanner>();
					break;
				case BannerType.Scuttlegeist:
					item = ItemType<ScuttlegiestBanner>();
					break;
				case BannerType.EtherealRemitter:
					item = ItemType<EtheralRemitterBanner>();
					break;
				case BannerType.Mistwraith:
					item = ItemType<MistwraithBanner>();
					break;
				case BannerType.PHMDesert_UNUSED:
					break;
				case BannerType.ShadowArtilery:
					item = ItemType<ShadowArtileryBanner>();
					break;
				case BannerType.BansheeHive:
					item = ItemType<BansheeHiveBanner>();
					break;
			}
			Item.NewItem(i * 16, j * 16, 16, 48, item);
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.LocalPlayer;
				BannerType style = (BannerType)(Main.tile[i, j].frameX / 18);
                switch (style)
                {
                    case BannerType.MarbleSlime:
						player.NPCBannerBuff[NPCType<MarbleSlime>()] = true;
						break;
                    case BannerType.MountainSlime:
						player.NPCBannerBuff[NPCType<MountainSlime>()] = true;
                        break;
                    case BannerType.SoulBoundSlime:
						player.NPCBannerBuff[NPCType<SoulBoundSlime>()] = true;
                        break;
                    case BannerType.Undying:
						player.NPCBannerBuff[NPCType<TheUndying_1>()] = true;
                        break;
                    case BannerType.UndyingArcher:
						player.NPCBannerBuff[NPCType<TheUndying_Archer>()] = true;
						break;
                    case BannerType.UndyingNecro:
						player.NPCBannerBuff[NPCType<TheUndying_Necromancer>()] = true;
						break;
                    case BannerType.UnleashedSpirit:
						player.NPCBannerBuff[NPCType<UnleashedSpirit>()] = true;
						break;
                    case BannerType.MistEater:
						player.NPCBannerBuff[NPCType<MistEater>()] = true;
						break;
                    case BannerType.MistDevor:
						player.NPCBannerBuff[NPCType<MistDevourer_Head>()] = true;
						break;
                    case BannerType.FallenCrimera:
						player.NPCBannerBuff[NPCType<FallenCrimera>()] = true;
                        break;
                    case BannerType.HMCrimson_UNUSED:
                        break;
                    case BannerType.SpectralBiter:
						player.NPCBannerBuff[NPCType<SpectralBitter>()] = true;
						break;
                    case BannerType.ShelledHorror:
						player.NPCBannerBuff[NPCType<ShelledHorror>()] = true;
                        break;
                    case BannerType.PHMOcean_UNUSED:
                        break;
                    case BannerType.SpectralShark:
						player.NPCBannerBuff[NPCType<SpectralShark>()] = true;
                        break;
                    case BannerType.Scuttlegeist:
						player.NPCBannerBuff[NPCType<Scuttlegeist>()] = true;
                        break;
                    case BannerType.EtherealRemitter:
						player.NPCBannerBuff[NPCType<EtherealRemitter>()] = true;
                        break;
                    case BannerType.Mistwraith:
						player.NPCBannerBuff[NPCType<Mistwraith>()] = true;
                        break;
                    case BannerType.PHMDesert_UNUSED:
                        break;
                    case BannerType.ShadowArtilery:
						player.NPCBannerBuff[NPCType<ShadowArtilery>()] = true;
                        break;
					case BannerType.BansheeHive:
						player.NPCBannerBuff[NPCType<BansheeHive>()] = true;
						break;
                }
				player.hasBanner = true;
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
	}

	enum BannerType
    {
		MarbleSlime,
		MountainSlime,
		SoulBoundSlime,
		Undying,
		UndyingArcher,
		UndyingNecro,
		UnleashedSpirit,
		MistEater,
		MistDevor,
		FallenCrimera,
		HMCrimson_UNUSED,
		SpectralBiter,
		ShelledHorror,
		PHMOcean_UNUSED,
		SpectralShark,
		Scuttlegeist,
		EtherealRemitter,
		Mistwraith,
		PHMDesert_UNUSED,
		ShadowArtilery,
		BansheeHive
    }
}
