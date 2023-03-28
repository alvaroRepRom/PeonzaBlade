using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Start,
        Game,
        End
    }

    public GameState State { get; private set; }
    public bool IsGameActive { get; private set; }


    private Timer timer;
    private float startTime;
    private float gameTime = 20;
    private float endTime;

    private void Awake()
    {
        Instance = this;
        timer = new Timer( startTime );
        Application.targetFrameRate = 60;
    }


    private void Start()
    {
        State = GameState.Game;
    }

    private void Update()
    {
        switch ( State )
        {
            case GameState.Start:
                if ( timer.HasTimeUp() )
                {
                    timer.SetNewTime( gameTime );
                    State = GameState.Game;
                }
                break;

            case GameState.Game:
                if ( timer.HasTimeUp() )
                {
                    timer.SetNewTime( endTime );
                    State = GameState.End;
                }
                break;

            case GameState.End:
                if ( timer.HasTimeUp() )
                {
                    // Main Menu
                }
                break;
        }
    }



    public float GameTime() => gameTime;
    public float TimeElapsed() => timer.SecondsElapsed();

}
