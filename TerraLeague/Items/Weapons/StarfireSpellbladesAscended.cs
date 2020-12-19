using Microsoft.Xna.Framework;
using TerraLeague.Items.Weapons.Abilities;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons
{
    public class StarfireSpellbladesAscended : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfire Spellblade");
            Tooltip.SetDefault("");
        }

        string GetWeaponTooltip()
        {
            return "You have ascended!" +
                "\nFire a wave of starfire that deals " + (int)(item.damage * 0.75) + " + " + TerraLeague.CreateScalingTooltip(DamageType.MEL, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().MEL, 30) + " + " + TerraLeague.CreateScalingTooltip(DamageType.SUM, Main.LocalPlayer.GetModPlayer<PLAYERGLOBAL>().SUM, 50) + " melee damage";
        }

        public override void SetDefaults()
        {
            item.damage = 110;
            item.width = 56;
            item.height = 56;       
            item.melee = true;
            item.useTime = 36;
            item.useAnimation = 18;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 6;
            item.value = 200000;
            item.rare = ItemRarityID.Yellow;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.shoot = ProjectileType<StarfireSpellblades_Firewave>();
            item.shootSpeed = 8;

            AbilityItemGLOBAL abilityItem = item.GetGlobalItem<AbilityItemGLOBAL>();
            abilityItem.SetAbility(AbilityType.R, new DivineJudgement(this));
            abilityItem.ChampQuote = "Behold, the righteous flame!";
            abilityItem.getWeaponTooltip = GetWeaponTooltip;
            abilityItem.IsAbilityItem = true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 velocity = TerraLeague.CalcVelocityToMouse(position, 16f);
            damage = (int)(item.damage * 0.75) + (int)(player.GetModPlayer<PLAYERGLOBAL>().MEL * 0.3) + (int)(player.GetModPlayer<PLAYERGLOBAL>().SUM * 0.5);
            int numberProjectiles = 24;
            float startingAngle = 24;
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(startingAngle));
                startingAngle -= 2f;
                Projectile proj = Projectile.NewProjectileDirect(position, perturbedSpeed, type, damage, knockBack, player.whoAmI, i);
            }

            return false;
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Daybreak, 60);
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
        }

        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust dust = Dust.NewDustDirect(hitbox.TopLeft(), hitbox.Width, hitbox.Height, 87, 0, 0, 100, default(Color), 0.7f);
            dust.noGravity = true;
            base.MeleeEffects(player, hitbox);
        }

        public override void UpdateInventory(Player player)
        {
            if (player.GetModPlayer<PLAYERGLOBAL>().AscensionStacks != 6)
            {
                byte prefix = item.prefix;
                item.SetDefaults(ModContent.ItemType<StarfireSpellblades>());
                item.prefix = prefix;
            }

            base.UpdateInventory(player);
        }

        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            byte prefix = item.prefix;
            item.SetDefaults(ModContent.ItemType<StarfireSpellblades>());
            item.prefix = prefix;

            base.Update(ref gravity, ref maxFallSpeed);
        }
    }
}
