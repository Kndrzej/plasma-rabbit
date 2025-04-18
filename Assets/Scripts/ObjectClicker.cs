using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(GraphicRaycaster))]
public class ObjectClicker : MonoBehaviour
{
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

        var pointer = new PointerEventData(_eventSystem)
        {
            position = Input.mousePosition
        };
        var uiResults = new List<RaycastResult>();
        _uiRaycaster.Raycast(pointer, uiResults);

        foreach (var r in uiResults)
        {
            // Handle WinScreen separately
            var winScreen = r.gameObject.GetComponent<WinScreen>();
            if (winScreen != null)
            {
                winScreen.OnClicked();
                return;
            }

            var rot = r.gameObject.GetComponent<IRotatable>();
            if (rot != null)
            {
                rot.Rotate();
                return;
            }
        }

        // Physics raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var winScreen = hit.collider.GetComponent<WinScreen>();
            if (winScreen != null)
            {
                winScreen.OnClicked();
                return;
            }

            var rot = hit.collider.GetComponent<IRotatable>();
            if (rot != null)
            {
                rot.Rotate();
            }
        }
    }
}
