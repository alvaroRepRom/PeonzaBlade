using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button bunnyButton;


    private void Start()
    {
        
    }




    private void Hide() => gameObject.SetActive( false );
}
