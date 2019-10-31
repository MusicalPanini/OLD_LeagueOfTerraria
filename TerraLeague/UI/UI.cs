using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using TerraLeague;
using Terraria.UI;
using System;
using Terraria.ID;
using System.Linq;

namespace TerraLeague.UI
{
    internal class StatUI : UIState
    {

        public UIPanel Panel;
        public static int visible = 0;
        public static bool panelLocked = false;
        public override void OnInitialize()
        {
            Panel = new UIPanel();
            Panel.SetPadding(0);
            Panel.Left.Set(1400f, 0f);
            Panel.Top.Set(5f, 0f);
            Panel.Width.Set(170f, 0f);
            Panel.Height.Set(70f, 0f);
            Panel.BackgroundColor = new Color(73, 94, 171);

            Panel.OnMouseDown += new UIElement.MouseEvent(DragStart);
            Panel.OnMouseUp += new UIElement.MouseEvent(DragEnd);

            Stats stats = new Stats(300, 400);
            stats.Left.Set(5, 0f);
            stats.Top.Set(5, 0f);
            Panel.Width.Pixels = 200;
            Panel.Append(stats);
            base.Append(Panel);
            Recalculate();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Panel.Left.Set(Main.screenWidth - (Panel.Width.Pixels + 310 ), 0f);
            Panel.Top.Set(5f, 0f);

            Recalculate();
        }

        Vector2 offset;
        public bool dragging = false;
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!panelLocked)
            {
                offset = new Vector2(evt.MousePosition.X - Panel.Left.Pixels, evt.MousePosition.Y - Panel.Top.Pixels);
                dragging = true;
            }
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 MousePosition = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            if (Panel.ContainsPoint(MousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
        }

        public override void MiddleClick(UIMouseEvent evt)
        {
        }
    }

    class Stats : UIElement
    {
        private UIText text;
        private float width;
        private float height;
        int rowsLastCycle = 0;
        public Stats(int height, int width)
        {
            this.width = width;
            this.height = height;
        }

        public override void OnInitialize()
        {
            Height.Set(height, 0f);
            Width.Set(width, 0f);   

            text = new UIText("0|0"); 
            text.Width.Set(width, 0f);
            text.Height.Set(height, 0f);
            text.SetPadding(10);
            base.Append(text);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            width = 420;
            text.Width.Pixels = 420;
            Parent.Width.Pixels = 210;
            text.OverflowHidden = true;
            int rows = 0;
            text.TextColor = new Color(255, 255, 255, 0);
            Player player = Main.player[Main.myPlayer];

            string stattext = "         Stats           \n";
            stattext += "Armor: " + (player.GetModPlayer<PLAYERGLOBAL>().armor + "\n");
            stattext += "Resist: " + (player.GetModPlayer<PLAYERGLOBAL>().resist + "\n");
            rows += 2;
            if (100 - player.GetModPlayer<PLAYERGLOBAL>().Cdr * 100 != 0)
            {
                rows++;
                stattext += "CDR: " + (100 - player.GetModPlayer<PLAYERGLOBAL>().Cdr * 100 + "%\n");
            }
            rows++;
            if (player.GetModPlayer<PLAYERGLOBAL>().ConsumeAmmoChance * 100 != 0)
            {
                rows++;
                stattext += "Ammo: " + (player.GetModPlayer<PLAYERGLOBAL>().ConsumeAmmoChance * 100) + "%\n";
            }
            if (player.GetModPlayer<PLAYERGLOBAL>().healPower != 1)
            {
                rows++;
                stattext += "Heal Power: " + ((int)(player.GetModPlayer<PLAYERGLOBAL>().healPower * 100) + "%\n");
            }

            text.SetText(stattext);

            height = 12 * rows;
            Parent.Height.Pixels = 27f * rows + 25;
            if (rowsLastCycle != rows)
            {
                Recalculate();
                Parent.Recalculate();
            }
            rowsLastCycle = rows;
            base.Update(gameTime);
        }
    }
}