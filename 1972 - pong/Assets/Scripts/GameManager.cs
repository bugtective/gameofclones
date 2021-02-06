using UnityEngine;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        Init,
        MainMenu,
        Ready,
        Playing,
        Paused,
        GameOver
    }

    private enum GameMode
    {
        PlayerVsPlayer = 0,
        PlayerVsAI = 1
    }

    [SerializeField] private GameObject _playerPrefab = default;
    [SerializeField] private GameObject _ballPrefab = default;

    [SerializeField] private Transform _player1InitialPos = default;
    [SerializeField] private Transform _player2InitialPos = default;

    [SerializeField] private UIController _uiController = default;
    [SerializeField] private AudioManager _audioManager = default;

    [SerializeField] private int _matchScore = 11;

    private Ball _ball = default;
    private PaddleController _player1 = default;
    private PaddleController _player2 = default;
    private AI _AIPlayer = default;

    private int _player1Points = 0;
    private int _player2Points = 0;

    private GameState _gameState = GameState.Init;
    private GameMode _gameMode = default;

    void Start()
    {
        _ball = GameObject.Instantiate(_ballPrefab).GetComponent<Ball>();
        _ball.Initialize(OnGoal, OnBounce);

        _player1 = GameObject.Instantiate(_playerPrefab).AddComponent<PaddleController>();
        _player1.Initialize("Vertical", "Player1", _player1InitialPos.position);

        _player2 = GameObject.Instantiate(_playerPrefab).AddComponent<PaddleController>();
        _player2.Initialize("Vertical2", "Player2", _player2InitialPos.position);

        _AIPlayer = GameObject.Instantiate(_playerPrefab).AddComponent<AI>();
        _AIPlayer.Initialize("Player2", _player2InitialPos.position, _ball.RigidBody);

        _uiController.Initialize(OnGameModeSelected);

        SetState(GameState.MainMenu);
    }

    private void TogglePlayers(bool value)
    {
        _player1.gameObject.SetActive(value);
        _player2.gameObject.SetActive(value && _gameMode == GameMode.PlayerVsPlayer);
        _AIPlayer.gameObject.SetActive(value && _gameMode == GameMode.PlayerVsAI);
    }

    private void Update()
    {
        switch (_gameState)
        {
            case GameState.Ready:
            {
                // Wait for input to start
                if (Input.GetKeyDown(KeyCode.Space))
                {
                   SetState(GameState.Playing);
                } 
            }
            break;

            case GameState.GameOver:
            {
                // Wait for input to start
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _player1Points = 0;
                    _player2Points = 0;
                    _uiController.OnScoreChanged(_player1Points, _player2Points);
                    SetState(GameState.MainMenu);
                } 
            }
            break;

            default:
                break;
        }
    }

    private void SetState(GameState newState)
    {
        switch (newState)
        {
            case GameState.MainMenu:
            {                
                TogglePlayers(false);
                _uiController.ToggleMainMenu(true);
            }
            break;

            case GameState.Ready:
            {
                _ball.Reset();
                _player1.Reset();

                if (_gameMode == GameMode.PlayerVsPlayer)
                {
                    _player2.Reset();
                }
                else
                {
                    _AIPlayer.Reset();
                }
                
                _uiController.ToggleKickOffMessage(true);
            }
            break;

            case GameState.Playing:
            {
                _uiController.ToggleKickOffMessage(false);
                _ball.KickOff();
            }
            break;

            case GameState.GameOver:
            {
                _uiController.ShowWinMessage((_player1Points == _matchScore ? "Player1" : "Player2"));
                _ball.KickOff();
            }
            break;

            default:
                break;
        }

        _gameState = newState;
    }

    private void OnGoal(bool player1Score)
    {
        if (player1Score)
        {
            _player1Points++;
        }
        else
        {
            _player2Points++;
        }

        _uiController.OnScoreChanged(_player1Points, _player2Points);

        if (_player1Points == _matchScore || _player2Points == _matchScore)
        {
            SetState(GameState.GameOver);
        }
        else
        {
            SetState(GameState.Ready);
        }
    }

    private void OnBounce(string soundName)
    {
        _audioManager.PlaySound(soundName);
    }

    private void OnGameModeSelected(int gameMode)
    {
        _gameMode = (GameMode)gameMode;
        _uiController.ToggleMainMenu(false);
        TogglePlayers(true);
        SetState(GameState.Ready);
    }
}
