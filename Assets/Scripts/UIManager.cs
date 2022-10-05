using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Image chipImage;
    public CanvasGroup blood;
    public CanvasGroup loose;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ToggleChip(bool equip)
    {
        chipImage.enabled = equip;
    }

    public void SetBloodLevel(float level)
    {
        blood.alpha = level;
    }

    public void Loose()
    {
        loose.DOFade(1, 1);
    }
}