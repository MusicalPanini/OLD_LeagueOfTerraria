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
            return "Heal you self and a near by player for 30% of your max life" +
                "\nYou both gain 'Swiftness'";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.lifeToHeal += (int)((player.statLifeMax2 * 0.3) * modPlayer.healPower);
            player.AddBuff(BuffID.Swiftness, 360);

            int target = -1;
            float distance = 700f;
            for (int k = 0; k < 200; k++)
            {
                if (Main.player[k].active && k != player.whoAmI)
                {
                    float distanceCheck = player.Distance(Main.player[k].Center);

                    if (distanceCheck < distance)
                    {
                        distance = distanceCheck;
                        target = k;
                    }
                }
            }

            if (target != -1 && Main.netMode == NetmodeID.MultiplayerClient && Main.player[target].active)
            {
                modPlayer.SendHealPacket((int)((Main.player[target].statLifeMax2 * 0.4) * modPlayer.healPower), target, -1, player.whoAmI);
                modPlayer.SendBuffPacket(BuffID.Swiftness, 360, target, -1, player.whoAmI);
                PacketHandler.SendHeal(-1, player.whoAmI, player.whoAmI, target);
                Efx(Main.player[target], false);
                
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
