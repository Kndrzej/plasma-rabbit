using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
public class ObjectClicker : MonoBehaviour
{
    // The UI raycaster on this Canvas
    private GraphicRaycaster _uiRaycaster;
    private EventSystem _eventSystem;

    void Awake()
    {
        _uiRaycaster = GetComponent<GraphicRaycaster>();
        _eventSystem = EventSystem.current;
    }

    void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        // 1) First try a UI raycast:
        var pointer = new PointerEventData(_eventSystem)
        {
            position = Input.mousePosition
        };
        var uiResults = new List<RaycastResult>();
        _uiRaycaster.Raycast(pointer, uiResults);

        foreach (var r in uiResults)
        {
            var rot = r.gameObject.GetComponent<IRotatable>();
            if (rot != null)
            {
                rot.Rotate();
                return;
            }
        }

        // 2) If nothing hit in UI, try a Physics raycast into the 3D world:
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var rot = hit.collider.GetComponent<IRotatable>();
            if (rot != null)
                rot.Rotate();
        }
    }
}
