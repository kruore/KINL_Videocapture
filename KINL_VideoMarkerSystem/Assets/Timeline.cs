using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Timeline : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private Camera cam;
    public GM_VideoPlayer _vm;
    bool _isDrag= false;
    [SerializeField]
    private RectTransform tooltipContainerRect;


    public void OnBeginDrag(PointerEventData eventData)
    {
        _isDrag = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if(_isDrag)
        {
            GetPreviewPoint();
            _vm._videoPlayer.Pause();
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        _isDrag = false;
        _vm._videoPlayer.time = _vm._videoPlayer.clip.length* GetPreviewPoint();
        Debug.Log((_vm._videoPlayer.clip.length * GetPreviewPoint()).ToString("N3"));
        _vm._videoPlayer.Play();
    }

    private float GetPreviewPoint()
    {
        Vector2 screenMousePosition = RectTransformUtility.WorldToScreenPoint(cam, Input.mousePosition);
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(tooltipContainerRect, screenMousePosition, cam, out localMousePosition);
        localMousePosition -= tooltipContainerRect.rect.position;
        //Debug.Log(Mathf.Clamp01((localMousePosition[0]) / tooltipContainerRect.rect.size[0]));
        return Mathf.Clamp01((localMousePosition[0]) / tooltipContainerRect.rect.size[0]);
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        _vm = GM_VideoPlayer.instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
