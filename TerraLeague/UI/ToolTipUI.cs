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

namespace TerraLeague.UI
{
    public class ToolTipUI : UIState
    {
        public static int MaxLines = 16;
        public int itemType;
        public bool drawText = false;
        UIToolTipLine[] uiLines = new UIToolTipLine[MaxLines];
        string[] tooltip = new string[16];

        public ToolTipUI()
        {
            Left.Set(0, 0f);
            Top.Set(0, 0f);
            Width.Set(500, 0f);

            for (int i = 0; i < uiLines.Length; i++)
            {
                uiLines[i] = new UIToolTipLine(0, (28 * i), i.ToString());
                Append(uiLines[i]);
            }
        }

        public void DrawText(string[] tooltipContents)
        {
            int lines = 0;
            for (int i = 0; i < tooltipContents.Length; i++)
            {
                lines += (tooltipContents[i] != null ? 1 : 0);
            }
            int dif = MaxLines - lines;
            for (int i = MaxLines-1; i >= 0; i--)
            {
                if (i - dif >= 0)
                    tooltip[i] = tooltipContents[i - dif];
                else
                    tooltip[i] = "";
            }

            drawText = true;
        }

        public override void Update(GameTime gameTime)
        {
            Left.Set(0, 0f);
            Top.Set(0, 0f);

            for (int i = 0; i < uiLines.Length; i++)
            {
                uiLines[i].Top.Set((Main.screenHeight - 620) + (28 * i), 0);
                uiLines[i].Left.Set(Main.screenWidth/2 - 760, 0);
                uiLines[i].VAlign = 0;
                uiLines[i].HAlign = 0;

                
                uiLines[i].TextColor = TerraLeague.PulseText(Color.White);
            }

            if (drawText)
            {
                try
                {
                    for (int i = 0; i < MaxLines; i++)
                    {
                        if (tooltip[i] != null || tooltip[i].TrimStart(' ') != "")
                        {
                            uiLines[i].SetText("                                                                         " + tooltip[i].TrimStart(' '));
                        }
                    }
                }
                catch (Exception)
                {
                    for (int i = 0; i < uiLines.Length; i++)
                    {
                        uiLines[i].SetText("");
                    }
                }
                
            }
            else
            {
                for (int i = 0; i < uiLines.Length; i++)
                {
                    uiLines[i].SetText("");
                }
            }

            drawText = false;

            Recalculate();
        }
    }

    class UIToolTipLine : UIText
    {
        public UIToolTipLine(int left, int top, string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            SetText(text,textScale, large);
            Left.Set(left, 0f);
            Top.Set(top, 0f);
            Width.Set(500, 0f);
            Height.Set(28, 0);
        }
    }
}