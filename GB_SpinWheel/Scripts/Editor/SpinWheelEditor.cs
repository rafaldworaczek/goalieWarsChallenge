using UnityEditor;
using UnityEngine;
namespace GameBench
{
    [CustomEditor(typeof(SpinWheelSetup))]
    public class SpinWheelEditor : Editor
    {
        Texture[] previewTextures;
        private void OnEnable()
        {
            previewTextures = Resources.LoadAll<Texture>("ThemePreview");
        }
        private SpinWheelSetup instance;
        Vector2 scrollViewPos;
        public override void OnInspectorGUI()
        {
            instance = (SpinWheelSetup)target;
            CenterTitle("Settings For Fortune Wheel");
            instance.spinDurationMin = EditorGUILayout.IntSlider("Spin Duration Min", instance.spinDurationMin, 1, 10);
            instance.spinDurationMax = EditorGUILayout.IntSlider("Spin Duration Max", instance.spinDurationMax, 1, 10);
            instance.speed = EditorGUILayout.IntSlider("Speed Multiplier X", instance.speed, 1, 10);

            instance.freeTurn = EditorGUILayout.Toggle("Free Turn Only?", instance.freeTurn);
            instance.paidTurn = EditorGUILayout.Toggle("Paid Turn Only?", instance.paidTurn);
            if (instance.paidTurn)
                instance.spinTurnCost = EditorGUILayout.IntSlider("Spin Turn Cost", instance.spinTurnCost, 100, 500);
            if (instance.freeTurn || instance.paidTurn)
            {
                EditorGUILayout.HelpBox(string.Format("Free Turns cost no money, Paid Turns are for {0} coins", instance.spinTurnCost), MessageType.Info);
            }
            if (!instance.freeTurn && !instance.paidTurn)
            {
                EditorGUILayout.HelpBox("When No turn is on then spins are free", MessageType.Info);
            }
            instance.theme = (WheelTheme)EditorGUILayout.EnumPopup("Select Theme", instance.theme);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Button(previewTextures[(int)instance.theme], EditorStyles.label,
                GUILayout.MaxWidth(100), GUILayout.MaxHeight(100));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            DrawLine();
            scrollViewPos = EditorGUILayout.BeginScrollView(scrollViewPos);
            for (int i = 0; i < 8; i++)
            {
                CenterTitle("Reward " + (i + 1));
                instance.rewarItem[i].rewardSprite = (Sprite)EditorGUILayout.ObjectField("Icon Image",
                    instance.rewarItem[i].rewardSprite, typeof(Sprite), false);
                instance.rewarItem[i].rewardQuantity = EditorGUILayout.IntField("Quantity", instance.rewarItem[i].rewardQuantity);
                instance.rewarItem[i].rewardType = (RewardType)EditorGUILayout.EnumPopup("Type of Reward", instance.rewarItem[i].rewardType);
                EditorGUILayout.Space();
            }
            EditorGUILayout.EndScrollView();
            DrawLine();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Documentation"))
            {
                Application.OpenURL("https://goo.gl/zU07xu");
            }
            if (GUILayout.Button("Contact"))
            {
                EditorUtility.DisplayDialog("Contact Info", "Game Bench: info.gamebench@gmail.com", "OK");
                string mailSubject = System.Uri.EscapeDataString("Help needed Game Bench Fortune Wheel");
                string mailURL = "mailto:mailto:info.gamebench@gmail.com" + "?subject=" + mailSubject;
                Application.OpenURL(mailURL);
            }
            if (GUILayout.Button("Version Details"))
            {
                EditorUtility.DisplayDialog("GB Fortune Wheel Version", "Game Bench Fortune Wheel Plugin Version is 1.0", "OK");
            }
            EditorGUILayout.EndHorizontal();
            SpinWheelSetup.DirtyEditor();
        }
        public static void CenterTitle(string text)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(text, EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        public static void DrawLine()
        {
            EditorGUI.indentLevel--;
            EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
            EditorGUI.indentLevel++;
        }
    }
}