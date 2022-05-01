using ArchotechWeaponry.Defs;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ArchotechWeaponry.Harmony.Patches
{
    [HarmonyPatch(typeof(CompBladelinkWeapon))]
    public class PatchCanAddTrait
    {
        public void Postfix(WeaponTraitDef trait, CompBiocodable __instance, ref bool __result)
        {
            if (__result)
            {
                ThingWithComps weapon = __instance.parent;
                bool isArchotechOnly = trait.HasModExtension<ArchotechTraitExtension>() &&
                                       trait.GetModExtension<ArchotechTraitExtension>().generateOnArchotech;
                if (weapon.def.weaponTags != null && weapon.def.GetModExtension<ArchotechTraitExtension>() is ArchotechTraitExtension traitExtension && weapon.def.weaponTags.Any(tag => traitExtension.allowedTags.Contains(tag)))
                {
                    __result = isArchotechOnly;
                }
                else
                {
                    __result = !isArchotechOnly;
                }
            }
        }
    }
}