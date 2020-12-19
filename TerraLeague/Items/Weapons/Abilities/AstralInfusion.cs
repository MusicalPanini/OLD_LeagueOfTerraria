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
    public class AstralInfusion : Ability
    {
        public AstralInfusion(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Astral Infusion";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/AsteralInfusion";
        }

        public override string GetAbilityTooltip()
        {
            return "Costs 10% of your Max Life per cast" +
                "\nCan't be cast below 15% life." +
                "\nTarget an ally and heal them.";
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
                    return 60;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 6;
        }

        public override int GetBaseManaCost()
        {
            return 40;
        }

        public override string GetDamageTooltip(Player player)
        {
            return TerraLeague.CreateScalingTooltip(DamageType.NONE, GetAbilityBaseDamage(player), 100, true) + " + " + GetScalingTooltip(player, DamageType.MAG, true) + " healing";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override bool CanCurrentlyBeCast(Player player)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return modPlayer.GetRealHeathWithoutShield() / (float)modPlayer.GetRealHeathWithoutShield(true) > 0.15f;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            int target = TerraLeague.PlayerMouseIsHovering(30, player.whoAmI, player.team);
            if (target != -1)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(), true))
                {
                    player.statLife -= player.GetModPlayer<PLAYERGLOBAL>().GetRealHeathWithoutShield(true) / 10;

                    Vector2 position = player.position;
                    Vector2 velocity = new Vector2(0, 0);
                    int projType = ProjectileType<Item_Heal>();
                    int healing = player.GetModPlayer<PLAYERGLOBAL>().ScaleValueWithHealPower(GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG));
                    int knockback = 0;

                    SetAnimation(player, 10, 10, Main.player[target].Center);
                    Projectile.NewProjectile(position, velocity, projType, healing, knockback, player.whoAmI, target);
                    SetCooldowns(player, type);
                }
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(SoundID.Item20, player.Center);
        }
    }
}
