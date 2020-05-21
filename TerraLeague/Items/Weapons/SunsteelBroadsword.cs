using TerraLeague.Buffs;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class SunsteelBroadsword : AbilityItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sunsteel Broadsword");
            Tooltip.SetDefault("");
        }

        public override string GetWeaponTooltip()
        {
            return "";
        }

        public override string GetQuote()
        {
            return "DEMACIA!";
        }

        public override string GetAbilityName(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "Decisive Strike";
            else if (type == AbilityType.W)
                return "Courage";
            else
                return base.GetAbilityName(type);
        }

        public override string GetIconTexturePath(AbilityType type)
        {
            if (type == AbilityType.Q)
                return "AbilityImages/DecisiveStrike";
            else if (type == AbilityType.W)
                return "AbilityImages/Courage";
            else
                return base.GetIconTexturePath(type);
        }

        public override string GetAbilityTooltip(AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                return "You gain 15% movement speed and your next melee attack will deal 1.5x damage and apply 'Slowed'";
            }
            else if(type == AbilityType.W)
            {
                return "Gain 6 armor and resist for 5 seconds";
            }
            else
            {
                return base.GetAbilityTooltip(type);
            }
        }

        public override int GetAbilityBaseDamage(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return (int)0;
            else if (type == AbilityType.W)
                return (int)0;
            else
                return base.GetAbilityBaseDamage(player, type);
        }

        public override int GetBaseManaCost(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 0;
            else if (type == AbilityType.W)
                return 0;
            else
                return base.GetBaseManaCost(type);
        }

        public override string GetDamageTooltip(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
                return "";
            else if (type == AbilityType.W)
                return "";
            else
                return base.GetDamageTooltip(player, type);
        }

        public override bool CanBeCastWhileUsingItem(AbilityType type)
        {
            if (type == AbilityType.W || type == AbilityType.Q)
                return true;
            else
                return false;
        }

        public override int GetRawCooldown(AbilityType type)
        {
            if (type == AbilityType.Q)
                return 9;
            else if (type == AbilityType.W)
                return 10;
            else
                return base.GetRawCooldown(type);
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    player.AddBuff(BuffType<DecisiveStrike>(), 300);
                    SetCooldowns(player, type);
                }
            }
            else if (type == AbilityType.W)
            {
                if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost(type), true))
                {
                    player.AddBuff(BuffType<Courage>(), 300);
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
            item.damage = 10;
            item.width = 32;
            item.height = 32;
            item.melee = true;
            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 3;
            item.value = 1000;
            item.rare = ItemRarityID.Blue;
            item.UseSound = SoundID.Item1;
        }

        public override bool GetIfAbilityExists(AbilityType type)
        {
            if (type == AbilityType.Q || type == AbilityType.W)
                return true;
            return base.GetIfAbilityExists(type);
        }

        public override void Efx(Player player, AbilityType type)
        {
            if (type == AbilityType.Q)
            {

            }
            else if (type == AbilityType.W)
            {

            }
        }
    }
}
