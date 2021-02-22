using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.UI;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.SummonerSpells
{
    public class TeleportRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Teleport Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Tp";
        }

        public override string GetSpellName()
        {
            return "Teleport";
        }

        public override int GetRawCooldown()
        {
            return 180;
        }

        public override string GetTooltip()
        {
            return "Open a menu of teleport targets";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            //player.TeleportationPotion();

            TeleportUI.visible = !TeleportUI.visible;

            //SetCooldowns(player, spellSlot);
        }

        public static void Efx(Vector2 teleportPoint)
        {
            TerraLeague.PlaySoundWithPitch(teleportPoint, 2, 6, 0);
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(teleportPoint - (Vector2.One * 16), 32, 42, 173, 0, 0, 0, default, 4);
                dust.noGravity = true;
                dust.noLight = true;
                dust.velocity *= 2;
            }
        }

		public static void AttemptTP(Player player, TeleportType type)
        {
			if (type == TeleportType.Random)
            {
				player.TeleportationPotion();
				Efx(player.position);
				SetTPCooldown();
				PacketHandler.SendTeleport(-1, player.whoAmI, player.position);
            }
			else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
				PacketHandler.SendTeleportRequest(-1, player.whoAmI, (int)type);
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
			{
                switch (type)
                {
                    case TeleportType.LeftBeach:
						DoTP(player, LeftBeach());
                        break;
                    case TeleportType.RightBeach:
						DoTP(player, RightBeach());
						break;
                    case TeleportType.Dungeon:
						DoTP(player, Dungeon());
						break;
                    case TeleportType.Hell:
						DoTP(player, Hell(player));
						break;
                    case TeleportType.Random:
						DoTP(player, RandomTP());
						break;
                    default:
                        break;
                }
            }
        }

		public static void AttemptTP(Player player, int playerTarget)
		{
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				PacketHandler.SendTeleportRequestPlayer(-1, player.whoAmI, playerTarget);
			}
			else if (Main.netMode == NetmodeID.SinglePlayer)
			{
				DoTP(player, Main.player[playerTarget].position);
			}
		}

		public static Vector2 RandomTP()
        {
            return Vector2.Zero;
        }
        public static Vector2 LeftBeach()
        {
            int x = 300;
            for (int y = 0; y < Main.maxTilesY; y++)
            {
				var s= Main.tile[x, y];
                if (Collision.SolidTiles(x, x, y, y))
                {
                    return new Vector2(x * 16, (y - 3) * 16);
                }
            }

            return Main.LocalPlayer.position;
        }
        public static Vector2 RightBeach()
        {
            int x = Main.maxTilesX - 300;
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                if (Collision.SolidTiles(x, x, y, y))
                {
                    return new Vector2(x * 16, (y - 3) * 16);
                }
            }

            return Main.LocalPlayer.position;
        }
        public static Vector2 Hell(Player player)
        {
			bool flag = false;
			int num = Main.maxTilesX / 2;
			int num2 = 100;
			int num3 = num2 / 2;
			int teleportStartY = Main.maxTilesY - 200 + 20;
			int teleportRangeY = 80;
			RandomTeleportationAttemptSettings settings = new RandomTeleportationAttemptSettings
			{
				mostlySolidFloor = true,
				avoidAnyLiquid = true,
				avoidLava = true,
				avoidHurtTiles = true,
				avoidWalls = true,
				attemptsBeforeGivingUp = 1000,
				maximumFallDistanceFromOrignalPoint = 30
			};
			Vector2 vector = CheckForGoodTeleportationSpot(player, ref flag, num - num3, num2, teleportStartY, teleportRangeY, settings);
			if (!flag)
			{
				vector = CheckForGoodTeleportationSpot(player, ref flag, num - num2, num3, teleportStartY, teleportRangeY, settings);
			}
			if (!flag)
			{
				vector = CheckForGoodTeleportationSpot(player, ref flag, num + num3, num3, teleportStartY, teleportRangeY, settings);
			}
			if (flag)
			{
				Vector2 vector2 = vector;
				return vector2;
			}
			else
			{
				Vector2 position = player.position;
				return position;
			}
        }
        public static Vector2 Dungeon()
        {
            return new Vector2(Main.dungeonX * 16, Main.dungeonY * 16 - 48);
        }

        public static void DoTP(Player player, Vector2 teleportPoint)
        {
			if (Main.LocalPlayer.whoAmI == player.whoAmI)
			{
				if (Vector2.Distance(teleportPoint, player.position) > 16 * 10)
					SetTPCooldown();
			}

			player.Teleport(teleportPoint, 10, 0);
            Efx(teleportPoint);
        }

		static void SetTPCooldown()
		{
			PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

			for (int i = 0; i < modPlayer.sumSpells.Length; i++)
			{
				if (modPlayer.sumSpells[i].Name == "TeleportRune")
				{
					modPlayer.sumCooldowns[i] = (int)(modPlayer.sumSpells[i].GetCooldown() * 60);
					return;
				}
			}
		}

		static private Vector2 CheckForGoodTeleportationSpot(Player player, ref bool canSpawn, int teleportStartX, int teleportRangeX, int teleportStartY, int teleportRangeY, RandomTeleportationAttemptSettings settings)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int width = player.width;
			Vector2 vector = new Vector2((float)num2, (float)num3) * 16f + new Vector2((float)(-width / 2 + 8), (float)(-player.height));
			while (!canSpawn && num < settings.attemptsBeforeGivingUp)
			{
				num++;
				num2 = teleportStartX + Main.rand.Next(teleportRangeX);
				num3 = teleportStartY + Main.rand.Next(teleportRangeY);
				int num4 = 5;
				num2 = (int)MathHelper.Clamp((float)num2, (float)num4, (float)(Main.maxTilesX - num4));
				num3 = (int)MathHelper.Clamp((float)num3, (float)num4, (float)(Main.maxTilesY - num4));
				vector = new Vector2((float)num2, (float)num3) * 16f + new Vector2((float)(-width / 2 + 8), (float)(-player.height));
				int num7;
				Tile tileSafely;
				if (!Collision.SolidCollision(vector, width, player.height))
				{
					if (Main.tile[num2, num3] == null)
					{
						Tile[,] tile = Main.tile;
						int num5 = num2;
						int num6 = num3;
						Tile tile2 = new Tile();
						tile[num5, num6] = tile2;
					}
					if (settings.avoidWalls && Main.tile[num2, num3].wall > 0)
					{
						continue;
					}
					if (Main.tile[num2, num3].wall == 87 && (double)num3 > Main.worldSurface && !NPC.downedPlantBoss)
					{
						continue;
					}
					if (Main.wallDungeon[Main.tile[num2, num3].wall] && (double)num3 > Main.worldSurface && !NPC.downedBoss3)
					{
						continue;
					}
					num7 = 0;
					while (num7 < settings.maximumFallDistanceFromOrignalPoint)
					{
						if (Main.tile[num2, num3 + num7] == null)
						{
							Tile[,] tile3 = Main.tile;
							int num8 = num2;
							int num9 = num3 + num7;
							Tile tile4 = new Tile();
							tile3[num8, num9] = tile4;
						}
						Tile tile5 = Main.tile[num2, num3 + num7];
						vector = new Vector2((float)num2, (float)(num3 + num7)) * 16f + new Vector2((float)(-width / 2 + 8), (float)(-player.height));
						Collision.SlopeCollision(vector, player.velocity, width, player.height, player.gravDir, false);
						if (!Collision.SolidCollision(vector, width, player.height))
						{
							num7++;
						}
						else
						{
							if (tile5.active() && !tile5.inActive() && Main.tileSolid[tile5.type])
							{
								break;
							}
							num7++;
						}
					}
					vector.Y -= 16f;
					int i = (int)vector.X / 16;
					int j = (int)vector.Y / 16;
					int num10 = (int)(vector.X + (float)width * 0.5f) / 16;
					int j2 = (int)(vector.Y + (float)player.height) / 16;
					tileSafely = Framing.GetTileSafely(i, j);
					Tile tileSafely2 = Framing.GetTileSafely(num10, j2);
					if (settings.avoidAnyLiquid && tileSafely2.liquid > 0)
					{
						continue;
					}
					if (settings.mostlySolidFloor)
					{
						Tile tileSafely3 = Framing.GetTileSafely(num10 - 1, j2);
						Tile tileSafely4 = Framing.GetTileSafely(num10 + 1, j2);
						if (tileSafely3.active() && !tileSafely3.inActive() && Main.tileSolid[tileSafely3.type] && !Main.tileSolidTop[tileSafely3.type] && tileSafely4.active() && !tileSafely4.inActive() && Main.tileSolid[tileSafely4.type] && !Main.tileSolidTop[tileSafely4.type])
						{
							//goto IL_034e;
						}
                        else
                        {
							continue;
                        }
                    }
					else
                    {
						//goto IL_034e;
                    }
				}
                else
                {
					continue;
                }
			//IL_034e:
				if ((!settings.avoidWalls || tileSafely.wall <= 0) && (!settings.avoidAnyLiquid || !Collision.WetCollision(vector, width, player.height)) && (!settings.avoidLava || !Collision.LavaCollision(vector, width, player.height)) && (!settings.avoidHurtTiles || !(Collision.HurtTiles(vector, player.velocity, width, player.height, false).Y > 0f)) && !Collision.SolidCollision(vector, width, player.height) && num7 < settings.maximumFallDistanceFromOrignalPoint - 1)
				{
					Vector2 vector2 = Vector2.UnitX * 16f;
					if (!(Collision.TileCollision(vector - vector2, vector2, player.width, player.height, false, false, (int)player.gravDir) != vector2))
					{
						vector2 = -Vector2.UnitX * 16f;
						if (!(Collision.TileCollision(vector - vector2, vector2, player.width, player.height, false, false, (int)player.gravDir) != vector2))
						{
							vector2 = Vector2.UnitY * 16f;
							if (!(Collision.TileCollision(vector - vector2, vector2, player.width, player.height, false, false, (int)player.gravDir) != vector2))
							{
								vector2 = -Vector2.UnitY * 16f;
								if (!(Collision.TileCollision(vector - vector2, vector2, player.width, player.height, false, false, (int)player.gravDir) != vector2))
								{
									canSpawn = true;
									num3 += num7;
									break;
								}
							}
						}
					}
				}
			}
			return vector;
		}

		private class RandomTeleportationAttemptSettings
		{
			public bool mostlySolidFloor;

			public bool avoidLava;

			public bool avoidAnyLiquid;

			public bool avoidHurtTiles;

			public bool avoidWalls;

			public int attemptsBeforeGivingUp;

			public int maximumFallDistanceFromOrignalPoint;
		}
	}
}


