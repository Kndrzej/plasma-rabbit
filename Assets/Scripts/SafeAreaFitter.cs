using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private RectTransform _safeArea;
    [SerializeField] private RectTransform _topNavbar;
    [SerializeField] private RectTransform _bottomNavbar;
    [SerializeField] private RectTransform _areaForCards;

    private const float TopBarHeight = 120f;
    private const float BottomBarHeight = 130f;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
        SetupLayout();
    }

    private void Update()
    {
        ApplySafeArea();
        SetupLayout();
    }

    private void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        _rectTransform.anchorMin = anchorMin;
        _rectTransform.anchorMax = anchorMax;
        _rectTransform.offsetMin = Vector2.zero;
        _rectTransform.offsetMax = Vector2.zero;
    }

    private void SetupLayout()
    {
        _topNavbar.anchorMin = new Vector2(0, 1);
        _topNavbar.anchorMax = new Vector2(1, 1);
        _topNavbar.pivot = new Vector2(0.5f, 1f);
        _topNavbar.offsetMin = new Vector2(0, -TopBarHeight);
        _topNavbar.offsetMax = new Vector2(0, 0);

        _bottomNavbar.anchorMin = new Vector2(0, 0);
        _bottomNavbar.anchorMax = new Vector2(1, 0);
        _bottomNavbar.pivot = new Vector2(0.5f, 0f);
        _bottomNavbar.offsetMin = new Vector2(0, 0);
        _bottomNavbar.offsetMax = new Vector2(0, BottomBarHeight);

        _areaForCards.anchorMin = new Vector2(0, 0);
        _areaForCards.anchorMax = new Vector2(1, 1);
        _areaForCards.pivot = new Vector2(0.5f, 0.5f);
        _areaForCards.offsetMin = new Vector2(0, BottomBarHeight);
        _areaForCards.offsetMax = new Vector2(0, -TopBarHeight);
    }
}
