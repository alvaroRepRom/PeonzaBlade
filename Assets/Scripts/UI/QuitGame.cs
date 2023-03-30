using UnityEngine;
using UnityEngine.UI;

public class QuitGame : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener( () => QuitGamet() );
    }

    private void QuitGamet()
    {
        Application.Quit();
    }
}
