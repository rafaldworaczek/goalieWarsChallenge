using UnityEngine;
using System.Collections;

public class BannerControl : MonoBehaviour {


	public float stayTime;
	private float currentTime;
	public bool count = true;

	public Texture texture;
	public int nrOfParts;
	public  int currentPart = 1;

	public float currentOffset;
	
	public float waitPeriod;

	void Start () {
		if(stayTime  < 0.1f){
			stayTime = 10f;
		}

		StartCoroutine (StartBanners());
	}

	private IEnumerator StartBanners(){


		yield return new WaitForSeconds (waitPeriod);
		StartCoroutine (Counter());

	}

	private IEnumerator Counter(){
		while(true){

			currentTime += 1.0f;

			if(currentTime >= stayTime){
				yield return StartCoroutine (MoveOffset());
				currentPart++;
				currentTime = 0.0f;
				if(currentPart>= 5 ){
					currentOffset = 0f;
					transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector3(0f,currentOffset,0f);
					currentPart =1;
				}
			}
			yield return new WaitForSeconds(1.0f);
		}
	}

	private IEnumerator MoveOffset(){
//		Vector3 offset = new Vector3 (0f,0f,0f);

		while(currentOffset < 0.25f*currentPart){

			currentOffset += 0.1f * Time.deltaTime;

			if(currentOffset >= 0.25f*currentPart){
				currentOffset = 0.25f*currentPart;
			}
			transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector3(0f,currentOffset,0f);

			yield return null;
		}
	}

}
