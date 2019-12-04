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
using TerraLeague.Projectiles;
using TerraLeague.Tiles;
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
    public class WORLDGLOBAL : ModWorld
    {
        internal WorldPacketHandler PacketHandler = ModNetHandler.worldHandler;

        public static bool BlackMistEvent = false;

        public static bool TargonOreSpawned = false;
        public static bool ManaOreSpawned = false;
        public static bool VoidOreSpawned = false;
        public static bool CelestialMeteorCanSpawn = false;
        public static int marbleBlocks = 0;
        public int startingFrames = 0;


        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
        {
            int ShinyIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));
            if (ShinyIndex != -1)
            {
                tasks.Insert(ShinyIndex + 1, new PassLegacy("Ferrospike", GenerateFerrospike));
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
                for (int x = X - biomeWidth/2; x < X + biomeWidth/2; x++)
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

        public override void PostWorldGen()
        {
            // Place some items in Wood Chests
            int[] itemsToPlaceInWoodChests = new int[] { ItemType<LongSword>(), ItemType<Dagger>(), ItemType<AmpTome>(), ItemType<RubyCrystal>(), ItemType<SapphireCrystal>(), ItemType<NullMagic>(), ItemType<ClothArmor>()};
            int itemsToPlaceInWoodChestsChoice = 0;
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 0 * 36 && Main.rand.Next(0, 4) == 0)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == 0 )
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
                        if (chest.item[inventoryIndex].type == 0)
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
                if (chest != null && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 13 * 36 )
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == 0)
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
                        if (chest.item[inventoryIndex].type == 0)
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
                            if (chest.item[inventoryIndex].type == 0)
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
            if (NPC.downedBoss1) OreSpawned.Add("TargonOreSpawned");
            if (NPC.downedBoss2) OreSpawned.Add("ManaOreSpawned");
            if (NPC.downedBoss3) OreSpawned.Add("VoidOreSpawned");
            if (NPC.downedGolemBoss) OreSpawned.Add("CelestialMeteorCanSpawn");

            return new TagCompound {
                {"OreSpawned", OreSpawned},
                {"BlackMistEvent", BlackMistEvent}
            };
        }

        public override void Load(TagCompound tag)
        {
            var OreSpawned = tag.GetList<string>("OreSpawned");
            TargonOreSpawned = OreSpawned.Contains("TargonOreSpawned");
            ManaOreSpawned = OreSpawned.Contains("ManaOreSpawned");
            VoidOreSpawned = OreSpawned.Contains("VoidOreSpawned");
            CelestialMeteorCanSpawn = OreSpawned.Contains("CelestialMeteorCanSpawn");

            BlackMistEvent = tag.GetBool("BlackMistEvent");
        }

        public override void NetSend(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = BlackMistEvent;
            writer.Write(flags);
            base.NetSend(writer);
        }

        public override void NetReceive(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            BlackMistEvent = flags[0];
            base.NetReceive(reader);
        }

        public override void PostUpdate()
        {
            if (!Main.dayTime && Main.time == 1 && !Main.bloodMoon && Main.netMode != 1)
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
                                if (Main.netMode == 0)
                                    Main.NewText("The Harrowing has begun...", new Color(0, 255, 125));
                                else if (Main.netMode == 2)
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
            if (Main.dayTime && BlackMistEvent && Main.netMode != 1)
            {
                BlackMistEvent = false;
                //NetMessage.SendData(MessageID.WorldData);
                //NetSend(new BinaryWriter(mod.GetPacket().BaseStream));
                PacketHandler.SendBlackMist(-1, -1, BlackMistEvent);
            }

            if (Main.hardMode) 
            {
                if (!TargonOreSpawned)
                {
                    DropTargon();
                }
            }

            if (NPC.downedBoss2)
            {
                if (!ManaOreSpawned)
                {
                    ManaOreSpawned = true;
                    
                    if (Main.netMode == 0)
                        Main.NewText("The Evil is no longer suppressing the magic in the jungle", 0, 130, 255);
                    else if (Main.netMode == 2)
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

                    if (Main.netMode == 0)
                        Main.NewText("The Void has morphed some of this worlds matter", 255, 0, 255);
                    else if (Main.netMode == 2)
                    {
                        NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("The Void has morphed some of this worlds matter"), new Color(255, 0, 255), -1);
                        NetMessage.SendData(MessageID.WorldData);
                    }

                    for (int k = 0; k < (int)((double)(Main.maxTilesX * Main.maxTilesY) * 1E-05); k++)  
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

                    if (Main.netMode == 0)
                        Main.NewText("While the Moon denys the Sun, the Aspects will rain gifts of power", 0, 0, 255);
                    else if (Main.netMode == 2)
                    {
                        NetMessage.BroadcastChatMessage(NetworkText.FromLiteral("While the Moon denys the Sun, the Aspects will rain gifts of power"), new Color(0, 0, 255), -1);
                        NetMessage.SendData(MessageID.WorldData);
                    }
                }
            }

            if (Main.eclipse && Main.worldRate != 0 && CelestialMeteorCanSpawn)
            {
                if (Main.dayTime)
                {
                    float num139 = (float)(Main.maxTilesX / 4200f);
                    if ((float)Main.rand.Next(/*8000*/16000) < 10f * num139)
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
                        Projectile.NewProjectile(vector.X, vector.Y, num142, num143, ProjectileType<CelestialMeteorite>(), 10000, 10f, Main.myPlayer, 0f, 0f);
                    }
                }
            }

            base.PostUpdate();
        }

        public override void ResetNearbyTileEffects()
        {
            marbleBlocks = 0;
        }

        public override void TileCountsAvailable(int[] tileCounts)
        {
            marbleBlocks = tileCounts[TileID.Marble] + tileCounts[TileType<PetrifiedGrass>()] + tileCounts[TileType<PetrifiedFlora>()] + tileCounts[TileType<Limestone>()];
        }

        public void DropTargon()
        {
            bool flag = true;
            if (Main.netMode == 1)
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

            if (Main.netMode == 0)
            {
                Main.NewText("The Aspects are pleased. A gift has been droped from the heavens", 0, 200, 255); 

            }
            else if (Main.netMode == 2)
            {
                NetMessage.BroadcastChatMessage(NetworkText.FromKey("The Aspects are pleased. A gift has been droped from the heavens", new object[0]), new Color(50, 255, 130), -1);
            }
            if (Main.netMode != 1)
            {
                NetMessage.SendTileSquare(-1, i, j, 40, TileChangeType.None);
            }
            TargonOreSpawned = true;
            return true;
        }
    }
}
