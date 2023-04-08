using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public event Action OnLaunchBlades;
    //public event Action OnGameCompleted;


    public static GameManager Instance { get; private set; }



    public enum GameState
    {
        Init,
        Start,
        Game,
        End
    }

    public GameState State { get; private set; }
    public bool IsGameActive { get; private set; }


    private Timer timer;
    private bool isPlayersSet = false;
    private bool isLaunched = false;
    private float startTime = 20;
    private float gameTime = 120;


    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
    }


    private void Start()
    {
        MultiplayerInputManager.Instance.OnAllPlayersReady += MultiplayerInputManager_OnAllPlayersReady;
        SpawnBladeManager.Instance.OnBladesLaunched += SpawnBladeManager_OnBladesLaunched;

        State = GameState.Init;
        timer = new Timer( gameTime );
    }

    private void SpawnBladeManager_OnBladesLaunched()
    {
        isLaunched = true;
    }

    private void MultiplayerInputManager_OnAllPlayersReady()
    {
        isPlayersSet = true;
    }

    private void Update()
    {
        switch ( State )
        {
        case GameState.Init:
            Preparation();
            break;
        case GameState.Start:
            LaunchBlade();
            break;
        case GameState.Game:
            Game();
            break;
        case GameState.End:
            Result();
            break;
        }
    }


    private void Preparation()
    {
        if ( isPlayersSet )
        {
            State = GameState.Start;
            timer = new Timer( startTime );
            Debug.Log( "To launch blade" );
        }
    }

    private void LaunchBlade()
    {
        if ( timer.HasTimeUp() || isLaunched )
        {
            timer.SetNewTime( gameTime );
            State = GameState.Game;
            Debug.Log( "To game" );
        }
    }

    private void Game()
    {
        if ( timer.HasTimeUp() )
        {
            State = GameState.End;
            Debug.Log( "To results" );
        }
    }

    private void Result()
    {
        // Results Menu
    }


    public void ResetGame() => SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    public float GameTime() => gameTime;
    public float TimeElapsed() => timer.SecondsElapsed();

}
