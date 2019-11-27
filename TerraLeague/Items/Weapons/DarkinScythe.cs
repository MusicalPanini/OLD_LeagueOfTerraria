using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DarkinScythe : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkin Scythe");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "Attacks will mark enemies";
        }

        public override string GetQuote()
        {
            return "From the shadow comes the slaughter";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.R)
                return "Umbral Trespass";
            else if (type == AbilityType.Q)
                return "Reaping Slash";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.R)
                return "AbilityImages/UmbralTrespass";
            else if (type == AbilityType.Q)
                return "AbilityImages/ReapingSlash";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.R)
            {
                return "Become invulnerable and infest a marked enemy for 4 seconds" +
                "\nAfter 1 second, you can recast to leave the enemy early";
            }
            else if (type == AbilityType.Q)
            {
                return "Lunge towards the curser, swinging the scythe around you";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return 0;
            else if (type == AbilityType.Q)
                return (int)(item.damage * 2);
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetAbilityScalingAmount(Player player, AbilityType type, DamageType dam)
        {
            PLAYERGLOBAL modPlayer = Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>();
            if (type == AbilityType.Q)
            {
                if (dam == DamageType.MEL)
                    return 65;
            }
            return base.GetAbilityScalingAmount(player, type, dam);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.R)
                return 150;
            else if (type == AbilityType.Q)
                return 30;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return "";
            else if (type == AbilityType.Q)
                return GetAbilityBaseDamage(player, type) + " + " + GetScalingTooltip(player, type, DamageType.MEL) + " melee damage";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.R)
                return true;
            else
                return false;
        }

        public override bool CurrentlyHasSpecialCast(Player player, AbilityType type)
        {
            if (type == AbilityType.R && player.HasBuff(BuffType<UmbralTrespassing>()) && player.GetModPlayer<PLAYERGLOBAL>().AbilityCooldowns[3] <= GetCooldown(type) * 60 - 60)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.R)
                return 45;
            else if (type == AbilityType.Q)
                return 12;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type)) && !player.HasBuff(BuffType<UmbralTrespassing>()))
                {
                    List<NPC> npcs = Main.npc.OfType<NPC>().ToList();
                    if (npcs.Count != 0)
                    {
                        for (int i = 0; i < npcs.Count; i++)
                        {
                            if (npcs[i].Hitbox.Intersects(new Rectangle((int)Main.MouseWorld.X - 5, (int)Main.MouseWorld.Y - 5, 10, 10)) && npcs[i].GetGlobalNPC<NPCs.NPCsGLOBAL>().umbralTrespass && npcs[i].aiStyle != 91 && player.CheckMana(GetBaseManaCost(type), true))
                            {
                                player.GetModPlayer<PLAYERGLOBAL>().umbralTaggedNPC = npcs[i];
                                player.AddBuff(BuffType<UmbralTrespassing>(), 240);
                                PacketHandler.SendUmbralNPC(-1, player.whoAmI, i, player.whoAmI);
                                SetCooldowns(player, type);
                                break;
                            }
                        }
                    }

                }
                else if (CurrentlyHasSpecialCast(player, type))
                {
                    player.ClearBuff(BuffType<UmbralTrespassing>());
                    player.immuneTime = 60;
                }
            }
            else if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    player.velocity = TerraLeague.CalcVelocityToMouse(player.Center, 10f);
                    Vector2 position = player.Center;
                    Vector2 velocity = Vector2.Zero;
                    int projType = ProjectileType<ReapingSlash>();
                    int damage = GetAbilityBaseDamage(player, type) + GetAbilityScalingDamage(player, type, DamageType.MEL);
                    int knockback = 4;

                    Projectile.NewProjectile(position, velocity, projType, damage, knockback, player.whoAmI, player.velocity.X > 0 ? 1 : -1);
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
            item.damage = 32;
            item.width = 60;
            item.height = 54;       
            item.melee = true;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.knockBack = 3;
            item.value = 6000;
            item.rare = 4;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.useTurn = true;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffType<UmbralTrespass>(), 300);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Sickle, 1);
            recipe.AddRecipeGroup("TerraLeague:DemonGroup", 20);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.R || type == AbilityType.Q)
                return true;
            return base.GetIfAbilityExists(type);
        }
    }
}
