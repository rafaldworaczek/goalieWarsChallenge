using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using graphicsCommonNS;
using GlobalsNS;
using AudioManagerNS;
public class ballMovement : MonoBehaviour
{
	private controllerRigid playerMainScript;
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
	private bool isBonusActive = false;
	private int MAX_BALLS_TEXTURE = 2;

	private bool updateVelTeamA = false;
	private bool updateVelTeamB = false;

	private float headLastTimeCollisionPlayerUp;

	private Vector3 lastBallVel;
	public bool goalUpCrossBarJustHit = false;
	public bool bonusCoinHit = false;
	public bool bonusDiamondHit = false;
	public bool fansChantIsPlaying = false;

	public float playerDownLastGkCollision = 0f;
	public float playerUpLastGkCollision = 0f;

	public float goalUpLastTimeHitCrossbar = 0f;
	public float goalUpLastTimeHitPosts = 0f;

	public float goalDownLastTimeHitCrossbar = 0f;
	public float goalDownLastTimeHitPosts = 0f;

	public float goalUpCornertLastTimeHit = 0f;
	public float goalDownCornertLastTimeHit = 0f;

	public Powers powersScript;

	// Use this for initialization
	void Awake()
	{
		playerMainScript = GameObject.Find("playerDown").GetComponent<controllerRigid>();

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

		graphics.setMesh(ballGO, "ball/fbx/ball" + randTexture.ToString());
		graphics.setMaterialByName(ballGO, "ball/material/ball" + randTexture.ToString(), 0);
	}

	void Start()
	{
		isBonusActive = Globals.isBonusActive;
		rb = GetComponent<Rigidbody>();
		goalCollision = false;
		cpuPlayerCollision = false;
		audioManager = FindObjectOfType<AudioManager>();
		whoTouchBallLastTime = 0;
		playerMainScript = GameObject.Find("playerDown").GetComponent<controllerRigid>();
		lastTimeSaveAupdate = lastTimeSaveBupdate = Time.time;
		lastTimeGkCollisionUpdateCpu = Time.time;
		lastTimeGkCollisionUpdatePlayer = Time.time;
		teamHostID = playerMainScript.getTeamHostId();
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


		if (other.collider.name.Contains("playerUp") ||
			other.collider.tag.Contains("playerUp"))
		{
			if ((other.collider.name.Contains("HandCollider") ||
				 other.collider.tag.Contains("HandCollider") ||
			 	 other.collider.tag.Contains("Spine")) &&
				(Time.time - lastTimeGkCollisionUpdateCpu <= 1.0f))
			{
				rb.velocity = lastTimeGkCollisionUpdateCpuBallVel;
				//print("DBGNEWGKPHYS CPU DISTX ballVel velocity OVERWRITE onCollisionStay " + rb.velocity);
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
				//print("DBGNEWGKPHYS PLAYER DISTX ballVel velocity OVERWRITE onCollisionStay " + rb.velocity);
			}
		}

		//print("DBGNEWGKPHYS CPU DISTX ballVel STAY other.collider.name " + other.collider.name);
	}

	public void OnCollisionEnter(Collision other)
	{
		/*LEVEL DEPENDENT = MAKE DIVIDOR BIGGER FOR BETTER GK*/
		float randDiv = getRandFloat(1.5f, 3.0f);

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

		if (other.collider.name.Contains("bonusDiamond"))
		{
			bonusDiamondHit = true;
			audioManager.PlayNoCheck("moneySound");
		}

		if (other.collider.name.Contains("bonusCoin"))
		{
			bonusCoinHit = true;
			audioManager.PlayNoCheck("moneySound");
		}

		if (other.collider.name.Contains("goalDownNet") ||
			other.collider.name.Contains("goalUpNet"))
		{
			audioManager.PlayNoCheck("net1");
		}

		if (other.collider.name == "floor")
		{
			//lastTimeBallFloorCollision = Time.time;
			//print("BALLFLOOR COLLISION SPEED " + rb.velocity + " TRANSFORM " 
			//	+ rb.transform.position);

			if (rb.velocity.y > 2.0f &&
				!playerMainScript.isOnBall() &&
				!playerMainScript.cpuPlayer.isOnBall())
			{
				//print("BALLBOUNCEPLAYNOW");
				audioManager.PlayNoCheck("ballbounce2");
			}

			ballTrailRendererStop();

			return;
		}

		if (other.collider.name.Contains("playerDown") ||
			other.collider.tag.Contains("playerDown"))
			{
			//print("CPUMOVEDEBUG123X_NOCPU BALLCOLLIDERNAME PLAYERDOWN " + other.collider.name);


			if (!playerMainScript.isShotActive())
			{
				ballTrailRendererStop();
			}

			if (playerMainScript.cpuPlayer.getShotActive())
			{
				playerDownLastGkCollision = Time.time;

				if (other.collider.name.Contains("HandCollider") ||
					other.collider.tag.Contains("HandCollider") ||
					other.collider.tag.Contains("Spine"))
				{
					print("#DBGNOCPUPLAYERCOL HIT ballPos " + transform.position + " other " + other.collider.name + 
						 " ballRbLoc " + 
						 playerMainScript.InverseTransformPointUnscaled(playerMainScript.getRbTransform(), transform.position));

					if (Time.time - lastTimeGkCollisionUpdatePlayer > 1.0f)
					{
						ballVelocityChange(false, other);
						lastTimeGkCollisionUpdatePlayer = Time.time;
						lastTimeGkCollisionUpdatePlayerBallVel = rb.velocity;
						//print("DBGNEWGKPHYS PLAYER DISTX ballVel velocity saved " + lastTimeGkCollisionUpdatePlayerBallVel);
					}
				}

				audioManager.PlayNoCheck("gksave1");
				float distX = Mathf.Abs(playerMainScript.getGkLastDistXCord());
				audioManager.Commentator_PlayRandomSave(distX);

				if (distX > 4.0f)
				{
					if (!audioManager.isPlayingByName("crowdOvation1Short") &&
						!isTrainingActive &&
						!isBonusActive)
					{
						audioManager.Play("crowdOvation1Short");
					}
				}

				if ((Time.time - lastTimeSaveAupdate) > 1.5f)
				{
					if (playerMainScript.setShotSaveStatistics("teamA"))
					{
						saveAupdate = true;
						lastTimeSaveAupdate = Time.time;
					}
				}
			}

			whoTouchBallLastTime = 1;
			playerMainScript.cpuPlayer.setShotActive(false);
			playerMainScript.setGkHelperImageVal(false);
			return;
		}

		if (other.collider.name.Contains("playerUp") ||
			other.collider.tag.Contains("playerUp"))
		{

			///print("#DBGCPUPLAYERCOL HIT ballPos " + transform.position + " other " + other.collider.name +
			//			 " ballRbLoc " +
			//			 playerMainScript.InverseTransformPointUnscaled(playerMainScript.cpuPlayer.getRbTransform(), transform.position));


			cpuPlayerCollision = true;
			playerUpLastGkCollision = Time.time;

			//print("BALLCOLLIDERNAME PLAYER UP " + other.collider.name);
			//print("GKDEBUG800 PLAYINGANIMATIONOW BALL HITTED $$$$");
			//print("VELCHANGE UP " + (Time.time - lastTimeUpdatePlayerUp));

			//Animator animatorTmp = playerMainScript.getAnimator();
			//print("ANIMATIONPLAYING PLAYERUP " + playerMainScript.nameAnimationPlaying(animatorTmp, 1.0f) + " normTime " +
			//		animatorTmp.GetCurrentAnimatorStateInfo(0).normalizedTime);

			if (!playerMainScript.cpuPlayer.getShotActive())
			{
				ballTrailRendererStop();
			}

			if (playerMainScript.isShotActive())
			{
				if (other.collider.name.Contains("HandCollider") ||
					other.collider.tag.Contains("HandCollider") ||
					other.collider.tag.Contains("Spine"))
				{
				
					/*print("#DBGCPUPLAYERCOL HIT ballPos " + transform.position + " other " + other.collider.name +
						 " ballRbLoc " +
						 playerMainScript.InverseTransformPointUnscaled(playerMainScript.getRbTransform(), transform.position));*/

					if (Time.time - lastTimeGkCollisionUpdateCpu > 1.0f)
					{
						ballVelocityChange(true, other);
						lastTimeGkCollisionUpdateCpu = Time.time;
						lastTimeGkCollisionUpdateCpuBallVel = rb.velocity;
						//print("DBGNEWGKPHYS CPU DISTX ballVel velocity saved " + lastTimeGkCollisionUpdateCpuBallVel);
					}
				}

				audioManager.PlayNoCheck("gksave1");
				float distX = Mathf.Abs(playerMainScript.getGkLastDistXCord());
				audioManager.Commentator_PlayRandomSave(distX);

				if (Mathf.Abs(playerMainScript.cpuPlayer.getGkLastDistXCord()) > 3.8f)
				{
					if (!audioManager.isPlayingByName("crowdWondered1Short") &&
						!isTrainingActive &&
						!isBonusActive)
					{
						audioManager.Play("crowdWondered1Short");
					}
				}

				Animator animator = playerMainScript.cpuPlayer.getAnimator();
				string name = playerMainScript.nameAnimationPlaying(animator, 1.0f);
				//print("GKDEBUG3CPU PLAYINGANIMATIONOW " + name + "  "
				//		+ animator.GetCurrentAnimatorStateInfo(0).IsName(name) + " ANIMNORMTIME "
				//			+ animator.GetCurrentAnimatorStateInfo(0).normalizedTime);

				if ((Time.time - lastTimeSaveBupdate) > 1.5f)
				{
					playerMainScript.setShotSaveStatistics("teamB");
					lastTimeSaveBupdate = Time.time;
					saveBupdate = true;
				}
			}

			if ((other.collider.name.Contains("HandCollider") ||
				other.collider.tag.Contains("HandCollider") ||
				other.collider.tag.Contains("Spine")) &&
				(Time.time - lastTimeGkCollisionUpdateCpu <= 1.0f))
			{
				rb.velocity = lastTimeGkCollisionUpdateCpuBallVel;
				//print("DBGNEWGKPHYS CPU DISTX ballVel velocity OVERWRITE " + rb.velocity);
			}

			whoTouchBallLastTime = 2;
			ballCollided = true;

			if (other.collider.name.Contains("Head") ||
				other.collider.tag.Contains("Head"))
				headLastTimeCollisionPlayerUp = Time.time;

			return;
		}

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
				} else
				{
					goalDownLastTimeHitCrossbar = Time.time;
				}

				//Handheld.Vibrate();
				ballCollided = true;

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
			ballCollided = true;


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

		playerMainScript.setGkHelperImageVal(false);
	}

	public void OnTriggerEnter(Collider other)
	{
		//used by commentator
		if (other.GetComponent<Collider>().name == "goalUpCornerLeftCollider" ||
			other.GetComponent<Collider>().name == "goalUpCornerRightCollider")
		{
			goalUpCornertLastTimeHit = Time.time;
		}

		if (other.GetComponent<Collider>().name == "goalDownCornerLeftCollider" ||
			other.GetComponent<Collider>().name == "goalDownCornerRightCollider")
		{
			goalDownCornertLastTimeHit = Time.time;
		}

		if (other.GetComponent<Collider>().name == "goalUpBallCrossLine" ||
			other.GetComponent<Collider>().name == "goalDownBallCrossLine")
		{
			lastTimeGkCollisionUpdateCpu = 0f;
			lastTimeGkCollisionUpdatePlayer = 0f;

			if (playerMainScript.getTimeToShotExceeded() ||
				playerMainScript.getGameEnded())
				return;

			if (goalCollision == false)
			{
				if (other.GetComponent<Collider>().name == "goalUpBallCrossLine")
				{
					if (Globals.score1 == Globals.score2)
					{
						audioManager.setWasDrawBefore(true);
					}

					if (Globals.playerPlayAway)
					{
						if (powersScript.getGoalUpHandicap() == 0)
							Globals.score2++;
						else
							Globals.score2 += powersScript.getGoalUpHandicap();
						//idScoredTeam = 2;
					}
					else
					{						
						if (powersScript.getGoalUpHandicap() == 0)
							Globals.score1++;
						else
							Globals.score1 += powersScript.getGoalUpHandicap();

						//Globals.score1++;
						//idScoredTeam = 1;
					}

					audioManager.Commentator_PlayRandomGoal(false);
					StartCoroutine(audioManager.Commentator_AfterGoal(1.3f, 2f));

					idScoredTeam = 1;
					if (!Globals.isTrainingActive &&
						!Globals.isBonusActive)
					{

						for (int i = 0; i < goalUpFlare.Length; i++)
						{
							goalUpFlare[i].Play();
						}
					}

					if (((Time.time - lastTimeSaveBupdate) < 3.0f) &&
						saveBupdate)
					{
						playerMainScript.decSavesStatistics("teamB", 1);
						saveBupdate = false;
					}

					//idScoredTeam = 1;
					//idScoredTeam = 1;

					if (!isTrainingActive && !isBonusActive)
					{
						if ((teamHostID == 1 && !Globals.playerPlayAway) ||
							(teamHostID == 2 && Globals.playerPlayAway))
						{
							audioManager.Play("crowdGoal1");
							StartCoroutine(playFansChant(1.1f));
						}
						else
						{
							audioManager.Play("goallose1");
							StartCoroutine(audioManager.Commentator_FansWhistle(5f));
						}
					}
				}
				else
				{
					int rand = Random.Range(1, 3);
					if (rand == 1 || whoTouchBallLastTime == 1)
					{
						if (!isTrainingActive && !isBonusActive)
						{

							if ((teamHostID == 1 && Globals.playerPlayAway) ||
								(teamHostID == 2 && !Globals.playerPlayAway))
							{
								audioManager.Play("crowdGoal1");
								StartCoroutine(playFansChant(1.1f));
							}
							else
							{
								audioManager.Play("crowdMiss1");
							}
						}
						//	if (!isTrainingActive) 
						//			audioManager.Play("crowdMiss1");
					}
					else
					{
						if (!isTrainingActive && !isBonusActive)
						{
							if ((teamHostID == 1 && Globals.playerPlayAway) ||
								(teamHostID == 2 && !Globals.playerPlayAway))
							{
								audioManager.Play("crowdGoal1");
							StartCoroutine(playFansChant(1.1f));
							}
							else
							{
								audioManager.Play("goallose1");
								StartCoroutine(audioManager.Commentator_FansWhistle(5f));
							}
						}
					}

					if (((Time.time - lastTimeSaveAupdate) < 3.0f) &&
						saveAupdate)
					{
						playerMainScript.decSavesStatistics("teamA", 1);
						saveAupdate = false;
					}

					if (Globals.score1 == Globals.score2)
					{
						audioManager.setWasDrawBefore(true);
					}

					if (Globals.playerPlayAway)
					{
						if (powersScript.getGoalDownHandical() == 0)
							Globals.score1++;
						else
							Globals.score1 += powersScript.getGoalDownHandical();
						//Globals.score1++;
						//idScoredTeam = 1;
					}
					else
					{
						if (powersScript.getGoalDownHandical() == 0)
							Globals.score2++;
						else
							Globals.score2 += powersScript.getGoalDownHandical();

						//Globals.score2++;
						//idScoredTeam = 2;
					}

					audioManager.Commentator_PlayRandomGoal(true);

					if (!Globals.isTrainingActive &&
						!Globals.isBonusActive)
					{
						for (int i = 0; i < goalDownFlare.Length; i++)
						{
							goalDownFlare[i].Play();
						}
					}
					idScoredTeam = 2;
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
		{
			audioManager.Play("fanschant" + randChant.ToString(), 0.28f);
		}

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

	public void OnTriggerExit(Collider other)
	{
		if (other.GetComponent<Collider>().name == "goalUp")
		{
			goalCollision = true;
			//print("COLLISION DETECTED");
		}

		/*if (other.GetComponent<Collider>().name == "playerUp")
		{
			cpuPlayerCollision = false;
			print("COLLISION DETECTED");
		}*/
	}

	public void ballVelocityChange(bool isCpu, Collision other)
	{
		string lastGkAnimPlayed =
			playerMainScript.cpuPlayer.getLastGkAnimPlayed();

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
			string lastAnimPlayed = playerMainScript.cpuPlayer.getLastGkAnimPlayed();

			/*CPU IMPLEMENTATION*/
			if (playerMainScript.isLobShotActive() &&
			   (playerMainScript.cpuPlayer.getLastGkAnimPlayed().Contains("straight")))
			{
				//print("DEBUGONCOLLISIONVELOCITY BEFORE " + rb.velocity);
				Vector3 playerPos =
						playerMainScript.cpuPlayer.getRbPosition();
				Transform playerTransform =
					playerMainScript.cpuPlayer.getRbTransform();

				Vector3 localPlayerPos;
				if (lastGkAnimPlayed.Contains("up"))
				{
					localPlayerPos = new Vector3(0f, rb.transform.position.y, 10f);
					lobBounceSpeed = 4f;
				}
				else
				{
					localPlayerPos = new Vector3(0f, rb.transform.position.y / 2.0f, 6f);
				}

				Vector3 worldBallDirection =
					playerMainScript.TransformPointUnscaled(playerTransform, localPlayerPos);
				rb.velocity = (worldBallDirection - rb.transform.position).normalized * lobBounceSpeed;

				//print("DEBUGONCOLLISIONVELOCITY ballPOS " + rb.transform.position + " POINT WORLD" +
				//	worldBallDirection + " playerTransform " + playerTransform.position);
				//print("DEBUGONCOLLISIONVELOCITY AFTER " + rb.velocity);
			}
			else
			{
				if (lastAnimPlayed.Contains("punch") && (
					ballVelocity > 30f || lastAnimPlayed.Contains("up")))
				{
					notPossibleToCatch = true;
				}

				////print("#DEBUGBALLLOCAL lastAnimPlayed CPU " + lastAnimPlayed
				////	+ " ballVel " + ballVelocity
				////	+ " notPossibleToCatch " + notPossibleToCatch);

				tooFarFromHand = true;
				if (other.collider.name.Contains("HandCollider") &&
					!notPossibleToCatch)
				{

					leftPalmLocal = playerMainScript.InverseTransformPointUnscaled(
											playerMainScript.getCpuLeftPalm().transform,
											ballPos);

					rightPalmLocal = playerMainScript.InverseTransformPointUnscaled(
											playerMainScript.getCpuRightPalm().transform,
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
							if (Mathf.Abs(leftPalmLocal.x) > 0.6f ||
								Mathf.Abs(leftPalmLocal.y) > 0.4f ||
								Mathf.Abs(rightPalmLocal.x) > 0.6f ||
								Mathf.Abs(rightPalmLocal.y) > 0.4f)
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

				rb.angularVelocity /= (Mathf.Max(10f, ballVelocity) * 0.12f);
			}
			//rb.angularVelocity = new Vector3(0.1f, 0.1f, 0.1f);
		}
		else
		{ /*PLAYER IMPLEMENTATION*/
			string lastAnimPlayed = playerMainScript.getLastGkAnimPlayed();

			if (playerMainScript.cpuPlayer.isLobShotActive() &&
				playerMainScript.getLastGkAnimPlayed().Contains("straight"))
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
	
}