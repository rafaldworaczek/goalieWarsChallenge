using UnityEngine;
using UnityEditor;
using System.Collections;

public class DuplicateWithLightmap : MonoBehaviour
{

    [MenuItem("Edit/Duplicate with Lightmap/ Duplicate with Baked Lightmap Connection %#d")]
    static void DuplicateWithLightmapConnected()
    {

        GameObject[] original = Selection.gameObjects;

        SceneView.lastActiveSceneView.Focus();
        EditorWindow.focusedWindow.SendEvent(EditorGUIUtility.CommandEvent("Duplicate"));

        GameObject[] duplicate = Selection.gameObjects;

        for (int i = 0; i < duplicate.Length; i++)
        {

            Renderer[] originalRenderers = original[i].GetComponentsInChildren<Renderer>();
            Renderer[] duplicateRenderers = duplicate[i].GetComponentsInChildren<Renderer>();

            int cnt = 0;

            foreach (Renderer originalRenderer in originalRenderers)
            {
                if (!originalRenderer.gameObject.GetComponent<LightmapConnection>())
                {

                    RemoveBatchingStaticFlag(duplicateRenderers[cnt].gameObject);
                    RemoveLightmapStaticFlag(duplicateRenderers[cnt].gameObject);
                    LightmapConnection connection = duplicateRenderers[cnt].gameObject.AddComponent<LightmapConnection>();
                    connection.AddRendererConnection(originalRenderer);
                    //Hide Behaviour in Inspector
                    connection.hideFlags = HideFlags.HideInInspector;

                }
                cnt++;
            }
        }
        resetDisplayAfterPlaymode();
    }

    private static void RemoveLightmapStaticFlag(GameObject g)
    {
        StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(g);
        flags = flags & ~(StaticEditorFlags.ContributeGI);
        GameObjectUtility.SetStaticEditorFlags(g.gameObject, flags);
    }

    private static void RemoveBatchingStaticFlag(GameObject g)
    {
        StaticEditorFlags flags = GameObjectUtility.GetStaticEditorFlags(g);
        flags = flags & ~(StaticEditorFlags.BatchingStatic);
        GameObjectUtility.SetStaticEditorFlags(g.gameObject, flags);
    }

    /* 
	 // You Probably won't need this. If you have a special case where you don't want to have the Lightmap Values connected to another Renderer, use this.
	 
	[MenuItem ("Edit/Duplicate with Lightmap/ Duplicate with Baked Lightmap - No Connection to Original Baked Object")]
	static void DuplicateObjectWithLightmap () {

		GameObject[] original = Selection.gameObjects;
		
		SceneView.lastActiveSceneView.Focus ();
		EditorWindow.focusedWindow.SendEvent (EditorGUIUtility.CommandEvent ("Duplicate"));
		
		GameObject[] duplicate = Selection.gameObjects;

		for (int i = 0; i < duplicate.Length; i++) {

			Renderer[] originalRenderers = original[i].GetComponentsInChildren<Renderer>();
			Renderer[] duplicateRenderers = duplicate[i].GetComponentsInChildren<Renderer>();

			int cnt=0;
			foreach(Renderer originalRenderer in originalRenderers){

				if(!originalRenderer.gameObject.GetComponent<LightmapConnection>()){

					LightmapConnection connection = duplicateRenderers[cnt].gameObject.AddComponent<LightmapConnection> ();
					connection.AddData(originalRenderer.lightmapIndex,originalRenderer.lightmapScaleOffset);
					//Hide Behaviour in Inspector
					connection.hideFlags=HideFlags.HideInInspector;
				}
				cnt++;
			}
		}
		resetDisplayAfterPlaymode ();
	} */


    [MenuItem("Edit/Duplicate with Lightmap/ Reset Lightmap Display in Editor (after PlayMode) %#r")]
    static void resetDisplayAfterPlaymode()
    {
        foreach (LightmapConnection LMCon in GameObject.FindObjectsOfType<LightmapConnection>())
        {
            Renderer r = LMCon.gameObject.GetComponent<Renderer>();
            if (LMCon.connectedRenderer)
            {
                r.lightmapIndex = LMCon.connectedRenderer.lightmapIndex;
                r.lightmapScaleOffset = LMCon.connectedRenderer.lightmapScaleOffset;
            }
            else {
                r.lightmapIndex = LMCon.lightmapIndex;
                r.lightmapScaleOffset = LMCon.lightmapScaleOffset;
            }
        }
    }

    [MenuItem("Edit/Duplicate with Lightmap/ Select Lightmapped Original for Renderer")]
    private static void SelectLightmappedOriginal()
    {
        if (Selection.activeGameObject.GetComponent<LightmapConnection>() && 
            Selection.activeGameObject.GetComponent<Renderer>())
        {
            Selection.activeGameObject =
                Selection.activeGameObject.GetComponent<LightmapConnection>().connectedRenderer.gameObject;
        }
        else {
            Debug.LogWarning("Selected Object [" + Selection.activeGameObject.name + "] is either not connected to another Objects Lightmap, or has no Renderer. Select a Renderer of a duplicated lightmapped object to find its original.");
        }
    }

    [MenuItem("Edit/Duplicate with Lightmap/ Break Lightmap Connection for selection")]
    private static void BreakLightmapConnection()
    {
        GameObject[] selection = Selection.gameObjects;

        for (int i = 0; i < selection.Length; i++)
        {
            Renderer[] renderers = selection[i].GetComponentsInChildren<Renderer>();

            foreach (Renderer r in renderers)
            {
                if (r.GetComponent<LightmapConnection>())
                {
                    DestroyImmediate(r.GetComponent<LightmapConnection>());
                    r.GetComponent<Renderer>().lightmapIndex = 65535;
                    r.GetComponent<Renderer>().realtimeLightmapScaleOffset.Set(0, 0, 0, 0);
                }
            }
        }


    }
}
