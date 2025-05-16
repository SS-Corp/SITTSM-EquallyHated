using System.IO;
using System.Reflection;
using UnityEngine;

namespace S.EquallyHated {
    internal class AssetsHandler {
        internal static AssetBundle reusedAssets;
        internal static AssetBundle weightlimit;
        internal static AssetBundle towerAssets;
        internal static AssetBundle fontsAssets;

        internal static void LoadAll() {
            string AssetsFolderPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets");

            reusedAssets = AssetBundle.LoadFromFile(Path.Combine(AssetsFolderPath, "reused"));
            weightlimit = AssetBundle.LoadFromFile(Path.Combine(AssetsFolderPath, "weightlimit"));
            towerAssets = AssetBundle.LoadFromFile(Path.Combine(AssetsFolderPath, "towerassets"));
            fontsAssets = AssetBundle.LoadFromFile(Path.Combine(AssetsFolderPath, "fonts"));
        }
    }
}
