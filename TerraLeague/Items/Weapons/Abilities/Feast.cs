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
    public class Feast : Ability
    {
        public Feast(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Feast";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Feast";
        }

        public override string GetAbilityTooltip()
        {
            return "Take a giant bite out of everything around you." +
                    "\nKilling an enemy will take there max life as stacks." +
                    "\nExecute any enemy hit with less health than your stacks." +
                    "\nGain a buff based on the amount of stacks you currently have." +
                    "\nLose all stacks on death." +
                    "\nTier 1: 500 | Tier 2: 2500 | Tier 3: 12500" +
                    "\nCurrent Stacks: " + Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().feastStacks;
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 1.5);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 70;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 70;
        }

        public override int GetBaseManaCost()
        {
            return 100;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MAG) + " + " +
                TerraLeague.CreateScalingTooltip(UI.HealthbarUI.RedHealthColor.Hex3(), "LIFE", player.GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep, 10) + " magic damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetBaseManaCost(), true))
            {
                Vector2 position = player.MountedCenter;
                Vector2 velocity = Vector2.Zero;
                int damage = GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MAG) + player.GetModPlayer<PLAYERGLOBAL>().maxLifeLastStep / 10;
                int knockback = 2;

                PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
                int stacks = 0;
                if (modPlayer.feast1)
                    stacks = 1;
                else if (modPlayer.feast2)
                    stacks = 2;
                else if (modPlayer.feast3)
                    stacks = 3;


                Projectile.NewProjectileDirect(position, velocity, ProjectileType<TerrorOfTheVoid_FeastTop>(), damage, knockback, player.whoAmI, stacks);
                Projectile.NewProjectileDirect(position, velocity, ProjectileType<TerrorOfTheVoid_FeastBot>(), damage, knockback, player.whoAmI, stacks);

                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position, 32, 32, DustID.Blood, 0f, -2f, 100, default(Color), 3f);
                dust.velocity *= 2;
            }
        }
    }
}
