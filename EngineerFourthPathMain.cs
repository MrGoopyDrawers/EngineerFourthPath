using MelonLoader;
using BTD_Mod_Helper;
using EngineerFourthPath;

[assembly: MelonInfo(typeof(EngineerFourthPath.EngineerFourthPathMain), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace EngineerFourthPath;

public class EngineerFourthPathMain : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<EngineerFourthPathMain>("EngineerFourthPath loaded!");
    }
}