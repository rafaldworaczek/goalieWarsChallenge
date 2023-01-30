namespace GameBench
{
    using UnityEngine;
    using System.IO;
#if UNITY_EDITOR
    using UnityEditor;
    [InitializeOnLoad]
#endif
    public class SpinWheelSetup : ScriptableObject
    {
        public RewardItem[] rewarItem = new RewardItem[8];
        public WheelTheme theme;
        public int speed, spinDurationMin, spinDurationMax, spinTurnCost = 300;
        public bool freeTurn, paidTurn;
        const string assetDataPath = "Assets/GB_SpinWheel/Resources/";
        const string assetName = "FortuneWheelSetup";
        const string assetExt = ".asset";
        private static SpinWheelSetup instance;
        public static SpinWheelSetup Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load(assetName) as SpinWheelSetup;
                    if (instance == null)
                    {
                        instance = CreateInstance<SpinWheelSetup>();
#if UNITY_EDITOR
                        if (!Directory.Exists(assetDataPath))
                        {
                            Directory.CreateDirectory(assetDataPath);
                        }
                        string fullPath = assetDataPath + assetName + assetExt;
                        AssetDatabase.CreateAsset(instance, fullPath);
                        AssetDatabase.SaveAssets();
#endif
                    }
                }
                return instance;
            }
        }
        public static void DirtyEditor()
        {
#if UNITY_EDITOR
            EditorUtility.SetDirty(Instance);
#endif
        }
#if UNITY_EDITOR
        [MenuItem("Tools/Edit Spin Wheel")]
        public static void Edit()
        {
            Selection.activeObject = Instance;
        }
#endif
    }

    public enum WheelTheme
    {
        Theme1,
        Theme2,
        Theme3,
        Theme4,
        Theme5
    }
}