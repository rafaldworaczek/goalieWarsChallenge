using UnityEngine.Audio;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using GlobalsNS;
using UnityEngine.SceneManagement;


namespace AudioManagerNS
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;

        [Range(0.0f, 1.0f)]
        public float volume = 1.0f;
        [Range(0.1f, 3.0f)]
        public float pitch = 0.3f;

        public bool loop;

        [HideInInspector]
        public AudioSource source;

        public void setSource(AudioSource source)
        {
            source = source;
            source.clip = clip;
        }
    }

    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        public Sound[] sounds;
        private controllerRigid playerMainScript;
        private ballMovement ballScript;
        private Powers powersScript;

        private Dictionary<string, float> commentatorPlayLastTime;
        private Dictionary<string, bool> commentatorCommentPlayed;

        public static AudioManager instance;
        private float ShotSpeedMin;
        private bool isTraining = true;
        private bool isBonus = true;
        private int randChangeEverySecond = 0;
        private float randChangeEverySecondLastTime = 0f;
        private int randNormalMovesMinute = 0;
        private int randNormalRightPlaceToShoot = 0;
        private bool isCommentatorActive = true;
        private bool wasDrawBefore = true;

        void Awake()
        {
            foreach (Sound sound in sounds)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;
                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;
                sound.source.loop = sound.loop;
            }

            if (Globals.commentatorStr.Equals("NO"))
            {
                isCommentatorActive = false;
                return;
            }


            /* if (instance == null)
                 instance = this;
             else
             {
                 Destroy(gameObject);
                 return;
             }

             DontDestroyOnLoad(gameObject);*/
        }

        private void Start()
        {
            if (SceneManager.GetActiveScene().name.Equals("gameScene") ||
                SceneManager.GetActiveScene().name.Equals("gameSceneSportsHall") ||
                SceneManager.GetActiveScene().name.Equals("gameSceneMultiplayer_Room_2"))
            {
                randChangeEverySecond = UnityEngine.Random.Range(0, 3);
                randNormalMovesMinute = UnityEngine.Random.Range(0, 50);
                randNormalRightPlaceToShoot = UnityEngine.Random.Range(0, 60);
                randChangeEverySecondLastTime = Time.time;
                commentatorPlayLastTime = new Dictionary<string, float>();
                commentatorCommentPlayed = new Dictionary<string, bool>();
                playerMainScript = GameObject.Find("playerDown").GetComponent<controllerRigid>();
                powersScript = GameObject.Find("extraPowers").GetComponent<Powers>();
                isTraining = playerMainScript.isTrainingEnable();
                isBonus = playerMainScript.isBonusEnable();
                ballScript = GameObject.Find("ball1").GetComponent<ballMovement>();
                ShotSpeedMin = playerMainScript.getShotSpeedMin();
            }
        }

        void Update()
        {
            if (!isCommentatorActive)
                return;

            if (!SceneManager.GetActiveScene().name.Equals("gameScene") &&
                !SceneManager.GetActiveScene().name.Equals("gameSceneSportsHall"))
                return;

            if (isTraining ||
                isBonus)
                return;


            if ((Time.time - randChangeEverySecondLastTime) >= 1f)
                randChangeEverySecond = UnityEngine.Random.Range(0, 3);

            Commentator_FinalWhistle();
            if (playerMainScript.doesGameEnded())
                return;


            //Commentator_RunToBallFast();

            if (!checkIfAnyCommentatorPlaying())
            {
                Commentator_CrossBar();
                Commentator_Post();
                Commentator_LongRangeShot();
                Commentator_missShot_playerDown();
                Commentator_missShot_playerUP();
                Commentator_NormalGameComment();
                Commentator_FantasticFansAtmosphere();
                ///        Commentator_ballOut();
                Commentator_ballNotOutLongTime();
                Commentator_duringGame();
                Commentator_onBallLongTime();
                Commentator_maxTimeToShot();
                Commentator_MatchAlmostOver();
                Commentator_rocketShot();
                Commentator_ComeBackToGoalFaster();
            }
        }

        public void Play(string name)
        {
            if (
                (Globals.stadiumNumber == 2))
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.Play();
                //print("SOUNDPLAY " + name);
            }
            {
                //print("Audio CLip not found "  + name);
            }
        }

        public void PlayNoCheck(string name)
        {            
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.Play();
                //print("SOUNDPLAY " + name);
            }
            {
                //print("Audio CLip not found "  + name);
            }
        }


        public void Play(string name, float volume)
        {
            if (
                (Globals.stadiumNumber == 2))
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.volume = volume;
                s.source.Play();
                //print("SOUNDPLAY " + name);
            }
            {
                //print("Audio CLip not found "  + name);
            }
        }

        public void Commentator_RunToBallFast()
        {
            if (!isCommentatorActive)
                return;

            string name = "com_runtoball1";
            if (
                (ballScript.getBallPos().z > 0f) ||
                (!Commentator_lastTimePlayedBiggerThan(name, 70f)) ||
                !playerMainScript.doesGameStarted())
                return;

            Vector3 getBallPos = ballScript.getBallPos();
            Vector3 getPlayerPos = playerMainScript.getPlayerPosition();
            float distPlayer1ToBall = Vector2.Distance(new Vector2(getPlayerPos.x, getPlayerPos.z),
                                                       new Vector2(getBallPos.x, getBallPos.z));

            if ((distPlayer1ToBall < 2f) ||
                (playerMainScript.getplayerVelocity() < 5f))
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.Play();
                commentatorPlayLastTime[name] = Time.time;
            }
        }

        public void Commentator_ComeBackToGoalFaster()
        {
            if (!isCommentatorActive)
                return;

            string name = "com_comebacktogoal1";
            if (
                (ballScript.getBallPos().z < 0f) ||
                !playerMainScript.doesGameStarted() ||
                !commentatorCommentPlayed.ContainsKey(name))
                return;

            if (!(((Mathf.Abs(playerMainScript.getPlayerPosition().x) < 12) ||
                   (playerMainScript.getPlayerPosition().z < -7f)) &&
                    playerMainScript.getplayerVelocity() < 3f))
            {
                return;
            }

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.Play();
                commentatorPlayLastTime[name] = Time.time;
                commentatorCommentPlayed[name] = true;
            }
        }

        public void Commentator_NormalGameComment()
        {
            if (!isCommentatorActive)
                return;

            if ((ballScript.getBallPos().z < 0f) ||
                !playerMainScript.doesGameStarted())
                return;

            Vector3 getBallPos = ballScript.getBallPos();
            Vector3 getPlayerPos = playerMainScript.getPlayerPosition();
            float distPlayer1ToBall = Vector2.Distance(new Vector2(getPlayerPos.x, getPlayerPos.z),
                                                       new Vector2(getBallPos.x, getBallPos.z));

            string name = "EMPTY";
            bool playAway = Globals.playerPlayAway;
            if (distPlayer1ToBall > 4f)
            {
                if ((playAway && (Globals.score2 > (Globals.score1 + 2))) ||
                    (!playAway && (Globals.score1 > (Globals.score2 + 2))))
                    name = "com_normalgame_1";

                if ((Globals.score1 == Globals.score2) &&
                    (playerMainScript.getMatchTimeMinute() > 30f))
                    name = "com_normalgame_2";
            }

            if (!Commentator_lastTimePlayedBiggerThan(name, 500f) ||
                name.Equals("EMPTY"))
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.Play();
                commentatorPlayLastTime[name] = Time.time;
            }
        }

        public string Commentator_GetRandomSave(float distX)
        {
            string name = "com_save1";
            for (int i = 0; i < 5; i++)
            {
                int randomSave = UnityEngine.Random.Range(1, 5);
                name = "com_save" + randomSave.ToString();
                if (distX < 1.0f)
                {
                    name = "com_savenormal1";
                }
                {
                    if (distX < 4.0f)
                        return "EMPTY";
                }

                if (Commentator_lastTimePlayedBiggerThan(name, 15f))
                    break;
            }

            return name;
        }

        public void Commentator_PlayRandomSave(float distX)
        {
            if (isTraining ||
                isBonus ||
                !isCommentatorActive)
                return;

            string name = Commentator_GetRandomSave(distX);
            if (name.Equals("EMPTY"))
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                stopAllCommentator();
                s.source.Play();
                commentatorPlayLastTime[name] = Time.time;
            }
        }

        public void setWasDrawBefore(bool val)
        {
            wasDrawBefore = val;
        }

        public bool getWasDrawBefore()
        {
            return wasDrawBefore;
        }

        public string Commentator_getRandGoalAudio(bool isCpu)
        {
            string name = "com_goal1";
            for (int i = 0; i < 5; i++)
            {

                int randomGoal = UnityEngine.Random.Range(1, 5);
                if (checkIfAmazingGoal())
                    randomGoal = UnityEngine.Random.Range(1, 4);

                name = "com_goal" + randomGoal.ToString();

                string isVolleyGoal = playerMainScript.getShotType();
                if (isCpu)
                    isVolleyGoal = playerMainScript.cpuPlayer.getShotType();

                bool isLobGoal = playerMainScript.isLobShotActive();
                if (isCpu)
                    isLobGoal = playerMainScript.cpuPlayer.isLobShotActive();

                if (isLobGoal &&
                   (UnityEngine.Random.Range(0, 3) > 0))
                {
                    name = "com_lobgoal1";
                }
                else if (isVolleyGoal.Contains("volley") &&
                         (UnityEngine.Random.Range(0, 3) > 0))
                {
                    name = "com_volleyshot_1";
                }
                else
                {
                    if ((Globals.score1 == 2 && Globals.score2 == 0) ||
                        (Globals.score2 == 0 && Globals.score1 == 2))
                    {
                        name = "com_result_2_0";
                    }

                    if ((Globals.score1 == Globals.score2))
                    {
                        if ((UnityEngine.Random.Range(0, 2) == 0))
                            name = "com_goalnormalequalizer1";
                        else
                            name = "com_goal1_draw";
                    }

                    // if ((Globals.score1 == (Globals.score2 + 1)) ||
                    //     ((Globals.score2 + 1) == Globals.score1))
                    if (getWasDrawBefore())
                    {
                        name = "com_goal1_lead";
                        setWasDrawBefore(false);
                    }
                }

                if (Commentator_lastTimePlayedBiggerThan(name, 18f))
                    break;
            }

            return name;
        }

        public bool checkIfAmazingGoal()
        {
            //cpu goal
            if (!powersScript.getCpuGoalEnlarge() &&
               ((Time.time - ballScript.getGoalUpCornertLastTimeHit() < 0.2f) ||
               ((Time.time - ballScript.getGoalUpLastTimeHitPosts()) < 0.2f) ||
               ((Time.time - ballScript.getGoalUpLastTimeHitCrossbar()) < 0.2f)))
                return true;

            //player goal
            if (powersScript.getPlayerGoalEnlarge() &&
               ((Time.time - ballScript.getGoalDownCornertLastTimeHit() < 0.2f) ||
               ((Time.time - ballScript.getGoalDownLastTimeHitPosts()) < 0.2f) ||
               ((Time.time - ballScript.getGoalDownLastTimeHitCrossbar()) < 0.2f)))
                return true;

            return false;
        }

        public void Commentator_PlayRandomGoal(bool isCpu)
        {

            if (isTraining ||
                isBonus ||
                !isCommentatorActive)
                return;

            string name = Commentator_getRandGoalAudio(isCpu);

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                stopAllCommentator();
                s.source.Play();
                commentatorPlayLastTime[name] = Time.time;
            }
        }

        public void Commentator_Post()
        {
            string name = "com_goalpost1";

            if (!playerMainScript.doesGameStarted())
                return;

            if (((Time.time - ballScript.getGoalUpLastTimeHitPosts()) > 0.5f) &&
                ((Time.time - ballScript.getGoalDownLastTimeHitPosts()) > 0.5f))
                return;

            if (randChangeEverySecondLastTime > 0)
            {
                if (((Time.time - ballScript.getGoalDownLastTimeHitPosts()) > 0.4f) &&
                    ((Time.time - ballScript.getGoalDownLastTimeHitPosts()) > 0.4f) &&
                    playerMainScript.isBallinGame())
                    name = "com_post1";
            }

            if (!Commentator_lastTimePlayedBiggerThan(name, 20f))
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                stopAllCommentator();
                s.source.Play();
                commentatorPlayLastTime[name] = Time.time;
            }
        }

        public void Commentator_CrossBar()
        {
            string name = "com_crossbarhit1";
            if (
                !Commentator_lastTimePlayedBiggerThan(name, 20f) ||
                !playerMainScript.doesGameStarted())
                return;

            if (((Time.time - ballScript.getGoalUpLastTimeHitCrossbar()) > 0.2f) &&
                ((Time.time - ballScript.getGoalDownLastTimeHitCrossbar()) > 0.2f))
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                stopAllCommentator();
                s.source.Play();
                commentatorPlayLastTime[name] = Time.time;
            }
        }

        public IEnumerator Commentator_FantasticFansAtmosphere_Couroutine(float delay,
                                                                          string name)
        {
            yield return new WaitForSeconds(delay);

            if (checkIfAnyCommentatorPlaying())
                yield break;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.Play();
                commentatorPlayLastTime["fansatmosphere"] = Time.time;
            }
        }

        public void Commentator_FantasticFansAtmosphere()
        {
            if (
                !playerMainScript.setTextureScript.isFlareEnable() ||
                !playerMainScript.doesGameStarted())
                return;

            if (!isPlayingByName("fanschant3") &&
                !isPlayingByName("fanschant4"))
                return;

            string name = "com_fansatmosphere_1";
            if (UnityEngine.Random.Range(0, 2) == 1)
                name = "com_fansloud1";

            if (!Commentator_lastTimePlayedBiggerThan("fansatmosphere", 50f))
                return;

            StartCoroutine(
                Commentator_FantasticFansAtmosphere_Couroutine(1.5f, name));
        }
        public void Commentator_MatchAlmostOver()
        {
            if ((playerMainScript.getMatchTimeMinute() < 75f) ||
                 commentatorCommentPlayed.ContainsKey("com_matchalmostover1"))

                return;

            string name = "com_matchalmostover1";
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.Play();
                commentatorCommentPlayed[name] = true;
            }
        }

        public IEnumerator Commentator_FansWhistle(float time)
        {
            float execTime = 0f;

            if (isTraining ||
                isBonus ||
                !isCommentatorActive)
                yield break;

            if (!Commentator_lastTimePlayedBiggerThan("com_fans_whistling1", 35f))
                yield break;

            while (true)
            {
                if (execTime > time)
                    break;

                execTime += 0.5f;
                if (checkIfAnyCommentatorPlaying())
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                string name = "com_fans_whistling1";
                Sound s = Array.Find(sounds, sound => sound.name == name);
                if (s != null && !s.source.isPlaying)
                {
                    s.source.Play();
                    commentatorPlayLastTime[name] = Time.time;
                    break;
                }
            }
        }

        private void Commentator_LongRangeShot()
        {
            bool cpuShotActive = playerMainScript.cpuPlayer.getShotActive();
            bool playerShotActive = playerMainScript.isShotActive();
            string name = "com_longrange_1";
            if (!Commentator_lastTimePlayedBiggerThan(name, 30f))
                return;

            if (!cpuShotActive &&
                !playerShotActive)
                return;

            Vector3 getBallPos = ballScript.getBallPos();
            if (cpuShotActive)
            {
                Vector3 getCpuPos = playerMainScript.cpuPlayer.getPlayerPosition();
                float distPlayer2ToBall = Vector2.Distance(new Vector2(getCpuPos.x, getCpuPos.z),
                                                           new Vector2(getBallPos.x, getBallPos.z));
                float distToGoal = Vector2.Distance(new Vector2(getCpuPos.x, getCpuPos.z),
                                                      new Vector2(0f, -playerMainScript.getPitchHeight()));
                if (distPlayer2ToBall > 5f ||
                    distToGoal < 22f)
                    return;
            }
            else
            {
                Vector3 getPlayerPos = playerMainScript.getPlayerPosition();
                float distPlayer1ToBall = Vector2.Distance(new Vector2(getPlayerPos.x, getPlayerPos.z),
                                                           new Vector2(getBallPos.x, getBallPos.z));
                float distToGoal = Vector2.Distance(new Vector2(getPlayerPos.x, getPlayerPos.z),
                                                    new Vector2(0f, playerMainScript.getPitchHeight()));
                if (distPlayer1ToBall > 5f ||
                    distToGoal < 22f)
                    return;
            }

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.Play();
                commentatorPlayLastTime[name] = Time.time;
            }
        }

        public IEnumerator Commentator_AfterGoal(float delayBefore,
                                                 float maxWaitTime)
        {
            if (isTraining ||
                isBonus ||
                !isCommentatorActive)
                yield break;

            float execTime = 0f;
            if (!checkIfAmazingGoal())
                yield break;

            yield return new WaitForSeconds(delayBefore);

            string name = "com_aftergoal1";
            if (!Commentator_lastTimePlayedBiggerThan(name, 50f))
                yield break;

            while (true)
            {
                if (execTime > maxWaitTime)
                    break;

                execTime += 0.5f;
                if (checkIfAnyCommentatorPlaying())
                {
                    yield return new WaitForSeconds(0.5f);
                    continue;
                }

                Sound s = Array.Find(sounds, sound => sound.name == name);
                if (s != null && !s.source.isPlaying)
                {
                    s.source.Play();
                    commentatorPlayLastTime[name] = Time.time;
                    break;
                }
            }
        }

        public void Commentator_FinalWhistle()
        {
            if (commentatorCommentPlayed.ContainsKey("matchFinalWhistle") ||
                !playerMainScript.doesGameEnded())
                return;

            string name = "com_finalwhistle1";
            if (UnityEngine.Random.Range(0, 2) == 1)
                name = "com_finalwhistle2";

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                stopAllCommentator();
                s.source.Play();
                commentatorCommentPlayed["matchFinalWhistle"] = true;
            }
        }

        public void Commentator_rocketShot()
        {
            if (!playerMainScript.doesGameStarted())
                return;

            string name = "";

            int randShot = UnityEngine.Random.Range(0, 2);

            if ((playerMainScript.cpuPlayer.getShotActive() &&
                (playerMainScript.cpuPlayer.getTimeOfBallFly() < (ShotSpeedMin + 50f)) &&
                (playerMainScript.cpuPlayer.getCurveShotFlyPercent() > 0.7f) &&
                playerMainScript.cpuPlayer.isShotCurve()) ||
               (playerMainScript.isShotActive() &&
               (playerMainScript.getTimeOfBallFly() < (ShotSpeedMin + 50f)) &&
               (playerMainScript.cpuPlayer.getCurveShotFlyPercent() > 0.7f) &&
                playerMainScript.isShotCurve()))
            {
                name = "com_shot1";
                if ((UnityEngine.Random.Range(0, 2) == 0) &&
                    (playerMainScript.cpuPlayer.getShotActive() &&
                    playerMainScript.isShotOnTarget(
                         playerMainScript.cpuPlayer.getEndPosOrg(),
                         playerMainScript.getGoalSizePlr1())) ||
                    (playerMainScript.isShotActive() &&
                     playerMainScript.isShotOnTarget(
                     playerMainScript.getEndPosOrg(),
                     playerMainScript.getGoalSizePlr2())))
                {
                    name = "com_shot1";
                }
                ///TODOextrasho            
            }

            if (!Commentator_lastTimePlayedBiggerThan(name, 25))
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                commentatorPlayLastTime[name] = Time.time;
                s.source.Play();
            }
        }

        public void Commentator_ballNotOutLongTime()
        {
            float howLongballNotOut =
               Time.time - playerMainScript.getLastTimeBallWasOut();

            if (
               (howLongballNotOut < 30f) ||
               !playerMainScript.doesGameStarted())
                return;

            string name = "";
            if (((Time.time - ballScript.getPlayerUpLastGkCollision()) > 2f) &&
                (Time.time - ballScript.getPlayerDownLastGkCollision() > 2f))
                name = "com_exchange1";

            if (!Commentator_lastTimePlayedBiggerThan(name, 50f) ||
                name.Equals(""))
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.Play();
                commentatorPlayLastTime[name] = Time.time;
                commentatorCommentPlayed[name] = true;
            }
        }

        public void Commentator_extraPower(string type)
        {
            if (!isCommentatorActive)
                return;

            string name = "";
            switch (type)
            {
                case "enlargeGoal":
                    name = "com_goalenlarge_1";
                    break;
                case "twoextragoals":
                    name = "com_extraPower_extragoals_1";
                    break;
                case "wall":
                    name = "com_extraPower_wall_1";
                    if (UnityEngine.Random.Range(0, 2) == 1)
                        name = "com_extraPower_wall_2";
                    break;
            }

            if (UnityEngine.Random.Range(0, 3) == 1)
                name = "com_extraPowerUsed_1";

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                stopAllCommentator();
                s.source.Play();
                commentatorPlayLastTime[name] = Time.time;
            }
        }

        public void Commentator_ballOut()
        {
            if (!playerMainScript.isBallOutOfPitch() ||
                (playerMainScript.getDelayAfterGoal() > 0.4f) ||
                 !playerMainScript.doesGameStarted())
                return;

            string name = "com_out";
            if (!Commentator_lastTimePlayedBiggerThan(name, 15f))
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.Play();
                commentatorPlayLastTime[name] = Time.time;
            }
        }

        public void Commentator_onBallLongTime()
        {
            if (
                !playerMainScript.doesGameStarted())
                return;

            float runTime = 0f;
            if ((ballScript.getBallPos().z < 0) &&
                playerMainScript.getLastTimePlayerOnBall() != -1f)
                runTime = Time.time - playerMainScript.getLastTimePlayerOnBall();

            if ((ballScript.getBallPos().z >= 0) &&
                playerMainScript.cpuPlayer.getLastTimePlayerOnBall() != -1f)
                runTime = Time.time - playerMainScript.cpuPlayer.getLastTimePlayerOnBall();

            if (runTime > 4f)
            {
                string name = "com_duringame1";
                if (UnityEngine.Random.Range(0, 2) == 1)
                    name = "com_duringame2";
                if (UnityEngine.Random.Range(0, 2) == 1)
                    name = "com_misleadgk1";

                if (!Commentator_lastTimePlayedBiggerThan(name, 50f))
                    return;

                Sound s = Array.Find(sounds, sound => sound.name == name);
                if (s != null && !s.source.isPlaying)
                {
                    s.source.Play();
                    commentatorPlayLastTime[name] = Time.time;
                }
            }
        }

        public void Commentator_duringGame()
        {
            if (
                !playerMainScript.doesGameStarted() ||
                !playerMainScript.getIsPlayerOnTheBallNow() ||
                !playerMainScript.isBallinGame() ||
                (playerMainScript.getMatchTimeMinute() < randNormalMovesMinute))
                return;

            /*  Vector3 getBallPos = ballScript.getBallPos();
              Vector3 getPlayerPos = playerMainScript.getPlayerPosition();
              Vector3 getCpuPos = playerMainScript.cpuPlayer.getPlayerPosition();
              float distPlayer1ToBall = Vector2.Distance(new Vector2(getPlayerPos.x, getPlayerPos.z),
                                                         new Vector2(getBallPos.x, getBallPos.z));
              float distPlayer2ToBall = Vector2.Distance(new Vector2(getPlayerPos.x, getPlayerPos.z),
                                                         new Vector2(getCpuPos.x, getCpuPos.z));

              if (distPlayer1ToBall < 5f ||
                  distPlayer2ToBall < 5f)
                  return;*/

            string name = "com_gkgoodmove1";
            if ((playerMainScript.getMatchTimeMinute() > 50f) &&
                (Mathf.Abs(Globals.score1 - Globals.score2) <= 2f))
                name = "com_dynamicgame1";

            if (!Commentator_lastTimePlayedBiggerThan(name, 50f))
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.Play();
                commentatorPlayLastTime[name] = Time.time;
                commentatorCommentPlayed[name] = true;
            }
        }

        public void Commentator_maxTimeToShot()
        {
            int timeToShot = (int)playerMainScript.getTimeOfShot();
            int maxTimeToShot = (int)playerMainScript.getMaxTimeToShot();


            if (!playerMainScript.doesGameStarted() ||
                 !playerMainScript.isBallinGame())
                return;

            string name = "EMTPY";
            if ((randChangeEverySecond == 0) &&
                (playerMainScript.getMatchTimeMinute() > randNormalRightPlaceToShoot) &&
                ((timeToShot > 3 && Globals.level > 2) ||
                ((Globals.level == 1 && timeToShot > 8))) &&
                Commentator_lastTimePlayedBiggerThan("com_rightplacetoshot_1", 80f) &&
                (playerMainScript.getIsPlayerOnTheBallNow() ||
                                   playerMainScript.cpuPlayer.getIsPlayerOnTheBallNow()))
            {
                name = "com_rightplacetoshot_1";
                if ((UnityEngine.Random.Range(0, 3) == 0) &&
                    Commentator_lastTimePlayedBiggerThan("com_rightplacetoshot_2", 80f))
                {
                    name = "com_rightplacetoshot_2";
                }
            }
            else if (((timeToShot + 3) == maxTimeToShot) &&
                    (randChangeEverySecond == 1) &&
                    Commentator_lastTimePlayedBiggerThan("com_secondstoshoot3", 25f))
            {
                name = "com_secondstoshoot3";
            }
            else if (((timeToShot + 2) == maxTimeToShot) &&
                     (Commentator_lastTimePlayedBiggerThan("com_secondstoshoot2", 35f)))
            {
                name = "com_secondstoshoot2";
            }

            if (name.Equals("EMPTY") ||
                (name.Contains("com_secondstoshoot") &&
                (!Commentator_lastTimePlayedBiggerThan("com_secondstoshoot3", 35f) ||
                 !Commentator_lastTimePlayedBiggerThan("com_secondstoshoot2", 35f))))
                return;

            if (name.Contains("com_rightplacetoshot") &&
                !Commentator_lastTimePlayedBiggerThan(name, 50f))
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null && !s.source.isPlaying)
            {
                s.source.Play();
                commentatorPlayLastTime[name] = Time.time;
            }
        }

        private bool Commentator_lastTimePlayedBiggerThan(string name, float time)
        {
            float keyValue = 0f;
            if (commentatorPlayLastTime.TryGetValue(name, out keyValue))
            {
                if ((Time.time - keyValue) < time)
                    return false;
            }

            return true;
        }

        bool playerUpWasShotActive = false;
        private void Commentator_missShot_playerUP()
        {
            if (
                !playerMainScript.doesGameStarted())
                return;

            bool cpuShotActive = playerMainScript.cpuPlayer.getShotActive();

            if (cpuShotActive)
            {
                playerUpWasShotActive = true;
            }

            if (playerMainScript.isBallOutOfPitch() &&
                !cpuShotActive &&
                playerUpWasShotActive &&
               ((Time.time - ballScript.getPlayerDownLastGkCollision()) > 5f))
            {
                Vector3 endPos = playerMainScript.cpuPlayer.getEndPosOrg();
                Vector3 goalSize = playerMainScript.getGoalSizePlr1();
                bool isMissActive = false;

                string name = "com_goalmiss_1";
                int rand = UnityEngine.Random.Range(0, 3);
                if ((Mathf.Abs(endPos.x) < (goalSize.x + 1.5f)) &&
                    (Mathf.Abs(endPos.x) > (goalSize.x + 0.3f)) &&
                    (endPos.y < (goalSize.y + 1)))
                {
                    name = "com_soclose_miss_1";
                    if (rand == 1)
                        name = "com_soclose_miss_2";
                    else if (rand == 2)
                    {
                        name = "com_goalmiss_1";
                    }

                    isMissActive = true;
                }

                if ((Mathf.Abs(endPos.x) > (goalSize.x + 3f)) ||
                    (endPos.y > (goalSize.y + 3)))
                {
                    name = "com_goalbigmiss_1";
                    isMissActive = true;
                }

                if (!Commentator_lastTimePlayedBiggerThan(name, 15f) ||
                    !isMissActive)
                    return;

                Sound s = Array.Find(sounds, sound => sound.name == name);
                if (s != null && !s.source.isPlaying)
                {
                    stopAllCommentator();
                    s.source.Play();
                    commentatorPlayLastTime[name] = Time.time;
                }
            }

            if (!cpuShotActive &&
                playerUpWasShotActive)
                playerUpWasShotActive = false;
        }

        bool playerDownWasShotActive = false;
        private void Commentator_missShot_playerDown()
        {
            if (
                !playerMainScript.doesGameStarted())
                return;

            bool shotActive = playerMainScript.isShotActive();
            if (shotActive)
            {
                playerDownWasShotActive = true;
            }

            if (playerMainScript.isBallOutOfPitch() &&
                !shotActive &&
                playerDownWasShotActive &&
               ((Time.time - ballScript.getPlayerUpLastGkCollision()) > 5f))
            {
                Vector3 endPos = playerMainScript.getEndPosOrg();
                Vector3 goalSize = playerMainScript.getGoalSizePlr2();
                bool isMissActive = false;

                string name = "com_goalmiss_1";
                int rand = UnityEngine.Random.Range(0, 3);
                if ((Mathf.Abs(endPos.x) < (goalSize.x + 1.5f)) &&
                    (Mathf.Abs(endPos.x) > (goalSize.x + 0.3f)) &&
                    (endPos.y < (goalSize.y + 1)))
                {
                    name = "com_soclose_miss_1";
                    if (rand == 1)
                        name = "com_soclose_miss_2";
                    else if (rand == 2)
                    {
                        name = "com_goalmiss_1";
                    }

                    isMissActive = true;
                }

                if ((Mathf.Abs(endPos.x) > (goalSize.x + 3f)) ||
                    (endPos.y > (goalSize.y + 3)))
                {
                    name = "com_goalbigmiss_1";
                    isMissActive = true;
                }

                if (!Commentator_lastTimePlayedBiggerThan(name, 15f) ||
                    !isMissActive)
                    return;

                Sound s = Array.Find(sounds, sound => sound.name == name);
                if (s != null && !s.source.isPlaying)
                {
                    stopAllCommentator();
                    s.source.Play();
                    commentatorPlayLastTime[name] = Time.time;
                }
            }

            if (!shotActive &&
                playerDownWasShotActive)
                playerDownWasShotActive = false;
        }

        public void PlayAtTheSameTime(string name)
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null)
            {
                s.source.Play();
                //print("SOUNDPLAY " + name);
            }
            {
                //print("Audio CLip not found "  + name);
            }
        }

        public void Stop(string name)
        {
            if (
               (Globals.stadiumNumber == 2))
                return;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null)
            {
                s.source.Stop();
            }
            else
            {
                //print("Audio CLip not found " + name);
            }
        }

        public bool isPlaying()
        {
            if (
               (Globals.stadiumNumber == 2))
                return false;

            foreach (Sound sound in sounds)
            {
                if (sound.source.isPlaying) return true;
            }
            return false;
        }

        private bool checkIfAnyCommentatorPlaying()
        {
            if (
               (Globals.stadiumNumber == 2))
                return false;

            foreach (Sound sound in sounds)
            {
                if (sound.name.Contains("com_") &&
                    sound.source.isPlaying)
                    return true;
            }

            return false;
        }

        public bool isPlayingByName(string name)
        {
            if (
                (Globals.stadiumNumber == 2))
                return false;

            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s != null)
            {
                return s.source.isPlaying;
            }

            return false;
        }

        public void stopAllCommentator()
        {
            foreach (Sound sound in sounds)
            {
                if (sound.name.StartsWith("com_"))
                    sound.source.Stop();
            }
        }


    }
}
