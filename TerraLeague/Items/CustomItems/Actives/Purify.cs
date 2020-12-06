using Microsoft.Xna.Framework;
using TerraLeague.Buffs;
using TerraLeague.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace TerraLeague.Items.CustomItems.Actives
{
    public class Purify : Active
    {
        public Purify(int Cooldown)
        {
            activeCooldown = Cooldown;
        }

        public override string Tooltip(Player player, LeagueItem modItem)
        {
            return TooltipName("PURIFY") + TerraLeague.CreateColorString(ActiveSecondaryColor, "Create a cleansing field at the target location for 10 seconds.\nAllies will be immune to most debuffs while in this field") +
                 "\n" + TerraLeague.CreateColorString(ActiveSubColor, GetScaledCooldown(player) + " second cooldown");
        }

        public override void DoActive(Player player, LeagueItem modItem)
        {
            if (cooldownCount <= 0)
            {
                Projectile.NewProjectileDirect(Main.MouseWorld, Vector2.Zero, ProjectileType<Item_CleanseField>(), 0, 0, player.whoAmI);
                SetCooldown(player);
            }
        }

        public override void PostPlayerUpdate(Player player, LeagueItem modItem)
        {
            base.PostPlayerUpdate(player, modItem);
        }

        override public void Efx(Player user)
        {
        }
    }
}

