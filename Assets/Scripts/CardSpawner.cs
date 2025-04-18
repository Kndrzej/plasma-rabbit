using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private RectTransform _areaForCards;
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private int _columns = 5;
    [SerializeField] private int _rows = 5;

    private GridLayoutGroup _grid;
    private Vector2 _lastSize;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.25f);
        SetupGrid();
        SpawnCards(_columns * _rows);
    }

    private void Update()
    {
        Vector2 currentSize = new Vector2(Screen.width, Screen.height);
        if (_lastSize != currentSize)
        {
            _lastSize = currentSize;
            StartCoroutine(DelayedLayoutRebuild());
        }
    }

    private IEnumerator DelayedLayoutRebuild()
    {
        yield return new WaitForSeconds(0.25f);
        LayoutRebuilder.ForceRebuildLayoutImmediate(_areaForCards);
        SetupGrid();
    }

    private void SetupGrid()
    {
        if (_grid == null)
        {
            _grid = _areaForCards.GetComponent<GridLayoutGroup>();
            if (_grid == null)
                _grid = _areaForCards.gameObject.AddComponent<GridLayoutGroup>();
        }

        _grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _grid.constraintCount = _columns;
        _grid.childAlignment = TextAnchor.MiddleCenter;
        _grid.startAxis = GridLayoutGroup.Axis.Horizontal;
        _grid.startCorner = GridLayoutGroup.Corner.UpperLeft;

        float width = _areaForCards.rect.width;
        float height = _areaForCards.rect.height;

        float paddingLeft = _grid.padding.left;
        float paddingRight = _grid.padding.right;
        float paddingTop = _grid.padding.top;
        float paddingBottom = _grid.padding.bottom;

        float totalHorizontalSpacing = _grid.spacing.x * (_columns - 1);
        float totalVerticalSpacing = _grid.spacing.y * (_rows - 1);

        float availableWidth = width - totalHorizontalSpacing - paddingLeft - paddingRight;
        float availableHeight = height - totalVerticalSpacing - paddingTop - paddingBottom;

        if (availableWidth / _columns < 100f)
            _columns = Mathf.Max(1, Mathf.FloorToInt(availableWidth / 100f));

        availableWidth = width - _grid.spacing.x * (_columns - 1) - paddingLeft - paddingRight;
        float cellWidth = availableWidth / _columns;
        float cellHeight = availableHeight / _rows;

        float squareSize = Mathf.Min(cellWidth, cellHeight);

        _grid.cellSize = new Vector2(squareSize, squareSize);
    }

    private void SpawnCards(int total)
    {
        if (total == 0 || _cardPrefab == null || _areaForCards == null)
            return;

        foreach (Transform child in _areaForCards)
            Destroy(child.gameObject);

        for (int i = 0; i < total; i++)
            Instantiate(_cardPrefab, _areaForCards);
    }
}
