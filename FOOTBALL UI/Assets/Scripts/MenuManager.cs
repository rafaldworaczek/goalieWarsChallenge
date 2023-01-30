using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public GameObject MainScreen;
    public GameObject SettingsScreen;
    public GameObject SpinScreen;

    public GameObject ShopScreen;
    public GameObject GameOverScreen;

    public GameObject ChatDialog;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingsScreen.activeSelf)
            {
                SettingsScreen.SetActive(false);
            }
            else if (SpinScreen.activeSelf)
            {
                SpinScreen.SetActive(false);
            }
            else
            {

                ShowMainScreen();
            }
        }
    }
    void DisableAll()
    {
        MainScreen.SetActive(false);
        SettingsScreen.SetActive(false);
        ShopScreen.SetActive(false);
        GameOverScreen.SetActive(false);
    }

    public void ShowMainScreen()
    {
        DisableAll();
        MainScreen.SetActive(true);
    }

    public void ToggleSettingsScreen(bool status)
    {

        DisableAll();
        MainScreen.SetActive(true);
        if (status)
            SettingsScreen.SetActive(true);
        else
            SettingsScreen.SetActive(false);
    }
     public void ToggleSpinScreen(bool status)
    {

        DisableAll();
        MainScreen.SetActive(true);
        if (status)
            SpinScreen.SetActive(true);
        else
            SpinScreen.SetActive(false);
    }
    public void ShowShopScreen()
    {
        DisableAll();
        ShopScreen.SetActive(true);
    }
    public void ShowGameOverScreen()
    {
        DisableAll();
        GameOverScreen.SetActive(true);
    }
    public void MessageButtonClick()
    {
        if (ChatDialog.activeSelf)
            ChatDialog.SetActive(false);
        else
            ChatDialog.SetActive(true);
    }
}
