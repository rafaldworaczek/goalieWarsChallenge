﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMgr : MonoBehaviour {
	
	public void LoadLevel(string LevelName){
		SceneManager.LoadScene (LevelName);
	}
}
