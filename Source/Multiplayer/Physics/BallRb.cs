using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using graphicsCommonNS;
using GlobalsNS;

public class BallRb : MonoBehaviour
{
	private playerControllerMultiplayer playerMainScript;
	public Rigidbody rb;
	private bool goalCollision;
	private bool cpuPlayerCollision;
	private GameObject trailBall;
	public GameObject drawPrefabBallTrail;
	private bool ballTrailRendererActive = false;
	public bool ballCollided = false;
	private GraphicsCommon graphics;
	string objectName;
	private int idScoredTeam;
	private float lastTimeSaveAupdate;
	private float lastTimeSaveBupdate;

	private float lastTimeWallCollisionUpdate;
	private float lastTimeWallCollisionUpdateCpu;

	private float lastTimeGoalCollisionUpdate;
	private float lastTimeGoalCollisionUpdateCpu;

	private float lastTimeGkCollisionUpdateCpu;
	private float lastTimeGkCollisionUpdatePlayer;
	private Vector3 lastTimeGkCollisionUpdateCpuBallVel;
	private Vector3 lastTimeGkCollisionUpdatePlayerBallVel;
	private float lastTimeHandCollide;

	public ParticleSystem[] goalUpFlare;
	public ParticleSystem[] goalDownFlare;

	private bool saveAupdate = false;
	private bool saveBupdate = false;

	private int teamHostID;
	public PredictCollision predictCollisionScript;

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
	public string lastGkAnimPlayed;


	public Rigidbody virtualPlayerRb;
	private Vector3 ballSpeedAfterCollision;
	GameObject virtualPlayer;
	GameObject virtualPlayerLeftHand;
	GameObject virtualPlayerRightHand;

	GameObject virtualPlayerLeftPalm;
	GameObject virtualPlayerRightPalm;


	// Use this for initialization
	void Awake()
	{
		objectName = gameObject.name;
		//playerMainScript = GameObject.Find("playerDown").GetComponent<controllerRigid>();
		ballSpeedAfterCollision = Globals.INCORRECT_VECTOR;
		headLastTimeCollisionPlayerUp = -Time.time;

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
		virtualPlayer = GameObject.Find("virtualPlayer");
		virtualPlayerLeftHand = 
			playerMainScript.getChildWithName(virtualPlayer, "virtualPlayer1LeftHand");

		virtualPlayerRightHand =
			playerMainScript.getChildWithName(virtualPlayer, "virtualPlayer1RightHand");

		virtualPlayerLeftPalm =
			playerMainScript.getChildWithName(virtualPlayer, "virtualPlayer1LeftPalm");

		virtualPlayerRightPalm =
			playerMainScript.getChildWithName(virtualPlayer, "virtualPlayer1RightPalm");


		//rb = GetComponent<Rigidbody>();
		goalCollision = false;
		cpuPlayerCollision = false;
		whoTouchBallLastTime = 0;
		lastTimeSaveAupdate = lastTimeSaveBupdate = Time.time;
		lastTimeGkCollisionUpdateCpu = Time.time - 2f; ;
		lastTimeGkCollisionUpdatePlayer = Time.time - 2f;

		lastTimeWallCollisionUpdate = Time.time - 2f;
		lastTimeWallCollisionUpdateCpu = Time.time - 2f;

		lastTimeGoalCollisionUpdate = Time.time - 2f;
		lastTimeGoalCollisionUpdateCpu = Time.time - 2f;


		lastGkAnimPlayed = predictCollisionScript.getLastAnimPlayed();
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

//		print("#DBGSHOT ONCOLLISSTAY " + other.collider.name + " vel "
//			+ rb.velocity + " getShotCurrentPlace " + predictCollisionScript.getShotCurrentPlace()
//			+ " ballRb " + rb.transform.position);
		return;

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


		//if (other.collider.name.Contains("playerUp") ||
		//	other.collider.tag.Contains("playerUp"))
		//{
		//	if ((other.collider.name.Contains("HandCollider") ||
		//		 other.collider.tag.Contains("HandCollider") ||
	//		 	 other.collider.tag.Contains("Spine")) &&
	//			(Time.time - lastTimeGkCollisionUpdateCpu <= 1.0f))
	//		{
	//			rb.velocity = lastTimeGkCollisionUpdateCpuBallVel;
				//print("DBGNEWGKPHYS CPU DISTX ballVel velocity OVERWRITE onCollisionStay " + rb.velocity);
	//		}
	//	}

		if (other.collider.name.Contains("virtualPlayer1") ||
			other.collider.tag.Contains("virtualPlayer1"))
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

		if (other.collider.name == "floor")
		{	
			ballTrailRendererStop();
			return;
		}

		if (objectName.Equals("virtualBall") &&
			PredictCollision.getBallVelocitySet() &&
			(other.collider.name.Contains("virtualPlayer1") ||
			 other.collider.name.Contains("Spine") ||
 			 other.collider.tag.Contains("virtualPlayer1")))
		{
			/*print("DEBUGGK1045 VIRTUAL collisionOutput onCollisionEnter virtual 1 " + other.collider.name + " vel " + 
				rb.velocity + " ballPos " + rb.transform.position);
			print("##DBGVEL1CALC virtualPlayerRb pos during collision virtual 1 #### " + virtualPlayerRb.transform.position
				+ " vel " + virtualPlayerRb.velocity + " ang " + virtualPlayerRb.angularVelocity);
			print("CPUMOVEDEBUG123X_NOCPU BALLCOLLIDERNAME PLAYERDOWN " + other.collider.name);
			*/
			//if (!playerMainScript.isShotActive())
			//{
			///		ballTrailRendererStop();
			//			}

			//if (playerMainScript.cpuPlayer.getShotActive())
			//{
			//if (other.collider.name.Contains("HandCollider") ||
		//		other.collider.tag.Contains("HandCollider") ||
		//	  	other.collider.tag.Contains("Spine"))
			//{
				//print("DBGSHOT Time.time - lastTimeGkCollisionUpdatePlayer " +
				//			(Time.time - lastTimeGkCollisionUpdatePlayer));
				//if (Time.time - lastTimeGkCollisionUpdatePlayer > 1.0f)
				//{
				lastTimeHandCollide = Time.time;

				if (rb.transform.position.z > 0)
				{
						if ((Time.time - lastTimeGkCollisionUpdateCpu) > 1.0f)
						{
							ballVelocityChange(false, other);
							lastTimeGkCollisionUpdateCpu = Time.time;
							//print("DEBUGGK1045 ballVelocityChange 1");
							ballSpeedAfterCollision = rb.velocity;
						}
				}
				else
				{
					if ((Time.time - lastTimeGkCollisionUpdatePlayer) > 1.0f)
					{
						lastTimeGkCollisionUpdatePlayer = Time.time;
						ballVelocityChange(true, other);
						//print("DEBUGGK1045 ballVelocityChange 2");
						ballSpeedAfterCollision = rb.velocity;
					}
				}

	       	    lastTimeGkCollisionUpdatePlayerBallVel = rb.velocity;
				lastTimeHandCollide = Time.time;
			//}

			whoTouchBallLastTime = 1;
			ballCollided = true;		
			//if ((Time.time - lastTimeHandCollide) > 1f)
			//	ballSpeedAfterCollision = rb.velocity;			
			//rb.angularVelocity = Vector3.zero;

			return;
		}

		//ballTrailRendererStop();

		if (!goalCollision)
		{
			if (objectName.Equals("virtualBall2") &&				
				(other.collider.name.Contains("goalDownPostCollider") || 
				 other.collider.name.Contains("goalUpPostCollider")))
			{
				//Handheld.Vibrate();
				//print("DBGCOLLISIONCALC1024DB CCCC ballCollided other.collider.name " + other.collider.name);
				//print("#DBGCOLLISIONCALC collisionOutput onCollisionEnter " + other.collider.name);
				if ((Time.time - lastTimeGoalCollisionUpdate) > 1f)
				{
					ballCollided = true;
					ballSpeedAfterCollision = rb.velocity;
					lastTimeGoalCollisionUpdate = Time.time;
					//print("DBGWALL collision speed 1 " + ballSpeedAfterCollision
					//   + " rb.transform.position " + rb.transform.position);	
				}

				//lastTimeGkCollisionUpdateCpu = 0f;
				//lastTimeGkCollisionUpdatePlayer = Time.time;
			}

			if (objectName.Equals("virtualBall2") && 
				(other.collider.name == "goalDownCrossBarCollider" ||
				 other.collider.name == "goalUpCrossBarCollider"))
			{
				//Handheld.Vibrate();

				if ((Time.time - lastTimeGoalCollisionUpdateCpu) > 1.0f)
				{
					ballCollided = true;

					//if (Time.time - lastTimeGkCollisionUpdatePlayer > 1.0f)
					//{
					//print("DBGCOLLISIONCALC1024D update ballSpeedAfterCollision crossbar " + rb.velocity);
					///ballSpeedAfterCollision = rb.velocity;
					//}

					ballSpeedAfterCollision = rb.velocity;
					lastTimeGoalCollisionUpdateCpu = Time.time;

					//print("DBGWALL collision speed 2 " + ballSpeedAfterCollision
				//		+ " rb.transform.position " + rb.transform.position);

				}
				//rb.velocity = Vector3.zero;
				//rb.angularVelocity = Vector3.zero;
				//print("DBGSHOT ballCollided other.collider.name " + other.collider.name);
				//print("#DBGCOLLISIONCALC1024DB collisionOutput onCollisionEnter " + other.collider.name);

				//lastTimeGkCollisionUpdateCpu = 0f;
				//lastTimeGkCollisionUpdatePlayer = Time.time;
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
		if (other.collider.name.Contains("wall") && 
			objectName.Equals("virtualBall2"))
		{
			//print("DBG342344COL wall collision " + other.collider.name);

			if ((Time.time - lastTimeWallCollisionUpdate) > 1.0f)
			{
				ballCollided = true;
				//if (Time.time - lastTimeGkCollisionUpdatePlayer > 1.0f)
				//{
				//print("DBGCOLLISIONCALC1024D update ballSpeedAfterCollision crossbar " + rb.velocity);
				ballSpeedAfterCollision = rb.velocity;
				//print("DBGWALL collision speed 3 " + ballSpeedAfterCollision + " rb.pos " + rb.transform.position);
				lastTimeWallCollisionUpdate = Time.time;
			}
			//}

			//rb.velocity = Vector3.zero;
			//rb.angularVelocity = Vector3.zero;
			//print("DBGCOLLISIONCALC CCCC ballCollided other.collider.name " + other.collider.name);
			//print("#DBGCOLLISIONCALC1024D collisionOutput onCollisionEnter " + other.collider.name
		//		+ " ballRb " + rb.transform.position);


			//print("wall collision before " + rb.velocity);

			/*rb.velocity = new Vector3(rb.velocity.x / wallRandDiv, 
								      rb.velocity.y / wallRandDiv, 
									  rb.velocity.z / wallRandDiv);
			rb.velocity = Vector3.zero;*/

			//print("wall collision after " + rb.velocity);

			//lastTimeGkCollisionUpdateCpu = 0f;
			//lastTimeGkCollisionUpdatePlayer = Time.time;
		}

		//print("DBGCOLLISIONCALC CCCC ballCollided other.collider.name " + other.collider.name);

		//ballCollided = true;
		//ballSpeedAfterCollision = rb.velocity;
		//rb.velocity = Vector3.zero;
		//rb.angularVelocity = Vector3.zero;

	//	print("DBGPREDICTCOLLISON PREDICTION ONCOLLISOIONENTER ballCollided other.collider.name " + other.collider.name);
	}

	public void OnTriggerEnter(Collider other)
	{
		///print("#DBG342344COL_NEWDBG34 ontrigger other.collider.name.Contain " + other.GetComponent<Collider>().name + " goalCollision " + goalCollision);

		if (other.GetComponent<Collider>().name == "goalDownBallCrossLine" ||
			other.GetComponent<Collider>().name == "goalUpBallCrossLine")
		{
			//lastTimeGkCollisionUpdateCpu = 0f;
			//lastTimeGkCollisionUpdatePlayer = 0f;

			//print("DBGCOLLISIONCALC1024D onTrigggerEnter  " + other.GetComponent<Collider>().name);

			if (playerMainScript.getTimeToShotExceeded() ||
				playerMainScript.getGameEnded())
				return;

			goalCollision = true;
		}
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
		
	}

	public void ballVelocityChange(bool isMaster, Collision other)
	{
		string lastGkAnimPlayed =
			playerMainScript.getLastGkAnimNameVirtual();

		//float randGkDiv = getRandFloat(2f, 2.6f);
		float randGkDiv = getRandFloat(2f, 4.0f);
		float lobBounceSpeed = 3f;
		float maxDistanceHandFromBall = 0.45f;
		bool tooFarFromHand = true;
		float ballVelocity = PredictCollision.getBallVelocity();
			//playerMainScript.peerPlayer.getBallShotVel();
		Vector3 ballPos = rb.transform.position;
		bool notPossibleToCatch = false;
		Vector3 leftPalmLocal = Vector3.zero;
		Vector3 rightPalmLocal = Vector3.zero;

		
		//lastGkAnimPlayed =
		//			playerMainScript.getLastGkAnimNameVirtual();


		string lastAnimPlayed = PredictCollision.predictedAnimName;
		// playerMainScript.getLastGkAnimNameVirtual();

		leftPalmLocal = playerMainScript.InverseTransformPointUnscaled(
							virtualPlayerLeftPalm.transform,
							//virtualPlayerLeftHand.transform,
							ballPos);

		rightPalmLocal = playerMainScript.InverseTransformPointUnscaled(
										virtualPlayerRightPalm.transform,
										//virtualPlayerRightHand.transform,
										ballPos);

		///print("DEBUGGK1045 ballVelocityChange ballvel " + ballVelocity + " rb.vel before " + rb.velocity
		///	+ " lastAnimPlayed " + lastAnimPlayed);

		if (playerMainScript.peerPlayer.isLobShotActive() &&
			lastAnimPlayed.Contains("straight"))
		{
			//print("DEBUGONCOLLISIONVELOCITY BEFORE " + rb.velocity);
			Vector3 playerPos =
					virtualPlayer.transform.position;
			Transform playerTransform =
					virtualPlayer.transform;
			Vector3 localPlayerPos = new Vector3(0f, 0.6f, 2f);

			Vector3 worldBallDirection =
				playerMainScript.TransformPointUnscaled(playerTransform, localPlayerPos);
			rb.velocity = (worldBallDirection - rb.transform.position).normalized * lobBounceSpeed;
		}
		else
		{
			if (lastAnimPlayed.Contains("punch") &&
			    //(ballVelocity > 33f || 
				lastAnimPlayed.Contains("up"))

			//  (ballVelocity > 33f || lastAnimPlayed.Contains("up")))
			{
				notPossibleToCatch = true;
			}

			//print("DEBUGGK1045 ballVelocityChange lastAnimPlayed PLAYER " + lastAnimPlayed + " ballVel " + ballVelocity
			//	+ " notPossibleToCatch " + notPossibleToCatch
			//	+ " other.collider.name " + other.collider.name);

			tooFarFromHand = true;
//			if (other.collider.name.Contains("HandCollider") &&

			if (!notPossibleToCatch)
			{


				//leftHandLocal = playerMainScript.InverseTransformPointUnscaled(
				//			virtualPlayerLeftHand.transform,
			//				ballPos);

		//		rightHandLocal = playerMainScript.InverseTransformPointUnscaled(
		//									virtualPlayerRightHand.transform,
			//								ballPos);


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

				//print("DBGVEL1CALC tooFarFromHand " + tooFarFromHand);

				//print("DEBUGGK1045 ballVelocityChange leftPalm " + leftPalmLocal
				//	+ " rightPalm " + rightPalmLocal + " tooFarFromHand " + tooFarFromHand);
			}

			if (!tooFarFromHand)
			{
				Transform playerTransform =
					virtualPlayer.transform;

				Vector3 localBouncePos =
					new Vector3(
						getRandFloat(-0.1f, 0.1f),
						getRandFloat(0f, 0.35f),
						getRandFloat(7f, 10f));

				if (lastAnimPlayed.Contains("left"))
				{
					Vector3 leftHandLocalPos = playerMainScript.InverseTransformPointUnscaled(
														virtualPlayer.transform,
														virtualPlayerLeftHand.transform.position);

					localBouncePos =
						new Vector3(leftHandLocalPos.x,
									leftHandLocalPos.y,
									getRandFloat(7f, 10f));
					//print("DBGVEL1CALC leftHandPos " + leftHandLocalPos);
				}
				else
				{
					if (lastAnimPlayed.Contains("right"))
					{
						Vector3 rightHandLocalPos = playerMainScript.InverseTransformPointUnscaled(
													virtualPlayer.transform,
													virtualPlayerRightHand.transform.position);

						localBouncePos =
							new Vector3(rightHandLocalPos.x,
										rightHandLocalPos.y,
										getRandFloat(7f, 10f));

						//print("DBGVEL1CALC rightHandLocalPos " + rightHandLocalPos);

					}

				}

				Vector3 worldBallDirection =
					playerMainScript.TransformPointUnscaled(playerTransform, localBouncePos);
				float ballSpeedPerc = Mathf.InverseLerp(0.0f, 34.5f, Mathf.Abs(ballVelocity));
				if (Mathf.Abs(lastBallVel.z) >= 34.5f)
					ballSpeedPerc = 1f;

				float finalSpeed = Mathf.Lerp(0.5f, 5.5f, ballSpeedPerc);
				rb.velocity = (worldBallDirection - rb.transform.position).normalized * finalSpeed;
			
				///print("DEBUGGK1045 ballVelocityChange " + rb.transform.position + " rb.velocity new " + rb.velocity
				//	+ " worldBallDirection " + worldBallDirection
			//		+ " isMaster " + isMaster);
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

				///print("#DBGVEL1CALC calculated ball vel before " + rb.velocity);
				rb.velocity = new Vector3(
						  rb.velocity.x / randGkDiv,
						  rb.velocity.y / randGkDiv,
						  rb.velocity.z / randGkDiv);
				//print("DEBUGGK1045 ballVelocityChange calculated ball vel after new " + rb.velocity);
			}
		}

		//print("COLLISIONDBG123 getBallVelocity " + ballVelocity + " rb.vel after " + rb.velocity);
		if (rb.velocity.y > 3f)
		{
			//if (rb.velocity.y < 0)
			//	rb.velocity = new Vector3(rb.velocity.x, -5f, rb.velocity.z);
			//else
			rb.velocity = new Vector3(rb.velocity.x, 3f, rb.velocity.z);
		}

		if (Mathf.Abs(rb.velocity.z) > 13f)
		{
			if (rb.velocity.z < 0)
				rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, -13f);
			else
				rb.velocity = new Vector3(rb.velocity.x,  rb.velocity.y, 13f);
		}

		rb.angularVelocity /= (Mathf.Max(10f, ballVelocity) * 0.12f);
	}

	/*public void ballVelocityChange(bool isCpu, Collision other, bool testAbove)
	{
		//string lastGkAnimPlayed =
		//	playerMainScript.cpuPlayer.getLastGkAnimPlayed();
		string lastAnimPlayed = "";
		//float randGkDiv = getRandFloat(2f, 2.6f);
		float randGkDiv = getRandFloat(2f, 4.0f);
		float lobBounceSpeed = 3f;
		float maxDistanceHandFromBall = 0.45f;
		bool tooFarFromHand = true;
		float ballVelocity = predictCollisionScript.getBallShotVel();
		Vector3 ballPos = rb.transform.position;
		bool notPossibleToCatch = false;
		Vector3 leftPalmLocal = Vector3.zero;
		Vector3 rightPalmLocal = Vector3.zero;

		//if (!isCpu)
		//{
		//	lastGkAnimPlayed =
		//		playerMainScript.getLastGkAnimPlayed();
		//}

		print("DBGSHOT ballVelocityChange ballVel " + rb.velocity);

		if (isCpu)
		{
			
		}
		else
		{ 
			//string lastAnimPlayed = playerMainScript.getLastGkAnimPlayed();

			//if (playerMainScript.cpuPlayer.isLobShotActive() &&
			if (predictCollisionScript.getLastGkAnimPlayed().Contains("straight"))
			{
				//print("DEBUGONCOLLISIONVELOCITY BEFORE " + rb.velocity);
				Vector3 playerPos =
						predictCollisionScript.getPlayerPosition();
				Transform playerTransform =
					predictCollisionScript.getRbTransform();
				Vector3 localPlayerPos = new Vector3(0f, 0.6f, 2f);
				Vector3 worldBallDirection =
					predictCollisionScript.TransformPointUnscaled(playerTransform, localPlayerPos);
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

					leftPalmLocal = predictCollisionScript.InverseTransformPointUnscaled(
												predictCollisionScript.getLeftPalm().transform,
												ballPos);

					rightPalmLocal = predictCollisionScript.InverseTransformPointUnscaled(
												predictCollisionScript.getRightPalm().transform,
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
						predictCollisionScript.getRbTransform();

					Vector3 localBouncePos =
						new Vector3(
							getRandFloat(-0.1f, 0.1f),
							getRandFloat(0f, 0.35f),
							getRandFloat(7f, 10f));

					if (lastAnimPlayed.Contains("left"))
					{
						Vector3 leftHandLocalPos = predictCollisionScript.InverseTransformPointUnscaled(
													predictCollisionScript.getRbTransform(),
													predictCollisionScript.getLeftHand().transform.position);

						localBouncePos =
							new Vector3(leftHandLocalPos.x,
										leftHandLocalPos.y,
										getRandFloat(7f, 10f));
					}
					else
					{
						if (lastAnimPlayed.Contains("right"))
						{
							Vector3 rightHandLocalPos = predictCollisionScript.InverseTransformPointUnscaled(
														predictCollisionScript.getRbTransform(),
														predictCollisionScript.getRightHand().transform.position);

							localBouncePos =
								new Vector3(rightHandLocalPos.x,
											rightHandLocalPos.y,
											getRandFloat(7f, 10f));
						}
					}

					Vector3 worldBallDirection =
						predictCollisionScript.TransformPointUnscaled(playerTransform, localBouncePos);
					float ballSpeedPerc = Mathf.InverseLerp(0.0f, 34.5f, Mathf.Abs(ballVelocity));
					if (Mathf.Abs(lastBallVel.z) >= 34.5f)
						ballSpeedPerc = 1f;

					float finalSpeed = Mathf.Lerp(0.5f, 5.5f, ballSpeedPerc);
					rb.velocity = (worldBallDirection - rb.transform.position).normalized * finalSpeed;

					print("DBGSHOT ballVelocityChange NEW CALC ballVel " + rb.velocity);

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

					if (predictCollisionScript.isLobShotActive())
						randGkDiv = 2.0f;

					rb.velocity = new Vector3(
								  rb.velocity.x / randGkDiv,
								  rb.velocity.y / randGkDiv,
								  rb.velocity.z / randGkDiv);

					print("DBGSHOT ballVelocityChange DIV ballVel " + rb.velocity);
				}
			}

			print("#DBGVEL " + rb.velocity);

			rb.angularVelocity /= (Mathf.Max(10f, ballVelocity) * 0.12f);
		}
	}
	*/

	public float getHeadLastTimeCollisionPlayerUp()
	{
		return headLastTimeCollisionPlayerUp;
	}

	public bool getBallCollided()
	{
		return ballCollided;
	}
	public Vector3 getBallCollidingSpeed()
    {
		return ballSpeedAfterCollision;
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