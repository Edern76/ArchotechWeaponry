using Verse;

namespace ArchotechWeaponry
{
    [StaticConstructorOnStartup]
    public static class Bootstrap
    {
        static Bootstrap()
        {
            Log.Message("[ArchotechWeaponry] Done intialization");
        }    
    }
}