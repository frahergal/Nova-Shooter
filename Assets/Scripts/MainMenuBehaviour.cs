﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;


public class MainMenuBehaviour : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject playerSelectionMenu;
    public ToggleGroup spaceshipSelector;
    public GameObject creditsMenu;

    public static string spaceShipName;

    void Start()
    {
        InitializeMenu();
    }

    public void LoadLevel(string levelName)
    {
        if (PauseMenuBehaviour.isPaused)
        {
            // The parameters of the game should be resumed, so if it starts again, it won't be paused
            PauseMenuBehaviour.instance.ResumeGame();
        }

        SceneManager.LoadScene(levelName);
    }

    public void InitializeMenu()
    {
        mainMenu.SetActive(true);
        playerSelectionMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    public void OpenSpaceshipSelector()
    {
        mainMenu.SetActive(false);
        playerSelectionMenu.SetActive(true);
    }

    public void CloseSpaceshipSelector()
    {
        IEnumerator<Toggle> toggleEnum = spaceshipSelector.ActiveToggles().GetEnumerator();
        toggleEnum.MoveNext();
        if (toggleEnum.Current != null)
        {
            spaceShipName = toggleEnum.Current.name;
        }

        InitializeMenu();
    }

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    public static string GetSpaceshipSpriteName()
    {
        if (String.IsNullOrEmpty(spaceShipName))
        {
            return "1_1";
        }

        return spaceShipName;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

}
