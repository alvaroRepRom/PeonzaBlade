using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public event Action OnWinnerDecided;
    public event Action<OnGameCompletedArgs> OnGameCompleted;
    public class OnGameCompletedArgs
    {
        public List<string> resultList;
    }

    public static GameManager Instance { get; private set; }


    public enum GameState
    {
        Init,
        Game,
        End
    }

    public GameState State { get; private set; }

    private Timer timer;
    private float resetTime = 7;

    private int playersDead = 0;
    private List<string> playerNameResultList = new List<string>();


    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        State = GameState.Init;
    }

    private void Start()
    {
        MultiplayerInputManager.Instance.OnAllPlayersReady += StartGame;
    }

    private void Update()
    {
        switch ( State )
        {
        case GameState.Init:
            break;
        case GameState.Game:
            Game();
            break;
        case GameState.End:
            Result();
            break;
        }
    }

    private void Game()
    {
        if ( playersDead >= MultiplayerInputManager.Instance.GetMaxNumOfPlayers() - 1 )
        {
            timer = new Timer( resetTime );
            State = GameState.End;
            OnWinnerDecided?.Invoke();
        }
    }

    private void Result()
    {
        if ( timer.HasTimeUp() )
            ResetGame();
    }

    public void BladeHasDied( string playerName )
    {
        playersDead++;
        playerNameResultList.Add( playerName );
    }

    public void SetWinner( string playerName )
    {
        playerNameResultList.Add( playerName );
        OnGameCompleted?.Invoke( new OnGameCompletedArgs { resultList = playerNameResultList } );
    }
    public void StartGame() => State = GameState.Game;
    public void ResetGame() => SceneManager.LoadScene( SceneManager.GetActiveScene().name );
}
