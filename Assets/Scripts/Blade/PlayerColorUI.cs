using UnityEngine;
using UnityEngine.UI;

public class PlayerColorUI : MonoBehaviour
{
    [SerializeField] private Image colorImage;

    public void SetPlayerColor( Color color )
    {
        colorImage.color = color;
    }
}
