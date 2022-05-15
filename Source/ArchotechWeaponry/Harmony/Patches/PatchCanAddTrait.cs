using ArchotechWeaponry.Defs;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ArchotechWeaponry.Harmony.Patches
{
    [HarmonyPatch(typeof(CompBladelinkWeapon))]
    [HarmonyPatch("CanAddTrait")]
    public class PatchCanAddTrait
    {
        public static void Postfix(WeaponTraitDef trait, CompBiocodable __instance, ref bool __result)
        {
            if (__result)
            {
                ThingWithComps weapon = __instance.parent;
                bool isArchotechOnly = trait.HasModExtension<ArchotechTraitExtension>() &&
                                       trait.GetModExtension<ArchotechTraitExtension>().generateOnArchotech;
                if (trait.GetModExtension<ArchotechTraitExtension>() is ArchotechTraitExtension traitExtension && traitExtension.generateOnArchotech)
                {
                    if (weapon.def.HasModExtension<ArchotechDamageExtension>())
                    {
                        __result = weapon.def.weaponTags.Any(tag => traitExtension.allowedTags.Contains(tag));
                    }
                    else
                    {
                        __result = false;
                    }
                }
            }
        }
    }
}