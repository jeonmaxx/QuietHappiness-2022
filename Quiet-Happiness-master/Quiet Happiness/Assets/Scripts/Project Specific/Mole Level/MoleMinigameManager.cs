using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoleMinigameManager : Module
{
    [SerializeField] private Slider _gameScore;
    [SerializeField] private string _mainMenuScene;
    [SerializeField] private List<Transform> _hammerUIs;

    [SerializeField] private Text _gameScoreFinal;
    [SerializeField] private Text _timerDisplay;

    [SerializeField] private AudioMixer _woodenHammerSounds;
    [SerializeField] private AudioMixer _ironHammerSounds;
    [SerializeField] private AudioMixer _incorrectHammerSounds;

    private Transform _currentHammer;

    [HideInInspector] public int MaxMoles;
    private float _currentScore;
    [SerializeField] [DropDown(nameof(_menuTypes))] private int _gameOverMenuType;
    private List<string> _menuTypes;

    public void UpdateScore(MoleType typeOfMole, bool correctHammer)
    {
        if (!correctHammer)
        {
            _currentScore -= 1f / MaxMoles;
            _incorrectHammerSounds.PlayRandomSource();
        }
        else
        {
            switch (typeOfMole)
            {
                case MoleType.None:
                    break;
                case MoleType.GoodBoi:
                    _currentScore -= 1f / MaxMoles;
                    break;

                case MoleType.Turnip:
                    _woodenHammerSounds.PlayRandomSource();
                    _currentScore += 1f / MaxMoles;
                    break;
                case MoleType.Shroom:
                    _ironHammerSounds.PlayRandomSource();
                    _currentScore += 1f / MaxMoles;
                    break;
                default:
                    break;
            }
        }
        _currentScore = Mathf.Clamp(_currentScore, -1, 1);
        _gameScore.SetValueWithoutNotify(_currentScore + 1 == 0 ? 0 : (_currentScore+1)/2);
    }


    public void UpdateHammerUI(int index)
    {
        if(_currentHammer != null)
        {
            _currentHammer.localPosition = new Vector3(0, 0, 0);
        } 
        _currentHammer = _hammerUIs[index];
        _currentHammer.localPosition = new Vector3(0, 100, 0);
    }

    public void GameDone()
    {
        _gameScoreFinal.text = ((int)(_gameScore.value * 100)).ToString() + "%";
        ModuleManager.GetModule<MenuManager>().ToggleMenu(_gameOverMenuType, false);
        if(_gameScore.value > 0.7f)
        {
            ModuleManager.GetModule<SaveGameManager>().SetCompletionInfo(CompletionIDs.MOLELEVELDONE, true);
        }
    }

    public void UpdateTimer(int display)
    {
        _timerDisplay.text = display.ToString();
    }

    private void OnValidate()
    {
        _menuTypes = MenuManager.MenuTypesForEditor;
    }
}

public enum MoleType
{
    None,
    Shroom,
    Turnip,
    GoodBoi
}
