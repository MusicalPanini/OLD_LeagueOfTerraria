using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Torment : Passive
    {
        int debuffDuration;

        public Torment(int DebuffDuration)
        {
            debuffDuration = DebuffDuration;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return "[c/0099cc:Passive: TORMENT -] [c/99e6ff:Minion and magic damage sets the target on fire for " + debuffDuration + " seconds, dealing increased damage to high health targets]" +
                "\n[c/99e6ff:Burn damage is doubled against slowed targets]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            if (proj.magic || proj.minion)
                target.AddBuff(BuffType<Buffs.Torment>(), debuffDuration * 60);
            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }
    }
}
