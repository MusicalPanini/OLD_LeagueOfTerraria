using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class DarkinBlade : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Darkin Blade");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            

            return "I am oblivion, I am destruction... I am doom";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.R)
                return "World Ender";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.R)
                return "AbilityImages/Template";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.R)
            {
                return "Gain 10% melee life steal and flight for 10 seconds";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return (int)0;
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.R)
                return 50;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
                return "";
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

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.R)
                return 40;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    player.AddBuff(BuffType<DarkinBuff>(), 600);
                    DoEfx(player, type);
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
            item.damage = 60;
            item.width = 58;
            item.height = 58;
            item.melee = true;
            item.useTime = 32;
            item.useAnimation = 32;
            item.scale = 1.3f;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = 100000;
            item.rare = ItemRarityID.Pink;
            item.autoReuse = true;
            item.UseSound = SoundID.Item1;
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.R)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BreakerBlade, 1);
            recipe.AddRecipeGroup("TerraLeague:DemonGroup", 20);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddTile(TileID.DemonAltar);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.R)
            {
                Microsoft.Xna.Framework.Audio.SoundEffectInstance sound = Main.PlaySound(new LegacySoundStyle(4, 51).WithPitchVariance(0.8f), player.Center);
                if (sound != null)
                    sound.Pitch = -1f;
            }

            base.Efx(player, type);
        }
    }
}
