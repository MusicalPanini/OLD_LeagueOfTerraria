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
    public class Phase : Ability
    {
        LunariGunType gunType;

        public Phase(Terraria.ModLoader.ModItem item, LunariGunType lunariGunType)
        {
            abilityItem = item;
            gunType = lunariGunType;
        }

        public override string GetAbilityName()
        {
            return "Phase";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/Phase";
        }

        public override string GetAbilityTooltip()
        {
            switch (gunType)
            {
                case LunariGunType.Cal:
                    return "Swap weapon to Severum";
                case LunariGunType.Sev:
                    return "Swap weapon to Gravitum";
                case LunariGunType.Grv:
                    return "Swap weapon to Infernum";
                case LunariGunType.Inf:
                    return "Swap weapon to Crescendum";
                case LunariGunType.Cre:
                    return "Swap weapon to Calibrum";
                default:
                    return "Swap weapon to ???";
            }
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MAG:
                    return 100;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 1;
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type))
            {
                switch (gunType)
                {
                    case LunariGunType.Cal:
                        if (ChangeWeapon(player, ItemType<Calibrum>(), ItemType<Severum>()))
                        {
                            CombatText.NewText(player.Hitbox, new Color(216, 0, 32), "SEVERUM");
                            DoEfx(player, type);
                            SetCooldowns(player, type);
                        }
                        //abilityItem.item.SetDefaults(ItemType<Severum>());
                        break;
                    case LunariGunType.Sev:
                        //abilityItem.item.SetDefaults(ItemType<Gravitum>());
                        if (ChangeWeapon(player, ItemType<Severum>(), ItemType<Gravitum>()))
                        {
                            CombatText.NewText(player.Hitbox, new Color(200, 37, 255), "GRAVITUM");
                            DoEfx(player, type);
                            SetCooldowns(player, type);
                        }
                        break;
                    case LunariGunType.Grv:
                        //player.inventory[0].SetDefaults(ItemType<Infernum>());
                        if (ChangeWeapon(player, ItemType<Gravitum>(), ItemType<Infernum>()))
                        {
                            CombatText.NewText(player.Hitbox, new Color(0, 148, 255), "INFERNUM");
                            DoEfx(player, type);
                            SetCooldowns(player, type);
                        }
                        break;
                    case LunariGunType.Inf:
                        //abilityItem.item.SetDefaults(ItemType<Crescendum>());
                        if (ChangeWeapon(player, ItemType<Infernum>(), ItemType<Crescendum>()))
                        {
                            CombatText.NewText(player.Hitbox, new Color(255, 255, 255), "CRESCENDUM");
                            DoEfx(player, type);
                            SetCooldowns(player, type);
                        }
                        break;
                    case LunariGunType.Cre:
                        //abilityItem.item.SetDefaults(ItemType<Calibrum>());
                        if (ChangeWeapon(player, ItemType<Crescendum>(), ItemType<Calibrum>()))
                        {
                            CombatText.NewText(player.Hitbox, new Color(141, 252, 245), "CALIBRUM");
                            DoEfx(player, type);
                            SetCooldowns(player, type);
                        }
                        break;
                    default:
                        break;
                }

            }
        }

        bool ChangeWeapon(Player player, int currentItem, int newItem)
        {
            if (player.HeldItem.type == currentItem && player.HeldItem.type != player.inventory[58].type)
            {
                player.HeldItem.SetDefaults(newItem);
                return true;
            }
            else
            {
                for (int i = 0; i < player.inventory.Length; i++)
                {
                    if (player.inventory[i].type == currentItem && i != 58)
                    {
                        player.inventory[i].SetDefaults(newItem);
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public enum LunariGunType
    {
        Cal,
        Sev,
        Grv,
        Inf,
        Cre
    }
}
