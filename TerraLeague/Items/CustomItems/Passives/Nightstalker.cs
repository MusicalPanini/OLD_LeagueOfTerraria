using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Nightstalker : Passive
    {
        double meleeDamageModifier;
        int proximity;

        public Nightstalker(double MeleeDamageModifier, int Proximity)
        {
            meleeDamageModifier = MeleeDamageModifier;
            proximity = Proximity;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("NIGHTSTALKER") + TerraLeague.CreateColorString(PassiveSecondaryColor, "If there are no enemies within " + proximity + " blocks of you, gain 'Night Stalker'\n'Night Stalker' causes your next melee attack to deal " + meleeDamageModifier * 100 + "% damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.nightStalker)
                modPlayer.meleeModifer *= meleeDamageModifier;

                base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.nightStalker)
            {
                passiveStat = 0;
                Efx(player, target);
                SendEfx(player, target, modItem);
                modPlayer.nightStalker = false;
                player.ClearBuff(BuffType<Buffs.NightStalker>());
            }

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void NPCHitWithProjectile(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.nightStalker && proj.melee)
            {
                passiveStat = 0;
                Efx(player, target);
                SendEfx(player, target, modItem);
                modPlayer.nightStalker = false;
                player.ClearBuff(BuffType<Buffs.NightStalker>());
            }

            base.NPCHitWithProjectile(proj, target, ref damage, ref knockback, ref crit, ref hitDirection, ref OnHitDamage, player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (!modPlayer.nightStalker)
            {
                bool Prox = TerraLeague.IsThereAnNPCInRange(player.MountedCenter, proximity * 16);

                if (!Prox || player.invis)
                {
                    AddStat(player, 100, 2);
                }
            }

            if (passiveStat >= 100)
            {
                if (Main.rand.Next(0, 5) == 0)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 117, 0, 0, 0, new Color(50, 0, 0, 200), 1.3f);
                    dust.velocity *= 0.5f;
                    dust.alpha = 40;
                    dust.noGravity = true;
                }
                player.AddBuff(BuffType<Buffs.NightStalker>(), 1);
            }
            else
                player.ClearBuff(BuffType<Buffs.NightStalker>());

            base.PostPlayerUpdate(player, modItem);
        }

        public void AddStat(Player player, float maxStat, float statToAdd)
        {
            if (passiveStat < maxStat)
            {
                passiveStat += statToAdd;

                if (passiveStat >= maxStat)
                {
                    Main.PlaySound(new LegacySoundStyle(3, 54).WithPitchVariance(0.2f));
                }
            }

            if (passiveStat > maxStat)
            {
                passiveStat = maxStat;
            }
        }

        public override void Efx(Player user, NPC effectedNPC)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDustDirect(effectedNPC.position, effectedNPC.width, effectedNPC.height, 16, 0, 0, 0, new Color(50, 0, 0, 150), 1.5f);
                Dust.NewDustDirect(effectedNPC.position, effectedNPC.width, effectedNPC.height, 16, 0, 0, 0, new Color(50, 50, 50, 150), 1.5f);
            }
            Main.PlaySound(new LegacySoundStyle(4, 51).WithPitchVariance(0.3f), effectedNPC.position);
        }
    }
}
