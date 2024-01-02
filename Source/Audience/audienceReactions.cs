using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalsNS;


public class audienceReactions : MonoBehaviour
{
    private string[] names = { "idle", "applause", "applause2", "celebration", "celebration2", "celebration3" };
    private string[] celebrationNames = { "celebration", "celebration2", "celebration3" };
    private string[] applauseNames = { "applause", "applause2" };
    float timeStart = 0f;
    private Animation[] AudienceMembers;
    private bool lockPlayApplause = false;
    private int loopsCounter = 0;
    void Awake()
    {
        if (Globals.stadiumNumber == 2)
            return;


        AudienceMembers = gameObject.GetComponentsInChildren<Animation>();
    }

    void Update()
    {
        if (Globals.stadiumNumber == 2)
            return;

        loopsCounter++;
        playRandom();
    }

    // Use this for initialization
    //void Update()
    //{
    //print("TIMEDIFF " + (Time.time - timeStart));

    //if ((Time.time - timeStart) > 8f)
    //{

    // foreach (Animation anim in AudienceMembers)
    // {
    //    string thisAnimation = names[Random.Range(0, 5)];

    //anim.wrapMode = WrapMode.Loop;
    //anim.GetComponent<Animation>().CrossFade(thisAnimation);
    //anim[thisAnimation].time = Random.Range(0f, 3f);
    //     anim.Play(thisAnimation);
    // }
    //   timeStart = Time.time;
    //print("ANIMATIONPLAYE");
    // }
    //}

    public void playIdle()
    {
        if (Globals.stadiumNumber == 2)
            return;

        foreach (Animation anim in AudienceMembers)
        {
            anim.Play("idle");
        }
    }

    public void playApplause1()
    {
        if (Globals.stadiumNumber == 2)
            return;

        foreach (Animation anim in AudienceMembers)
        {            
            anim.Play("applause");
        }  
    }

    public void playApplause2()
    {
        if (Globals.stadiumNumber == 2)
            return;

        foreach (Animation anim in AudienceMembers)
        {
            anim.Play("applause2");
        }
    }

    public void playCelebration1()
    {
        if (Globals.stadiumNumber == 2)
            return;

        foreach (Animation anim in AudienceMembers) {
            anim.Play("celebration");
        }
    }

    public void playCelebration2()
    {
        if (Globals.stadiumNumber == 2)
            return;

        foreach (Animation anim in AudienceMembers)
        {
            anim.Play("celebration2");
        }
    }

    public void playCelebration3()
    {
        if (Globals.stadiumNumber == 2)
            return;

        foreach (Animation anim in AudienceMembers)
        {
            anim.Play("celebration3");
        }
    }

    public void playRandomCelebration()
    {
        if (Globals.stadiumNumber == 2)
            return;

        int rand = 0;
        foreach (Animation anim in AudienceMembers)
        {
            rand = Random.Range(0, celebrationNames.Length);
            anim.Play(celebrationNames[rand]);
        }
    }

    public void playRandomApplause()
    {
        if (Globals.stadiumNumber == 2)
            return;

        foreach (Animation anim in AudienceMembers)
        {
            int rand = 0;
            rand = Random.Range(0, applauseNames.Length);
            anim.Play(applauseNames[rand]);
        }
    }

    public void playOneOfApplause(float playSeconds, float delayOffsetSeconds)
    {
        if (Globals.stadiumNumber == 2)
            return;

        if (!lockPlayApplause)
            StartCoroutine(playOnceOfApplauseCourutine(playSeconds, delayOffsetSeconds)); 
    }

    public void playRandom()
    {
        if (Globals.stadiumNumber == 2)
            return;

        foreach (Animation anim in AudienceMembers)
        {
            int rand = 0;
            rand = Random.Range(0, names.Length);
            if ((loopsCounter > 280) && (UnityEngine.Random.Range(0, 2) == 0))                
                rand = 0;
    
            if (!anim.isPlaying)
            {
                anim.Play(names[rand]);
            } 
        }
    }

    IEnumerator playOnceOfApplauseCourutine(float playSeconds, float delayOffsetSeconds)
    {
        int rand = Random.Range(0, applauseNames.Length);
        float timeStart = Time.time;

        lockPlayApplause = true;

        while ((Time.time - timeStart) < playSeconds) {
            foreach (Animation anim in AudienceMembers)
            {
                //if (!anim.isPlaying(applauseNames[rand]));
                anim.Play(applauseNames[rand]);
            }

            yield return new WaitForSeconds(2f);
        }

        lockPlayApplause = false;
    }
}