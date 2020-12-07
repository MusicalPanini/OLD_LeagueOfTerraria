using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace TerraLeague.Items.SummonerSpells
{
    public class HealRune : SummonerSpell
    {
        static int effectRadius = 700;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Heal Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Heal";
        }

        public override string GetSpellName()
        {
            return "Heal";
        }

        public override int GetRawCooldown()
        {
            return 210;
        }
        public override string GetTooltip()
        {
            return "Heal you self and a nearby ally for " + TerraLeague.CreateScalingTooltip(UI.HealthbarUI.RedHealthColor.Hex3(), "LIFE", Main.LocalPlayer.statManaMax2, 20, true) +
                "\nCan target an ally to prioritize who gets healed" +
                "\nYou both gain 'Swiftness'";
        }

        public int GetPercentScalingAmount()
        {
            return 30;
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            int healValue = modPlayer.ScaleValueWithHealPower(modPlayer.GetRealHeathWithoutShield(true) * GetPercentScalingAmount() * 0.01f);

            modPlayer.lifeToHeal += healValue;
            player.AddBuff(BuffID.Swiftness, 360);

            // For Server
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                int healTarget = TerraLeague.GetClosestPlayer(player.MountedCenter, effectRadius, player.whoAmI, player.team, TerraLeague.PlayerMouseIsHovering(30, player.whoAmI, player.team));

                if (healTarget != -1)
                {
                    modPlayer.SendHealPacket(healValue, healTarget, -1, player.whoAmI);
                    modPlayer.SendBuffPacket(BuffID.Swiftness, 360, healTarget, -1, player.whoAmI);
                    PacketHandler.SendHeal(-1, player.whoAmI, player.whoAmI, healTarget);
                    Efx(Main.player[healTarget], false);
                }
            }

            Efx(player);
            PacketHandler.SendHeal(-1, player.whoAmI, player.whoAmI, player.whoAmI);

            SetCooldowns(player, spellSlot);
        }

        static public void Efx(Player player, bool playSound = true)
        {
            Color color = new Color(0, 255, 100);
            TerraLeague.DustElipce(110, 110, 0, player.MountedCenter, 261, color, 2, 180, true, 0.02f);
            int length = 80;

            Vector2 PointA = player.MountedCenter + new Vector2(-length/4, -length);
            Vector2 PointB = player.MountedCenter + new Vector2(length/4, -length);

            Vector2 PointC = player.MountedCenter + new Vector2(length/4, -length / 4);

            Vector2 PointD = player.MountedCenter + new Vector2(length, -length / 4);
            Vector2 PointE = player.MountedCenter + new Vector2(length, length / 4);

            Vector2 PointF = player.MountedCenter + new Vector2(length/4, length / 4);

            Vector2 PointG = player.MountedCenter + new Vector2(length/4, length);
            Vector2 PointH = player.MountedCenter + new Vector2(-length/4, length);

            Vector2 PointI = player.MountedCenter + new Vector2(-length/4, length / 4);

            Vector2 PointJ = player.MountedCenter + new Vector2(-length, length / 4);
            Vector2 PointK = player.MountedCenter + new Vector2(-length, -length / 4);

            Vector2 PointL = player.MountedCenter + new Vector2(-length/4, -length / 4);

            TerraLeague.DustLine(PointA, PointB, 261, 1, 2, color);
            TerraLeague.DustLine(PointB, PointC, 261, 1, 2, color);
            TerraLeague.DustLine(PointC, PointD, 261, 1, 2, color);
            TerraLeague.DustLine(PointD, PointE, 261, 1, 2, color);
            TerraLeague.DustLine(PointE, PointF, 261, 1, 2, color);
            TerraLeague.DustLine(PointF, PointG, 261, 1, 2, color);
            TerraLeague.DustLine(PointG, PointH, 261, 1, 2, color);
            TerraLeague.DustLine(PointH, PointI, 261, 1, 2, color);
            TerraLeague.DustLine(PointI, PointJ, 261, 1, 2, color);
            TerraLeague.DustLine(PointJ, PointK, 261, 1, 2, color);
            TerraLeague.DustLine(PointK, PointL, 261, 1, 2, color);
            TerraLeague.DustLine(PointL, PointA, 261, 1, 2, color);

            for (int j = 0; j < 18; j++)
            {
                Dust dust = Dust.NewDustDirect(new Vector2(Main.rand.Next((int)player.position.X - 8, (int)player.position.X + 8), player.position.Y + 16), player.width, player.height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(0, 255, 0, 0), Main.rand.Next(2, 3));
                dust.noGravity = true;
            }

            if (playSound)
            {
                Main.PlaySound(new LegacySoundStyle(2, 29), player.Center);
            }
        }
    }
}
