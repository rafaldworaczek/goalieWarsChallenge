using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeagueButtonClick : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject teamManagement;
    public GameObject leagueTable;
    public GameObject championsCup;
    public GameObject cupTable;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickTeamManagement()
    {
        teamManagement.SetActive(true);
    }

    public void onClickLeagueTable()
    {
        teamManagement.SetActive(false);
    }
}
