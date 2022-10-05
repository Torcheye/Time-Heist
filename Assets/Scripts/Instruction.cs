using TMPro;
using UnityEngine;

public class Instruction : MonoBehaviour
{
    public Canvas canvas;
    
    public void ToggleInstruction(bool show, string text)
    {
        canvas.enabled = show;
        if (show)
            canvas.GetComponentInChildren<TMP_Text>().text = text;
    }
}