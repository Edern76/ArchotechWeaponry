using ArchotechWeaponry.Defs;
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
            if (dinfo.Weapon.HasModExtension<ArchotechDamageExtension>())
            {
                if (!__instance.def.race.IsMechanoid)
                {
                    ArchotechDamageExtension extension = dinfo.Weapon.GetModExtension<ArchotechDamageExtension>();
                    dinfo.SetAmount(0);
                    if (extension.hediffToApplyOnOrganics != null)
                    {
                        HediffUtils.AddOrUpdateHediffWithSeverity(__instance, extension.hediffToApplyOnOrganics, null, extension.severityPerHit); //TO-DO : Fixed severity if mode is changed
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