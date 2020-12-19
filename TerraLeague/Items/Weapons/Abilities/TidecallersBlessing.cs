using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Dusts;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class TidecallersBlessing : Ability
    {
        public TidecallersBlessing(ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Tidecaller's Blessing";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/TidecallersBlessing";
        }

        public override string GetAbilityTooltip()
        {
            return "Bless yourself and all nearby allies for 6 seconds with 10% increased movement speed" +
                "\nTheir attacks will slow and gain On Hit damage";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 0.5f);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 40;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 14;
        }

        public override int GetBaseManaCost()
        {
            return 40;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " On Hit damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

                int buffDamage = modPlayer.ScaleValueWithHealPower(GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG));

                DoEfx(player, type);
                player.AddBuff(BuffType<Buffs.TidecallersBlessing>(), buffDamage);

                // For Server
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    var players = TerraLeague.GetAllPlayersInRange(player.MountedCenter, 300, player.whoAmI, player.team);

                    for (int i = 0; i < players.Count; i++)
                    {
                        modPlayer.SendBuffPacket(BuffType<Buffs.TidecallersBlessing>(), buffDamage, players[i], -1, player.whoAmI);
                    }
                }

                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 21).WithPitchVariance(-0.5f), player.Center);
            TerraLeague.DustBorderRing(300, player.MountedCenter, DustType<BubbledBubble>(), default(Color), 2);
            TerraLeague.DustRing(DustType<BubbledBubble>(), player, default(Color));

            var players = TerraLeague.GetAllPlayersInRange(player.MountedCenter, 300, -1, player.team);
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustType<BubbledBubble>(), 0, -2, 0, default, 1);
                }
            }
        }
    }
}
