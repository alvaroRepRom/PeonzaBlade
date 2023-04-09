using UnityEngine;
using TMPro;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI firstText;
    [SerializeField] private TextMeshProUGUI secondText;
    [SerializeField] private TextMeshProUGUI thirdText;
    [SerializeField] private TextMeshProUGUI fourthText;

    private TextMeshProUGUI[] positionsInverted;

    private void Start()
    {
        gameObject.SetActive( false );
        GameManager.Instance.OnGameCompleted += Show;

        positionsInverted = new[] {
            fourthText,
            thirdText, 
            secondText, 
            firstText
        };
    }


    private void Show( GameManager.OnGameCompletedArgs results )
    {
        for ( int i = 0; i < positionsInverted.Length; i++ )
        {
            positionsInverted[i].text = results.resultList[i];
        }

        gameObject.SetActive( true );
    }
}
