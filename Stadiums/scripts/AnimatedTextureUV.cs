using UnityEngine;
using System.Collections;

public class AnimatedTextureUV : MonoBehaviour
{
	public int colCount =  4;
	public int rowCount =  4;

	public int totalCells = 4;
	public int fps     = 10;
	
	public bool loop;
	public bool playOnAwake;
	
	Vector2 offset;
	Vector2 size;
	public int index;
	float frameProgress;
	
	Material mat;
	public bool isPlaying = false;
	
	public bool setAtEnd;
	
	void Start() {
		mat = GetComponent<Renderer>().material;
		size = new Vector2(1.0f / rowCount, 1.0f / colCount);
		mat.SetTextureScale  ("_MainTex", size);
		SetFrame(0);
	}
	
	void OnEnable() {
		SetFrame(0);
		if(playOnAwake)
			Play();
	}
	
	IEnumerator Run() {
		do {
			index = 0;
			bool hasNext = index < totalCells;
			while(hasNext)
			{
				PaintForCurrentFrame();
				
				frameProgress += Time.deltaTime * fps;
				int wholeFrames = Mathf.FloorToInt(frameProgress);
				frameProgress -= wholeFrames;
				index += wholeFrames;
				
				hasNext = index < totalCells;

				yield return null;
			}
		} while(loop);
		Stop ();
		isPlaying = false;
	}
	
	public void SetFrame(int index) {
		this.index = index;
		PaintForCurrentFrame();
	}
	
	void PaintForCurrentFrame() {
		int uIndex = index % colCount;
	    int vIndex = index / rowCount;
		
		float offsetX = uIndex * size.x;
	    float offsetY = (1.0f - size.y) - vIndex * size.y;
		
		Vector2 offset = new Vector2(offsetX,offsetY);
		if (mat != null)
	    	mat.SetTextureOffset ("_MainTex", offset);
	}
	
	public void Play() 
	{
		if (!isPlaying)
		{
			isPlaying = true;
			this.StartCoroutine("Run");
		}
	}
	
	public void Stop() {
		this.StopAllCoroutines();
		if (!setAtEnd)
			SetFrame(0);
		isPlaying = false;
		//gameObject.SetActive(false);
		//Destroy(gameObject);
	}
}