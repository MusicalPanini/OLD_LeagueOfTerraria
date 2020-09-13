using IL.Terraria.Utilities;
using System;
using System.Collections.Generic;
using TerraLeague.Items.Accessories;
using TerraLeague.Items.Placeable;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items
{
    public class HextechChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hextech Chest");
            Tooltip.SetDefault("Right click to open with a Hextech Key");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            item.maxStack = 999;
            item.width = 34;
            item.height = 34;
            item.rare = ItemRarityID.LightRed;
            item.value = 50000;
        }

        public override bool CanRightClick()
        {
            if (Main.LocalPlayer.HasItem(ItemType<HextechKey>()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void RightClick(Player player)
        {
            bool hasDroppedSomething = false;
            player.ConsumeItem(ItemType<HextechKey>());

			// Ores
			if (Main.rand.NextFloat() <= 0.75f)
			{
                // Sunstone, Brass, Petricite | 3
                int upperLimit = 2;

                // Darksteel, Silversteel | 5
                if (NPC.downedBoss2)
                    upperLimit = 4;

                // Celestial, Hextech Core | 7
                if (Main.hardMode)
                    upperLimit = 6;

				//// Harmonic | 8
				//if (NPC.downedMechBossAny)
				//    upperLimit = 7;

				// Harmonic, Void | 9
				if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                    upperLimit = 8;

                // Celestial Fragment | 10
                if (NPC.downedGolemBoss)
                    upperLimit = 9;

                switch (Main.rand.Next(upperLimit))
                {
                    case 0:
                        player.QuickSpawnItem(ItemType<Sunstone>(), Main.rand.Next(3, 13));
                        break;
                    case 1:
                        player.QuickSpawnItem(ItemType<BrassBar>(), Main.rand.Next(6, 19));
                        break;
                    case 2:
                        player.QuickSpawnItem(ItemType<Petricite>(), Main.rand.Next(12, 21));
                        break;
                    case 3:
                        player.QuickSpawnItem(ItemType<DarksteelBar>(), Main.rand.Next(6, 19));
                        break;
                    case 4:
                        player.QuickSpawnItem(ItemType<SilversteelBar>(), Main.rand.Next(6, 19));
                        break;
                    case 5:
                        player.QuickSpawnItem(ItemType<CelestialBar>(), Main.rand.Next(6, 19));
                        break;
                    case 6:
                        player.QuickSpawnItem(ItemType<HextechCore>(), Main.rand.Next(2, 9));
                        break;
                    case 7:
                        player.QuickSpawnItem(ItemType<HarmonicBar>(), Main.rand.Next(6, 19));
                        break;
                    case 8:
                        player.QuickSpawnItem(ItemType<VoidBar>(), Main.rand.Next(6, 19));
                        break;
                    case 9:
                        player.QuickSpawnItem(ItemType<FragmentOfTheAspect>(), Main.rand.Next(1, 3));
                        break;
                    default:
                        break;
                }
                hasDroppedSomething = true;
            }

			// Chest
			if (Main.rand.NextFloat() <= 0.1f)
			{
                player.QuickSpawnItem(ItemType<HextechKey>());
                player.QuickSpawnItem(ItemType<HextechChest>());
            }

			if (Main.rand.NextFloat() <= 0.1f)
			{
				if (Main.rand.NextFloat() <= 0.666f)
				{
					player.QuickSpawnItem(ItemType<HexCrystal>());
				}
				else
				{
					List<int> items = new List<int>() { ItemType<FlashofBrilliance>(), ItemType<PulseBoots>(), ItemType<XrayGoggles>(), ItemType<ExtendoGloves>() };
					player.QuickSpawnItem(items[Main.rand.Next(items.Count)]);
				}
				hasDroppedSomething = true;
			}

			// Vanity
			if (!hasDroppedSomething || Main.rand.Next(10) == 0)
			{
				if (Main.rand.Next(10) == 0)
				{
					switch (Main.rand.Next(1))
					{
						case 0:
							{
								player.QuickSpawnItem(2856);
								player.QuickSpawnItem(2858);
								break;
							}
						default:
							break;
					}
				}
				else
				{
					switch (Main.rand.Next(19))
					{
						case 0:
							{
								player.QuickSpawnItem(1749);
								player.QuickSpawnItem(1750);
								player.QuickSpawnItem(1751);
								break;
							}
						case 1:
							{
								player.QuickSpawnItem(1746);
								player.QuickSpawnItem(1747);
								player.QuickSpawnItem(1748);
								break;
							}
						case 2:
							{
								player.QuickSpawnItem(1752);
								player.QuickSpawnItem(1753);
								break;
							}
						case 3:
							{
								player.QuickSpawnItem(1767);
								player.QuickSpawnItem(1768);
								player.QuickSpawnItem(1769);
								break;
							}
						case 4:
							{
								player.QuickSpawnItem(1770);
								player.QuickSpawnItem(1771);
								break;
							}
						case 5:
							{
								player.QuickSpawnItem(1772);
								player.QuickSpawnItem(1773);
								break;
							}
						case 6:
							{
								player.QuickSpawnItem(1754);
								player.QuickSpawnItem(1755);
								player.QuickSpawnItem(1756);
								break;
							}
						case 7:
							{
								player.QuickSpawnItem(1757);
								player.QuickSpawnItem(1758);
								player.QuickSpawnItem(1759);
								break;
							}
						case 8:
							{
								player.QuickSpawnItem(1760);
								player.QuickSpawnItem(1761);
								player.QuickSpawnItem(1762);
								break;
							}
						case 9:
							{
								player.QuickSpawnItem(1763);
								player.QuickSpawnItem(1764);
								player.QuickSpawnItem(1765);
								break;
							}
						case 10:
							{
								player.QuickSpawnItem(1766);
								player.QuickSpawnItem(1767);
								player.QuickSpawnItem(1768);
								break;
							}
						case 11:
							{
								player.QuickSpawnItem(1777);
								player.QuickSpawnItem(1778);
								break;
							}
						case 12:
							{
								player.QuickSpawnItem(1779);
								player.QuickSpawnItem(1780);
								player.QuickSpawnItem(1781);
								break;
							}
						case 13:
							{
								player.QuickSpawnItem(1819);
								player.QuickSpawnItem(1820);
								break;
							}
						case 14:
							{
								player.QuickSpawnItem(1821);
								player.QuickSpawnItem(1822);
								player.QuickSpawnItem(1823);
								break;
							}
						case 15:
							{
								player.QuickSpawnItem(1824);
								break;
							}
						case 16:
							{
								player.QuickSpawnItem(1838);
								player.QuickSpawnItem(1839);
								player.QuickSpawnItem(1840);
								break;
							}
						case 17:
							{
								player.QuickSpawnItem(1841);
								player.QuickSpawnItem(1842);
								player.QuickSpawnItem(1843);
								break;
							}
						case 18:
							{
								player.QuickSpawnItem(1851);
								player.QuickSpawnItem(1852);
								break;
							}
					}
				}
			}
			else
			{
				player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(2, 6));
			}

            base.RightClick(player);
        }
    }
}
