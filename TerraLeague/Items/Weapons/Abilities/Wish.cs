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
    public class Wish : Ability
    {
        public Wish(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Wish";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Wish";
        }

        public override string GetAbilityTooltip()
        {
            return "Heal all allies, wherever they are." +
                    "\nHealing is increased by 50% if target is below 40% life.";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 3);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 50;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 90;
        }

        public override int GetBaseManaCost()
        {
            return 200;
        }

        public override string GetDamageTooltip(Player player)
        {
            return TerraLeague.CreateScalingTooltip(DamageType.NONE, GetAbilityBaseDamage(player), 100, true) + " + " + GetScalingTooltip(player, DamageType.MAG, true) + " healing";
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

                int userHeal = modPlayer.ScaleValueWithHealPower(GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG));
                if (modPlayer.GetRealHeathWithoutShield(false) < modPlayer.GetRealHeathWithoutShield(true) * 0.4)
                    userHeal = (int)(userHeal * 1.5);
                modPlayer.lifeToHeal += userHeal;

                // For Server
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    var players = TerraLeague.GetAllPlayersInRange(player.MountedCenter, 999999, player.whoAmI, player.team);

                    for (int i = 0; i < players.Count; i++)
                    {
                        PLAYERGLOBAL targetModPlayer = Main.player[players[i]].GetModPlayer<PLAYERGLOBAL>();

                        int Heal = modPlayer.ScaleValueWithHealPower(GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG));
                        if (targetModPlayer.GetRealHeathWithoutShield(false) < targetModPlayer.GetRealHeathWithoutShield(true) * 0.4)
                            Heal = (int)(Heal * 1.5);
                        modPlayer.SendHealPacket(Heal, players[i], -1, player.whoAmI);
                    }
                }

                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            var players = TerraLeague.GetAllPlayersInRange(player.MountedCenter, 999999, -1, player.team);

            for (int i = 0; i < players.Count; i++)
            {
                Player targetPlayer = Main.player[players[i]];
                for (int j = 0; j < 18; j++)
                {
                    Dust dust = Dust.NewDustDirect(new Vector2(Main.rand.Next((int)targetPlayer.position.X - 8, (int)targetPlayer.position.X + 8), targetPlayer.position.Y + 16), targetPlayer.width, targetPlayer.height, 261, 0, -Main.rand.Next(6, 18), 0, new Color(0, 255, 0, 0), Main.rand.Next(2, 6));
                    dust.noGravity = true;
                }
            }

            Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f), player.MountedCenter);
        }
    }
}
