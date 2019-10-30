using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class MagicVeil : Passive
    {
        int cooldown;

        public MagicVeil(int Cooldown)
        {
            cooldown = Cooldown;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return "[c/0099cc:Passive: MAGIC VEIL -] [c/99e6ff:Gain a shield that will protect from one projectile at full charge]" +
                "\n[c/007399:" + (int)(cooldown * modPlayer.cdrLastStep) + " second cooldown]";
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.accessoryStat[TerraLeague.FindAccessorySlotOnPlayer(player, modItem)] <= 0)
            {
                player.AddBuff(Terraria.ModLoader.ModContent.BuffType<Buffs.MagicVeil>(), 2);
            }
            base.UpdateAccessory(player, modItem);
        }

        public override void OnHitByProjectile(NPC npc, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.veil)
            {
                Efx(player);
                player.endurance = 1;
                AddStat(player, modItem, cooldown * 60, (int)(cooldown * 60 * modPlayer.Cdr));
            }

            base.OnHitByProjectile(npc, ref damage, ref crit, player, modItem);
        }

        public override void OnHitByProjectile(Projectile proj, ref int damage, ref bool crit, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.veil)
            {
                Efx(player);
                player.endurance = 1;
                AddStat(player, modItem, cooldown * 60, (int)(cooldown * 60 * modPlayer.Cdr));
            }

            base.OnHitByProjectile(proj, ref damage, ref crit, player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            AddStat(player, modItem, cooldown * 60, -1, true);
            base.PostPlayerUpdate(player, modItem);
        }

        public override void Efx(Player user)
        {
            SoundEffectInstance sound = Main.PlaySound(new LegacySoundStyle(2, 29), user.position);
            if (sound != null)
                sound.Pitch = -0.75f;
            TerraLeague.DustRing(261, user, new Microsoft.Xna.Framework.Color(255, 0, 255, 0));
            base.Efx(user);
        }
    }
}
