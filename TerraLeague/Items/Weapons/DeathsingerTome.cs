using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.NPCs;
using TerraLeague.Projectiles;
using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using static Terraria.ModLoader.ModContent;
using Terraria.Audio;

namespace TerraLeague.Items.Weapons
{
    public class DeathsingerTome : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Death Singer's Tome");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Detonate the targeted area after 0.5 seconds" +
                "\nIf you only hit 1 enemy, the damage is doubled";
        }

        public override string GetQuote()
        {
            return "I am the Nightbringer~";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.E)
                return "Defile";
            else if (type == AbilityType.R)
                return "Requiem";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.E)
                return "AbilityImages/Defile";
            else if (type == AbilityType.R)
                return "AbilityImages/Requiem";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.E)
            {
                return "Curse the area around you";
            }
            else if (type == AbilityType.R)
            {
                return "Channel for 4 seconds then deal damage to all enemy npc's in the world";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return (int)(item.damage * 0.6);
            else if (type == AbilityType.R)
                return (int)(2.5 * item.damage);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            if (type == AbilityType.E)
            {
                if (dam == DamageType.MAG)
                    return 20;
            }
            else if (type == AbilityType.R)
            {
                if (dam == DamageType.MAG)
                    return 125;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.E)
                return 75;
            else if (type == AbilityType.R)
                return 200;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else if (type == AbilityType.R)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MAG) + " magic damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.E)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.E)
                return 8;
            else if (type == AbilityType.R)
                return 90;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.E)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    Vector2 position = player.MountedCenter;
                    Vector2 velocity = Vector2.Zero;
                    int projType = ProjectileType<DeathTomeAura>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MAG);
                    int knockback = 0;

                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.R)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    List<NPC> npcs = Main.npc.OfType<NPC>().ToList();
                    if (npcs.Count != 0)
                    {
                        player.GetModPlayer<PLAYERGLOBAL>().TaggedNPC = npcs;
                        for (int i = 0; i < npcs.Count - 1; i++)
                        {
                            if (!npcs[i].townNPC)
                            {
                                npcs[i].AddBuff(BuffType<Requiem>(), 240);
                            }
                        }
                    }

                    player.AddBuff(BuffType<RequiemChannel>(), 240);
                    SetCooldowns(player, type);
                }
            }
            else
            {
                base.DoEffect(player, type);
            }
        }

        public override void SetDefaults()
        {
            item.damage = 28;
            item.noMelee = true;
            item.magic = true;
            item.mana = 10;
            item.rare = 3;
            item.value = 10000;
            item.width = 28;
            item.height = 32;
            item.useTime = 35;
            item.useAnimation = 35;
            item.UseSound = new LegacySoundStyle(2,8);
            item.useStyle = 5;
            item.shoot = ProjectileType<DeathTomeShot>();
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectileDirect(Main.MouseWorld, Vector2.Zero, type, damage, 0, player.whoAmI, Main.MouseWorld.X, Main.MouseWorld.Y);

            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Book, 1);
            recipe.AddIngredient(ItemType<DamnedSoul>(), 50);
            recipe.AddIngredient(ItemID.HellstoneBar, 20);
            recipe.AddIngredient(ItemID.Bone, 50);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.E || type == AbilityType.R)
                return true;
            return base.GetIfAbilityExists(type);
        }
    }
}
