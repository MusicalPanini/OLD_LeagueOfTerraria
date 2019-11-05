using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using TerraLeague.UI;
using System.IO;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Linq;
using System;
using TerraLeague.Items.SummonerSpells;
using Terraria.GameContent.UI;
using TerraLeague.Projectiles;
using TerraLeague.NPCs;
using Microsoft.Xna.Framework.Graphics;
using TerraLeague.Buffs;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using TerraLeague.Backgrounds;
using Terraria.GameContent.Shaders;
using TerraLeague.Shaders;

namespace TerraLeague
{
    public class TerraLeague : Mod
    {
        internal static string MELColor;
        internal static string RNGColor;
        internal static string MAGColor;
        internal static string SUMColor;

        internal static TerraLeague instance;
        internal StatUI statUI;
        internal ItemUI itemUI;
        internal AbilityUI abilityUI;
        internal HealthbarUI healthbarUI;
        internal bool canLog = false;
        internal bool debugMode = false;
        internal bool disableModUI = false;
        internal int SumCurrencyID;
        private UserInterface userInterface1;
        private UserInterface userInterface2;
        private UserInterface userInterface3;
        public UserInterface HealthbarInterface;
        public static ModHotKey ToggleStats;
        public static ModHotKey Item1;
        public static ModHotKey Item2;
        public static ModHotKey Item3;
        public static ModHotKey Item4;
        public static ModHotKey Item5;
        public static ModHotKey Item6;
        public static ModHotKey Sum1;
        public static ModHotKey Sum2;
        public static ModHotKey QAbility;
        public static ModHotKey WAbility;
        public static ModHotKey EAbility;
        public static ModHotKey RAbility;
        public static ModHotKey Trinket;
        public static PlayerLayer ShieldEffect;
        public static PlayerLayer BreathBar;
        public static PlayerLayer AbilityItem;
        private static Dictionary<string, string> Keys;

        public TerraLeague()
        {
            instance = this;
        }

        /// <summary>
        /// Runs when initally loading the mod
        /// </summary>
        public override void Load()
        {
            Keys = new Dictionary<string, string>()
            {
                {"Escape", "Esc"},
                {"PrintScreen", "PrtSc"},
                {"OemTilde", "`"},
                {"D1", "1"},
                {"D2", "2"},
                {"D3", "3"},
                {"D4", "4"},
                {"D5", "5"},
                {"D6", "6"},
                {"D7", "7"},
                {"D8", "8"},
                {"D9", "9"},
                {"D0", "0"},
                {"OemPlus", "="},
                {"OemMinus", "_"},
                {"Insert", "Ins"},
                {"PageUp", "PgUp"},
                {"NumLock", "NumLk"},
                {"Divide", "/"},
                {"Multiply", "*"},
                {"Minus", "-"},

                {"OemOpenBracket", "["},
                {"OemCloseBracket", "]"},
                {"OemPipe", "\\"},
                {"Delete", "Dlt"},
                {"PageDown", "PgDw"},
                {"Plus", "+"},

                {"CapsLock", "Caps"},
                {"OemSemicolon", ";"},
                {"OemQuotes", "'"},

                {"RightShift", "ShiftR"},
                {"OemComma", ","},
                {"OemPeriod", "."},
                {"OemQuestion", "?"},

                {"LeftControl", "CtrlL"},
                {"LeftAlt", "AltL"},
                {"Space", "Spc"},

                {"Mouse1", "M1"},
                {"Mouse2", "M2"},
                {"Mouse3", "M3"},
                {"Mouse4", "M4"},
                {"Mouse5", "M5"},
                {"RightAlt", "AltR"},
                {"RightControl", "CtrlR"},
                {"RightWindows", "WinR"},
                {"LeftWindows", "WinL"},
                {"LeftShift", "ShiftL"},
                {"NumPad1", "NP1"},
                {"NumPad2", "NP2"},
                {"NumPad3", "NP3"},
                {"NumPad4", "NP4"},
                {"NumPad5", "NP5"},
                {"NumPad6", "NP6"},
                {"NumPad7", "NP7"},
                {"NumPad8", "NP8"},
                {"NumPad9", "NP9"},
                {"NumPad0", "NP0"},
                {"Decimal", "NP."},
                {"Scroll", "ScrLk"},
                {"Pause", "PsBrk"},
            };
            ToggleStats = RegisterHotKey("Toggle Stats Page", "L");
            Item1 = RegisterHotKey("Active item 1", "1");
            Item2 = RegisterHotKey("Active item 2", "2");
            Item3 = RegisterHotKey("Active item 3", "3");
            Item4 = RegisterHotKey("Active item 4", "4");
            Item5 = RegisterHotKey("Active item 5", "5");
            Item6 = RegisterHotKey("Active item 6", "6");
            Sum1 = RegisterHotKey("Summoner Spell 1", "F");
            Sum2 = RegisterHotKey("Summoner Spell 2", "G");
            QAbility = RegisterHotKey("Ability 1", "Z");
            WAbility = RegisterHotKey("Ability 2", "X");
            EAbility = RegisterHotKey("Ability 3", "C");
            RAbility = RegisterHotKey("Ability 4", "V");
            Trinket = RegisterHotKey("Trinket", "R");
            SumCurrencyID = CustomCurrencyManager.RegisterCurrency(new SummonerCurrency(ModContent.ItemType<VialofTrueMagic>(), 999L));

            ShieldEffect = new PlayerLayer("TerraLeague", "ShieldEffect", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
            {
                Player drawPlayer = drawInfo.drawPlayer;
                PLAYERGLOBAL modPlayer = drawPlayer.GetModPlayer<PLAYERGLOBAL>();

                int frame = 0;

                if (modPlayer.shieldFrame < 4)
                    frame = 0;
                else if (modPlayer.shieldFrame < 8)
                    frame = 1;
                else if (modPlayer.shieldFrame < 12)
                    frame = 2;
                else if (modPlayer.shieldFrame < 16)
                    frame = 3;
                else if (modPlayer.shieldFrame < 20)
                    frame = 4;
                else if (modPlayer.shieldFrame < 24)
                    frame = 5;

                if (drawInfo.shadow != 0f)
                {
                    return;
                }


                if (drawPlayer.HasBuff(ModContent.BuffType<DivineJudgementBuff>()))
                {
                    Color color = Color.Gold;
                    color.MultiplyRGB(Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16));
                    color.A = 100;
                    Rectangle destRec = new Rectangle((int)(drawPlayer.position.X - Main.screenPosition.X - 19), (int)(drawPlayer.position.Y - Main.screenPosition.Y - 10), 60, 60);

                    Lighting.AddLight(drawPlayer.Center, color.ToVector3());

                    Texture2D texture = instance.GetTexture("Projectiles/NormalShield");
                    Rectangle sourRec = new Rectangle(0, 0 + (60 * frame), 60, 60);
                    DrawData data = new DrawData(texture, destRec, sourRec, color, 0, Vector2.Zero, SpriteEffects.None, 1);
                    Main.playerDrawData.Add(data);
                }
                else if (modPlayer.GetTotalShield() > 0)
                {
                    Color color = modPlayer.Shields.Last().ShieldColor;
                    color.MultiplyRGB(Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16));
                    color.A = 100;
                    Rectangle destRec = new Rectangle((int)(drawPlayer.position.X - Main.screenPosition.X - 19), (int)(drawPlayer.position.Y - Main.screenPosition.Y - 10), 60, 60);

                    Lighting.AddLight(drawPlayer.Center, color.ToVector3());

                    Texture2D texture = instance.GetTexture("Projectiles/NormalShield");
                    Rectangle sourRec = new Rectangle(0, 0 + (60 * frame), 60, 60);
                    DrawData data = new DrawData(texture, destRec, sourRec, color, 0, Vector2.Zero, SpriteEffects.None, 1);
                    Main.playerDrawData.Add(data);
                }
                else if (modPlayer.veil)
                {
                    Color color = Color.Purple;
                    color.MultiplyRGB(Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16));
                    color.A = 100;
                    Rectangle destRec = new Rectangle((int)(drawPlayer.position.X - Main.screenPosition.X - 19), (int)(drawPlayer.position.Y - Main.screenPosition.Y - 10), 60, 60);

                    Lighting.AddLight(drawPlayer.Center, color.ToVector3());

                    Texture2D texture = instance.GetTexture("Projectiles/NormalShield");
                    Rectangle sourRec = new Rectangle(0, 0 + (60 * frame), 60, 60);
                    DrawData data = new DrawData(texture, destRec, sourRec, color, 0, Vector2.Zero, SpriteEffects.None, 1);
                    Main.playerDrawData.Add(data);
                }
            });

            BreathBar = new PlayerLayer("TerraLeague", "BreathBar", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
            {
                Player drawPlayer = drawInfo.drawPlayer;
                PLAYERGLOBAL modPlayer = drawPlayer.GetModPlayer<PLAYERGLOBAL>();
                Color color = Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16);
                Rectangle destRec = new Rectangle((int)(drawPlayer.Center.X - Main.screenPosition.X - 58), (int)(drawPlayer.position.Y - Main.screenPosition.Y - 32), 116, 20);
                Rectangle destRec2 = new Rectangle((int)(drawPlayer.Center.X - Main.screenPosition.X - 50), (int)(drawPlayer.position.Y - Main.screenPosition.Y - 30), (int)(100 * (drawPlayer.breath/(double)drawPlayer.breathMax)), 16);

                if (drawInfo.shadow != 0f)
                {
                    return;
                }

                if (drawPlayer.breath != drawPlayer.breathMax)
                {
                    Texture2D texture = instance.GetTexture("UI/BreathBar");
                    Rectangle sourRec = new Rectangle(0, 0, 116, 20);
                    DrawData data = new DrawData(texture, destRec, sourRec, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                    Main.playerDrawData.Add(data);

                    Texture2D texture2 = instance.GetTexture("UI/Blank");
                    Rectangle sourRec2 = new Rectangle(0, 0, 16, 16);
                    DrawData data2 = new DrawData(texture2, destRec2, sourRec2, Color.DarkCyan, 0, Vector2.Zero, SpriteEffects.None, 1);
                    Main.playerDrawData.Add(data2);
                }
            });

            AbilityItem = new PlayerLayer("TerraLeague", "AbilityItem", PlayerLayer.HeldItem, delegate (PlayerDrawInfo drawInfo)
            {
                Player drawPlayer = drawInfo.drawPlayer;
                PLAYERGLOBAL modPlayer = drawPlayer.GetModPlayer<PLAYERGLOBAL>();
                if (modPlayer.abilityItem != null)
                {
                    Texture2D texture = Main.itemTexture[modPlayer.abilityItem.type];
                    Vector2 pos = modPlayer.abilityItemPosition + (drawPlayer.Center - Main.screenPosition);
                    if (drawPlayer.shadow == 0f && !drawPlayer.frozen && modPlayer.abilityAnimation > 0 && !drawPlayer.dead)
                    {
                        Color color = Lighting.GetColor((int)((double)drawPlayer.Center.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)drawPlayer.Center.Y + (double)drawPlayer.height * 0.5) / 16.0)); ;
                        if (drawPlayer.gravDir == -1f)
                        {
                            DrawData drawData = new DrawData(texture, pos, new Rectangle(0, 0, texture.Width, texture.Height), modPlayer.abilityItem.GetAlpha(color), modPlayer.abilityRotation - (float)Math.PI/ (-4f * drawPlayer.direction), new Vector2((float)texture.Width * 0.5f - (float)texture.Width * 0.5f * (float)drawPlayer.direction, 0f), modPlayer.abilityItem.scale, SpriteEffects.FlipHorizontally, 0);
                            Main.playerDrawData.Add(drawData);
                        }
                        else
                        {
                            DrawData drawData = new DrawData(texture, pos, new Rectangle(0, 0, texture.Width, texture.Height), modPlayer.abilityItem.GetAlpha(color), modPlayer.abilityRotation - (float)Math.PI / (-4f * drawPlayer.direction), new Vector2((float)texture.Width * 0.5f - (float)texture.Width * 0.5f * (float)drawPlayer.direction, (float)texture.Height), modPlayer.abilityItem.scale, SpriteEffects.FlipHorizontally, 0);
                            Main.playerDrawData.Add(drawData);
                        }
                    }
                }
            });

            MELColor = "FFA500";
            RNGColor = "20B2AA";
            MAGColor = "8E70DB";
            SUMColor = "87CEEB";

            if (!Main.dedServ)
            {
                AddEquipTexture(new Items.Accessories.DarkinHead(), null, EquipType.Head, "DarkinHead", "TerraLeague/Items/Accessories/Darkin_Head");
                AddEquipTexture(new Items.Accessories.DarkinBody(), null, EquipType.Body, "DarkinBody", "TerraLeague/Items/Accessories/Darkin_Body", "TerraLeague/Items/Accessories/Darkin_Arms");
                AddEquipTexture(new Items.Accessories.DarkinLegs(), null, EquipType.Legs, "DarkinLegs", "TerraLeague/Items/Accessories/Darkin_Legs");

                Filters.Scene["TerraLeague:TheBlackMist"] = new Filter(new BlackMistShaderData("FilterSandstormForeground").UseColor(0,2,1).UseSecondaryColor(0,0,0).UseImage(GetTexture("Backgrounds/Fog"), 0, null).UseIntensity(3.5f).UseOpacity(0.2f).UseImageScale(new Vector2(8, 8)), EffectPriority.High);
                Overlays.Scene["TerraLeague:TheBlackMist"] = new SimpleOverlay("Images/Misc/Perlin", new BlackMistShaderData("FilterSandstormBackground").UseColor(0,1,0).UseSecondaryColor(0,0,0).UseImage(GetTexture("Backgrounds/Fog"), 0, null).UseIntensity(5).UseOpacity(1f).UseImageScale(new Vector2(4, 4)), EffectPriority.High, RenderLayers.Landscape);
                SkyManager.Instance["TerraLeague:TheBlackMist"] = new BlackMistSky();

                userInterface1 = new UserInterface();
                statUI = new StatUI();
                StatUI.visible = 1;
                userInterface1.SetState(statUI);

                userInterface2 = new UserInterface();
                itemUI = new ItemUI();
                ItemUI.visible = true;
                userInterface2.SetState(itemUI);

                userInterface3 = new UserInterface();
                abilityUI = new AbilityUI();
                AbilityUI.visible = true;
                userInterface3.SetState(abilityUI);

                HealthbarInterface = new UserInterface();
                healthbarUI = new HealthbarUI();
                HealthbarUI.visible = true;
                HealthbarInterface.SetState(healthbarUI);
            }
            base.Load();
        }

        /// <summary>
        /// <para>Runs when disabling the mod.</para>
        /// Be sure to unload any static variables in the mod instance as they are not unloaded automaticly and will cause crashes
        /// </summary>
        public override void Unload()
        {
            Keys = null;
            instance = null;
            ShieldEffect = null;
            BreathBar = null;
            AbilityItem = null;
            ToggleStats = null;
            Item1 = null;
            Item2 = null;
            Item3 = null;
            Item4 = null;
            Item5 = null;
            Item6 = null;
            Sum1 = null;
            Sum2 = null;
            QAbility = null;
            WAbility = null;
            EAbility = null;
            RAbility = null;
            Trinket = null;
            MELColor = null;
            RNGColor = null;
            MAGColor = null;
            SUMColor = null;

            base.Unload();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (ToggleStats.JustReleased)
            {
                ItemUI.extraStats = ItemUI.extraStats ? false : true;
            }

            base.UpdateUI(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            if (!disableModUI)
            {
                int resourseBar = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
                if (resourseBar < 0)
                    resourseBar = 7;
                else
                {
                    layers[resourseBar].Active = false;
                    //layers.RemoveAt(resourseBar);
                }
                layers.Insert(resourseBar, new LegacyGameInterfaceLayer("TerraLeague: Resource Bar",
                delegate
                {
                    if (HealthbarUI.visible)
                    {
                        HealthbarInterface.Update(Main._drawInterfaceGameTime);
                        healthbarUI.Draw(Main.spriteBatch);
                    }
                    return true;
                },
                InterfaceScaleType.UI));

                int inventoryLayer = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Inventory"));
                if (inventoryLayer < 0)
                    inventoryLayer = 4;

                layers.Insert(inventoryLayer, new LegacyGameInterfaceLayer(
                "TerraLeague: Stat Hud",
                delegate
                {
                    if (StatUI.visible < 0)
                    {
                        userInterface1.Update(Main._drawInterfaceGameTime);
                        statUI.Draw(Main.spriteBatch);
                    }
                    return true;
                },
                InterfaceScaleType.UI));

                layers.Insert(inventoryLayer, new LegacyGameInterfaceLayer(
                "TerraLeague: Item Hud",
                delegate
                {
                    if (ItemUI.visible)
                    {
                        userInterface2.Update(Main._drawInterfaceGameTime);
                        itemUI.Draw(Main.spriteBatch);
                    }
                    return true;
                },
                InterfaceScaleType.UI));

                layers.Insert(inventoryLayer, new LegacyGameInterfaceLayer(
                "TerraLeague: Ability Hud",
                delegate
                {
                    if (AbilityUI.visible)
                    {
                        userInterface3.Update(Main._drawInterfaceGameTime);
                        abilityUI.Draw(Main.spriteBatch);
                    }
                    return true;
                },
                InterfaceScaleType.UI));

                //layers.RemoveAll(layer => layer.Name.Equals("Vanilla: Interface Logic 2"));
            }
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
            {
                return;
            }
            if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist)
            {
                music = MusicID.Eerie;
                priority = MusicPriority.Event;
            }
            base.UpdateMusic(ref music, ref priority);
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            ModNetHandler.HandlePacket(reader, whoAmI);
        }

        public override void AddRecipeGroups()
        {
            #region Pre Hardmode Bars
            RecipeGroup CopperGroup = new RecipeGroup(() => "Copper or Tin Bar", new int[]
            {
                ItemID.CopperBar,
                ItemID.TinBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:CopperGroup", CopperGroup);

            RecipeGroup IronGroup = new RecipeGroup(() => "Iron or Lead Bar", new int[]
            {
                ItemID.IronBar,
                ItemID.LeadBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:IronGroup", IronGroup);

            RecipeGroup SilverGroup = new RecipeGroup(() => "Silver or Tungston Bar", new int[]
            {
                ItemID.SilverBar,
                ItemID.TungstenBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:SilverGroup", SilverGroup);

            RecipeGroup GoldGroup = new RecipeGroup(() => "Gold or Platinum Bar", new int[]
            {
                ItemID.GoldBar,
                ItemID.PlatinumBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:GoldGroup", GoldGroup);

            RecipeGroup DemonGroup = new RecipeGroup(() => "Demonite or Crimtane Bar", new int[]
            {
                ItemID.DemoniteBar,
                ItemID.CrimtaneBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:DemonGroup", DemonGroup);

            RecipeGroup DemonPartGroup = new RecipeGroup(() => "Shadow Scale or Tissue Sample", new int[]
            {
                ItemID.ShadowScale,
                ItemID.TissueSample
            });
            RecipeGroup.RegisterGroup("TerraLeague:DemonPartGroup", DemonPartGroup);

            RecipeGroup EvilDropGroup = new RecipeGroup(() => "Rotten Chunk or Vertebrae", new int[]
            {
                ItemID.RottenChunk,
                ItemID.Vertebrae
            });
            RecipeGroup.RegisterGroup("TerraLeague:EvilDropGroup", EvilDropGroup);
            #endregion

            #region Hardmode Bars
            RecipeGroup Tier1Bar = new RecipeGroup(() => "Cobalt or Palladium Bar", new int[]
            {
                ItemID.CobaltBar,
                ItemID.PalladiumBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:Tier1Bar", Tier1Bar);

            RecipeGroup Tier2Bar = new RecipeGroup(() => "Mythril or Orichalcum Bar", new int[]
            {
                ItemID.MythrilBar,
                ItemID.OrichalcumBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:Tier2Bar", Tier2Bar);

            RecipeGroup Tier3Bar = new RecipeGroup(() => "Adamantite or Titanium Bar", new int[]
            {
                ItemID.AdamantiteBar,
                ItemID.TitaniumBar
            });
            RecipeGroup.RegisterGroup("TerraLeague:Tier3Bar", Tier3Bar);

            RecipeGroup EvilPartGroup = new RecipeGroup(() => "Cursed Flames or Ichor", new int[]
            {
                ItemID.CursedFlame,
                ItemID.Ichor
            });
            RecipeGroup.RegisterGroup("TerraLeague:EvilPartGroup", EvilPartGroup);
            #endregion
        }

        public override void AddRecipes()
        {
            RecipeFinder finder = new RecipeFinder();
            finder.AddIngredient(ItemID.RottenChunk, 5);

            foreach (Recipe recipe in finder.SearchRecipes())
            {
                RecipeEditor editor = new RecipeEditor(recipe);
                editor.DeleteRecipe();
            }
            
            ModRecipe leather1 = new ModRecipe(this);
            leather1.AddRecipeGroup("TerraLeague:EvilDropGroup", 2);
            leather1.AddTile(TileID.WorkBenches);
            leather1.SetResult(ItemID.Leather);
            leather1.AddRecipe();

            base.AddRecipes();
        }

        /// <summary>
        /// Creates a ring of dust from a specific player/npc
        /// </summary>
        /// <param name="type">Dust ID to be used</param>
        /// <param name="entity">Player/NPC</param>
        /// <param name="color">Dust color</param>
        internal static void DustRing(int type, Entity entity, Color color)
        {
            for (int i = 0; i < 18; i++)
            {
                Vector2 vel = new Vector2(20, 0).RotatedBy(MathHelper.ToRadians(20 * i));

                Dust dust = Dust.NewDustPerfect(entity.Center, type, vel, 0, color);
                dust.noGravity = true;
            }
        }

        /// <summary>
        /// Sends a message to chat if logging is enabled
        /// </summary>
        /// <param name="message">Message to be sent</param>
        /// <param name="color">Color of the text</param>
        internal static void Log(string message, Color color)
        {
            if (instance.canLog)
            {
                color.A = 0;

                Main.NewText(message, color);
            }
        }

        /// <summary>
        /// Calculates a velocity between position and the mouse
        /// </summary>
        /// <param name="pos">Starting position for velocity</param>
        /// <param name="speed">Magnitude of the velocity</param>
        /// <returns>Altered velocity</returns>
        internal static Vector2 CalcVelocityToMouse(Vector2 pos, float speed)
        {
            double angle = CalcAngle(pos, Main.MouseWorld);

            if (Main.MouseWorld.X < pos.X)
                return new Vector2(-speed, 0).RotatedBy(angle);
            else
                return new Vector2(speed, 0).RotatedBy(angle);
        }

        internal static Vector2 CalcVelocityToPoint(Vector2 pos, Vector2 point, float speed)
        {
            double angle = CalcAngle(pos, point);

            if (point.X < pos.X)
                return new Vector2(-speed, 0).RotatedBy(angle);
            else
                return new Vector2(speed, 0).RotatedBy(angle);
        }

        /// <summary>
        /// Calculates the angle between a center point and a position
        /// </summary>
        /// <param name="center">The origin point (0,0)</param>
        /// <param name="point">The other point</param>
        /// <returns>The angle between the 2 points</returns>
        internal static double CalcAngle(Vector2 center, Vector2 point)
        {
            double xDis = point.X - center.X;
            double yDis = point.Y - center.Y;

            return Math.Atan(yDis / xDis);
        }

        /// <summary>
        /// <para>Checks if the mouse is hovering over an NPC</para>
        /// Returns -1 if mouse was not hovering
        /// </summary>
        /// <param name="mouseLength">Width and height of the mouse hitbox</param>
        /// <param name="includeCritters">Include small animals in the check</param>
        /// <param name="includeTownNPCS">Include Town NPCs in the check</param>
        /// <returns></returns>
        internal static int NPCMouseIsHovering(int mouseLength = 30, bool includeCritters = false, bool includeTownNPCS = false)
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (!includeCritters && Main.npc[i].lifeMax != 5 && !Main.npc[i].friendly || !includeTownNPCS && !Main.npc[i].townNPC)
                {
                    if (Main.npc[i].Hitbox.Intersects(new Rectangle((int)Main.MouseWorld.X - mouseLength/2, (int)Main.MouseWorld.Y - mouseLength/2, mouseLength, mouseLength)) && !Main.npc[i].immortal && Main.npc[i].active)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// <para>Checks if the mouse is hovering over a Player</para>
        /// Returns -1 if mouse was not hovering
        /// </summary>
        /// <param name="mouseLength">Width and height of the mouse hitbox</param>
        /// <returns></returns>
        internal static int PlayerMouseIsHovering(int mouseLength = 30)
        {
            for (int i = 0; i < Main.player.Length; i++)
            {
                if (Main.player[i].active)
                {
                    if (Main.player[i].Hitbox.Intersects(new Rectangle((int)Main.MouseWorld.X - mouseLength/2, (int)Main.MouseWorld.Y - mouseLength/2, mouseLength, mouseLength)))
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Find the itemslot an accessory is in
        /// Returns -1 if not equiped
        /// </summary>
        /// <param name="player">Player to check</param>
        /// <param name="accessory">Accessory to look for</param>
        /// <returns></returns>
        internal static int FindAccessorySlotOnPlayer(Player player, ModItem accessory)
        {
            if (accessory.item.accessory)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (player.armor[i + 3].type == accessory.item.type)
                    {
                        return i;
                    }
                }

            }
            return -1;
        }

        /// <summary>
        /// Removes a buff from an NPC for the local client and all other clients connected to the server
        /// </summary>
        /// <param name="buffType">Buff ID</param>
        /// <param name="target">NPC.whoAmI</param>
        internal static void RemoveBuffFromNPC(int buffType, int target)
        {
            NPC npc = Main.npc[target];

            if (npc.FindBuffIndex(buffType) >= 0)
            {
                npc.DelBuff(npc.FindBuffIndex(buffType));

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    npc.GetGlobalNPC<NPCsGLOBAL>().PacketHandler.SendRemoveBuff(-1, Main.myPlayer, buffType, target);
                }
            }
        }

        /// <summary>
        /// Checks if the projectile is one of the vanilla minions. Base Terraia has no damage type associated with them
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        internal static bool IsMinionDamage(Projectile proj)
        {
            if (proj.Name == "Imp Fireball"
                || proj.Name == "Baby Slime"
                || proj.Name == "Hornet Stinger"
                || proj.Name == "Baby Spider"
                || proj.Name == "Mini Sharkron"
                || proj.Name == "Pygmy"
                || proj.Name == "UFO Ray"
                || proj.Name == "Mini Retina Laser"
                || proj.Name == "Stardust Cell"
                || proj.Name == "Frost Blast"
                || proj.Name == "Lunar Portal Laser"
                || proj.Name == "Rainbow Explosion"
                || proj.Name == "Lightning Aura"
                || proj.Name == "Ballista"
                || proj.Name == "Explosive Trap"
                || proj.Name == "Flameburst Tower"
                || proj.Name == "Starburst"
                || proj.minion)
                return true;
            else
                return false;
        }

        //Old Method for old Runnans
        internal static bool DoNotCountRangedDamage(Projectile proj)
        {
            if (proj.Name == "Just false this"
                //|| proj.Name == "Hallow Star"
                //|| proj.Name == "Baby Spider"
                //|| proj.Name == "Mini Sharkron"
                //|| proj.Name == "Pygmy"
                //|| proj.Name == "UFO Ray"
                //|| proj.Name == "Mini Retina Laser"
                //|| proj.Name == "Stardust Cell"
                //|| proj.Name == "Frost Blast"
                //|| proj.Name == "Lunar Portal Laser"
                //|| proj.Name == "Rainbow Explosion"
                //|| proj.Name == "Lightning Aura"
                //|| proj.Name == "Ballista"
                //|| proj.Name == "Explosive Trap"
                //|| proj.Name == "Flameburst Tower"
                //|| proj.Name == "Starburst"
                )
                return true;
            else
                return false;
        }

        /// <summary>
        /// Redundent
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        internal static bool IsProjectileHoming(Projectile proj)
        {
            if (proj.type == 207 // Chloro Bullet
                || proj.type == 316 // Bat
                || proj.type == 297 // Specter Staff
                || proj.type == 181 // Bee
                || proj.type == 566 // Large Bee
                || proj.type == 189 // Wasp
                || proj.type == 307 // Scourge Corruptor
                || proj.type == 409 // Razorblade Typhoon
                || proj.type == 634 // Nebula Blaze
                || proj.type == 635 // Nebula Blaze EX
                || proj.type == 618 // Vortex Rocket
                || proj.type == 616 // Vortex Rocket again?
                || proj.type == 617 // Nebula Arcanum
                || proj.type == 619 // Nebula Arcanum again?
                || proj.type == 620 // Nebula Arcanum again...
                || proj.type == 659 // Spirit Flame
                )
                return true;
            else
                return false;
        }

        /// <summary>
        /// Some projectiles are actually npcs so you can hit and destroy them. This checks for that
        /// </summary>
        /// <param name="npc"></param>
        /// <returns></returns>
        internal static bool IsEnemyActuallyProj(NPC npc)
        {
            if (npc.type == 25
                || npc.type == 30
                || npc.type == 33
                || npc.type == 72
                || npc.type == 112
                || npc.type == 371
                || npc.type == 378
                || npc.type == 401
                || npc.type == 516
                || npc.type == 521
                || npc.type == 522
                || npc.type == 523)
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Checks if the requested Projectile is considered a melee swing by the mod
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        internal static bool IsProjActuallyMeleeAttack(Projectile proj)
        {
            if (proj.aiStyle == 19 || proj.aiStyle == 20 || proj.aiStyle == 75 || proj.type == ModContent.ProjectileType<DarksteelDecimate>() || proj.type == ModContent.ProjectileType<ReapingSlash>())
                return true;
            else
                return false;
        }

        /// <summary>
        /// Converts Terrarias Hot Key strings to shorter, cleaner version for interfaces and tooltips
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        internal static string ConvertKeyString(ModHotKey key)
        {
            string keyConvertedString;
            string keyString = "N/A";
            if (key.GetAssignedKeys().Count > 0)
                keyString = key.GetAssignedKeys().First();
                

            if (Keys.ContainsKey(keyString))
                keyConvertedString = Keys[keyString];
            else
                keyConvertedString = keyString;

            return keyConvertedString;

        }
    }
}

enum TerraLeagueHandleType : byte
{
    Player
}
