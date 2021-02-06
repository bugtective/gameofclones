using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _player1ScoreTxt = default;
    [SerializeField] private TextMeshProUGUI _player2ScoreTxt = default;
    [SerializeField] private TextMeshProUGUI _winText = default;

    [SerializeField] private GameObject _kickOffMessage = default;

    [SerializeField] private GameObject _mainMenu = default;

    [SerializeField] private List<TextMeshProUGUI> _gameModeTexts = default;

    [SerializeField] private Color _textSelectedColor = Color.yellow;
    [SerializeField] private Color _textNormalColor = Color.white;

    private bool _showingMainMenu = false;
    private int _currentGameMode = 0;

    private Action<int> _onGameModeSelected = null;

    public void Initialize(Action<int> onGameModeSelected)
    {
        _onGameModeSelected = onGameModeSelected;
    }

    public void ToggleKickOffMessage(bool value)
    {
        _kickOffMessage.SetActive(value);
        _winText.gameObject.SetActive(false);
    }

    public void ShowWinMessage(string winner)
    {
        _winText.text = $"{winner} is the winner!";
        _winText.gameObject.SetActive(true);
    }
    
    public void OnScoreChanged(int score1, int score2)
    {
        _player1ScoreTxt.text = score1.ToString();
        _player2ScoreTxt.text = score2.ToString();
    }

    public void ToggleMainMenu(bool value)
    {
        _mainMenu.SetActive(value);
        _showingMainMenu = value;

        if (value)
        {
            _gameModeTexts[_currentGameMode].color = _textSelectedColor;
            _winText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!_showingMainMenu)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _gameModeTexts[_currentGameMode].color = _textNormalColor;
            _currentGameMode--;

            if (_currentGameMode < 0)
            {
                _currentGameMode = _gameModeTexts.Count - 1; 
            }

            _gameModeTexts[_currentGameMode].color = _textSelectedColor;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _gameModeTexts[_currentGameMode].color = _textNormalColor;
            
            _currentGameMode++;

            if (_currentGameMode == _gameModeTexts.Count)
            {
                _currentGameMode = 0; 
            }

            _gameModeTexts[_currentGameMode].color =_textSelectedColor;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            _onGameModeSelected?.Invoke(_currentGameMode);
        }
    }

}
