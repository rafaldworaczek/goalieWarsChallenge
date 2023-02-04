using UnityEngine;
using System.Collections;

[System.Serializable]
public class LightmapConnection : MonoBehaviour {
	
	[HideInInspector]
	public bool useoverrideObject=false;
	[HideInInspector]
	public Renderer overrideObject;

	public Renderer connectedRenderer;
	[HideInInspector]
	public int lightmapIndex;
	[HideInInspector]
	public Vector4 lightmapScaleOffset;

	 void Start () {

		if (!useoverrideObject) {
			if(!connectedRenderer){
				this.GetComponent<Renderer> ().lightmapIndex = lightmapIndex;
				this.GetComponent<Renderer> ().lightmapScaleOffset = lightmapScaleOffset;
			}else{
				this.GetComponent<Renderer> ().lightmapIndex = connectedRenderer.lightmapIndex;
				this.GetComponent<Renderer> ().lightmapScaleOffset = connectedRenderer.lightmapScaleOffset;
			}
		} else {
			this.GetComponent<Renderer> ().lightmapIndex = overrideObject.lightmapIndex;
			this.GetComponent<Renderer> ().lightmapScaleOffset = overrideObject.lightmapScaleOffset;
		}
	}

	public void AddData(int lightmapIndex,Vector4 lightmapScaleOffset){
		this.lightmapIndex = lightmapIndex;
		this.lightmapScaleOffset = lightmapScaleOffset;
	} 

	public void AddRendererConnection(Renderer r){
		connectedRenderer = r;
	}
}
