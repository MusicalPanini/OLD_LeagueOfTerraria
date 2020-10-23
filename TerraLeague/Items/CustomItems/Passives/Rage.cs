using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Rage : Passive
    {
        int duration;

        public Rage(int SecondsDuration)
        {
            duration = SecondsDuration;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("RAGE") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Melee attacks increased movement speed for " + duration + " seconds");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            player.AddBuff(BuffType<Buffs.Rage>(), duration * 60);
            
            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            if (proj.melee)
                player.AddBuff(BuffType<Buffs.Rage>(), duration * 60);

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
