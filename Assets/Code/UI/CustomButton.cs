using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButton : Button
{
    public bool ClickedThisFrame => ClickFrame == Time.frameCount;

    private int ClickFrame = -1;
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        ClickFrame = Time.frameCount;
    }
}
