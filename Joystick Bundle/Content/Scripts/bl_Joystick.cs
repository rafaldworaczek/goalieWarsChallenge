using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class bl_Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Settings")]
    public bool SmoothReturn = true;
    public bool UseFallbackOnEditor = false;
    [Range(0.1f, 2f)] public float stickArea = 0.5f;//the ratio of the circumference of the joystick

    [Header("Transition")]
    public ColorTransition[] colorTransition;
    [Range(0.1f, 5)] public float Duration = 1;

    [Header("Fallback")]
    public string FallbackVerticalAxis = "Vertical";
    public string FallbackHorizontalAxis = "Horizontal";

    [Header("Reference")]
    public RectTransform StickRect;//The middle joystick UI
    public RectTransform joystickBase;
    //Privates
    private int lastId = -2;
    public Vector3 inputVector { get; set; }

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        if (StickRect == null)
        {
            Debug.LogError("Please add the stick for joystick work!.");
            this.enabled = false;
            return;
        }

        foreach (var item in colorTransition)
        {
            foreach (var sub in item.graphics)
            {
                sub?.canvasRenderer.SetColor(item.normalColor);
            }
        }
    }

    /// <summary>
    /// When click here event
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerDown(PointerEventData data)
    {
        //Detect if is the default touchID
        if (lastId == -2)
        {
            //then get the current id of the current touch.
            //this for avoid that other touch can take effect in the drag position event.
            //we only need get the position of this touch
            lastId = data.pointerId;
            OnDrag(data);

            for (int i = 0; i < colorTransition.Length; i++)
            {
                colorTransition[i]?.CrossFade(colorTransition[i].touchColor, Duration);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void OnDrag(PointerEventData data)
    {
        //If this touch id is the first touch in the event
        if (data.pointerId == lastId)
        {
            Vector2 pos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBase, data.position, null, out pos))
            {
                pos.x = (pos.x / joystickBase.sizeDelta.x);
                pos.y = (pos.y / joystickBase.sizeDelta.y);

                inputVector = new Vector3(pos.x, 0, pos.y);
                inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
                StickRect.anchoredPosition = new Vector3(inputVector.x * (joystickBase.sizeDelta.x * stickArea), inputVector.z * (joystickBase.sizeDelta.y * stickArea));
            }
        }
    }

    /// <summary>
    /// When touch is Up
    /// </summary>
    /// <param name="data"></param>
    public void OnPointerUp(PointerEventData data)
    {
        //leave the default id again
        if (data.pointerId == lastId)
        {
            //-2 due -1 is the first touch id
            lastId = -2;
            StickRect.anchoredPosition = Vector3.zero;
            inputVector = Vector3.zero;

            for (int i = 0; i < colorTransition.Length; i++)
            {
                colorTransition[i]?.CrossFade(colorTransition[i].normalColor, Duration);
            }
        }
    }

    /// <summary>
    /// Value Horizontal of the Joystick
    /// Get this for get the horizontal value of joystick
    /// </summary>
    public float Horizontal
    {
        get
        {
#if UNITY_EDITOR
            if (UseFallbackOnEditor)
            {
                return Input.GetAxis(FallbackHorizontalAxis);
            }
#endif
            return inputVector.x;
        }
    }

    /// <summary>
    /// Value Vertical of the Joystick
    /// Get this for get the vertical value of joystick
    /// </summary>
    public float Vertical
    {
        get
        {
#if UNITY_EDITOR
            if (UseFallbackOnEditor)
            {
                return Input.GetAxis(FallbackVerticalAxis);
            }
#endif
            return inputVector.z;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (joystickBase == null) return;
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(joystickBase.position, Vector3.forward, joystickBase.sizeDelta.x * (stickArea + 0.125f));
        UnityEditor.Handles.color = Color.white;
    }

    void OnValidate()
    {
        for (int i = 0; i < colorTransition.Length; i++)
        {
            for (int e = 0; e < colorTransition[i].graphics.Length; e++)
            {
                colorTransition[i]?.graphics[e]?.canvasRenderer?.SetColor(colorTransition[i].normalColor);
            }
        }
    }
#endif

    /// <summary>
    /// Get the touch by the store touchID 
    /// </summary>
    public int GetTouchID
    {
        get
        {
            //find in all touchesList
            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].fingerId == lastId)
                {
                    return i;
                }
            }
            return -1;
        }
    }

    [System.Serializable]
    public class ColorTransition
    {
        public Graphic[] graphics;
        public Color normalColor = Color.white;
        public Color touchColor = Color.white;

        public void CrossFade(Color color, float duration)
        {
            for (int i = 0; i < graphics.Length; i++)
            {
                graphics[i]?.CrossFadeColor(color, duration, true, true);
            }
        }
    }
}