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
    public class ToxicCask : Ability
    {
        public ToxicCask(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Toxic Cask";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/VenomCask";
        }

        public override string GetAbilityTooltip()
        {
            return "Throw a cask that releases clouds of toxic gas that apply stacks of 'Deadly Venom'";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(7 * Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().rangedDamageLastStep);
        }

        public override int GetRawCooldown()
        {
            return 11;
        }

        public override int GetBaseManaCost()
        {
            return 20;
        }

        public override string GetDamageTooltip(Player player)
        {
            return "";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost()))
            {
                Vector2 position = player.Center;
                Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 12f);
                int projType = ProjectileType<ChemCrossbow_VenomCask>();
                int damage = 5;
                int knockback = 0;

                Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);

                DoEfx(player, type);
                SetCooldowns(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(SoundID.Item20, player.Center);
        }
    }
}
