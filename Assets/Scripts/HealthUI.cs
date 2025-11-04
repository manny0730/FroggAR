using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public bool isActive;
    [SerializeField] private Image Foreground;

    public void DisableHealth()
    {
        isActive = false;
        Foreground.gameObject.SetActive(false);
    }
}
