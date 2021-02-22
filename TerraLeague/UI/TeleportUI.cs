using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using System;
using Terraria.ID;
using TerraLeague.Items.SummonerSpells;
using TerraLeague.Items.CustomItems;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TerraLeague.UI
{
    public class TeleportUI : UIState
    {
        int width = 262;
        int height = 140;

        public float LeftPercent = 0;
        public float TopPercent = 0;
        static public bool visible = false;

        static public string nameText = "";

        public UIElement MainPanel;
        public UIText TeleName;
        Texture2D backgroundTexture;

        TeleportButton[] TeleportButtons;
        PlayerTeleportButton TeleportPlayerButton;
        ArrowButton NextPlayer;
        ArrowButton PreviousPlayer;

        bool moveMode = false;
        float mouseXOri = 0;
        float mouseYOri = 0;


        public override void OnInitialize()
        {
            if (backgroundTexture == null)
                backgroundTexture = TerraLeague.instance.GetTexture("UI/TeleportBackground");

            LeftPercent = ((float)Main.screenWidth - width) / 2 / (float)Main.screenWidth;
            TopPercent = 0.3f;
            Left.Set(0, LeftPercent);
            Top.Set(0, TopPercent);
            Width.Set(width, 0f);
            Height.Set(height, 0f);

            TeleportButtons = new TeleportButton[] {
                new TeleportButton(TeleportType.LeftBeach),
                new TeleportButton(TeleportType.Dungeon),
                new TeleportButton(TeleportType.Random),
                new TeleportButton(TeleportType.Hell),
                new TeleportButton(TeleportType.RightBeach)
            };
            for (int i = 0; i < TeleportButtons.Length; i++)
            {
                TeleportButtons[i].Left.Set(9 + ((6 + TeleportButtons[i].Width.Pixels) * i), 0);
                TeleportButtons[i].Top.Set(35, 0);

                Append(TeleportButtons[i]);
            }

            TeleportPlayerButton = new PlayerTeleportButton();
            TeleportPlayerButton.Left.Set(109, 0);
            TeleportPlayerButton.Top.Set(87, 0);
            Append(TeleportPlayerButton);

            NextPlayer = new ArrowButton(false);
            NextPlayer.Left.Set(159, 0);
            NextPlayer.Top.Set(87, 0);
            Append(NextPlayer);

            PreviousPlayer = new ArrowButton(true);
            PreviousPlayer.Left.Set(59, 0);
            PreviousPlayer.Top.Set(87, 0);
            Append(PreviousPlayer);

            TeleName = new UIText("");
            TeleName.Left.Set(0, 0);
            TeleName.Top.Set(9, 0);
            TeleName.Width.Set(width, 0f);
            TeleName.Height.Set(24, 0f);
            Append(TeleName);

            base.OnInitialize();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                for (int i = 0; i < TeleportButtons.Length; i++)
                {
                    TeleportButtons[i].Left.Set(9 + ((6 + TeleportButtons[i].Width.Pixels) * i), 0);
                    TeleportButtons[i].Top.Set(35, 0);

                    Append(TeleportButtons[i]);
                }

                CalculatedStyle dimensions = this.GetDimensions();
                Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
                spriteBatch.Draw(backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            }
            else
            {
                nameText = "";
            }
            base.DrawSelf(spriteBatch);
        }

        bool CheckForTeleportSum(Player player)
        {
            bool hasTP = false;
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            for (int i = 0; i < modPlayer.sumSpells.Length; i++)
            {
                hasTP = modPlayer.sumSpells[i].Name == "TeleportRune";

                if (hasTP)
                {
                    if (modPlayer.sumCooldowns[i] <= 0)
                        break;
                    else
                        return false;
                }
            }

            return hasTP;
        }
        
        public override void Update(GameTime gameTime)
        {
            if (CheckForTeleportSum(Main.LocalPlayer) && !Main.LocalPlayer.dead)
            {
                if (visible)
                {
                    Main.LocalPlayer.mouseInterface = IsMouseHovering;

                    if (moveMode)
                    {
                        Left.Set(0, LeftPercent + ((Main.MouseScreen.X - mouseXOri) / (float)Main.screenWidth));
                        Top.Set(0, TopPercent + ((Main.MouseScreen.Y - mouseYOri) / (float)Main.screenHeight));
                    }
                    else
                    {
                        var dimentions = this.GetDimensions();
                        if (dimentions.X < 0)
                        {
                            LeftPercent = 0;
                            Left.Set(0, LeftPercent);
                        }
                        if (dimentions.X + Width.Pixels > Main.screenWidth)
                        {
                            LeftPercent = (Main.screenWidth - Width.Pixels) / (float)Main.screenWidth;
                            Left.Set(0, LeftPercent);
                        }
                        if (dimentions.Y < 0)
                        {
                            TopPercent = 0;
                            Top.Set(0, TopPercent);
                        }
                        if (dimentions.Y + Height.Pixels > Main.screenHeight)
                        {
                            TopPercent = (Main.screenHeight - Height.Pixels) / (float)Main.screenHeight;
                            Top.Set(0, TopPercent);
                        }
                    }

                    for (int i = 0; i < TeleportButtons.Length; i++)
                    {
                        TeleportButtons[i].Left.Set(9 + ((6 + TeleportButtons[i].Width.Pixels) * i), 0);
                        TeleportButtons[i].Top.Set(35, 0);
                    }

                    TeleName.HAlign = 0.5f;
                    TeleName.SetText(nameText);

                    Recalculate();

                    TeleportPlayerButton.Update(gameTime);
                }
            }
            else if (visible)
            {
                visible = false;
            }
        }

        public override void RightMouseDown(UIMouseEvent evt)
        {
            moveMode = true;

            mouseXOri = Main.MouseScreen.X;
            mouseYOri = Main.MouseScreen.Y;

            base.RightMouseDown(evt);
        }

        public override void RightMouseUp(UIMouseEvent evt)
        {
            moveMode = false;

            LeftPercent = Left.Percent;
            TopPercent = Top.Percent;

            base.RightMouseUp(evt);
        }
    }

    class TeleportButton : UIState
    {
        public TeleportType type;
        public int playerNum = -1;
        Texture2D backgroundTexture;
        Texture2D forgroundTexture;

        int width = 44;
        int height = 44;

        public TeleportButton(TeleportType Type)
        {
            type = Type;
        }

        public override void OnInitialize()
        {
            backgroundTexture = TerraLeague.instance.GetTexture("UI/TeleportBorder");

            switch (type)
            {
                case TeleportType.LeftBeach:
                    forgroundTexture = TerraLeague.instance.GetTexture("UI/LeftBeach");
                    break;
                case TeleportType.RightBeach:
                    forgroundTexture = TerraLeague.instance.GetTexture("UI/RightBeach");
                    break;
                case TeleportType.Dungeon:
                    forgroundTexture = TerraLeague.instance.GetTexture("UI/Dungeon");
                    break;
                case TeleportType.Hell:
                    forgroundTexture = TerraLeague.instance.GetTexture("UI/Hell");
                    break;
                case TeleportType.Random:
                    forgroundTexture = TerraLeague.instance.GetTexture("UI/Random");
                    break;
                default:
                    break;
            }

            if (backgroundTexture != null)
            {
                width = backgroundTexture.Width;
                height = backgroundTexture.Height;
            }
            else
            {
                width = 44;
                height = 44;
            }

            Width.Set(width, 0);
            Height.Set(height, 0);

            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (TeleportUI.visible)
            {
            }

            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (TeleportUI.visible)
            {
                CalculatedStyle dimensions = this.GetDimensions();
                Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
                spriteBatch.Draw(backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);

                switch (type)
                {
                    case TeleportType.LeftBeach:
                    case TeleportType.RightBeach:
                    case TeleportType.Dungeon:
                    case TeleportType.Hell:
                    case TeleportType.Random:
                        dimensions = this.GetDimensions();
                        point1 = new Point((int)dimensions.X + 4, (int)dimensions.Y + 4);
                        spriteBatch.Draw(forgroundTexture, new Rectangle(point1.X, point1.Y, 36, 36), Color.White);
                        break;
                    default:
                        break;
                }
                
            }
            base.DrawSelf(spriteBatch);
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            TeleportRune.AttemptTP(Main.LocalPlayer, type);

            //switch (type)
            //{
            //    case TeleportType.LeftBeach:
            //        TeleportRune.DoTP(Main.LocalPlayer, TeleportRune.LeftBeach());
            //        break;
            //    case TeleportType.RightBeach:
            //        TeleportRune.DoTP(Main.LocalPlayer, TeleportRune.RightBeach());
            //        break;
            //    case TeleportType.Dungeon:
            //        TeleportRune.DoTP(Main.LocalPlayer, TeleportRune.Dungeon());
            //        break;
            //    case TeleportType.Hell:
            //        TeleportRune.DoTP(Main.LocalPlayer, TeleportRune.Hell(Main.LocalPlayer));
            //        break;
            //    case TeleportType.Random:
            //        TeleportRune.DoTP(Main.LocalPlayer, TeleportRune.RandomTP());
            //        break;
            //    default:
            //        break;
            //}
            
            base.MouseUp(evt);
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            string name = "";

            switch (type)
            {
                case TeleportType.LeftBeach:
                    name = "Left Ocean";
                    break;
                case TeleportType.RightBeach:
                    name = "Right Ocean";
                    break;
                case TeleportType.Dungeon:
                    name = "Dungeon";
                    break;
                case TeleportType.Hell:
                    name = "Hell";
                    break;
                case TeleportType.Random:
                    name = "Random";
                    break;
                default:
                    break;
            }

            TeleportUI.nameText = name;

            backgroundTexture = TerraLeague.instance.GetTexture("UI/TeleportBorderSelected");

            base.MouseOver(evt);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            TeleportUI.nameText = "";

            backgroundTexture = TerraLeague.instance.GetTexture("UI/TeleportBorder");

            base.MouseDown(evt);
        }
    }

    class PlayerTeleportButton : UIState
    {
        public static int playerNum = -1;
        Texture2D backgroundTexture;
        Texture2D noPlayers;
        Texture2D ded;
        int activePlayers = 0;

        int width = 44;
        int height = 44;

        public PlayerTeleportButton()
        {
        }

        public override void OnInitialize()
        {
            backgroundTexture = TerraLeague.instance.GetTexture("UI/TeleportBorder");
            noPlayers = TerraLeague.instance.GetTexture("UI/NoPlayers");
            ded = TerraLeague.instance.GetTexture("UI/Ded");
            if (backgroundTexture != null)
            {
                width = backgroundTexture.Width;
                height = backgroundTexture.Height;
            }
            else
            {
                width = 44;
                height = 44;
            }

            Width.Set(width, 0);
            Height.Set(height, 0);

            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (TeleportUI.visible)
            {
                var activePlayerList = TerraLeague.GetAllPlayersInRange(Main.LocalPlayer.MountedCenter, 99999, Main.LocalPlayer.whoAmI, Main.LocalPlayer.team, true);
                int currentActivePlayers = activePlayerList.Count();

                if (currentActivePlayers != activePlayers)
                {
                    activePlayers = currentActivePlayers;
                    if (activePlayers <= 0)
                    {
                        playerNum = -1;
                    }
                    else if (playerNum == -1)
                    {
                        playerNum = activePlayerList[0];
                    }

                    if (playerNum != -1)
                    {
                        if ((!Main.player[playerNum].active))
                            playerNum = Main.player[activePlayerList[0]].whoAmI;
                    }
                }
            }

            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (TeleportUI.visible)
            {
                CalculatedStyle dimensions = this.GetDimensions();
                Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
                spriteBatch.Draw(backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);

                if (playerNum == -1)
                {
                    dimensions = this.GetDimensions();
                    point1 = new Point((int)dimensions.X + 6, (int)dimensions.Y + 6);
                    spriteBatch.Draw(noPlayers, new Rectangle(point1.X, point1.Y, 32, 32), Color.White);
                }
                else
                {
                    dimensions = Parent.GetDimensions();
                    DrawPlayerHead(Main.player[playerNum], dimensions.X + Left.Pixels, dimensions.Y + Top.Pixels, 1, 1.5f);
                    if (Main.player[playerNum].dead)
                    {
                        spriteBatch.Draw(noPlayers, new Rectangle(point1.X, point1.Y, 32, 32), Color.White);
                    }
                }
            }
            base.DrawSelf(spriteBatch);
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            if (playerNum != -1)
                if (!Main.player[playerNum].dead)
                    TeleportRune.AttemptTP(Main.LocalPlayer, playerNum);

            base.MouseUp(evt);
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            string name = "";

            if (playerNum != -1)
            {
                if (!Main.player[playerNum].dead)
                {
                    name = Main.player[playerNum].name;
                    backgroundTexture = TerraLeague.instance.GetTexture("UI/TeleportBorderSelected");
                }
            }

            TeleportUI.nameText = name;


            base.MouseOver(evt);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            TeleportUI.nameText = "";

            backgroundTexture = TerraLeague.instance.GetTexture("UI/TeleportBorder");

            base.MouseDown(evt);
        }

        void SetTPCooldown()
        {
            bool hasTP = false;
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();

            for (int i = 0; i < modPlayer.sumSpells.Length; i++)
            {
                hasTP = modPlayer.sumSpells[i].Name == "TeleportRune";

                if (hasTP)
                {
                    modPlayer.sumCooldowns[i] = (int)(modPlayer.sumSpells[i].GetCooldown() * 60);
                }
            }
        }

        public static void GetNextPlayer(bool previous)
        {
            List<int> activePlayers = TerraLeague.GetAllPlayersInRange(Main.LocalPlayer.MountedCenter, 99999, Main.LocalPlayer.whoAmI, Main.LocalPlayer.team, true);

            int playerPos = activePlayers.First(x => x == playerNum);

            playerPos += previous ? -1 : 1;

            if (playerPos >= activePlayers.Count)
                playerPos = 0;
            if (playerPos < 0)
                playerPos = activePlayers.Count - 1;

            playerNum = activePlayers[playerPos];
        }

        void DrawPlayerHead(Player drawPlayer, float X, float Y, float Alpha = 1f, float Scale = 1f)
        {
            List<PlayerHeadLayer> drawHeadLayers = PlayerHooks.GetDrawHeadLayers(drawPlayer);
            var spriteBatch = Main.spriteBatch;
            Vector2 position = new Vector2(X - 9, Y - 5);
            short hairDye = drawPlayer.hairDye;

            int helmetDye = 0;
            if (drawPlayer.dye[0] != null)
            {
                helmetDye = drawPlayer.dye[0].dye;
            }

            int faceDye = 0;
            if (drawPlayer.dye[0] != null)
            {
                faceDye = drawPlayer.dye[0].dye;
            }

            bool hairCheck = false;
            if (drawPlayer.head == 10 || drawPlayer.head == 12 || drawPlayer.head == 28 || drawPlayer.head == 62 || drawPlayer.head == 97 || drawPlayer.head == 106 || drawPlayer.head == 113 || drawPlayer.head == 116 || drawPlayer.head == 119 || drawPlayer.head == 133 || drawPlayer.head == 138 || drawPlayer.head == 139 || drawPlayer.head == 163 || drawPlayer.head == 178 || drawPlayer.head == 181 || drawPlayer.head == 191 || drawPlayer.head == 198)
            {
                hairCheck = true;
            }
            bool altHairCheck = false;
            if (drawPlayer.head == 161 || drawPlayer.head == 14 || drawPlayer.head == 15 || drawPlayer.head == 16 || drawPlayer.head == 18 || drawPlayer.head == 21 || drawPlayer.head == 24 || drawPlayer.head == 25 || drawPlayer.head == 26 || drawPlayer.head == 40 || drawPlayer.head == 44 || drawPlayer.head == 51 || drawPlayer.head == 56 || drawPlayer.head == 59 || drawPlayer.head == 60 || drawPlayer.head == 67 || drawPlayer.head == 68 || drawPlayer.head == 69 || drawPlayer.head == 114 || drawPlayer.head == 121 || drawPlayer.head == 126 || drawPlayer.head == 130 || drawPlayer.head == 136 || drawPlayer.head == 140 || drawPlayer.head == 145 || drawPlayer.head == 158 || drawPlayer.head == 159 || drawPlayer.head == 184 || drawPlayer.head == 190 || (double)drawPlayer.head == 92.0 || drawPlayer.head == 195)
            {
                altHairCheck = true;
            }
            ItemLoader.DrawHair(drawPlayer, ref hairCheck, ref altHairCheck);

            if (drawPlayer.head == 0 && hairDye == 0)
            {
                hairDye = 1;
            }

            for (int i = 0; i < drawHeadLayers.Count; i++)
            {
                if (drawHeadLayers[i] != PlayerHeadLayer.Head)
                {
                    if (drawHeadLayers[i] != PlayerHeadLayer.Hair)
                    {
                        if (drawHeadLayers[i] != PlayerHeadLayer.AltHair)
                        {
                            if (drawHeadLayers[i] != PlayerHeadLayer.Armor)
                            {
                                if (drawHeadLayers[i] != PlayerHeadLayer.FaceAcc)
                                {
                                    //drawHeadLayers[i].Draw(ref playerHeadDrawInfo);
                                }
                                else if (drawPlayer.face > 0)
                                {
                                    DrawData value = (drawPlayer.face == 7) ? new DrawData(Main.accFaceTexture[drawPlayer.face], position, new Rectangle(0, 0, 40, 56), new Color(200, 200, 200, 150), 0, Vector2.Zero, Scale, SpriteEffects.None, 0) : new DrawData(Main.accFaceTexture[drawPlayer.face], position, new Rectangle(0, 0, 40, 56), Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                                    GameShaders.Armor.Apply(faceDye, drawPlayer, value);
                                    value.Draw(Main.spriteBatch);
                                    Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                                }
                            }
                            else if (drawPlayer.head == 23)
                            {
                                DrawData HairHeadArmorDrawData;
                                if (!drawPlayer.invis)
                                {
                                    HairHeadArmorDrawData = new DrawData(Main.playerHairTexture[drawPlayer.hair], position, new Rectangle(0, 0, 40, 56), drawPlayer.GetHairColor(false), 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                                    GameShaders.Hair.Apply(hairDye, drawPlayer, HairHeadArmorDrawData);
                                    HairHeadArmorDrawData.Draw(Main.spriteBatch);
                                    Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                                }
                                // Helmet
                                HairHeadArmorDrawData = new DrawData(Main.armorHeadTexture[drawPlayer.head], position, new Rectangle(0, 0, 40, 56), drawPlayer.GetHairColor(false), 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                                GameShaders.Armor.Apply(helmetDye, drawPlayer, HairHeadArmorDrawData);
                                HairHeadArmorDrawData.Draw(Main.spriteBatch);
                                Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                            }
                            else if (drawPlayer.head == 14 || drawPlayer.head == 56 || drawPlayer.head == 158)
                            {
                                // Helmet
                                DrawData HelmetDrawData = new DrawData(Main.armorHeadTexture[drawPlayer.head], position, new Rectangle(0, 0, 40, 56), Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                                GameShaders.Armor.Apply(helmetDye, drawPlayer, HelmetDrawData);
                                HelmetDrawData.Draw(Main.spriteBatch);
                                Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                            }
                            else if (drawPlayer.head > 0 && drawPlayer.head != 28)
                            {
                                // Helmet
                                DrawData HelmetDrawData = new DrawData(Main.armorHeadTexture[drawPlayer.head], position, new Rectangle(0, 0, 40, 56), Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                                GameShaders.Armor.Apply(helmetDye, drawPlayer, HelmetDrawData);
                                HelmetDrawData.Draw(Main.spriteBatch);
                                Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                            }
                            else
                            {
                                DrawData value5 = new DrawData(Main.playerHairTexture[drawPlayer.hair], position, new Rectangle(0, 0, 40, 56), drawPlayer.GetHairColor(false), 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                                GameShaders.Hair.Apply(hairDye, drawPlayer, value5);
                                value5.Draw(Main.spriteBatch);
                                Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                            }
                        }
                        else if (altHairCheck)
                        {
                            DrawData AltHairDrawData = new DrawData(Main.playerHairAltTexture[drawPlayer.hair], position, new Rectangle(0, 0, 40, 56), drawPlayer.GetHairColor(false), 0, Vector2.Zero, Scale, SpriteEffects.None, 0); 
                            GameShaders.Hair.Apply(hairDye, drawPlayer, AltHairDrawData);
                            AltHairDrawData.Draw(Main.spriteBatch);
                            Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                        }

                    }
                    else if (hairCheck)
                    {
                        DrawData value7 = new DrawData(Main.armorHeadTexture[drawPlayer.head], position, new Rectangle(0, 0, 40, 56), drawPlayer.GetHairColor(false), 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        GameShaders.Armor.Apply(helmetDye, drawPlayer, value7);
                        value7.Draw(Main.spriteBatch);
                        Main.pixelShader.CurrentTechnique.Passes[0].Apply();

                        value7 = new DrawData(Main.playerHairTexture[drawPlayer.hair], position, new Rectangle(0, 0, 40, 56), drawPlayer.GetHairColor(false), 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                        GameShaders.Hair.Apply(hairDye, drawPlayer, value7);
                        value7.Draw(Main.spriteBatch);
                        Main.pixelShader.CurrentTechnique.Passes[0].Apply();
                    }
                }
                else if (drawPlayer.head != 38 && drawPlayer.head != 135 && ItemLoader.DrawHead(drawPlayer))
                {
                    var skinVariant = drawPlayer.skinVariant;
                    Main.spriteBatch.Draw(Main.playerTextures[skinVariant, 0], position, new Rectangle(0, 0, 40, 56), drawPlayer.skinColor, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(Main.playerTextures[skinVariant, 1], position, new Rectangle(0, 0, 40, 56), Color.White, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                    Main.spriteBatch.Draw(Main.playerTextures[skinVariant, 2], position, new Rectangle(0, 0, 40, 56), drawPlayer.eyeColor, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
                }
            }
        }

        void LoadAccFace(int i)
        {
            if (!Main.accFaceLoaded[i])
            {
                Main.accFaceTexture[i] = Main.instance.OurLoad<Texture2D>("Images/Acc_Face_" + i.ToString());
                Main.accFaceLoaded[i] = true;
            }
        }

        void LoadHair(int i)
        {
            if (!Main.hairLoaded[i])
            {
                Texture2D[] array = Main.playerHairTexture;
                char directorySeparatorChar = Path.DirectorySeparatorChar;
                string str = directorySeparatorChar.ToString();
                int num = i + 1;
                array[i] = Main.instance.OurLoad<Texture2D>("Images" + str + "Player_Hair_" + num.ToString());
                Texture2D[] array2 = Main.playerHairAltTexture;
                directorySeparatorChar = Path.DirectorySeparatorChar;
                string str2 = directorySeparatorChar.ToString();
                num = i + 1;
                array2[i] = Main.instance.OurLoad<Texture2D>("Images" + str2 + "Player_HairAlt_" + num.ToString());
                Main.hairLoaded[i] = true;
            }
        }

        void LoadArmorHead(int i)
        {
            if (!Main.armorHeadLoaded[i])
            {
                Main.armorHeadTexture[i] = Main.instance.OurLoad<Texture2D>("Images" + Path.DirectorySeparatorChar.ToString() + "Armor_Head_" + i.ToString());
                Main.armorHeadLoaded[i] = true;
            }
        }

        Color quickAlpha(Color oldColor, float alpha)
        {
            Color result = oldColor;
            result.R = (byte)((float)(int)result.R * alpha);
            result.G = (byte)((float)(int)result.G * alpha);
            result.B = (byte)((float)(int)result.B * alpha);
            result.A = (byte)((float)(int)result.A * alpha);
            return result;
        }
    }

    class ArrowButton : UIState
    {
        public bool previous;
        Texture2D backgroundTexture;

        int width = 44;
        int height = 44;

        public ArrowButton(bool Previous)
        {
            previous = Previous;

            Width.Set(width, 0);
            Height.Set(height, 0);
        }

        public override void OnInitialize()
        {
            if (previous)
                backgroundTexture = TerraLeague.instance.GetTexture("UI/LeftButton");
            else
                backgroundTexture = TerraLeague.instance.GetTexture("UI/RightButton");

            if (backgroundTexture != null)
            {
                width = backgroundTexture.Width;
                height = backgroundTexture.Height;
            }
            else
            {
                width = 44;
                height = 44;
            }

            Width.Set(width, 0);
            Height.Set(height, 0);

            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {

            base.Update(gameTime);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (TeleportUI.visible)
            {
                CalculatedStyle dimensions = this.GetDimensions();
                Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
                spriteBatch.Draw(backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), Color.White);
            }
            base.DrawSelf(spriteBatch);
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            if (PlayerTeleportButton.playerNum != -1)
            {
                if (previous)
                    backgroundTexture = TerraLeague.instance.GetTexture("UI/LeftButtonPressed");
                else
                    backgroundTexture = TerraLeague.instance.GetTexture("UI/RightButtonPressed");
            }

            base.MouseDown(evt);
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            if (previous)
                backgroundTexture = TerraLeague.instance.GetTexture("UI/LeftButton");
            else
                backgroundTexture = TerraLeague.instance.GetTexture("UI/RightButton");

            if (PlayerTeleportButton.playerNum != -1)
            {
                PlayerTeleportButton.GetNextPlayer(previous);
            }

            base.MouseUp(evt);
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            //string name = "";

            //TeleportUI.nameText = name;

            //base.MouseOver(evt);
        }

        public override void MouseOut(UIMouseEvent evt)
        {
            //TeleportUI.nameText = "";

            //backgroundTexture = TerraLeague.instance.GetTexture("UI/TeleportBorder");

            //base.MouseDown(evt);
        }
    }

    public enum TeleportType
    {
        LeftBeach,
        RightBeach,
        Dungeon,
        Hell,
        Random,
    }
}