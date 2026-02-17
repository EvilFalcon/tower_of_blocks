using UnityEditor;

[InitializeOnLoad]
public static class NuGetForceRestoreOnLoad
{
    private const string RestoredThisSessionKey = "NuGetForceRestoreOnLoad.Done";

    static NuGetForceRestoreOnLoad()
    {
        EditorApplication.delayCall += () =>
        {
            if (SessionState.GetBool(RestoredThisSessionKey, false))
                return;

            SessionState.SetBool(RestoredThisSessionKey, true);

            try
            {
                NugetForUnity.PackageRestorer.Restore(slimRestore: false);
                AssetDatabase.Refresh();
            }
            catch
            {
                // Restore may fail if NuGetForUnity isn't ready; user can run manually
            }
        };
    }
}
