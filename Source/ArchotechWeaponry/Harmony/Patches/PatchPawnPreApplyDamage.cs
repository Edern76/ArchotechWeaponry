using System.Linq;
using ArchotechWeaponry.Defs;
using ArchotechWeaponry.Defs.Traits;
using ArchotechWeaponry.Utils;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ArchotechWeaponry.Harmony.Patches
{
    [HarmonyPatch(typeof(Pawn))]
    [HarmonyPatch("PreApplyDamage")]
    public class PatchPawnPreApplyDamage
    {
        [HarmonyPrefix]
        public static void Prefix(ref DamageInfo dinfo, Pawn __instance)
        {
            HandleArchotechWeapon(ref dinfo, __instance);
        }

        private static void HandleArchotechWeapon(ref DamageInfo dinfo, Pawn __instance)
        {
            if (dinfo.Weapon.HasModExtension<ArchotechDamageExtension>() && dinfo.Instigator is Pawn instigator && instigator.equipment.Primary.def == dinfo.Weapon)
            {
                ThingWithComps weaponComp = instigator.equipment.Primary;
                if (!__instance.def.race.IsMechanoid)
                {
                    ArchotechDamageExtension extension = dinfo.Weapon.GetModExtension<ArchotechDamageExtension>();
                    dinfo.SetAmount(0);
                    if (extension.hediffToApplyOnOrganics != null)
                    {
                        float severity = extension.severityPerHit;
                        if (weaponComp.TryGetComp<CompBladelinkWeapon>() is CompBladelinkWeapon compBladelink &&
                            compBladelink.TraitsListForReading.Any(trait =>
                                trait.HasModExtension<PlaguebearerExtension>()))
                        {
                            PlaguebearerExtension plaguebearerTrait = compBladelink.TraitsListForReading
                                .Find(trait => trait.HasModExtension<PlaguebearerExtension>())
                                .GetModExtension<PlaguebearerExtension>();
                            severity += plaguebearerTrait.extraSeverityOnHit;
                            if (severity > 1f)
                            {
                                severity = 1f;
                            }
                        }
                        HediffUtils.AddOrUpdateHediffWithSeverity(__instance, extension.hediffToApplyOnOrganics, null, severity); //TO-DO : Fixed severity if mode is changed
                    }
                }
                else
                {
                    dinfo.Def = DamageDefOf.EMP;
                }
            }
        }
    }
}