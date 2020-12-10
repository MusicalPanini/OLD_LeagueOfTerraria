using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Passives
{
    public class Spellblade : Passive
    {
        double damageModifier;
        int type = 0;

        public Spellblade(double DamageModifier)
        {
            damageModifier = DamageModifier;
        }

        public Spellblade(double DamageModifier, int Type)
        {
            damageModifier = DamageModifier;
            type = Type;
        }

        public override string Tooltip(Player player, ModItem modItem)
        {
            return TooltipName("SPELLBLADE") + TerraLeague.CreateColorString(PassiveSecondaryColor, "After using an ability, your next melee attack will deal " + (damageModifier) * 100 + "% damage");
        }

        public override void UpdateAccessory(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            modPlayer.spellblade = true;

            base.UpdateAccessory(player, modItem);
        }

        public override void NPCHit(Item item, NPC target, ref int damage, ref float knockback, ref bool crit, ref int OnHitDamage, Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.spellBladeBuff)
            {
                modPlayer.meleeModifer *= damageModifier;

                if (modPlayer.summonedBlade)
                {
                    Efx(player, target);
                    SendEfx(player, target, modItem);
                    player.ClearBuff(BuffType<SpellBlade>());
                }
                else
                {
                    Efx(player, target);
                    SendEfx(player, target, modItem);
                    player.ClearBuff(BuffType<SpellBlade>());
                }
                if (modPlayer.icyZone)
                {
                    var npcs = TerraLeague.GetAllNPCsInRange(player.MountedCenter, 120 + 2 * (modPlayer.armor), true, true);

                    for (int i = 0; i < npcs.Count; i++)
                    {
                        NPC npc = Main.npc[npcs[i]];

                        if (i != target.whoAmI)
                        {
                            npc.AddBuff(BuffType<Slowed>(), 300);
                            player.ApplyDamageToNPC(npc, modPlayer.armor, 0, 0, false);
                        }
                    }

                    target.AddBuff(BuffType<Slowed>(), 300);

                    Efx(player, target);
                    SendEfx(player, target, modItem);
                }
            }

            base.NPCHit(item, target, ref damage, ref knockback, ref crit, ref OnHitDamage, player, modItem);
        }

        public override void PostPlayerUpdate(Player player, ModItem modItem)
        {
            PLAYERGLOBAL modPlayer = player.GetModPlayer<PLAYERGLOBAL>();

            if (modPlayer.spellBladeBuff)
            {
                if (Main.rand.Next(0, 2) == 0)
                {
                    Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 261, 0, 0, 0);
                    dust.velocity = Vector2.Zero;
                    dust.noGravity = true;
                    if (modPlayer.triForce)
                    {
                        dust.color = new Color(255, 0, 0, 150);
                    }
                    else if (modPlayer.summonedBlade)
                    {
                        dust.color = new Color(255, 0, 255, 150);
                    }
                    else // Sheen and Iceborne
                    {
                        dust.color = new Color(0, 150, 255, 150);
                    }
                }
            }

            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player User, NPC HitNPC)
        {
            Main.PlaySound(new LegacySoundStyle(2, 30), HitNPC.Center);
            PLAYERGLOBAL modPlayer = User.GetModPlayer<PLAYERGLOBAL>();
            if (modPlayer.icyZone)
            {
                TerraLeague.DustRing(261, User, new Color(0, 150, 255, 150));

                int effectRadius = 75 + 2 * (User.statDefense + modPlayer.armor);

                for (int i = 0; i < effectRadius / 5; i++)
                {
                    Vector2 pos = new Vector2(effectRadius, 0).RotatedBy(MathHelper.ToRadians(360 * (i / (effectRadius / 5f)))) + User.Center;

                    Dust dustR = Dust.NewDustPerfect(pos, 113, Vector2.Zero, 0, default(Color), 1);
                    dustR.noGravity = true;
                }
            }

            for (int k = 0; k < 15; k++)
            {
                Dust dust = Dust.NewDustDirect(HitNPC.position, HitNPC.width, HitNPC.height, 261, 0, 0, 0);
                dust.velocity = Vector2.Zero;
                dust.noGravity = true;
                if (modPlayer.summonedBlade)
                {
                    dust.color = new Color(255, 0, 255, 150);
                }
                else
                {
                    dust.color = new Color(0, 150, 255, 150);
                }
            }
        }
    }
}
