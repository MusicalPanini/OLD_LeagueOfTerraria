using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.SummonerSpells
{
    public class BarrierRune : SummonerSpell
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Barrier Rune");
            Tooltip.SetDefault("");
            base.SetStaticDefaults();
        }
        public override string GetIconTexturePath()
        {
            return "Items/SummonerSpells/Barrier";
        }

        public override string GetSpellName()
        {
            return "Barrier";
        }

        public override int GetRawCooldown()
        {
            return 120;
        }

        public int GetShieldStat()
        {
            if (NPC.downedGolemBoss)
                return 200;
            else if (NPC.downedPlantBoss)
                return 175;
            else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                return 150;
            else if (NPC.downedMechBossAny)
                return 125;
            else if (Main.hardMode)
                return 100;
            else if (NPC.downedBoss2)
                return 75;
            else
                return 50;
        }

        public override string GetTooltip()
        {
            return "You gain a shield that protects from " + TerraLeague.CreateScalingTooltip(DamageType.NONE, GetShieldStat(), 100, true) + " damage for 10 seconds" +
                "\nShield strength scales through the game";
        }

        public override void DoEffect(Player player, int spellSlot)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            player.AddBuff(BuffType<Barrier>(), 600);
            modPlayer.AddShieldAttachedToBuff(modPlayer.ScaleValueWithHealPower(GetShieldStat()), BuffType<Barrier>(), Color.Orange, ShieldType.Basic);

            Efx(player);
            PacketHandler.SendBarrier(-1, player.whoAmI, player.whoAmI);

            SetCooldowns(player, spellSlot);
        }

        static public void Efx(Player player)
        {
            //Main.PlaySound(new LegacySoundStyle(2, 29).WithPitchVariance(-0.5f), player.Center);
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 70, 0.75f);

            TerraLeague.DustElipce(128, 128, 0, player.MountedCenter, 174, default, 2, 180, true, 0.02f);
        }

        static public void ShieldBreak(Player player)
        {

        }
    }
}
