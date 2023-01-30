using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DAX_PrefGen_V1_1 : MonoBehaviour 
{
	public GameObject[] Items;
	public string[] Descr;
	public Text OutText;
	public Text OutDescr;
	public int curIndex = 0;
	

	GameObject[] tmpPref; 

	void Awake()
	{
		showPrefab();
	}

	void FixedUpdate () 
	{
		this.OutText.text = string.Format( "{0}/{1}", this.curIndex+1, this.Items.Length );
	}

	void Update()
	{

	}

	public void Next()
	{
		this.curIndex += 1;
		if (this.curIndex >= this.Items.Length )
		{
			this.curIndex = 0;
		}
		showPrefab();
		//Application.LoadLevel( );
		
	}

	public void Prev()
	{
		this.curIndex -= 1;
		if (this.curIndex <0) { this.curIndex = this.Items.Length-1;};
		showPrefab();
	}

	public void  showPrefab( )
	{
		if (tmpPref!=null) 
		{ 
			for (int i=0;i<9;i++)
			{
				GameObject.DestroyObject( tmpPref[i] ); 
			}
		}
			

		tmpPref = new GameObject[9];
		
		Vector3 StandardOffset = new Vector3(0.0f,0.0f,0.0f);
		tmpPref[0] = Instantiate( this.Items[ this.curIndex ], StandardOffset, Quaternion.identity) as GameObject;
		
		StandardOffset.x = -90.0f;
		tmpPref[1] = Instantiate( this.Items[ this.curIndex ], StandardOffset, Quaternion.identity) as GameObject;
		
		StandardOffset.z = -90.0f;
		tmpPref[2] = Instantiate( this.Items[ this.curIndex ], StandardOffset, Quaternion.identity) as GameObject;
		
		StandardOffset.x = 0.0f;
		tmpPref[3] = Instantiate( this.Items[ this.curIndex ], StandardOffset, Quaternion.identity) as GameObject;		
		
		StandardOffset.x = +90.0f;
		tmpPref[4] = Instantiate( this.Items[ this.curIndex ], StandardOffset, Quaternion.identity) as GameObject;
		
		StandardOffset.z = 0.0f;
		tmpPref[5] = Instantiate( this.Items[ this.curIndex ], StandardOffset, Quaternion.identity) as GameObject;
	
		StandardOffset.z = +90.0f;
		tmpPref[6] = Instantiate( this.Items[ this.curIndex ], StandardOffset, Quaternion.identity) as GameObject;
	
		StandardOffset.x = 0.0f;
		tmpPref[7] = Instantiate( this.Items[ this.curIndex ], StandardOffset, Quaternion.identity) as GameObject;		
		
		StandardOffset.x = -90.0f;
		tmpPref[8] = Instantiate( this.Items[ this.curIndex ], StandardOffset, Quaternion.identity) as GameObject;			
		
	}
	
	

}
