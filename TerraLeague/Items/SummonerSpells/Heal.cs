﻿using Microsoft.Xna.Framework;
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
            return "Heal you self and a nearby ally for " + GetPercentScalingAmount() + "% of your max life" +
                "Can target an ally to prioritize who gets healed" +
                "\nYou both gain 'Swiftness'";
        }

        public int GetPercentScalingAmount()
        {
            return 30;
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.lifeToHeal += (int)((player.statLifeMax2 * GetPercentScalingAmount() * 0.01) * modPlayer.healPower);
            player.AddBuff(BuffID.Swiftness, 360);

            // For Server
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                int healTarget = TerraLeague.GetClosestPlayer(player.MountedCenter, effectRadius, player.whoAmI, player.team, TerraLeague.PlayerMouseIsHovering(30, player.whoAmI, player.team));

                if (healTarget != -1)
                {
                    modPlayer.SendHealPacket((int)((player.statLifeMax2 * GetPercentScalingAmount() * 0.01) * modPlayer.healPower), healTarget, -1, player.whoAmI);
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
