using Terraria;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class CauterizedWounds : Passive
    {
        int percentDamageReduction;

        public CauterizedWounds(int PercentDamageReduction)
        {
            percentDamageReduction = PercentDamageReduction;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("IGNORE PAIN") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Stores " + percentDamageReduction + "% of damage taken" +
                   "\nTake a third of the stored damage every second");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            passiveStat = modPlayer.cauterizedDamage;

            base.UpdateAccessory(player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        public override void OnHitByNPC(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.cauterizedDamage += (int)(damage * (percentDamageReduction * 0.01));
            damage = (int)(damage * (1 - (percentDamageReduction * 0.01)));
            base.OnHitByNPC(npc, ref damage, ref crit, player, modItem);
        }

        public override void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.cauterizedDamage += (int)(damage * (percentDamageReduction * 0.01));
            damage = (int)(damage * (1 - (percentDamageReduction * 0.01)));
            base.OnHitByProjectile(npc, ref damage, ref crit, player, modItem);
        }

        public override void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            modPlayer.cauterizedDamage += (int)(damage * (percentDamageReduction * 0.01));
            damage = (int)(damage * (1 - (percentDamageReduction * 0.01)));
            base.OnHitByProjectile(proj, ref damage, ref crit, player, modItem);
        }
    }
}
