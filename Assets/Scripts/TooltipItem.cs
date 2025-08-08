using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string titlu = null;
    [SerializeField] private string desc = null;

    #region Pointer Events Implementation
     
    // pentru 2d cand e in canvas mai trebuie sa lucrez la el
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(ToolTipManager.Instance)
            ToolTipManager.Instance.SetToolTipAtPosWithMess(transform.position, desc, titlu);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ToolTipManager.Instance)
            ToolTipManager.Instance.DeactivateToolTip();
    }
    #endregion


    private void OnMouseEnter()
    {
        if (ToolTipManager.Instance)
            ToolTipManager.Instance.SetToolTipAtPosWithMess(transform.position, desc, titlu, false);
    }

    private void OnMouseExit()
    {
        if (ToolTipManager.Instance)
            ToolTipManager.Instance.DeactivateToolTip();
    }
}
