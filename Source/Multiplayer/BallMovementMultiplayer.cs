using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using graphicsCommonNS;
using GlobalsNS;
using Com.Osystems.GoalieStrikerFootball;
using AudioManagerMultiNS;
using Photon.Pun;

public class BallMovementMultiplayer : MonoBehaviour
{
	private playerControllerMultiplayer playerMainScript;
	private GameObject ballGO;
	public Rigidbody rb;
	private bool goalCollision;
	private bool cpuPlayerCollision;
	private AudioManager audioManager;
	private GameObject trailBall;
	public GameObject drawPrefabBallTrail;
	private bool ballTrailRendererActive = false;
	public bool ballCollided = false;
	private GraphicsCommon graphics;
	private bool isPlayerDownCollided;
	private bool isWallCollided = false;

	private int idScoredTeam;
	private float lastTimeSaveAupdate;
	private float lastTimeSaveBupdate;

	private float lastTimeGkCollisionUpdateCpu;
	private float lastTimeGkCollisionUpdatePlayer;
	private Vector3 lastTimeGkCollisionUpdateCpuBallVel;
	private Vector3 lastTimeGkCollisionUpdatePlayerBallVel;

	public ParticleSystem[] goalUpFlare;
	public ParticleSystem[] goalDownFlare;

	private bool saveAupdate = false;
	private bool saveBupdate = false;

	private int teamHostID;

	//private float lastTimeBallFloorCollision = Time.time;
	//private float lastTimeBallOver2unitsY = Time.time;

	private float lastTimeSave;
	private int whoTouchBallLastTime;
	private bool isTrainingActive = Globals.isTrainingActive;
	private int MAX_BALLS_TEXTURE = 2;

	private bool updateVelTeamA = false;
	private bool updateVelTeamB = false;

	private float headLastTimeCollisionPlayerUp;

	private Vector3 lastBallVel;
	public bool goalUpCrossBarJustHit = false;
	public bool bonusCoinHit = false;
	public bool bonusDiamondHit = false;
	public bool fansChantIsPlaying = false;

	private bool isMaster = false;

	public float playerDownLastGkCollision = 0f;
	public float playerUpLastGkCollision = 0f;

	public float goalUpLastTimeHitCrossbar = 0f;
	public float goalUpLastTimeHitPosts = 0f;

	public float goalDownLastTimeHitCrossbar = 0f;
	public float goalDownLastTimeHitPosts = 0f;

	public float goalUpCornertLastTimeHit = 0f;
	public float goalDownCornertLastTimeHit = 0f;

	// Use this for initialization
	void Awake()
	{
		//		playerMainScript = Game/Object.Find("playerDown").GetComponent<controllerRigid>();


		headLastTimeCollisionPlayerUp = -Time.time;

		ballGO = GameObject.Find("ball1");
		int randTexture = UnityEngine.Random.Range(0, MAX_BALLS_TEXTURE);

		/*1 ball prefable*/
		int randVal = UnityEngine.Random.Range(0, MAX_BALLS_TEXTURE * 2);
		if (randVal <= MAX_BALLS_TEXTURE)
			randTexture = 0;

		graphics = new GraphicsCommon();

		/*use first ball for the time being*/
		//randTexture = 1;

		//graphics.setMesh(ballGO, "ball/fbx/ball" + randTexture.ToString());
		//graphics.setMaterialByName(ballGO, "ball/material/ball" + randTexture.ToString(), 0);
	}

	void Start()
	{

		playerMainScript = Globals.player1MainScript;
		//print("#DBGINITBALLMOVEMENT " + playerMainScript + " Globals.player1MainScript; " + Globals.player1MainScript);
		isMaster = PhotonNetwork.IsMasterClient;
		rb = GetComponent<Rigidbody>();
		goalCollision = false;
		cpuPlayerCollision = false;
		audioManager = FindObjectOfType<AudioManager>();
		whoTouchBallLastTime = 0;
		//playerMainScript = GameObject.Find("playerDown").GetComponent<controllerRigid>();
		lastTimeSaveAupdate = lastTimeSaveBupdate = Time.time;
		lastTimeGkCollisionUpdateCpu = Time.time;
		lastTimeGkCollisionUpdatePlayer = Time.time;

		//playerMainScript = GameManager.player1.GetComponent<playerControllerMultiplayer>();

		//teamHostID = playerMainScript.getTeamHostId();
	}

	void Update()
	{
		lastBallVel = rb.velocity;

		/*Shoud it be in fixed update or update? */
		if (ballTrailRendererActive)
			ballTrailRendererChangePos();

		//print("#DBGNEWGKPHYS CPU DISTX ballVel UPDTATE " + rb.velocity + " ballPos " + rb.transform.position);
	}

	/*void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(rb.transform.position, 0.2585f);
	}*/

	/*1 - PLAYER 1 */
	/*2 - PLAYER 2 CPU */
	public int whoTouchBallLast()
	{
		return whoTouchBallLastTime;
	}

	public void setwhoTouchBallLast(int val)
	{
		whoTouchBallLastTime = val;
	}

	public void OnCollisionStay(Collision other)
	{
		//if (!other.collider.name.Contains("floor"))
	//		print("DBGSHOT ballCollidedCOLISSSION OnCollisionStay!!!! other.collider.name ##### " + other.collider.name + " ballVel " +
//			rb.velocity + " rbPos " + rb.transform.position);

		/*Collider[] hitColliders = Physics.OverlapSphere(
					rb.transform.position, playerMainScript.getBallRadius() + 0.1f,
					Physics.AllLayers,
					QueryTriggerInteraction.Collide);
		foreach (Collider collider in hitColliders)
		{
			if (collider.name.Contains("Hand") ||
				collider.name.Contains("ForeArm") ||
				collider.name.Contains("Shoulder") ||
				collider.name.Contains("Head") ||
				collider.name.Contains("Spine") ||
				collider.name.Contains("Area"))
				print("#DBGHITCOLLIDERS CPU MAINSTAY " + collider.name);
		}*/


		/*if (other.collider.name.Contains("playerUp") ||
			other.collider.tag.Contains("playerUp"))
		{
			if ((other.collider.name.Contains("HandCollider") ||
				 other.collider.tag.Contains("HandCollider") ||
			 	 other.collider.tag.Contains("Spine")) &&
				(Time.time - lastTimeGkCollisionUpdateCpu <= 1.0f))
			{
				rb.velocity = lastTimeGkCollisionUpdateCpuBallVel;
			}
		}

		if (other.collider.name.Contains("playerDown") ||
			other.collider.tag.Contains("playerDown"))
		{
			if ((other.collider.name.Contains("HandCollider") ||
				 other.collider.tag.Contains("HandCollider") ||
			 	 other.collider.tag.Contains("Spine")) &&
				(Time.time - lastTimeGkCollisionUpdatePlayer <= 1.0f))
			{
				rb.velocity = lastTimeGkCollisionUpdatePlayerBallVel;
				//print("DBGNEWGKPHYS PLAYER DISTX balDBGCOLLISIONCALC1024D ballShotlVel velocity OVERWRITE onCollisionStay " + rb.velocity);
			}
		}*/

		if (other.collider.name == "floor")
			return;

		//print("DBGCOLLISIONCALC1024D on Collision Stay vel " + rb.velocity + " other.collider.name " + other.collider.name);
		//print("DBGNEWGKPHYS CPU DISTX ballVel STAY other.collider.name " + other.collider.name);
	}


	public void OnCollisionEnter(Collision other)
	{
		/*LEVEL DEPENDENT = MAKE DIVIDOR BIGGER FOR BETTER GK*/
		float randDiv = getRandFloat(1.5f, 3.0f);

		//print("DBGCOLLISION DBGSHOT ballCollidedCOLISSSION OnTriggerEnter!!!! other.collider.name ##### "
		//+ other.collider.name + " ballVel " +
		///rb.velocity + " rbPos " + rb.transform.position);

		//print("DBGNEWGKPHYS CPU DISTX ballVel ENTER other.collider.name " + other.collider.name);

		//float wallRandDiv = getRandFloat(4.0f, 6.0f);#DBGNEWGKPHYS CPU DISTX ballVel UPDTATE
		//float randYdiv = getRandFloat(1.9f, 2.8f);

		//print("GKDEBUG800 COLLLSION OCCURBALL " + other.collider.name + " TAG " + other.collider.tag);

		/*Collider[] hitColliders = Physics.OverlapSphere(
					rb.transform.position, playerMainScript.getBallRadius() + 0.1f,
					Physics.AllLayers,
					QueryTriggerInteraction.Collide);
		foreach (Collider collider in hitColliders)
		{
			if (collider.name.Contains("Hand") ||
				collider.name.Contains("ForeArm") ||
				collider.name.Contains("Shoulder") ||
				collider.name.Contains("Head") ||
				collider.name.Contains("Spine") ||
				collider.name.Contains("Area"))
					print("#DBGHITCOLLIDERS CPU MAIN " + collider.name);
		}*/

		/*if (other.collider.name.Contains("bonusDiamond"))
		{
			bonusDiamondHit = true;
			audioManager.Play("moneySound");
		}

		if (other.collider.name.Contains("bonusCoin"))
		{
			bonusCoinHit = true;
			audioManager.Play("moneySound");
		}
		*/

		//if (!other.collider.name.Contains("floor"))
		//{
	//		print("DBGWALL ballPos " + rb.transform.position + " ballVel " + rb.velocity
	//			+ " other.collider.name " + other.collider.name);
	//	}

		if (other.collider.name.Contains("goalDownNet") ||
			other.collider.name.Contains("goalUpNet"))
		{
			audioManager.PlayNoCheck("net1");
		}

		if (other.collider.name == "floor" ||
			other.collider.name.Contains("playerDown") || 
			other.collider.name.Equals("Spine"))
		{
			//lastTimeBallFloorCollision = Time.time;
			//print("BALLFLOOR COLLISION SPEED " + rb.velocity + " TRANSFORM " 
			//	+ rb.transform.position);

			//if (rb.velocity.y > 2.0f &&
			//	!playerMainScript.isOnBall() &&
			//	!playerMainScript.cpuPlayer.isOnBall())
			//{
			//print("BALLBOUNCEPLAYNOW");
			//	audioManager.Play("ballbounce2");
			//}

			//ballTrailRendererStop();

			if (other.collider.name.Contains("playerDown") ||
				other.collider.name.Equals("Spine"))
			{
				if (other.collider.transform.position.z > 0)
					whoTouchBallLastTime = 2;
				else
					whoTouchBallLastTime = 1;
			
					///print("DEBUGGK1045 ballCollidedCOLISSSION ballRb velocity apply onCollisionEnter whoTouchLast " + whoTouchBallLastTime + " other.collider.name " +
					//other.collider.name);
				playerMainScript.setGkHelperImageVal(false);
				//print("COLLISIONDBG123 ORIGINAL NAME " + other.collider.name + " vel " + rb.velocity);
			}

			return;
		}

		//print("DBGCOLLISIONCALC1024DB NORMAL BALL ballRb velocity apply ORIGINAL NAME " + other.collider.name + " vel " + rb.velocity
		//	+ " ballRb.transform.position " + rb.transform.position);

		///Debug.Log("DBGSHOT12 collision !!" + other.collider.name);

		ballTrailRendererStop();

		if (!goalCollision)
		{
			if (other.collider.name.Contains("goalDownPostCollider") || 
				other.collider.name.Contains("goalUpPostCollider"))
			{
				audioManager.PlayNoCheck("goalpost1");
				if (other.collider.name.Contains("goalUpPostCollider"))
					goalUpLastTimeHitPosts = Time.time;
				else
					goalDownLastTimeHitPosts = Time.time;

				//Handheld.Vibrate();
				///print("DBGSHOT ballCollided other.collider.name " + other.collider.name);
				isWallCollided = true;
				ballCollided = true;

				lastTimeGkCollisionUpdateCpu = 0f;
				lastTimeGkCollisionUpdatePlayer = 0f;
			}

			if (other.collider.name == "goalDownCrossBarCollider" ||
				other.collider.name == "goalUpCrossBarCollider")
			{

				audioManager.PlayNoCheck("crossbar");
				if (other.collider.name == "goalUpCrossBarCollider")
				{
					goalUpCrossBarJustHit = true;
					goalUpLastTimeHitCrossbar = Time.time;
				}
				else
				{
					goalDownLastTimeHitCrossbar = Time.time;
				}

				isWallCollided = true;
				//Handheld.Vibrate();
				ballCollided = true;
				///print("DBGSHOT ballCollided other.collider.name " + other.collider.name);

				lastTimeGkCollisionUpdateCpu = 0f;
				lastTimeGkCollisionUpdatePlayer = 0f;
			}

			ballTrailRendererStop();
		}

		/*Check collision with wall*/
		/*if (other.collider.name == "wallLeft" ||
			other.collider.name == "wallRight" ||
			other.collider.name == "wallUpLeft" ||
			other.collider.name == "wallDown" ||
			other.collider.name == "wallDownRight" ||
			other.collider.name == "wallUpRight")*/
		if (other.collider.name.Contains("wall"))
		{
			///Debug.Log("DBGWALL collision with wall " + other.collider.name
			//	+ " rb.vel " + rb.velocity
			//	+ " rbPos  " + rb.transform.position);
			isWallCollided = true;
			ballCollided = true;
			//print("DBGSHOT ballCollided other.collider.name ##### " + other.collider.name + " ballVel " +
		//		rb.velocity + " rbPos " + rb.transform.position);

			//print("wall collision before " + rb.velocity);

			/*rb.velocity = new Vector3(rb.velocity.x / wallRandDiv, 
								      rb.velocity.y / wallRandDiv, 
									  rb.velocity.z / wallRandDiv);
			rb.velocity = Vector3.zero;*/

			//print("wall collision after " + rb.velocity);
			audioManager.PlayNoCheck("bounceMetal");

			lastTimeGkCollisionUpdateCpu = 0f;
			lastTimeGkCollisionUpdatePlayer = 0f;
		}

		ballCollided = true;

		//print("DBG342344COL onCollisionEnter " + other.collider.name + " ball Vel " + rb.velocity);
		playerMainScript.setGkHelperImageVal(false);
	}

	public void OnTriggerEnter(Collider other)
	{

	/*	print("DBGCOLLISION DBGSHOT ballCollidedCOLISSSION OnTriggerEnter!!!! other.collider.name ##### "
			+ other.GetComponent<Collider>().name + " ballVel " +
			rb.velocity + " rbPos " + rb.transform.position);*/

		string colliderName = other.GetComponent<Collider>().name;


		//used by commentator
		if (colliderName == "goalUpCornerLeftCollider" ||
			colliderName == "goalUpCornerRightCollider")
		{
			goalUpCornertLastTimeHit = Time.time;
		}

		if (colliderName == "goalDownCornerLeftCollider" ||
			colliderName == "goalDownCornerRightCollider")
		{
			goalDownCornertLastTimeHit = Time.time;
		}

		if (colliderName.Contains("playerDown") ||
			colliderName.Contains("Spine"))
		{
			if (other.GetComponent<Collider>().transform.position.z > 0)
				whoTouchBallLastTime = 2;
			else
				whoTouchBallLastTime = 1;

			isPlayerDownCollided = true;
			//print("DEBUGGK1045 colliderName onCollisionTrigger isPlayerDownCollided " + isPlayerDownCollided
			//+ "  rb.transform.position " + rb.transform.position
			//	+ " colliderName " + colliderName);
			return;
		}


		if (!goalCollision)
		{
			if (colliderName.Contains("wall")) {
				isWallCollided = true;
				///Debug.Log("DBGWALL isWallCollided 1 " + isWallCollided + " ballPos " + 
				///	rb.transform.position + " ballVel " + rb.velocity);
			}

			if (colliderName.Contains("goalDownPostCollider") ||
				colliderName.Contains("goalUpPostCollider"))
			{
				audioManager.PlayNoCheck("goalpost1");
				if (colliderName.Contains("goalUpPostCollider"))
					goalUpLastTimeHitPosts = Time.time;
				else
					goalDownLastTimeHitPosts = Time.time;

				isWallCollided = true;
				//Debug.Log("DBGWALL isWallCollided 2 " + isWallCollided + " ballPos " +
				//	rb.transform.position + " ballVel " + rb.velocity);

				//Handheld.Vibrate();
				///print("DBGSHOT ballCollided other.collider.name " + other.collider.name);

				ballCollided = true;

				lastTimeGkCollisionUpdateCpu = 0f;
				lastTimeGkCollisionUpdatePlayer = 0f;
			}

			if (colliderName == "goalDownCrossBarCollider" ||
				colliderName == "goalUpCrossBarCollider")
			{		
				audioManager.PlayNoCheck("crossbar");
				if (colliderName == "goalUpCrossBarCollider")
				{
					goalUpCrossBarJustHit = true;
					goalUpLastTimeHitCrossbar = Time.time;
				}
				else
				{
					goalDownLastTimeHitCrossbar = Time.time;
				}

				//Handheld.Vibrate();
				ballCollided = true;
				isWallCollided = true;
				//Debug.Log("DBGWALL isWallCollided 3 " + isWallCollided + " ballPos " +
				//			rb.transform.position + " ballVel " + rb.velocity);
				///print("DBGSHOT ballCollided other.collider.name " + other.collider.name);

				lastTimeGkCollisionUpdateCpu = 0f;
				lastTimeGkCollisionUpdatePlayer = 0f;
			}

			ballTrailRendererStop();
		}

		if (colliderName == "goalUpBallCrossLine" ||
			colliderName == "goalDownBallCrossLine")
		{
			lastTimeGkCollisionUpdateCpu = 0f;
			lastTimeGkCollisionUpdatePlayer = 0f;

			if (playerMainScript.getTimeToShotExceeded() ||
				playerMainScript.getGameEnded())
				return;

			if (goalCollision == false)
			{
				//this collider is disabled for master
				if (colliderName == "goalUpBallCrossLine")
				{
					idScoredTeam = 2;
					playerMainScript.setIsGoalJustScored(true);
					/*if (isMaster)
					{
						Globals.score1++;
						playerMainScript.photonView.RPC("RPC_goalUpdate",
														 RpcTarget.Others,
														 Globals.score1, 
														 2);
						StartCoroutine(
							playerMainScript.sendAndACKGoal(1, Globals.score1, 2));
					    for (int i = 0; i < goalUpFlare.Length; i++)
						{
							goalUpFlare[i].Play();
						}
					} else
					{*/
						Globals.score2++;
						audioManager.Commentator_PlayRandomGoal(false);
						StartCoroutine(audioManager.Commentator_AfterGoal(1.3f, 2f));
						if ((Time.time - playerMainScript.lastTimeSaveAupdate) < 3.0f)
							playerMainScript.decSavesStatistics("teamA", 1);
	
						playerMainScript.photonView.RPC("RPC_goalUpdate",
									 					 RpcTarget.Others,
								  	  					 Globals.score2,
														 1);
						//print("Send RPC_goalUpdate score " + Globals.score2);
						for (int i = 0; i < goalUpFlare.Length; i++)
						{
							goalUpFlare[i].Play();
						}
						StartCoroutine(
							playerMainScript.sendAndACKGoal(1, Globals.score2, 1));
					//}

				
					//}
					//else
					//{
					//	Globals.score2++;
					//}

					//if (!Globals.isTrainingActive)
					//{

				
					//}					

					//idScoredTeam = 1;
					//idScoredTeam = 1;

					//if (!isTrainingActive)
					//{
					///if (teamHostID == 1 && isMaster) 
					//||
					//	(teamHostID == 2 && !isMaster))
					//{
					//if (teamHostID == 1 && isMaster) {
					audioManager.Play("crowdGoal1");
					StartCoroutine(playFansChant(1.1f));
					//}
					//else
					//	{
					//		audioManager.Play("goallose1");
					//	}
					//}
				}
				else
				{
					//this collider is disabled for !master goalDown
					int rand = Random.Range(1, 3);
					if (rand == 1 || whoTouchBallLastTime == 1)
					{
						idScoredTeam = 2;
						playerMainScript.setIsGoalJustScored(true);
						//owngoal
						//if (isMaster)
						//	{
							Globals.score2++;
							playerMainScript.photonView.RPC("RPC_goalUpdate",
															 RpcTarget.Others,
															 Globals.score2,
															 1);
							StartCoroutine(
								playerMainScript.sendAndACKGoal(1, Globals.score2, 1));
							for (int i = 0; i < goalDownFlare.Length; i++)
							{
								goalDownFlare[i].Play();
							}

							if ((Time.time - playerMainScript.lastTimeSaveAupdate) < 3.0f)
								playerMainScript.decSavesStatistics("teamA", 1);
		
						audioManager.Play("crowdGoal1");						
					}
					else
					{
						idScoredTeam = 2;
						playerMainScript.setIsGoalJustScored(true);

						audioManager.Play("goallose1");
			
							Globals.score2++;
							playerMainScript.photonView.RPC("RPC_goalUpdate",
															 RpcTarget.Others,
															 Globals.score2,
															 1);
							StartCoroutine(
								playerMainScript.sendAndACKGoal(1, Globals.score2, 1));
							for (int i = 0; i < goalDownFlare.Length; i++)
							{
								goalDownFlare[i].Play();
							}

							if ((Time.time - playerMainScript.lastTimeSaveAupdate) < 3.0f)
								playerMainScript.decSavesStatistics("teamA", 1);						
						idScoredTeam = 2;
					}
				}
			}

			goalCollision = true;
		}
	}

	IEnumerator playFansChant(float offsetDelay)
	{
		fansChantIsPlaying = true;
		yield return new WaitForSeconds(offsetDelay);

		int randChant = UnityEngine.Random.Range(3, 5);
		if (!audioManager.isPlayingByName("fanschant3") &&
			!audioManager.isPlayingByName("fanschant4"))
			audioManager.Play("fanschant" + randChant.ToString());
		audioManager.Play("crowdBassDrum");

		yield return new WaitForSeconds(9f);

		fansChantIsPlaying = false;

	}

	public bool isFansChantPlaying()
	{
		return fansChantIsPlaying;
	}

	public void setGoalUpCrossBarJustHit(bool val)
	{
		goalUpCrossBarJustHit = val;
	}

	public bool getGoalUpCrossBarJustHit()
	{
		return goalUpCrossBarJustHit;
	}

	public void setBonusCoinHit(bool val)
	{
		bonusCoinHit = val;
	}

	public bool getBonusCoinHit()
	{
		return bonusCoinHit;
	}

	public void setBonusDiamonHit(bool val)
	{
		bonusDiamondHit = val;
	}

	public bool getBonusDiamondHit()
	{
		return bonusDiamondHit;
	}

	public void setIsPlayerDownCollided(bool val)
	{
		isPlayerDownCollided = val;
	}

	public bool getIsWallCollided()
	{
		return isWallCollided;
	}

	public void setIsWallCollided(bool val)
	{
		isWallCollided = val;
	}

	public bool getIsPlayerDownCollided()
	{
		return isPlayerDownCollided;
	}


	public void OnTriggerExit(Collider other)
	{
		//if (other.GetComponent<Collider>().name == "goalUp")
		//{
	//		goalCollision = true;
	//		//print("COLLISION DETECTED");
//		}

		/*if (other.GetComponent<Collider>().name == "playerUp")
		{
			cpuPlayerCollision = false;
			print("COLLISION DETECTED");
		}*/
	}

	
	/*(public void ballVelocityChange(bool isCpu, Collision other)
	{
		string lastGkAnimPlayed = "";
			//playerMainScript.cpuPlayer.getLastGkAnimPlayed();

		//float randGkDiv = getRandFloat(2f, 2.6f);
		float randGkDiv = getRandFloat(2f, 4.0f);
		float lobBounceSpeed = 3f;
		float maxDistanceHandFromBall = 0.45f;
		bool tooFarFromHand = true;
		float ballVelocity = playerMainScript.getBallShotVel();
		Vector3 ballPos = rb.transform.position;
		bool notPossibleToCatch = false;
		Vector3 leftPalmLocal = Vector3.zero;
		Vector3 rightPalmLocal = Vector3.zero;

		if (!isCpu)
		{
			lastGkAnimPlayed =
				playerMainScript.getLastGkAnimPlayed();
		}

		if (isCpu)
		{
			
		}
		else
		{ 
			string lastAnimPlayed = playerMainScript.getLastGkAnimPlayed();

			//if (playerMainScript.cpuPlayer.isLobShotActive() &&
			if (playerMainScript.getLastGkAnimPlayed().Contains("straight"))
			{
				//print("DEBUGONCOLLISIONVELOCITY BEFORE " + rb.velocity);
				Vector3 playerPos =
						playerMainScript.getPlayerPosition();
				Transform playerTransform =
					playerMainScript.getRbTransform();
				Vector3 localPlayerPos = new Vector3(0f, 0.6f, 2f);
				Vector3 worldBallDirection =
					playerMainScript.TransformPointUnscaled(playerTransform, localPlayerPos);
				rb.velocity = (worldBallDirection - rb.transform.position).normalized * lobBounceSpeed;
			}
			else
			{
				if (lastAnimPlayed.Contains("punch") && (
					ballVelocity > 28f || lastAnimPlayed.Contains("up")))
				{
					notPossibleToCatch = true;
				}

				////print("#DEBUGBALLLOCAL lastAnimPlayed PLAYER " + lastAnimPlayed + " ballVel " + ballVelocity
				////	+ " notPossibleToCatch " + notPossibleToCatch);

				tooFarFromHand = true;
				if (other.collider.name.Contains("HandCollider") &&
					!notPossibleToCatch)
				{

					leftPalmLocal = playerMainScript.InverseTransformPointUnscaled(
												playerMainScript.getLeftPalm().transform,
												ballPos);

					rightPalmLocal = playerMainScript.InverseTransformPointUnscaled(
												playerMainScript.getRightPalm().transform,
												ballPos);

					tooFarFromHand = false;
					if (lastAnimPlayed.Contains("punch"))
					{
						if (lastAnimPlayed.Contains("left"))
						{
							if (Mathf.Abs(leftPalmLocal.x) > 0.6f ||
								Mathf.Abs(leftPalmLocal.y) > 0.4f)
								tooFarFromHand = true;
						}
						else
						{
							if (lastAnimPlayed.Contains("right"))
							{
								if (Mathf.Abs(rightPalmLocal.x) > 0.6f ||
									Mathf.Abs(rightPalmLocal.y) > 0.4f)
									tooFarFromHand = true;
							}
						}
					}
					else
					{

						if (lastAnimPlayed.Contains("_left_") ||
							lastAnimPlayed.Contains("_right_"))
							if (Mathf.Abs(rightPalmLocal.x) > 0.6f ||
								Mathf.Abs(rightPalmLocal.y) > 0.4f ||
								Mathf.Abs(leftPalmLocal.x) > 0.6f ||
								Mathf.Abs(leftPalmLocal.y) > 0.4f)
								tooFarFromHand = true;
					}

					////print("#DEBUGBALLLOCAL leftPalm " + leftPalmLocal
					////	+ " rightPalm " + rightPalmLocal + " tooFarFromHand " + tooFarFromHand);
				}

				if (!tooFarFromHand)
				{
					Transform playerTransform =
						playerMainScript.getRbTransform();

					Vector3 localBouncePos =
						new Vector3(
							getRandFloat(-0.1f, 0.1f),
							getRandFloat(0f, 0.35f),
							getRandFloat(7f, 10f));

					if (lastAnimPlayed.Contains("left"))
					{
						Vector3 leftHandLocalPos = playerMainScript.InverseTransformPointUnscaled(
													playerMainScript.getRbTransform(),
													playerMainScript.getLeftHand().transform.position);

						localBouncePos =
							new Vector3(leftHandLocalPos.x,
										leftHandLocalPos.y,
										getRandFloat(7f, 10f));
					}
					else
					{
						if (lastAnimPlayed.Contains("right"))
						{
							Vector3 rightHandLocalPos = playerMainScript.InverseTransformPointUnscaled(
														playerMainScript.getRbTransform(),
														playerMainScript.getRightHand().transform.position);

							localBouncePos =
								new Vector3(rightHandLocalPos.x,
											rightHandLocalPos.y,
											getRandFloat(7f, 10f));
						}
					}

					Vector3 worldBallDirection =
						playerMainScript.TransformPointUnscaled(playerTransform, localBouncePos);
					float ballSpeedPerc = Mathf.InverseLerp(0.0f, 34.5f, Mathf.Abs(ballVelocity));
					if (Mathf.Abs(lastBallVel.z) >= 34.5f)
						ballSpeedPerc = 1f;

					float finalSpeed = Mathf.Lerp(0.5f, 5.5f, ballSpeedPerc);
					rb.velocity = (worldBallDirection - rb.transform.position).normalized * finalSpeed;
				}
				else
				{
					float speedPerc = 1f;
					if (lastAnimPlayed.Contains("punch"))
					{
						ballVelocity = Mathf.Max(10f, 34.5f);
						speedPerc = Mathf.InverseLerp(34.5f, 10f, ballVelocity);
						randGkDiv = getRandFloat(2f, 4.0f);
					}

					if (playerMainScript.isLobShotActive())
						randGkDiv = 2.0f;

					rb.velocity = new Vector3(
								  rb.velocity.x / randGkDiv,
								  rb.velocity.y / randGkDiv,
								  rb.velocity.z / randGkDiv);
				}
			}

			rb.angularVelocity /= (Mathf.Max(10f, ballVelocity) * 0.12f);
		}
	}
	*/

	public void playFlares(int scoreNum)
	{

		if (scoreNum == 1)
		{
			if (isMaster)
			{
				for (int i = 0; i < goalUpFlare.Length; i++)
				{
					goalUpFlare[i].Play();
				}
			} else
			{
				for (int i = 0; i < goalUpFlare.Length; i++)
				{
					goalDownFlare[i].Play();
				}
			}
		}
		else
		{
			if (isMaster)
			{
				for (int i = 0; i < goalDownFlare.Length; i++)
				{
					goalDownFlare[i].Play();
				}
			}
			else
			{
				for (int i = 0; i < goalUpFlare.Length; i++)
				{
					goalUpFlare[i].Play();
				}
			}
		}
	}


	public float getHeadLastTimeCollisionPlayerUp()
	{
		return headLastTimeCollisionPlayerUp;
	}

	public bool getBallCollided()
	{
		return ballCollided;
	}

	public void setBallCollided(bool val)
	{
		ballCollided = val;
	}

	public void setWhoScored(int val)
	{
		idScoredTeam = val;
	}

	public int whoScored()
	{
		return idScoredTeam;
	}

	public bool getBallGoalCollisionStatus()
	{
		return goalCollision;
	}

	public void setBallGoalCollisionStatus(bool val)
	{
		goalCollision = val;
	}

	public bool getBallCpuPlayerStatus()
	{
		return cpuPlayerCollision;
	}

	public float getPlayerDownLastGkCollision()
	{
		return playerDownLastGkCollision;
	}

	public float getGoalUpCornertLastTimeHit()
	{
		return goalUpCornertLastTimeHit;
	}

	public float getGoalDownCornertLastTimeHit()
	{
		return goalDownCornertLastTimeHit;
	}
	public float getPlayerUpLastGkCollision()
	{
		return playerUpLastGkCollision;
	}

	public void setPlayerUpLastGkCollision(float time)
	{
		playerUpLastGkCollision = time;
	}

	public void setPlayerDownLastGkCollision(float time)
	{
		playerDownLastGkCollision = time;
	}

	public float getGoalUpLastTimeHitCrossbar()
	{
		return goalUpLastTimeHitCrossbar;
	}

	public float getGoalUpLastTimeHitPosts()
	{
		return goalUpLastTimeHitPosts;
	}

	public float getGoalDownLastTimeHitCrossbar()
	{
		return goalDownLastTimeHitCrossbar;
	}

	public float getGoalDownLastTimeHitPosts()
	{
		return goalDownLastTimeHitPosts;
	}

	public void setBallCpuPlayerCollision(bool val)
	{
		cpuPlayerCollision = val;
	}

	public void ballTrailRendererInit()
	{
		trailBall = (GameObject)Instantiate(drawPrefabBallTrail, rb.transform.position, Quaternion.identity);
		ballTrailRendererActive = true;
	}

	private void ballTrailRendererChangePos()
	{
		trailBall.transform.position = rb.transform.position;
	}

	private void ballTrailRendererStop()
	{
		if (!ballTrailRendererActive)
			return;

		ballTrailRendererActive = false;
		Destroy(trailBall);
	}

	private float getRandFloat(float min, float max)
	{
		return Random.Range(min, max);
	}

	public Vector3 getBallPos()
	{
		return transform.position;
	}

	//void LateUpdate()
	//{
	//if (updateVelTeamA)
	//{
	/*rb.velocity = new Vector3(rb.velocity.x / 4.0f,
							  rb.velocity.y / 4.0f,
							  rb.velocity.z / 4.0f);*/
	//		updateVelTeamA = false;
	//			print("DEBUG123 UPDATEVELTEAM A " + rb.velocity);

	//	}

	//	if (updateVelTeamB)
	//	{
	//rb.velocity = ballNewVel;
	/*rb.velocity = new Vector3(rb.velocity.x / 4.0f,
							  rb.velocity.y / 4.0f,
							  rb.velocity.z / 4.0f);*/
	//	updateVelTeamB = false;
	//		print("DEBUG123 UPDATEVELTEAM B " + rb.velocity);
	//		}
	//}
}