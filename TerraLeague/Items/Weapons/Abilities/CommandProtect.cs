using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class CommandProtect : Ability
    {
        public CommandProtect(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Command: Protect";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/CommandProtect";
        }

        public override string GetAbilityTooltip()
        {
            return "Shield yourself and nearby allies and increase their armor and resist by 15 for 6 seconds";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 0.4f);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.SUM:
                    return 60;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 24;
        }

        public override int GetBaseManaCost()
        {
            return 75;
        }

        public override string GetDamageTooltip(Player player)
        {
            return TerraLeague.CreateScalingTooltip(DamageType.NONE, GetAbilityBaseDamage(player), 100, true) + " + " + GetScalingTooltip(player, DamageType.SUM, true) + " shielding";
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

                int shield = modPlayer.ScaleValueWithHealPower(GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.SUM));
                int duration = 60 * 6;

                DoEfx(player, type);
                player.AddBuff(BuffType<Buffs.CommandProtect>(), duration);
                player.GetModPlayer<PLAYERGLOBAL>().AddShield(shield, duration, new Color(102, 243, 255), ShieldType.Basic);

                // For Server
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    var players = TerraLeague.GetAllPlayersInRange(player.MountedCenter, 300, player.whoAmI, player.team);

                    for (int i = 0; i < players.Count; i++)
                    {
                        modPlayer.SendShieldPacket(shield, players[i], ShieldType.Basic, duration, -1, player.whoAmI, new Color(102, 243, 255));
                        modPlayer.SendBuffPacket(BuffType<Buffs.CommandProtect>(), duration, players[i], -1, player.whoAmI);
                    }
                }

                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f), player.Center);
            TerraLeague.DustBorderRing(300, player.MountedCenter, 226, default(Color), 2);
            TerraLeague.DustRing(226, player, default(Color));
        }
    }
}
