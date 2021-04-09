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
using Terraria.Audio;
using Microsoft.Xna.Framework.Audio;
using TerraLeague.Items.Accessories;

namespace TerraLeague
{
    public class TerraLeague : Mod
    {
        internal static string MELColor = "FFA500";
        internal static string RNGColor = "20B2AA";
        internal static string MAGColor = "8E70DB";
        internal static string SUMColor = "6495ed";//"87CEEB";

        internal static string DEFColor = "A0A0A0";
        internal static string ARMORColor = "FFFF00";
        internal static string RESISTColor = "B0C4DE";
        internal static string HEALColor = "008000";
        internal static string CDRColor = "E51647";
        internal static string RNGATSColor = "808080";
        internal static string MANAREDUCTColor = "4169E1";
        internal static string MINIONMAXColor = "4682b4";

        internal static string TooltipHeadingColor = "0099cc";

        internal static string PassiveMainColor = "0099cc";
        internal static string PassiveSecondaryColor = "99e6ff";
        internal static string PassiveSubColor = "007399";

        internal static string ActiveMainColor = "ff4d4d";
        internal static string ActiveSecondaryColor = "ff8080";
        internal static string ActiveSubColor = "cc0000";

        internal static TerraLeague instance;
        internal StatUI statUI;
        internal ItemUI itemUI;
        internal AbilityUI abilityUI;
        internal HealthbarUI healthbarUI;
        internal ToolTipUI tooltipUI;
        internal TeleportUI teleportUI;
        internal PlayerUI playerUI;
        internal bool canLog = false;
        internal bool debugMode = false;
        public static bool UseModResourceBar = false;
        public static bool UseCustomManaRegen = false;
        public static bool UseCustomDefenceStat = false;
        internal int SumCurrencyID;
        private UserInterface userInterface1;
        private UserInterface userInterface2;
        private UserInterface userInterface3;
        private UserInterface PlayerInterface;
        public UserInterface HealthbarInterface;
        public UserInterface tooltipInterface;
        public UserInterface teleportInterface;
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

        public static float fogIntensity;
        //public static ModHotKey Trinket;
        public static PlayerLayer ShieldEffect;
        private static Dictionary<string, string> Keys;

        public static bool StopHealthandManaText = true;

        public TerraLeague()
        {
            instance = this;
        }

        /// <summary>
        /// Runs when initally loading the mod
        /// </summary>
        public override void Load()
        {
            Logger.InfoFormat("{0} logging", Name);

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
            //Trinket = RegisterHotKey("Trinket", "R");
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

                if (modPlayer.currentShieldColor.A != 0 && drawPlayer.active)
                {
                    
                    Color color = modPlayer.currentShieldColor;
                    color.MultiplyRGB(Lighting.GetColor((int)drawPlayer.Center.X / 16, (int)drawPlayer.Center.Y / 16));
                    color.A = 100;
                    Rectangle destRec = new Rectangle((int)(drawPlayer.RotatedRelativePoint(drawPlayer.Center).X - Main.screenPosition.X /*- 19 + 30*/), (int)(drawPlayer.RotatedRelativePoint(drawPlayer.Center).Y - Main.screenPosition.Y - 2 /*- 10 + 30*/), 60, 60);

                    Lighting.AddLight(drawPlayer.Center, color.ToVector3());

                    Texture2D texture = instance.GetTexture("Projectiles/NormalShield");
                    Rectangle sourRec = new Rectangle(0, 0 + (60 * frame), 60, 60);
                    DrawData data = new DrawData(texture, destRec, sourRec, color, 0, new Vector2(30, 30), SpriteEffects.None, 1);
                    Main.playerDrawData.Add(data);
                }
            });

            MELColor = "FFA500";
            RNGColor = "20B2AA";
            MAGColor = "8E70DB";
            SUMColor = "6495ed";//"87CEEB";

            DEFColor = "A0A0A0";
            ARMORColor = "FFFF00";
            RESISTColor = "B0C4DE";
            HEALColor = "008000";
            CDRColor = "FFDD8F";
            RNGATSColor = "808080";
            MANAREDUCTColor = "4169E1";
            MINIONMAXColor = "4682b4";

            TooltipHeadingColor = "0099cc";

            PassiveMainColor = "0099cc";
            PassiveSecondaryColor = "99e6ff";
            PassiveSubColor = "007399";

            ActiveMainColor = "ff4d4d";
            ActiveSecondaryColor = "ff8080";
            ActiveSubColor = "cc0000";

            if (!Main.dedServ)
            {
                AddEquipTexture(new Items.Accessories.DarkinHead(), null, EquipType.Head, "DarkinHead", "TerraLeague/Items/Accessories/Darkin_Head");
                AddEquipTexture(new Items.Accessories.DarkinBody(), null, EquipType.Body, "DarkinBody", "TerraLeague/Items/Accessories/Darkin_Body", "TerraLeague/Items/Accessories/Darkin_Arms");
                AddEquipTexture(new Items.Accessories.DarkinLegs(), null, EquipType.Legs, "DarkinLegs", "TerraLeague/Items/Accessories/Darkin_Legs");

                Filters.Scene["TerraLeague:TheBlackMist"] = new Filter(new BlackMistShaderData("FilterSandstormForeground").UseColor(0,2,1).UseSecondaryColor(0,0,0).UseImage(GetTexture("Backgrounds/Fog"), 0, null).UseIntensity(3.5f).UseOpacity(0.2f).UseImageScale(new Vector2(8, 8)), EffectPriority.High);
                Overlays.Scene["TerraLeague:TheBlackMist"] = new SimpleOverlay("Images/Misc/Perlin", new BlackMistShaderData("FilterSandstormBackground").UseColor(0,1,0).UseSecondaryColor(0,0,0).UseImage(GetTexture("Backgrounds/Fog"), 0, null).UseIntensity(5).UseOpacity(1f).UseImageScale(new Vector2(4, 4)), EffectPriority.High, RenderLayers.Landscape);
                SkyManager.Instance["TerraLeague:TheBlackMist"] = new BlackMistSky();

                Filters.Scene["TerraLeague:Targon"] = new Filter(new TargonShaderData("FilterMiniTower").UseColor(0.0f, 0.3f, 0.8f).UseOpacity(0.7f), EffectPriority.VeryHigh);
                SkyManager.Instance["TerraLeague:Targon"] = new TargonSky();

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

                tooltipInterface = new UserInterface();
                tooltipUI = new ToolTipUI();
                //ToolTipUI.visible = true;
                tooltipInterface.SetState(tooltipUI);

                teleportInterface = new UserInterface();
                teleportUI = new TeleportUI();
                TeleportUI.visible = false;
                teleportInterface.SetState(teleportUI);

                PlayerInterface = new UserInterface();
                playerUI = new PlayerUI();
                PlayerInterface.SetState(playerUI);
            }

            Main.instance.GUIBarsDraw();
            base.Load();
        }

        public override void PostSetupContent()
        {
            Mod bossChecklist = ModLoader.GetMod("BossChecklist");
            if (bossChecklist != null)
            {
                bossChecklist.Call(
                    "AddBoss",  // Call
                    3.1f,       // Boss Progresion
                    new List<int>() { ModContent.NPCType<TargonBoss>() }, // NPC Types
                    this, // Mod
                    "The Celestial Gate Keeper", // Name
                    (Func<bool>)(() => TerraLeagueWORLDGLOBAL.TargonArenaDefeated), // Completion Check
                    0, // Spawn Item 
                    new List<int>() { ModContent.ItemType<Items.Placeable.TargonBossTrophy>() }, // Collection Items
                    new List<int>() { ModContent.ItemType<Items.CelestialBar>(), ModContent.ItemType<Items.Placeable.TargonMonolith>(), ModContent.ItemType<Items.Accessories.BottleOfStardust>() }, // Drops
                    "Climb Mount Targon and accept the challenge at its peak", // Spawn Info
                    "",
                    "TerraLeague/NPCs/TargonBoss_Checklist",
                    "TerraLeague/NPCs/TargonBoss_Head");


                bossChecklist.Call(
                    "AddEvent",  // Call
                    2.8f,       // Boss Progresion
                    new List<int>() { ModContent.NPCType<TheUndying_1>(), ModContent.NPCType<TheUndying_Archer>(), ModContent.NPCType<TheUndying_Necromancer>(), ModContent.NPCType<BansheeHive>(), ModContent.NPCType<EtherealRemitter>(), ModContent.NPCType<FallenCrimera>(), ModContent.NPCType<MistEater>(), ModContent.NPCType<SoulBoundSlime>(), ModContent.NPCType<SpectralBitter>(), ModContent.NPCType<UnleashedSpirit>(), ModContent.NPCType<Scuttlegeist>(), ModContent.NPCType<MistDevourer_Head>(), ModContent.NPCType<ShelledHorror>(), ModContent.NPCType<SpectralShark>(), ModContent.NPCType<Mistwraith>(), ModContent.NPCType<ShadowArtilery>() }, // NPC Types
                    this, // Mod
                    "The Harrowing", // Name
                    (Func<bool>)(() => TerraLeagueWORLDGLOBAL.BlackMistDefeated), // Completion Check
                    0, // Spawn Item 
                    0, // Collection Items
                    new List<int>() { ModContent.ItemType<EternalFlame>(), ModContent.ItemType<Items.Tools.FadingMemories>(), ModContent.ItemType<Nightbloom>(), ModContent.ItemType<Items.Armor.NecromancersHood>(), ModContent.ItemType<Items.Armor.NecromancersRobe>(), ModContent.ItemType<Items.DamnedSoul>() }, // Drops
                    "If there is a player with more than 200 max life, there is a 1/12 chance each night for The Harrowing to begin. During New Moons, there is a 1/4 chance instead", // Spawn Info
                    "",
                    "TerraLeague/NPCs/BlackMist_Checklist",
                    "TerraLeague/Gores/MistPuff_1"
                    );
            }
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
            //Trinket = null;

            MELColor = null;
            RNGColor = null;
            MAGColor = null;
            SUMColor = null;
            DEFColor = null;
            ARMORColor = null;
            RESISTColor = null;
            HEALColor = null;
            CDRColor = null;
            RNGATSColor = null;
            MANAREDUCTColor = null;
            MINIONMAXColor = null;
            TooltipHeadingColor = null;
            PassiveMainColor = null;
            PassiveSecondaryColor = null;
            PassiveSubColor = null;
            ActiveMainColor = null;
            ActiveSecondaryColor = null;
            ActiveSubColor = null;

            StopHealthandManaText = false;
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
            int resourseBar = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));

            if (UseModResourceBar)
            {
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

                layers.Insert(resourseBar, new LegacyGameInterfaceLayer("TerraLeague: Resource Bar",
                delegate
                {
                    if (HealthbarUI.visible)
                    {
                        tooltipInterface.Update(Main._drawInterfaceGameTime);
                        tooltipUI.Draw(Main.spriteBatch);
                    }
                    return true;
                },
                InterfaceScaleType.UI));
            }

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
            "TerraLeague: Player Hud",
            delegate
            {
                    PlayerInterface.Update(Main._drawInterfaceGameTime);
                    playerUI.Draw(Main.spriteBatch);
                return true;
            },
            InterfaceScaleType.Game));
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

            layers.Insert(resourseBar, new LegacyGameInterfaceLayer(
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

            layers.Insert(resourseBar, new LegacyGameInterfaceLayer(
            "TerraLeague: Teleport UI",
            delegate
            {
                if (TeleportUI.visible)
                {
                    teleportInterface.Update(Main._drawInterfaceGameTime);
                    teleportUI.Draw(Main.spriteBatch);
                }
                return true;
            },
            InterfaceScaleType.UI));

            //layers.RemoveAll(layer => layer.Name.Equals("Vanilla: Interface Logic 2"));
        }

        public override void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (Main.myPlayer == -1 || Main.gameMenu || !Main.LocalPlayer.active)
            {
                return;
            }
            if (Main.LocalPlayer.HasBuff(ModContent.BuffType<InTargonArena>()) && NPC.CountNPCS(ModContent.NPCType<TargonBoss>()) > 0)
            {
                music = MusicID.Boss2;
                priority = MusicPriority.BossHigh;
            }
            if (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().zoneBlackMist)
            {
                music = MusicID.Eerie;
                priority = MusicPriority.Environment;
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
            leather1.AddRecipeGroup("TerraLeague:EvilDropGroup", 3);
            leather1.AddTile(TileID.WorkBenches);
            leather1.SetResult(ItemID.Leather, 2);
            leather1.AddRecipe();

            ModRecipe sharktooth = new ModRecipe(this);
            sharktooth.AddIngredient(ItemID.SharkFin, 4);
            sharktooth.AddIngredient(ItemID.Chain, 4);
            sharktooth.AddIngredient(ItemID.SoulofNight, 8);
            sharktooth.AddTile(TileID.MythrilAnvil);
            sharktooth.SetResult(ItemID.SharkToothNecklace);
            sharktooth.AddRecipe();

            ModRecipe feralClaws = new ModRecipe(this);
            feralClaws.AddIngredient(ItemID.Stinger, 5);
            feralClaws.AddIngredient(ItemID.Leather, 2);
            feralClaws.AddTile(TileID.MythrilAnvil);
            feralClaws.SetResult(ItemID.FeralClaws);
            feralClaws.AddRecipe();

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

        internal static void DustBorderRing(int radius, Vector2 center, int dustType, Color color, float scale, bool noLight = true, bool randomDis = true, float spacingScale = 0.2f)
        {
            float dis = randomDis ? Main.rand.NextFloat(360f / (radius * spacingScale)) : 0;
            for (int i = 0; i < (int)(radius * spacingScale) + 1; i++)
            {
                Vector2 pos = new Vector2(radius, 0).RotatedBy(MathHelper.ToRadians(360 * (i / (radius * spacingScale)) + dis)) + center;

                Dust dustR = Dust.NewDustPerfect(pos, dustType, Vector2.Zero, 0, color, scale);
                dustR.noGravity = true;
                dustR.noLight = noLight;
            }
        }

        internal static void DustElipce(float width, float height, float rotation,  Vector2 center, int dustType, Color color, float scale, int dustCount = 180, bool noLight = true, float pulseStrength = 0)
        {
            float avgRadius = (width + height) * 0.5f;


            for (int i = 0; i < dustCount; i++)
            {
                float time = MathHelper.TwoPi * i / (dustCount + 1);

                double X = width * Math.Cos(time) * Math.Cos(rotation) - height * Math.Sin(time) * Math.Sin(rotation);
                double Y = width * Math.Cos(time) * Math.Sin(rotation) + height * Math.Sin(time) * Math.Cos(rotation);

                Vector2 pos = new Vector2((float)X, (float)Y) + center;

                Dust dust = Dust.NewDustPerfect(pos, dustType, (center - pos) * pulseStrength, 0, color, scale);
                dust.noGravity = true;
                dust.noLight = noLight;
            }
        }

        internal static void DustLine(Vector2 pointA, Vector2 pointB, int dustType, float dustPerPoint, float scale = 1, Color color = default, bool noLight = true, float xSpeed = 0, float ySpeed = 0)
        {
            Vector2 velocity = new Vector2(xSpeed, ySpeed);
            float xDif = pointB.X - pointA.X;
            float yDif = pointB.Y - pointA.Y;
            int dustCount = (int)(Vector2.Distance(pointA, pointB) * dustPerPoint);

            for (int i = 0; i < dustCount; i++)
            {
                Vector2 position = new Vector2(pointA.X + (xDif * (i / (float)dustCount)), pointA.Y + (yDif * (i / (float)dustCount)));
                Dust dust = Dust.NewDustPerfect(position, dustType, null, 0, color, scale);
                dust.velocity = velocity;
                dust.noGravity = true;
                dust.noLight = noLight;
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
                if (!Main.npc[i].dontTakeDamage)
                {
                    if (!includeCritters && Main.npc[i].lifeMax != 5 && !Main.npc[i].friendly || !includeTownNPCS && !Main.npc[i].townNPC)
                    {
                        if (Main.npc[i].Hitbox.Intersects(new Rectangle((int)Main.MouseWorld.X - mouseLength / 2, (int)Main.MouseWorld.Y - mouseLength / 2, mouseLength, mouseLength)) && !Main.npc[i].immortal && Main.npc[i].active)
                        {
                            return i;
                        }
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
        internal static int PlayerMouseIsHovering(int mouseLength = 30, int doNotInclude = -1, int isOnTeam = -1, bool canBeDead = false)
        {
            int target = TerraLeague.GetClosestPlayer(Main.MouseWorld, mouseLength / 2, doNotInclude, isOnTeam, -1, canBeDead);

            return target;
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
            if (accessory.item.accessory && player.whoAmI == Main.LocalPlayer.whoAmI)
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
                    npc.GetGlobalNPC<TerraLeagueNPCsGLOBAL>().PacketHandler.SendRemoveBuff(-1, Main.myPlayer, buffType, target);
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
            if (npc.type == NPCID.BurningSphere
                || npc.type == NPCID.ChaosBall
                || npc.type == NPCID.WaterSphere
                || npc.type == NPCID.BlazingWheel
                || npc.type == NPCID.VileSpit
                || npc.type == NPCID.DetonatingBubble
                || npc.type == NPCID.ChatteringTeethBomb
                || npc.type == NPCID.MoonLordLeechBlob
                || npc.type == NPCID.SolarFlare
                || npc.type == NPCID.AncientCultistSquidhead
                || npc.type == NPCID.AncientLight
                || npc.type == NPCID.AncientDoom)
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
            if (proj.aiStyle == 19 || proj.aiStyle == 20 || proj.aiStyle == 75 || proj.type == ModContent.ProjectileType<DarksteelBattleaxe_Decimate>() || proj.type == ModContent.ProjectileType<DarkinScythe_ReapingSlash>() || proj.type == ModContent.ProjectileType<Severum_Slash>() || proj.type == ModContent.ProjectileType<AtlasGauntlets_Left>() || proj.type == ModContent.ProjectileType<AtlasGauntlets_Right>())
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


        public static void HealthAndManaHitBoxes()
        {
            if (!StopHealthandManaText)
            {
                return;
            }

            bool isHealthOver200 = (Main.LocalPlayer.statLifeMax2 > 200);
            int heartWidthTotal = isHealthOver200 ? 260 : (26 * Main.player[Main.myPlayer].statLifeMax2 / 20);

            int healthBarX = 500 + (Main.screenWidth - 800);
            int healthBarY = 32;
            int healthBarWidth = 500 + heartWidthTotal + (Main.screenWidth - 800);
            int healthBarHeight = isHealthOver200 ? Main.heartTexture.Height + 32 : 32;

            Rectangle healthBar = new Rectangle(healthBarX, healthBarY, healthBarWidth, healthBarHeight);


            int manaBarX = 762 + (Main.screenWidth - 800);
            int manaBarY = 30;
            int manaBarHeight = 28 * Main.LocalPlayer.statManaMax2 / 20;
            int manaBarWidth = Main.manaTexture.Width + 2;

            Rectangle manaBar = new Rectangle(manaBarX, manaBarY, manaBarWidth, manaBarHeight);

            StopHealthManaMouseOver(healthBar, manaBar);
        }

        public static void StopHealthManaMouseOver(Rectangle HealthHitBox, Rectangle ManaHitBox)
        {
            Main.mouseText = HealthHitBox.Contains(Main.mouseX, Main.mouseY) ||
                ManaHitBox.Contains(Main.mouseX, Main.mouseY) ? true : false;
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            HealthAndManaHitBoxes();
        }

        public static void ForceNPCStoRetarget(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                player.GetModPlayer<PLAYERGLOBAL>().PacketHandler.SendRetarget(-1, -1, player.whoAmI);
            }
            else
            {
                for (int i = 0; i < Main.npc.Length; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.active)
                    {
                        if (npc.target == player.whoAmI)
                        {
                            npc.TargetClosest();
                            npc.netUpdate = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns if 2 points are within a specified range of eachother
        /// </summary>
        /// <param name="startingPoint"></param>
        /// <param name="targetPoint"></param>
        /// <param name="Range"></param>
        /// <returns></returns>
        public static bool IsPointWithinRange(Vector2 startingPoint, Vector2 targetPoint, float range)
        {
            return Vector2.Distance(startingPoint, targetPoint) <= range;
        }

        /// <summary>
        /// Returns if a hitbox intersects with the range
        /// </summary>
        /// <param name="startingPoint"></param>
        /// <param name="hitbox"></param>
        /// <param name="Range"></param>
        /// <returns></returns>
        public static bool IsHitboxWithinRange(Vector2 startingPoint, Rectangle hitbox, float range)
        {
            if (IsPointWithinRange(hitbox.TopLeft(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.Right(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.TopRight(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.Left(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.BottomLeft(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.Top(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.BottomRight(), startingPoint, range))
            {
                return true;
            }
            else if (IsPointWithinRange(hitbox.Bottom(), startingPoint, range))
            {
                return true;
            }
            else if (hitbox.Intersects(new Rectangle((int)startingPoint.X, (int)startingPoint.X, 1, 1)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a list of all NPCs array positions within a circular area
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="includeSmallCreatures"></param>
        /// <param name="includeTownNPCs"></param>
        /// <param name="includeImmortal"></param>
        /// <returns></returns>
        public static List<int> GetAllNPCsInRange(Vector2 center, float radius, bool includeSmallCreatures = false, bool includeTargetDummy = false, bool includeTownNPCs = false, bool includeImmortal = false)
        {
            List<int> npcsInRange = new List<int>();
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active)
                {
                    if (npc.type == NPCID.TargetDummy && includeTargetDummy)
                    {
                        if (IsHitboxWithinRange(center, npc.Hitbox, radius))
                        {
                            npcsInRange.Add(i);
                        }
                    }
                    else if (!includeTownNPCs && !npc.townNPC || includeTownNPCs)
                    {
                        if (!includeSmallCreatures && npc.lifeMax > 5 || includeSmallCreatures)
                        {
                            if (!includeImmortal && !npc.immortal || includeImmortal)
                            {
                                if (IsHitboxWithinRange(center, npc.Hitbox, radius))
                                {
                                    npcsInRange.Add(i);
                                }
                            }
                        }
                    }
                }
            }

            return npcsInRange;
        }

        /// <summary>
        /// Get a list of all players within a set range
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="doNotInclude"></param>
        /// <param name="isOnTeam"></param>
        /// <param name="canBeDead"></param>
        /// <returns></returns>
        public static List<int> GetAllPlayersInRange(Vector2 center, float radius, int doNotInclude = -1, int isOnTeam = -1, bool canBeDead = false)
        {
            List<int> playersInRange = new List<int>();
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (i != doNotInclude)
                {
                    if (player.active)
                    {
                        if (!player.dead && !canBeDead || canBeDead)
                        {
                            if (player.team == isOnTeam && isOnTeam != -1 || isOnTeam == -1)
                            {
                                if (IsHitboxWithinRange(center, player.Hitbox, radius))
                                {
                                    playersInRange.Add(i);
                                }
                            }
                        }
                    }
                }
            }

            return playersInRange;
        }

        /// <summary>
        /// Get the closest Player within a set range
        /// </summary>
        /// <param name="position"></param>
        /// <param name="maxDistance"></param>
        /// <param name="doNotInclude"></param>
        /// <param name="isOnTeam"></param>
        /// <param name="canBeDead"></param>
        /// <returns></returns>
        public static int GetClosestPlayer(Vector2 position, float maxDistance, int doNotInclude = -1, int isOnTeam = -1, int prioritisePlayer = -1, bool canBeDead = false)
        {
            int currentChoice = -1;
            float range = maxDistance;

            if (prioritisePlayer != -1)
            {
                Player player = Main.player[prioritisePlayer];
                if (IsHitboxWithinRange(position, player.Hitbox, range))
                {
                    return currentChoice;
                }
            }

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                Player player = Main.player[i];
                if (i != doNotInclude)
                {
                    if (player.active)
                    {
                        if (!player.dead && !canBeDead || canBeDead)
                        {
                            if (player.team == isOnTeam && isOnTeam != -1 || isOnTeam == -1)
                            {
                                if (IsHitboxWithinRange(position, player.Hitbox, range))
                                {
                                    currentChoice = i;
                                    range = Vector2.Distance(position, player.Center);
                                }
                            }
                        }
                    }
                }
            }

            return currentChoice;
        }

        /// <summary>
        /// Gives all NPCs withing a circular area a buff
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="buff"></param>
        /// <param name="buffDuration"></param>
        /// <param name="includeSmallCreatures"></param>
        /// <param name="includeTownNPCs"></param>
        /// <param name="includeImmortal"></param>
        public static void GiveNPCsInRangeABuff(Vector2 center, float radius, int buff, int buffDuration, bool includeSmallCreatures = false, bool includeTargetDummy = false, bool includeTownNPCs = false, bool includeImmortal = false)
        {
            List<int> npcs = GetAllNPCsInRange(center, radius, includeSmallCreatures, includeTargetDummy, includeTownNPCs, includeImmortal);

            for (int i = 0; i < npcs.Count; i++)
            {
                Main.npc[npcs[i]].AddBuff(buff, buffDuration);
            }
        }

        /// <summary>
        /// Returns if there is at least 1 NPC in range (Can check if it has a buff too)
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="hasBuffType"></param>
        /// <returns></returns>
        public static bool IsThereAnNPCInRange(Vector2 center, float radius, int hasBuffType = -1)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !npc.immortal)
                {
                    if (IsHitboxWithinRange(center, npc.Hitbox, radius))
                    {
                        if (hasBuffType != -1)
                        {
                            if (npc.HasBuff(hasBuffType))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Gets the closests NPC to a point
        /// </summary>
        /// <param name="position"></param>
        /// <param name="doNotInclude"></param>
        /// <param name="includeSmallCreatures"></param>
        /// <param name="includeTownNPCs"></param>
        /// <param name="includeImmortal"></param>
        /// <returns></returns>
        public static int GetClosestNPC(Vector2 position, float maxDistance, int doNotInclude = -1, int prioritiseNPC = -1, bool includeSmallCreatures = false, bool includeTargetDummy = false, bool includeTownNPCs = false, bool includeImmortal = false)
        {
            int currentChoice = -1;
            float range = maxDistance;

            if (prioritiseNPC != -1)
            {
                NPC npc = Main.npc[prioritiseNPC];
                if (IsHitboxWithinRange(position, npc.Hitbox, range))
                {
                    return prioritiseNPC;
                }
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                
                if (npc.active && npc.whoAmI != doNotInclude)
                {
                    if (npc.type == NPCID.TargetDummy && includeTargetDummy)
                    {
                        if (IsHitboxWithinRange(position, npc.Hitbox, range))
                        {
                            currentChoice = i;
                            range = Vector2.Distance(position, npc.Center);
                        }
                    }
                    else if (!includeTownNPCs && !npc.townNPC || includeTownNPCs)
                    {
                        if (!includeSmallCreatures && npc.lifeMax > 5 || includeSmallCreatures)
                        {
                            if (!includeImmortal && !npc.immortal || includeImmortal)
                            {
                                if (IsHitboxWithinRange(position, npc.Hitbox, range))
                                {
                                    currentChoice = i;
                                    range = Vector2.Distance(position, npc.Center);
                                }
                            }
                        }
                    }
                }
            }

            return currentChoice;
        }

        /// <summary>
        /// Gets the closests NPC to a point not including the given array
        /// </summary>
        /// <param name="position"></param>
        /// <param name="doNotInclude"></param>
        /// <param name="includeSmallCreatures"></param>
        /// <param name="includeTownNPCs"></param>
        /// <param name="includeImmortal"></param>
        /// <returns></returns>
        public static int GetClosestNPC(Vector2 position, float maxDistance, int[] doNotInclude, bool includeSmallCreatures = false, bool includeTargetDummy = false, bool includeTownNPCs = false, bool includeImmortal = false)
        {
            int currentChoice = -1;
            float range = maxDistance;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && !doNotInclude.Contains(npc.whoAmI))
                {
                    if (npc.type == NPCID.TargetDummy && includeTargetDummy)
                    {
                        if (IsHitboxWithinRange(position, npc.Hitbox, range))
                        {
                            currentChoice = i;
                            range = Vector2.Distance(position, npc.Center);
                        }
                    }
                    else if (!includeTownNPCs && !npc.townNPC || includeTownNPCs)
                    {
                        if (!includeSmallCreatures && npc.lifeMax > 5 || includeSmallCreatures)
                        {
                            if (!includeImmortal && !npc.immortal || includeImmortal)
                            {
                                if (IsHitboxWithinRange(position, npc.Hitbox, range))
                                {
                                    currentChoice = i;
                                    range = Vector2.Distance(position, npc.Center);
                                }
                            }
                        }
                    }
                }
            }

            return currentChoice;
        }

        public static int GetClosestNPC(Vector2 position, float maxDistance, Vector2 collisionPosition, int collitionWidth, int collitionHeight, int doNotInclude = -1, int prioritiseNPC = -1, bool includeSmallCreatures = false, bool includeTargetDummy = false, bool includeTownNPCs = false, bool includeImmortal = false)
        {
            int currentChoice = -1;
            float range = maxDistance;

            if (prioritiseNPC != -1)
            {
                NPC npc = Main.npc[prioritiseNPC];
                if (IsHitboxWithinRange(position, npc.Hitbox, range))
                {
                    if (Collision.CanHitLine(collisionPosition, collitionWidth, collitionHeight, npc.position, npc.width, npc.height))
                    {
                        return prioritiseNPC;
                    }
                }
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (npc.active && i != doNotInclude)
                {
                    if (npc.type == NPCID.TargetDummy && includeTargetDummy)
                    {
                        if (IsHitboxWithinRange(position, npc.Hitbox, range))
                        {
                            if (Collision.CanHitLine(collisionPosition, collitionWidth, collitionHeight, npc.position, npc.width, npc.height))
                            {
                                currentChoice = i;
                                range = Vector2.Distance(position, npc.Center);
                            }
                        }
                    }
                    else if (!includeTownNPCs && !npc.townNPC || includeTownNPCs)
                    {
                        if (!includeSmallCreatures && npc.lifeMax > 5 || includeSmallCreatures)
                        {
                            if (!includeImmortal && !npc.immortal || includeImmortal)
                            {
                                if (IsHitboxWithinRange(position, npc.Hitbox, range))
                                {
                                    if (Collision.CanHitLine(collisionPosition, collitionWidth, collitionHeight, npc.position, npc.width, npc.height))
                                    {
                                        currentChoice = i;
                                        range = Vector2.Distance(position, npc.Center);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return currentChoice;
        }

        /// <summary>
        /// Plays a sound of specific pitch and returns the created instance
        /// </summary>
        /// <param name="position"></param>
        /// <param name="soundID"></param>
        /// <param name="style"></param>
        /// <param name="pitch"></param>
        public static SoundEffectInstance PlaySoundWithPitch(Vector2 position, int soundID, int style, float pitch)
        {
            if (pitch > 1)
                pitch = 1;
            else if (pitch < -1)
                pitch = -1;

            var sound = Main.PlaySound(new LegacySoundStyle(soundID, style), position);
            if (sound != null)
                sound.Pitch = pitch;

            return sound;
        }

        public static string CreateColorString(string hexValue, string text)
        {
            var splitText = text.Split('\n');
            string rejoinedText = "";
            for (int i = 0; i < splitText.Length; i++)
            {
                if (i != 0)
                    rejoinedText += "\n";
                rejoinedText += "[c/" + PulseText(ConvertHexToColor(hexValue)).Hex3() + ":" + splitText[i] + "]";
            }

            return rejoinedText;
        }

        public static string CreateColorString(Color color, string text)
        {
            var splitText = text.Split('\n');
            string rejoinedText = "";
            for (int i = 0; i < splitText.Length; i++)
            {
                if (i != 0)
                    rejoinedText += "\n";
                rejoinedText += "[c/" + PulseText(color).Hex3() + ":" + splitText[i] + "]";
            }

            return rejoinedText;
        }

        public static string CreateScalingTooltip(DamageType type, int valueToScale, int percentScaling, bool affectedByHealPower = false, string extraText = "")
        {
            string text = "";
            Color textColor;

            switch (type)
            {
                case DamageType.MEL:
                    textColor = ConvertHexToColor(MELColor);
                    break;
                case DamageType.RNG:
                    textColor = ConvertHexToColor(RNGColor);
                    break;
                case DamageType.MAG:
                    textColor = ConvertHexToColor(MAGColor);
                    break;
                case DamageType.SUM:
                    textColor = ConvertHexToColor(SUMColor);
                    break;
                default:
                    textColor = Color.White;
                    break;
            }
            int value = (int)(valueToScale * percentScaling * 0.01);
            
            if (ItemUI.extraStats)
            {
                switch (type)
                {
                    case DamageType.MEL:
                        text += percentScaling + "% MEL(" + value + extraText + ")";
                        break;
                    case DamageType.RNG:
                        text += percentScaling + "% RNG(" + value + extraText + ")";
                        break;
                    case DamageType.MAG:
                        text += percentScaling + "% MAG(" + value + extraText + ")";
                        break;
                    case DamageType.SUM:
                        text += percentScaling + "% SUM(" + value + extraText + ")";
                        break;
                    default:
                        text += value + extraText;
                        break;
                }
                text = CreateColorString(textColor, text);
                if (affectedByHealPower)
                    text += " + " + CreateColorString(ConvertHexToColor(HEALColor), "HEAL(" + (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(value, true) - value) + extraText + ")");
            }
            else
            {
                if (affectedByHealPower)
                    value = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(value, true);
                text += value + extraText;
                text = CreateColorString(textColor, text);
            }

            return text;
        }

        public static string CreateScalingTooltip(string color, string valueName, int valueToScale, int percentScaling, bool affectedByHealPower = false, string extraText = "")
        {
            string text = "[c/" + PulseText(ConvertHexToColor(color)).Hex3() + ":";

            int value = (int)(valueToScale * percentScaling * 0.01);
            

            if (ItemUI.extraStats)
            {
                text += percentScaling + "% " + valueName + "(" + value + extraText + ")]";
                if (affectedByHealPower)
                    text += " + [c/" + PulseText(ConvertHexToColor(HEALColor)).Hex3() + ":HEAL(" + (Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(value, true) - value) + ")]";
            }
            else
            {
                if (affectedByHealPower)
                    value = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(value, true);
                text += value + extraText + "]";
            }

            return text;
        }

        public static Color ConvertHexToColor(string hexValue)
        {
            if (hexValue != null)
            {
                if (hexValue.Length >= 6)
                {
                    int red = Int32.Parse(hexValue[0] + "" + hexValue[1], System.Globalization.NumberStyles.HexNumber);
                    int green = Int32.Parse(hexValue[2] + "" + hexValue[3], System.Globalization.NumberStyles.HexNumber);
                    int blue = Int32.Parse(hexValue[4] + "" + hexValue[5], System.Globalization.NumberStyles.HexNumber);
                    return new Color(red, green, blue);
                }
            }
            return Color.White;
        }

        public static Color PulseText(Color color)
        {
            float pulse = (float)(int)Main.mouseTextColor / 255f;
            return new Color((byte)(color.R * pulse), (byte)(color.G * pulse), (byte)(color.B * pulse), Main.mouseTextColor);
        }
    }
}

enum TerraLeagueHandleType : byte
{
    Player
}
