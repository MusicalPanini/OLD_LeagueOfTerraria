using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Items;
using TerraLeague.Items.AdvItems;
using TerraLeague.Items.BasicItems;
using TerraLeague.Items.SummonerSpells;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using TerraLeague.Tiles;
using TerraLeague.Walls;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague
{
    public class TerraLeagueWORLDGLOBAL : ModWorld
    {
        public static bool targonBossActive = false;
        static bool TargonArenaActive = false;
        static public bool TargonArenaDefeated = false;
        static int targonArenaWidth = 100;
        static int targonArenaHeight = 100;
        static Tile[,] TargonArenaSave = new Tile[100, 100];

        int[] floatingIslandHouse_XCord = new int[30];
        int[] floatingIslandHouse_YCord = new int[30];
        bool[] skyLake = new bool[30];

        int numIslandHouses = 0;
        int skyLakes = 1;

        internal WorldPacketHandler PacketHandler = ModNetHandler.worldHandler;

        public static bool BlackMistEvent = false;

        public static bool TargonOreSpawned = false;
        public static bool ManaOreSpawned = false;
        public static bool VoidOreSpawned = false;
        public static bool TargonUnlocked = false;
        public static bool CelestialMeteorCanSpawn = false;
        public static bool BlackMistDefeated = false;
        public static int marbleBlocks = 0;
        public static int targonMarker = 0;
        public int startingFrames = 0;

        public static int TargonCenterX = 0;
        public static int TargonWidthFromCenter = 0;


        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            TargonOreSpawned = false;
            ManaOreSpawned = false;
            VoidOreSpawned = false;
            CelestialMeteorCanSpawn = false;
            TargonUnlocked = false;
            TargonArenaDefeated = false;
            BlackMistDefeated = false;

            BlackMistEvent = false;
            TargonCenterX = 0;
            TargonWidthFromCenter = 0;

            //int TerrainIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Rock Layer Caves"));
            int TerrainIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Mount Caves"));
            if (TerrainIndex != -1)
            {
                tasks.Insert(TerrainIndex + 1, new PassLegacy("MountTargon", GenerateMountTargon));
            }

            int JungleIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Jungle"));
            if (TerrainIndex != -1)
            {
                tasks.Insert(JungleIndex + 1, new PassLegacy("ConvertTargon", ConvertMountTargon));
            }

            int ShinyIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShinyIndex != -1)
            {
                tasks.Insert(ShinyIndex + 1, new PassLegacy("Ferrospike", GenerateFerrospike));
                tasks.Insert(ShinyIndex + 1, new PassLegacy("TargonGranite", ConvertOreToTargon));
            }

            int MarbleIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Marble"));
            if (MarbleIndex != -1)
            {
                tasks.Insert(MarbleIndex + 1, new PassLegacy("Limestone", GenerateLimestone));
            }

            int genIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Spreading Grass"));
            if (genIndex == -1)
            {
                return;
            }
            tasks.Insert(genIndex + 1, new PassLegacy("Surface Marble", GenerateMarble));

            int end = tasks.FindIndex(genpass => genpass.Name.Equals("Final Cleanup"));
            if (genIndex == -1)
            {
                return;
            }
            tasks.Insert(end + 1, new PassLegacy("Surface Marble Walls", GenerateMarbleWall));

            int plant = tasks.FindIndex(genpass => genpass.Name.Equals("Flowers"));
            if (genIndex == -1)
            {
                return;
            }
            tasks.Insert(plant + 1, new PassLegacy("Sunstone Ore", GenerateMarblePlants));

            int desert = tasks.FindIndex(genpass => genpass.Name.Equals("Full Desert"));
            if (genIndex == -1)
            {
                return;
            }
            tasks.Insert(desert + 1, new PassLegacy("Surface Marble Plants", GenerateSunstone));

            int floatingIslands = tasks.FindIndex(genpass => genpass.Name.Equals("Floating Islands"));
            if (genIndex != -1)
            {
                tasks[floatingIslands] = new PassLegacy("Modified Floating Islands", GenerateFloatingIslands);
            }

            int floatingIslandHouse = tasks.FindIndex(genpass => genpass.Name.Equals("Floating Island Houses"));
            if (genIndex != -1)
            {
                tasks[floatingIslandHouse] = new PassLegacy("Modified Floating Island Houses", GenerateFloatingIslandHouse);
            }

            int microBiome = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
            if (genIndex == -1)
            {
                return;
            }
            tasks.Insert(microBiome + 1, new PassLegacy("Targon Arena", GenerateTargonArena));
        }

        private void GenerateFerrospike(GenerationProgress progress)
        {
            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 7E-05); k++)
            {
                int X = WorldGen.genRand.Next(0, Main.maxTilesX);
                int Y = WorldGen.genRand.Next((int)Main.maxTilesY / 2, Main.maxTilesY - 200);

                WorldGen.OreRunner(X, Y, WorldGen.genRand.Next(5, 8), WorldGen.genRand.Next(5, 11), (ushort)TileType<FerrospikeOre>());
            }
        }

        private void GenerateMarble(GenerationProgress progress)
        {
            bool biomeCreated = false;
            progress.Message = "Petrifing the surface";
            progress.Value += 0.01f;
            while (!biomeCreated)
            {
                int X = Main.rand.Next(2) == 0 ? WorldGen.genRand.Next((Main.maxTilesX * 1) / 5, (Main.maxTilesX * 2) / 5) : WorldGen.genRand.Next((Main.maxTilesX * 3) / 5, (Main.maxTilesX * 4) / 5);
                int biomeWidth = Main.rand.Next(Main.maxTilesX / 12, Main.maxTilesX / 10);
                int biomeDepth = Main.maxTilesY / 3;
                for (int x = X - biomeWidth / 2; x < X + biomeWidth / 2; x++)
                {
                    for (int y = 0; y < biomeDepth; y++)
                    {
                        Tile tile = Main.tile[x, y];
                        if (tile.type == TileID.Grass || tile.type == TileID.CorruptGrass || tile.type == TileID.FleshGrass)
                            Main.tile[x, y].type = (ushort)TileType<PetrifiedGrass>();
                        else if (tile.type == TileID.Stone)
                            Main.tile[x, y].type = TileID.Marble;
                        else if (tile.type == TileID.ClayBlock)
                            Main.tile[x, y].type = (ushort)TileType<Limestone>();
                    }
                }

                break;
            }
        }

        private void GenerateLimestone(GenerationProgress progress)
        {
            progress.Message = "You put the lime in the marble and you shake it all up";
            progress.Value += 0.01f;

            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 100E-05); k++)
            {
                int X = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                int Y = WorldGen.genRand.Next((int)Main.maxTilesY / 3, Main.maxTilesY - 200);

                bool marbleNearby = false;

                for (int xm = X - 5; xm < X + 6; xm++)
                {
                    for (int ym = Y - 5; ym < Y + 6; ym++)
                    {
                        if (Main.tile[xm, ym].type == TileID.Marble)
                        {
                            marbleNearby = true;
                        }
                    }
                }
                if (marbleNearby)
                {
                    WorldGen.OreRunner(X, Y, WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(15, 45), (ushort)TileType<Limestone>());
                }

            }

            #region old code
            //for (int x = 0; x < Main.maxTilesX; x++)   //40E-05 is how many veins ore is going to spawn , change 40 to a lover value if you want less vains ore or higher value for more veins ore
            //{
            //    for (int y = (int)Main.rockLayer; y < Main.maxTilesY; y++)
            //    {
            //        if(Main.tile[x,y].type == TileID.Marble)
            //        {
            //            int convertChance = 0;

            //            for (int xm = x - 2; xm < x + 3; xm++)
            //            {
            //                for (int ym = x - 2; ym < x + 3; ym++)
            //                {
            //                    if (Main.tile[x, y].type == TileType("Limestone"))
            //                    {
            //                        convertChance++;
            //                    }
            //                }
            //            }

            //            int adjustedChance = (int)((-7.0 / 144) * Math.Pow(convertChance - 12, 2) + 8) * 10;

            //            if (Main.rand.Next(adjustedChance, 100) == adjustedChance)
            //            {
            //                Main.tile[x, y].type = (ushort)TileType("Limestone");
            //                Main.tile[x, y].type = 472;
            //            }
            //        }
            //    }
            //}
            #endregion
        }

        private void GenerateSunstone(GenerationProgress progress)
        {
            progress.Message = "Adding warmth to the desert";
            progress.Value += 0.01f;

            for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 100E-05); k++)
            {
                int x = WorldGen.genRand.Next(10, Main.maxTilesX - 10);
                int y = WorldGen.genRand.Next((int)Main.maxTilesY / 3, Main.maxTilesY - 200);
                Tile tile = Main.tile[x, y];
                if (tile.type == TileID.Sandstone || tile.type == TileID.HardenedSand)
                    Main.tile[x, y].type = (ushort)TileType<SunstoneOre>();

            }
        }

        private void GenerateMarbleWall(GenerationProgress progress)
        {
            progress.Message = "Petrifing some more";
            progress.Value += 0.01f;
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY / 2; y++)
                {
                    if (Main.tile[x, y].type == TileID.Marble)
                    {
                        for (int xw = -1; xw < 2; xw++)
                        {
                            for (int yw = -1; yw < 2; yw++)
                            {
                                if (
                                    Main.tile[x + xw, y + yw].wall == WallID.GrassUnsafe ||
                                    Main.tile[x + xw, y + yw].wall == WallID.CorruptGrassUnsafe ||
                                    Main.tile[x + xw, y + yw].wall == WallID.FlowerUnsafe ||
                                    Main.tile[x + xw, y + yw].wall == WallID.CrimsonGrassUnsafe)
                                {
                                    if (Main.tile[x + xw, y + yw].active())
                                        Main.tile[x + xw, y + yw].wall = WallID.MarbleUnsafe;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GenerateMarblePlants(GenerationProgress progress)
        {
            progress.Message = "Petrifing the plants";
            progress.Value += 0.01f;
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY / 2; y++)
                {
                    if (Main.tile[x, y].type == TileType<PetrifiedGrass>())
                    {
                        if (!Framing.GetTileSafely(x, y - 1).active() && Main.rand.Next(3) == 0)
                        {
                            int style = Main.rand.Next(22);
                            if (WorldGen.PlaceObject(x, y - 1, TileType<PetrifiedFlora>(), false, style))
                                NetMessage.SendObjectPlacment(-1, x, y - 1, TileType<PetrifiedFlora>(), style, 0, -1, -1);
                        }
                    }
                }
            }
        }

        private void GenerateMountTargon(GenerationProgress progress)
        {
            Vector2 leftBottom = Vector2.Zero;
            Vector2 rightBottom = Vector2.Zero;

            progress.Message = "Reaching the heavens";
            progress.Value += 0.01f;
            int xCord = Main.rand.Next(2) == 0 ? Main.rand.Next(600, Main.maxTilesX / 3) : Main.rand.Next((Main.maxTilesX * 2) / 3, Main.maxTilesX - 600);
            int distanceFromGroundToSky = 75; // k
            int displacementFromTop = 50;
            for (int i = 0; i < Main.maxTilesY; i++)
            {
                if (Main.tile[xCord, i].active())
                {
                    distanceFromGroundToSky = i - 20;
                    break;
                }
            }

            int width = Main.maxTilesX / 21; // w
            float height = (float)Math.Pow(distanceFromGroundToSky, (1d / -width)); // h;

            float baseNoise = 0.3f * (width + height) / 2f; // N
            float steepness = 0.005f * (width + height) / 2f; // s

            int CenterHieght = (int)(Math.Pow(height, (steepness * Math.Abs(0)) - width) + CreateNoise(0, width, baseNoise));
            int YPeak = CenterHieght + displacementFromTop;

            for (int X = -width * 2 / 3; X < width * 2 / 3; X++)
            {
                float modifideNoise = (float)Math.Pow(baseNoise, -Math.Abs(X / (width * 3f)) * Main.rand.NextFloat(5, 10) + 0.9); // n

                int mountStart = (int)(-Math.Pow(height, (steepness * Math.Abs(X)) - width) + CreateNoise(X, width, modifideNoise)) + YPeak; // y

                for (int Y = 50; Y < (int)Main.worldSurface; Y++)
                {
                    if (Y >= mountStart)
                    {
                        Main.tile[xCord + X, Y].type = (ushort)TileID.Dirt;
                        //Main.tile[xCord + X, Y].wall = (ushort)WallID.StoneSlab;
                        Main.tile[xCord + X, Y].active(true);
                        Main.tile[xCord + X, Y].slope(0);
                    }
                }
            }

            float distFromMid = 0.3f;

            for (int y = (int)(Main.worldSurface * 0.35); y < Main.worldSurface; y++)
            {
                if (Main.tile[(int)(xCord - width * distFromMid), y].active())
                {
                    leftBottom = new Vector2(xCord - (width * distFromMid), y);
                    break;
                }
            }
            for (int y = (int)(Main.worldSurface * 0.35); y < Main.worldSurface; y++)
            {
                if (Main.tile[(int)(xCord - width * distFromMid), y].active())
                {
                    rightBottom = new Vector2(xCord + (width * distFromMid), y);
                    break;
                }
            }

            if (leftBottom != Vector2.Zero && rightBottom != Vector2.Zero)
            {
                Vector2 digVelocity = rightBottom - leftBottom; // Vector2.UnitX;
                digVelocity = digVelocity.SafeNormalize(Vector2.UnitX) * 2f;
                WorldGen.digTunnel(leftBottom.X, leftBottom.Y, digVelocity.X, digVelocity.Y, width/2, 3);

                digVelocity = leftBottom - rightBottom; // Vector2.UnitX;
                digVelocity = digVelocity.SafeNormalize(Vector2.UnitX) * 2f;
                WorldGen.digTunnel(rightBottom.X, rightBottom.Y, digVelocity.X, digVelocity.Y, width/2, 3);
            }

            TargonCenterX = xCord;
            TargonWidthFromCenter = width;
        }

        private void ConvertMountTargon(GenerationProgress progress)
        {
            progress.Message = "Blessing the earth";
            progress.Value += 0.01f;

            for (int x = TargonCenterX - TargonWidthFromCenter; x < TargonCenterX + TargonWidthFromCenter; x++)
            {
                for (int y = 0; y < (Main.worldSurface * 0.5) + 50; y++)
                {
                    Tile tile = Main.tile[x, y];

                    if (y < (Main.worldSurface * 0.35) + 50)
                    {
                        if (y < (Main.worldSurface * 0.35))
                        {
                            if (tile.active())
                            {
                                Main.tile[x, y].type = (ushort)TileType<TargonStone>();
                                if (Main.tile[x - 1, y].active() && Main.tile[x - 1, y - 1].active() && Main.tile[x, y - 1].active() && Main.tile[x + 1, y - 1].active() && Main.tile[x + 1, y].active() && Main.tile[x + 1, y + 1].active() && Main.tile[x, y + 1].active() && Main.tile[x - 1, y + 1].active())
                                    Main.tile[x, y].wall = (ushort)WallType<TargonStoneWall>();
                            }
                        }
                        else if (Main.rand.NextFloat() > (y - (Main.worldSurface * 0.35)) / 50f)
                        {
                            if (Main.tile[x, y].active())
                            {
                                Main.tile[x, y].type = (ushort)TileType<TargonStone>();
                                if (Main.tile[x - 1, y].active() && Main.tile[x - 1, y - 1].active() && Main.tile[x, y - 1].active() && Main.tile[x + 1, y - 1].active() && Main.tile[x + 1, y].active() && Main.tile[x + 1, y + 1].active() && Main.tile[x, y + 1].active() && Main.tile[x - 1, y + 1].active())
                                    Main.tile[x, y].wall = (ushort)WallType<TargonStoneWall>();
                            }
                        }
                        else
                        {
                            if (Main.tile[x, y].active())
                            {
                                Main.tile[x, y].type = TileID.SnowBlock;
                                if (Main.tile[x - 1, y].active() && Main.tile[x - 1, y - 1].active() && Main.tile[x, y - 1].active() && Main.tile[x + 1, y - 1].active() && Main.tile[x + 1, y].active() && Main.tile[x + 1, y + 1].active() && Main.tile[x, y + 1].active() && Main.tile[x - 1, y + 1].active())
                                    Main.tile[x, y].wall = WallID.SnowWallUnsafe;
                            }
                        }
                    }
                    else
                    {
                        if (y < (Main.worldSurface * 0.5))
                        {
                            if (Main.tile[x, y].active())
                            {
                                Main.tile[x, y].type = TileID.SnowBlock;
                                if (Main.tile[x - 1, y].active() && Main.tile[x - 1, y - 1].active() && Main.tile[x, y - 1].active() && Main.tile[x + 1, y - 1].active() && Main.tile[x + 1, y].active() && Main.tile[x + 1, y + 1].active() && Main.tile[x, y + 1].active() && Main.tile[x - 1, y + 1].active())
                                    Main.tile[x, y].wall = WallID.SnowWallUnsafe;
                            }
                        }
                        else if (Main.rand.NextFloat() > (y - (Main.worldSurface * 0.5)) / 50f)
                        {
                            if (Main.tile[x, y].active())
                            {
                                Main.tile[x, y].type = TileID.SnowBlock;
                                if (Main.tile[x - 1, y].active() && Main.tile[x - 1, y - 1].active() && Main.tile[x, y - 1].active() && Main.tile[x + 1, y - 1].active() && Main.tile[x + 1, y].active() && Main.tile[x + 1, y + 1].active() && Main.tile[x, y + 1].active() && Main.tile[x - 1, y + 1].active())
                                    Main.tile[x, y].wall = WallID.SnowWallUnsafe;
                            }
                        }
                    }
                }
            }
        }

        private void ConvertOreToTargon(GenerationProgress progress)
        {
            progress.Message = "Blessing the ore";
            progress.Value += 0.01f;

            for (int x = TargonCenterX - TargonWidthFromCenter; x < TargonCenterX + TargonWidthFromCenter; x++)
            {
                for (int y = 0; y < (Main.worldSurface); y++)
                {
                    Tile tile = Main.tile[x, y];
                    if (tile.type == TileID.Silver || tile.type == TileID.Tungsten || tile.type == TileID.Gold || tile.type == TileID.Platinum)
                    {
                        Main.tile[x, y].type = (ushort)TileType<TargonGranite>();
                    }
                }
            }
        }

        float CreateNoise(int x, int width, float noise)
        {
            return (float)(Math.Sin(x * (1 - (Math.Abs(x) / (2 * width)))) * (noise - ((noise * Math.Abs(x)) / (width * 2))));
        }

        public override void PostWorldGen()
        {
            // Place some items in Wood Chests
            int[] itemsToPlaceInWoodChests = new int[] { ItemType<LongSword>(), ItemType<Dagger>(), ItemType<AmpTome>(), ItemType<RubyCrystal>(), ItemType<SapphireCrystal>(), ItemType<NullMagic>(), ItemType<ClothArmor>() };
            int itemsToPlaceInWoodChestsChoice = 0;
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 0 * 36 && Main.rand.Next(0, 4) == 0)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(itemsToPlaceInWoodChests[itemsToPlaceInWoodChestsChoice]);
                            chest.item[inventoryIndex].Prefix(-1);
                            itemsToPlaceInWoodChestsChoice = (itemsToPlaceInWoodChestsChoice + 1) % itemsToPlaceInWoodChests.Length;
                            break;
                        }
                    }
                }
            }

            // Place some items in Gold Chests
            int[] itemsToPlaceInGoldChests = new int[] { ItemType<BFSword>(), ItemType<BlastingWand>(), ItemType<BrawlersGlove>(), ItemType<ChainVest>(), ItemType<NegatronCloak>(), ItemType<ForbiddenIdol>() };
            int itemsToPlaceInGoldChestsChoice = 0;
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && (Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 1 * 36 || Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 8 * 36 || Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 11 * 36 || Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 50 * 36 || Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 51 * 36) && Main.rand.Next(0, 5) == 0)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(itemsToPlaceInGoldChests[itemsToPlaceInGoldChestsChoice]);
                            chest.item[inventoryIndex].Prefix(-1);
                            itemsToPlaceInGoldChestsChoice = (itemsToPlaceInGoldChestsChoice + 1) % itemsToPlaceInGoldChests.Length;
                            break;
                        }
                    }
                }
            }

            // Place Celestial Bars in floating island chests
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 13 * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(ItemType<CelestialBar>());
                            chest.item[inventoryIndex].stack = Main.rand.Next(6, 13);
                            break;
                        }
                    }
                }
            }

            // Place Brass Bars in water chests
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 17 * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(ItemType<BrassBar>());
                            chest.item[inventoryIndex].stack = Main.rand.Next(4, 9);
                            break;
                        }
                    }
                }
            }

            // Place Vials of Raw Magic in random chests
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers)
                {
                    if (Main.rand.Next(3) == 0)
                    {
                        for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                        {
                            if (chest.item[inventoryIndex].type == ItemID.None)
                            {
                                chest.item[inventoryIndex].SetDefaults(ItemType<VialofTrueMagic>());
                                chest.item[inventoryIndex].stack = Main.rand.Next(1, 4);
                                break;
                            }
                        }
                    }
                }
            }
        }

        public override TagCompound Save()
        {
            var OreSpawned = new List<string>();
            if (Main.hardMode) OreSpawned.Add("TargonOreSpawned");
            if (NPC.downedBoss2) OreSpawned.Add("ManaOreSpawned");
            if (NPC.downedBoss3) OreSpawned.Add("VoidOreSpawned");
            if (NPC.downedGolemBoss) OreSpawned.Add("CelestialMeteorCanSpawn");
            if (TargonUnlocked) OreSpawned.Add("TargonUnlockedSpawned");
            if (TargonArenaDefeated) OreSpawned.Add("TargonArena");
            if (BlackMistDefeated) OreSpawned.Add("BlackMistDefeated");

            return new TagCompound {
                {"OreSpawned", OreSpawned},
                {"BlackMistEvent", BlackMistEvent},
                {"TargonXCord", TargonCenterX},
                {"TargonWidth", TargonWidthFromCenter }
            };
        }

        public override void Load(TagCompound tag)
        {
            var OreSpawned = tag.GetList<string>("OreSpawned");
            TargonOreSpawned = OreSpawned.Contains("TargonOreSpawned");
            ManaOreSpawned = OreSpawned.Contains("ManaOreSpawned");
            VoidOreSpawned = OreSpawned.Contains("VoidOreSpawned");
            CelestialMeteorCanSpawn = OreSpawned.Contains("CelestialMeteorCanSpawn");
            TargonUnlocked = OreSpawned.Contains("TargonUnlockedSpawned");
            TargonArenaDefeated = OreSpawned.Contains("TargonArena");
            BlackMistDefeated = OreSpawned.Contains("BlackMistDefeated");

            BlackMistEvent = tag.GetBool("BlackMistEvent");
            TargonCenterX = tag.GetInt("TargonXCord");
            TargonWidthFromCenter = tag.GetInt("TargonWidth");
            
            if (TargonCenterX != 0 && Main.netMode != NetmodeID.MultiplayerClient && NPC.CountNPCS(NPCType<NPCs.TargonSigil>()) == 0)
                NPC.NewNPC(TargonCenterX * 16, 45 * 16, NPCType<TargonSigil>());
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = BlackMistEvent;
            flags[1] = TargonArenaDefeated;
            flags[2] = BlackMistDefeated;
            flags[3] = TargonUnlocked;
            writer.Write(flags);
            writer.Write(TargonCenterX);
            writer.Write(TargonWidthFromCenter);
            base.NetSend(writer);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            BlackMistEvent = flags[0];
            TargonArenaDefeated = flags[1];
            BlackMistDefeated = flags[2];
            TargonUnlocked = flags[3];
            TargonCenterX = reader.ReadInt32();
            TargonWidthFromCenter = reader.ReadInt32();
            base.NetReceive(reader);
        }

        public override void PostUpdate()
        {
            ToggleTargonArena();
            targonBossActive = false;

            if (!Main.dayTime && Main.time == 1 && !Main.bloodMoon && Main.netMode != NetmodeID.MultiplayerClient)
            {
                for (int i = 0; i < Main.player.Length; i++)
                {
                    if (Main.player[i].active)
                    {
                        if (Main.player[i].GetModPlayer<PLAYERGLOBAL>().GetRealHeathWithoutShield(true) >= 200)
                        {
                            if (Main.rand.Next(0, Main.moonPhase == 4 ? 4 : 12) == 0)
                            {
                                BlackMistEvent = true;
                                if (Main.netMode == NetmodeID.SinglePlayer)
                                    Main.NewText("The Harrowing has begun...", new Color(0, 255, 125));
                                else if (Main.netMode == NetmodeID.Server)
                                {
                                    NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("The Harrowing has begun..."), new Color(50, 255, 130), -1);
                                    //NetMessage.SendData(MessageID.WorldData);
                                    //NetSend(new BinaryWriter(mod.GetPacket().BaseStream));
                                    PacketHandler.SendBlackMist(-1, -1, BlackMistEvent);
                                }
                                break;
                            }
                        }
                    }
                }
            }
            if (Main.dayTime && BlackMistEvent && Main.netMode != NetmodeID.MultiplayerClient)
            {
                BlackMistEvent = false;
                BlackMistDefeated = true;
                //NetMessage.SendData(MessageID.WorldData);
                //NetSend(new BinaryWriter(mod.GetPacket().BaseStream));
                if (Main.netMode == NetmodeID.Server)
                    PacketHandler.SendBlackMist(-1, -1, BlackMistEvent);
            }

            //if (Main.hardMode)
            //{
            //    if (!TargonOreSpawned)
            //    {
            //        DropTargon();
            //    }
            //}

            if (NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || NPC.downedQueenBee || Main.hardMode)
            {
                if (!TargonUnlocked)
                {
                    if (TargonCenterX != 0)
                    {
                        TargonUnlocked = true;

                        if (Main.netMode == NetmodeID.SinglePlayer)
                        {
                            Main.NewText("You have attracted the attention of The Celestials.", 0, 200, 255);
                            Main.NewText("You can now scale Mount Targon without taking damage.", 0, 200, 255);
                            Main.NewText("Climb to the highest peak to accept their challenge.", 0, 200, 255);
                        }
                        else if (Main.netMode == NetmodeID.Server)
                        {
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("You have attracted the attention of The Celestials."), new Color(0, 200, 255), -1);
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("You can now scale Mount Targon without taking damage!"), new Color(0, 200, 255), -1);
                            NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("Climb to the highest peak to accept their challenge."), new Color(0, 200, 255), -1);
                            NetMessage.SendData(MessageID.WorldData);
                        }
                    }
                }
            }

            if (NPC.downedBoss2)
            {
                if (!ManaOreSpawned)
                {
                    ManaOreSpawned = true;

                    if (Main.netMode == NetmodeID.SinglePlayer)
                        Main.NewText("The Evil is no longer suppressing the magic in the jungle", 0, 130, 255);
                    else if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("The Evil is no longer suppressing the magic in the jungle"), new Color(0, 130, 255), -1);
                        NetMessage.SendData(MessageID.WorldData);
                    }


                    for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 50E-05); k++)
                    {
                        int X = WorldGen.genRand.Next(0, Main.maxTilesX);
                        int Y = WorldGen.genRand.Next((int)Main.maxTilesY / 3, Main.maxTilesY - 200);

                        if (Main.tile[X, Y].type == TileID.Mud)
                        {
                            WorldGen.OreRunner(X, Y, WorldGen.genRand.Next(4, 5), WorldGen.genRand.Next(3, 8), (ushort)TileType<ManaStone>());
                        }
                    }
                }
            }

            if (NPC.downedBoss3)
            {
                if (!VoidOreSpawned)
                {
                    VoidOreSpawned = true;

                    if (Main.netMode == NetmodeID.SinglePlayer)
                        Main.NewText("The Void has morphed some of this worlds matter", 255, 0, 255);
                    else if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("The Void has morphed some of this worlds matter"), new Color(255, 0, 255), -1);
                        NetMessage.SendData(MessageID.WorldData);
                    }

                    for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 0.000005); k++)
                    {
                        int X = WorldGen.genRand.Next(0, Main.maxTilesX);
                        int Y = WorldGen.genRand.Next((int)(Main.rockLayer * 1.5), Main.maxTilesY - 200);
                        WorldGen.OreRunner(X, Y, WorldGen.genRand.Next(13, 18), WorldGen.genRand.Next(6, 8), (ushort)TileType<Tiles.VoidFragment>());
                    }
                }
            }

            if (NPC.downedGolemBoss)
            {
                if (!CelestialMeteorCanSpawn)
                {
                    CelestialMeteorCanSpawn = true;

                    if (Main.netMode == NetmodeID.SinglePlayer)
                        Main.NewText("While the Sun and Moon are one, The Celestials will rain gifts of power", 0, 200, 255);
                    else if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("While the Sun and Moon are one, The Celestials will rain gifts of power"), new Color(0, 200, 255), -1);
                        NetMessage.SendData(MessageID.WorldData);
                    }
                }
            }

            if (Main.eclipse && Main.worldRate != 0 && CelestialMeteorCanSpawn)
            {
                if (Main.dayTime)
                {
                    if (Main.time == 27000 || Main.rand.Next(54000) == 0)
                    {
                        int num140 = Main.rand.Next(Main.maxTilesX - 50) + 100;
                        num140 *= 16;
                        int num141 = Main.rand.Next((int)((double)Main.maxTilesY * 0.05));
                        num141 *= 16;
                        Vector2 vector = new Vector2((float)num140, (float)num141);
                        float num142 = (float)Main.rand.Next(-100, 101);
                        float num143 = (float)(Main.rand.Next(200) + 100);
                        float num144 = (float)Math.Sqrt((double)(num142 * num142 + num143 * num143));
                        num144 = 12f / num144;
                        num142 *= num144;
                        num143 *= num144;
                        Projectile.NewProjectile(vector.X, vector.Y, num142, num143, ProjectileType<World_CelestialMeteorite>(), 10000, 10f, Main.myPlayer, 0f, 0f);
                    }
                }
            }

            base.PostUpdate();
        }

        public override void ResetNearbyTileEffects()
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.zoneTargonMonolith = false;
            marbleBlocks = 0;
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            marbleBlocks = tileCounts[TileID.Marble] + tileCounts[TileType<PetrifiedGrass>()] + tileCounts[TileType<PetrifiedFlora>()] + tileCounts[TileType<Limestone>()];
        }

        public void DropTargon()
        {
            bool flag = true;
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                return;
            }
            for (int i = 0; i < 255; i++)
            {
                if (Main.player[i].active)
                {
                    flag = false;
                    break;
                }
            }
            int num = 0;
            float num2 = (float)(Main.maxTilesX / 4200);
            int num3 = (int)(400f * num2);
            for (int j = 5; j < Main.maxTilesX - 5; j++)
            {
                int num4 = 5;
                while ((double)num4 < Main.worldSurface)
                {
                    if (Main.tile[j, num4].active() && Main.tile[j, num4].type == (ushort)TileType<TargonGranite>())
                    {
                        num++;
                        if (num > num3)
                        {
                            return;
                        }
                    }
                    num4++;
                }
            }
            float num5 = 600f;
            while (!flag)
            {
                float num6 = (float)Main.maxTilesX * 0.08f;
                int num7 = Main.rand.Next(150, Main.maxTilesX - 150);
                while ((float)num7 > (float)Main.spawnTileX - num6 && (float)num7 < (float)Main.spawnTileX + num6)
                {
                    num7 = Main.rand.Next(150, Main.maxTilesX - 150);
                }
                int k = (int)(Main.worldSurface * 0.3);
                while (k < Main.maxTilesY)
                {
                    if (Main.tile[num7, k].active() && Main.tileSolid[(int)Main.tile[num7, k].type])
                    {
                        int num8 = 0;
                        int num9 = 15;
                        for (int l = num7 - num9; l < num7 + num9; l++)
                        {
                            for (int m = k - num9; m < k + num9; m++)
                            {
                                if (WorldGen.SolidTile(l, m))
                                {
                                    num8++;
                                    if (Main.tile[l, m].type == 189 || Main.tile[l, m].type == 202)
                                    {
                                        num8 -= 100;
                                    }
                                }
                                else if (Main.tile[l, m].liquid > 0)
                                {
                                    num8--;
                                }
                            }
                        }
                        if ((float)num8 < num5)
                        {
                            num5 -= 0.5f;
                            break;
                        }
                        flag = GenerateTargon(num7, k);
                        if (flag)
                        {
                            break;
                        }
                        break;
                    }
                    else
                    {
                        k++;
                    }
                }
                if (num5 < 100f)
                {
                    return;
                }
            }
        }

        public bool GenerateTargon(int i, int j)
        {
            ushort TargonID = (ushort)TileType<TargonGranite>();

            if (i < 50 || i > Main.maxTilesX - 50)
            {
                return false;
            }
            if (j < 50 || j > Main.maxTilesY - 50)
            {
                return false;
            }
            int num = 35;
            Rectangle rectangle = new Rectangle((i - num) * 16, (j - num) * 16, num * 2 * 16, num * 2 * 16);
            for (int k = 0; k < 255; k++)
            {
                if (Main.player[k].active)
                {
                    Rectangle value = new Rectangle((int)(Main.player[k].position.X + (float)(Main.player[k].width / 2) - (float)(NPC.sWidth / 2) - (float)NPC.safeRangeX), (int)(Main.player[k].position.Y + (float)(Main.player[k].height / 2) - (float)(NPC.sHeight / 2) - (float)NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
                    if (rectangle.Intersects(value))
                    {
                        return false;
                    }
                }
            }
            for (int l = 0; l < 200; l++)
            {
                if (Main.npc[l].active)
                {
                    Rectangle value2 = new Rectangle((int)Main.npc[l].position.X, (int)Main.npc[l].position.Y, Main.npc[l].width, Main.npc[l].height);
                    if (rectangle.Intersects(value2))
                    {
                        return false;
                    }
                }
            }
            for (int m = i - num; m < i + num; m++)
            {
                for (int n = j - num; n < j + num; n++)
                {
                    if (Main.tile[m, n].active() && TileID.Sets.BasicChest[(int)Main.tile[m, n].type])
                    {
                        return false;
                    }
                }
            }

            num = WorldGen.genRand.Next(17, 23);
            for (int num2 = i - num; num2 < i + num; num2++)
            {
                for (int num3 = j - num; num3 < j + num; num3++)
                {
                    if (num3 > j + Main.rand.Next(-2, 3) - 5)
                    {
                        float num4 = (float)Math.Abs(i - num2);
                        float num5 = (float)Math.Abs(j - num3);
                        if ((double)((float)Math.Sqrt((double)(num4 * num4 + num5 * num5))) < (double)num * 0.9 + (double)Main.rand.Next(-4, 5))
                        {
                            if (!Main.tileSolid[(int)Main.tile[num2, num3].type])
                            {
                                Main.tile[num2, num3].active(false);
                            }
                            Main.tile[num2, num3].type = TargonID;
                        }
                    }
                }
            }
            num = WorldGen.genRand.Next(8, 14);
            for (int num6 = i - num; num6 < i + num; num6++)
            {
                for (int num7 = j - num; num7 < j + num; num7++)
                {
                    if (num7 > j + Main.rand.Next(-2, 3) - 4)
                    {
                        float num8 = (float)Math.Abs(i - num6);
                        float num9 = (float)Math.Abs(j - num7);
                        if ((double)((float)Math.Sqrt((double)(num8 * num8 + num9 * num9))) < (double)num * 0.8 + (double)Main.rand.Next(-3, 4))
                        {
                            Main.tile[num6, num7].active(false);
                        }
                    }
                }
            }
            num = WorldGen.genRand.Next(25, 35);
            for (int num10 = i - num; num10 < i + num; num10++)
            {
                for (int num11 = j - num; num11 < j + num; num11++)
                {
                    float num12 = (float)Math.Abs(i - num10);
                    float num13 = (float)Math.Abs(j - num11);
                    if ((double)((float)Math.Sqrt((double)(num12 * num12 + num13 * num13))) < (double)num * 0.7)
                    {
                        if (Main.tile[num10, num11].type == 5 || Main.tile[num10, num11].type == 32 || Main.tile[num10, num11].type == 352)
                        {
                            WorldGen.KillTile(num10, num11, false, false, false);
                        }
                        Main.tile[num10, num11].liquid = 0;
                    }
                    if (Main.tile[num10, num11].type == TargonID)
                    {
                        if (!WorldGen.SolidTile(num10 - 1, num11) && !WorldGen.SolidTile(num10 + 1, num11) && !WorldGen.SolidTile(num10, num11 - 1) && !WorldGen.SolidTile(num10, num11 + 1))
                        {
                            Main.tile[num10, num11].active(false);
                        }
                        else if ((Main.tile[num10, num11].halfBrick() || Main.tile[num10 - 1, num11].topSlope()) && !WorldGen.SolidTile(num10, num11 + 1))
                        {
                            Main.tile[num10, num11].active(false);
                        }
                    }
                    WorldGen.SquareTileFrame(num10, num11, true);
                    WorldGen.SquareWallFrame(num10, num11, true);
                }
            }
            num = WorldGen.genRand.Next(23, 32);
            for (int num14 = i - num; num14 < i + num; num14++)
            {
                for (int num15 = j - num; num15 < j + num; num15++)
                {
                    if (num15 > j + WorldGen.genRand.Next(-3, 4) - 3 && Main.tile[num14, num15].active() && Main.rand.Next(10) == 0)
                    {
                        float num16 = (float)Math.Abs(i - num14);
                        float num17 = (float)Math.Abs(j - num15);
                        if ((double)((float)Math.Sqrt((double)(num16 * num16 + num17 * num17))) < (double)num * 0.8)
                        {
                            if (Main.tile[num14, num15].type == 5 || Main.tile[num14, num15].type == 32 || Main.tile[num14, num15].type == 352)
                            {
                                WorldGen.KillTile(num14, num15, false, false, false);
                            }
                            Main.tile[num14, num15].type = TargonID;
                            WorldGen.SquareTileFrame(num14, num15, true);
                        }
                    }
                }
            }
            num = WorldGen.genRand.Next(30, 38);
            for (int num18 = i - num; num18 < i + num; num18++)
            {
                for (int num19 = j - num; num19 < j + num; num19++)
                {
                    if (num19 > j + WorldGen.genRand.Next(-2, 3) && Main.tile[num18, num19].active() && Main.rand.Next(20) == 0)
                    {
                        float num20 = (float)Math.Abs(i - num18);
                        float num21 = (float)Math.Abs(j - num19);
                        if ((double)((float)Math.Sqrt((double)(num20 * num20 + num21 * num21))) < (double)num * 0.85)
                        {
                            if (Main.tile[num18, num19].type == 5 || Main.tile[num18, num19].type == 32 || Main.tile[num18, num19].type == 352)
                            {
                                WorldGen.KillTile(num18, num19, false, false, false);
                            }
                            Main.tile[num18, num19].type = TargonID;
                            WorldGen.SquareTileFrame(num18, num19, true);
                        }
                    }
                }
            }

            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText("The Celestials are pleased. You are now immune to Celesital Frostbite and a gift has been dropped from the heavens", 0, 200, 255);
            }
            else if (Main.netMode == NetmodeID.Server)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromKey("The Celestials are pleased. You are now immune to Celesital Frostbite and a gift has been dropped from the heavens", new object[0]), new Color(0, 200, 255), -1);
                NetMessage.SendData(MessageID.WorldData);
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(-1, i, j, 40, TileChangeType.None);
            }
            TargonOreSpawned = true;
            return true;
        }

        private void GenerateFloatingIslands(GenerationProgress progress)
        {
            floatingIslandHouse_XCord = new int[30];
            floatingIslandHouse_YCord = new int[30];
            skyLake = new bool[30];

            numIslandHouses = 0;
            skyLakes = 1;

            if (Main.maxTilesX > 8000)
            {
                skyLakes++;
            }
            if (Main.maxTilesX > 6000)
            {
                skyLakes++;
            }

            progress.Message = Lang.gen[12].Value;
            for (int num591 = 0; num591 < (int)((double)Main.maxTilesX * 0.0008) + skyLakes; num591++)
            {
                int num592 = 1000;
                int num593 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.1), (int)((double)Main.maxTilesX * 0.9));
                while (--num592 > 0)
                {
                    bool flag45 = true;
                    while (num593 > Main.maxTilesX / 2 - 80 && num593 < Main.maxTilesX / 2 + 80)
                    {
                        num593 = WorldGen.genRand.Next((int)((double)Main.maxTilesX * 0.1), (int)((double)Main.maxTilesX * 0.9));
                    }
                    int num594 = 0;
                    while (num594 < numIslandHouses)
                    {
                        if ((num593 <= floatingIslandHouse_XCord[num594] - 180 || num593 >= floatingIslandHouse_XCord[num594] + 180) && (num593 <= TargonCenterX - 180 || num593 >= TargonCenterX - 180))
                        {
                            num594++;
                            continue;
                        }
                        flag45 = false;
                        break;
                    }
                    if (flag45)
                    {
                        flag45 = false;
                        int num595 = 0;
                        int num596 = 200;
                        while ((double)num596 < Main.worldSurface)
                        {
                            if (!Main.tile[num593, num596].active())
                            {
                                num596++;
                                continue;
                            }
                            num595 = num596;
                            flag45 = true;
                            break;
                        }
                        if (flag45)
                        {
                            int val = WorldGen.genRand.Next(90, num595 - 100);
                            val = Math.Min(val, (int)WorldGen.worldSurfaceLow - 50);
                            if (num591 < skyLakes)
                            {
                                skyLake[numIslandHouses] = true;
                                WorldGen.CloudLake(num593, val);
                            }
                            else
                            {
                                WorldGen.CloudIsland(num593, val);
                            }
                            floatingIslandHouse_XCord[numIslandHouses] = num593;
                            floatingIslandHouse_YCord[numIslandHouses] = val;
                            numIslandHouses++;
                        }
                    }
                }
            }
        }

        private void GenerateFloatingIslandHouse(GenerationProgress progress)
        {
            for (int i = 0; i < numIslandHouses; i++)
            {
                if (!skyLake[i])
                {
                    WorldGen.IslandHouse(floatingIslandHouse_XCord[i], floatingIslandHouse_YCord[i]);
                }
            }
        }

        private void GenerateTargonArena(GenerationProgress progress)
        {
            int[] PlatformX = new int[] { 0, 17, 34, 55, 72, 89};
            int[] PlatformY = new int[] { 10, 20, 30, 40, 50, 60, 70, 80 };

            for (int x = 0; x < targonArenaWidth; x++)
            {
                for (int y = 0; y < targonArenaHeight; y++)
                {
                    targonArenaWidth = 100;
                    targonArenaHeight = 100;
                    int worldX = (TargonCenterX) - (targonArenaWidth / 2) + x;
                    int worldY = (int)(Main.worldSurface) + y;

                    //Tile tile = Main.tile[worldX, worldY];

                    //TargonArenaSave[x, y] = (Tile)Main.tile[worldX, worldY].Clone();

                    int wallThickness = 5;
                    if (x < wallThickness || x > targonArenaHeight - (1 + wallThickness) || y < wallThickness || y > targonArenaHeight - (1 + wallThickness))
                    {
                        Main.tile[worldX, worldY].type = (ushort)TileType<TargonStone_Arena>();
                        Main.tile[worldX, worldY].active(true);
                        Main.tile[worldX, worldY].slope((byte)0);
                        Main.tile[worldX, worldY].halfBrick(false);
                    }
                    else
                    {
                        bool onXLine = PlatformX.Contains(x - wallThickness);
                        bool onYLine = PlatformY.Contains(y - wallThickness);

                        if (onXLine || onYLine)
                        {
                            Main.tile[worldX, worldY].type = TileID.Platforms;
                            Main.tile[worldX, worldY].active(true);
                            Main.tile[worldX, worldY].slope((byte)0);
                            Main.tile[worldX, worldY].frameY = 29 * 18;
                            Main.tile[worldX, worldY].halfBrick(false);

                            if (onXLine && !onYLine)
                            {
                                if (x - wallThickness == 0)
                                    Main.tile[worldX, worldY].frameX = 6 * 18;
                                else if (x - wallThickness == 89)
                                    Main.tile[worldX, worldY].frameX = 7 * 18;
                                else
                                    Main.tile[worldX, worldY].frameX = 5 * 18;
                            }
                            else if (!onXLine && onYLine)
                            {
                                Main.tile[worldX, worldY].frameX = 0;
                            }
                            else
                            {
                                if (x - wallThickness == 0)
                                    Main.tile[worldX, worldY].frameX = 3 * 18;
                                else if (x - wallThickness == 89)
                                    Main.tile[worldX, worldY].frameX = 4 * 18;
                                else
                                    Main.tile[worldX, worldY].frameX = 0;
                            }

                        }
                        else
                        {
                            Main.tile[worldX, worldY].type = (ushort)0;
                            Main.tile[worldX, worldY].active(false);
                        }

                    }

                    Main.tile[worldX, worldY].wall = (ushort)WallType<TargonStoneWall_Arena>();
                    Main.tile[worldX, worldY].liquid = 0;
                }
            }
        }

        public static void ToggleTargonArena()
        {
            if (TargonCenterX > 0)
            {
                if (targonBossActive && TargonArenaActive)
                {
                    return;
                }
                else if (targonBossActive && !TargonArenaActive)
                {
                    //TargonArenaSave = new Tile[100, 100];
                    for (int i = 0; i < Main.player.Length; i++)
                    {
                        if (Main.player[i].active && !Main.player[i].dead)
                        {
                            Vector2 teleportPos = new Vector2((TargonCenterX * 16) - 16, (60 * 16) + (float)(Main.worldSurface * 16));

                            Main.player[i].Teleport(teleportPos, 1, 0);
                            NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, i, teleportPos.X, teleportPos.Y, 1, 0, 0);
                        }
                    }

                    TargonArenaActive = true;
                }
                else if (!targonBossActive && TargonArenaActive)
                {
                    for (int i = 0; i < Main.player.Length; i++)
                    {
                        if (Main.player[i].active && !Main.player[i].dead)
                        {
                            if (Main.player[i].GetModPlayer<PLAYERGLOBAL>().targonArena)
                            {
                                Vector2 teleportPos = new Vector2((TargonCenterX * 16) - 16, 40 * 16);

                                Main.player[i].Teleport(teleportPos, 1, 0);
                                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, i, teleportPos.X, teleportPos.Y, 1, 0, 0);
                            }
                        }
                    }
                    TargonArenaActive = false;
                }
            }
        }
    }
}
