using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SingleHUD : MonoBehaviour
{
    [SerializeField] private Image rpmImage;
    [SerializeField] private Image colorPlayerRPMImage;
    [SerializeField] private Image characterPortraitImage;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI rpmText;

    private int maxRPM;
    private const float MAX_FILL_AMOUNT = 0.75f;

    public void SetPlayerHUD( Sprite chrarcterSprite , Color playerColor , int currentRPM , int maxRPM )
    {
        this.maxRPM = maxRPM;
        rpmImage.fillAmount = MAX_FILL_AMOUNT * currentRPM / maxRPM;
        colorPlayerRPMImage.color = playerColor;
        characterPortraitImage.color = playerColor;
        characterImage.sprite = chrarcterSprite;
        rpmText.text = currentRPM.ToString();
    }

    public void UpdateHUD( int currentRPM )
    {
        rpmImage.fillAmount = MAX_FILL_AMOUNT * currentRPM / maxRPM;
        rpmText.text = currentRPM.ToString();
    }
}
