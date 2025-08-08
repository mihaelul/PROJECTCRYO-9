using TMPro;
using UnityEngine;

public class ToolTipManager : MonoBehaviour
{
    #region Singleton

    private static ToolTipManager instance = null;
    public static ToolTipManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { 
            DestroyImmediate(this);
        }
    }
    #endregion

    private Vector3 centerScreenPos = new Vector3(Screen.width / 2, Screen.height / 2);


    [SerializeField] private GameObject toolTipObject = null;
    [SerializeField] private Transform transformToolTip = null;
    [SerializeField] private TextMeshProUGUI titlul = null;
    [SerializeField] private TextMeshProUGUI descriere = null;
    
    // in plus pentru a urmari pozitita mouse ului
    [SerializeField ] private Canvas parentCanvas = null;
    public void Update()
    {
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out movePos);
        transformToolTip.position = parentCanvas.transform.TransformPoint(movePos);
        
    }
    //
    public void SetToolTipAtPosWithMess (Vector3 pos, string mess, string tit, bool isTwoDimension = true)
    {
        toolTipObject.SetActive(true);
        titlul.text = tit;
        descriere.text = mess;

        transformToolTip.position = centerScreenPos; //isTwoDimension ? GetPos_2D() : GetPos_3D();
    }

    public void DeactivateToolTip() => toolTipObject.SetActive(false);

    private Vector3 GetPos_2D() => Vector3.zero;
    private Vector3 GetPos_3D() => Vector3.zero;
}
