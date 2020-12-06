using Microsoft.Xna.Framework.Audio;
using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class MagicVeil : Passive
    {

        public MagicVeil(int Cooldown)
        {
            passiveCooldown = Cooldown;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            return TooltipName("Annul") + TerraLeague.CreateColorString(PassiveSecondaryColor, "Gain a shield that will protect from one projectile at full charge") +
                "\n" + TerraLeague.CreateColorString(PassiveSubColor, GetScaledCooldown(player) + " second cooldown");
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
                SetCooldown(player);
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
                SetCooldown(player);
            }

            base.OnHitByProjectile(proj, ref damage, ref crit, player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        public override void Efx(Player user)
        {
            TerraLeague.PlaySoundWithPitch(user.MountedCenter, 2, 29, -0.75f);
            TerraLeague.DustRing(261, user, new Microsoft.Xna.Framework.Color(255, 0, 255, 0));
            base.Efx(user);
        }
    }
}
