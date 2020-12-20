using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.Weapons.Abilities
{
    public class UmbralTrespass : Ability
    {
        public UmbralTrespass(Terraria.ModLoader.ModItem item)
        {
            abilityItem = item;
        }

        public override string GetAbilityName()
        {
            return "Umbral Trespass";
        }

        public override string GetIconTexturePath()
        {
            return "AbilityImages/UmbralTrespass";
        }

        public override string GetAbilityTooltip()
        {
            return "Become invulnerable and infest a marked enemy for 4 seconds" +
                "\nAfter 1 second, you can recast to rip yourself from them dealing damage";
        }

        public override int GetAbilityBaseDamage(Player player)
        {
            return (int)(abilityItem.item.damage * 4);
        }

        public override int GetAbilityScalingAmount(Player player, DamageType dam)
        {
            switch (dam)
            {
                case DamageType.MEL:
                    return 175;
                default:
                    return 0;
            }
        }

        public override int GetRawCooldown()
        {
            return 45;
        }

        public override int GetBaseManaCost()
        {
            return 150;
        }

        public override string GetDamageTooltip(Player player)
        {
            return GetAbilityBaseDamage(player) + " + " + GetScalingTooltip(player, DamageType.MEL) + " melee damage";
        }

        public override bool CanBeCastWhileUsingItem()
        {
            return true;
        }

        public override bool CurrentlyHasSpecialCast(Player player)
        {
            int pos = GetPositionOfAbilityInArray(abilityItem);
            if (pos != -1)
                return (player.HasBuff(BuffType<UmbralTrespassing>()) && player.GetModPlayer<PLAYERGLOBAL>().AbilityCooldowns[pos] <= GetCooldown() * 60 - 60);
            return false;
        }

        public override void DoEffect(Player player, AbilityType type)
        {
            if (CheckIfNotOnCooldown(player, type) && player.CheckMana(GetScaledManaCost()) && !player.HasBuff(BuffType<UmbralTrespassing>()))
            {
                List<NPC> npcs = Main.npc.OfType<NPC>().ToList();
                if (npcs.Count != 0)
                {
                    for (int i = 0; i < npcs.Count; i++)
                    {
                        if (npcs[i].Hitbox.Intersects(new Rectangle((int)Main.MouseWorld.X - 15, (int)Main.MouseWorld.Y - 15, 30, 30)) && npcs[i].GetGlobalNPC<NPCs.TerraLeagueNPCsGLOBAL>().umbralTrespass && npcs[i].aiStyle != 91 && player.CheckMana(GetBaseManaCost(), true))
                        {
                            player.GetModPlayer<PLAYERGLOBAL>().umbralTaggedNPC = npcs[i];
                            player.AddBuff(BuffType<UmbralTrespassing>(), 240);
                            PacketHandler.SendUmbralNPC(-1, player.whoAmI, i, player.whoAmI);
                            SetCooldowns(player, type);
                            break;
                        }
                    }
                }

            }
            else if (CurrentlyHasSpecialCast(player))
            {
                player.ApplyDamageToNPC(player.GetModPlayer<PLAYERGLOBAL>().umbralTaggedNPC, GetAbilityBaseDamage(player) + GetAbilityScaledDamage(player, DamageType.MEL), 0, 0, false);
                player.ClearBuff(BuffType<UmbralTrespassing>());
                player.GetModPlayer<PLAYERGLOBAL>().umbralTrespassing = false;
                player.velocity = TerraLeague.CalcVelocityToMouse(player.Center, 14f);
                player.immuneTime = 60;
                DoEfx(player, type);
            }
        }

        public override void Efx(Player player)
        {
            Main.PlaySound(SoundID.NPCDeath1, player.MountedCenter);
            TerraLeague.PlaySoundWithPitch(player.MountedCenter, 2, 71, -0.5f);

            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, 186, player.velocity.X / 5, player.velocity.Y / 5, 150);
                dust.scale = 1.5f;
                dust.color = new Color(255, 0, 0);
                dust = Dust.NewDustDirect(player.position, player.width, player.height, 186, player.velocity.X / 5, player.velocity.Y / 5, 150);

                dust.scale = 1.5f;
            }
        }
    }
}
