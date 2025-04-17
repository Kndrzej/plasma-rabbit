using System.Data.Common;
using UnityEngine;

public class CardSpawner : MonoBehaviour
{
    [SerializeField] private RectTransform _areaForCards;
    [SerializeField] private GameObject _cardPrefab;

    void Start()
    {
        SpawnCards(5,5);
    }

    private void SpawnCards(int columns,int rows)
    {
        //Guardian pattern, return early
        if (_areaForCards == null) return;
        if (_cardPrefab == null) return;
        if (columns == 0 && rows == 0) return;

        
        Vector2 size = _areaForCards.rect.size;
        float spacingX = size.x / columns;
        float spacingY = size.y / rows;

        //Loop trough columns and rows and simply instantiate cards
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                GameObject card = Instantiate(_cardPrefab, _areaForCards);

                RectTransform cardRect = card.GetComponent<RectTransform>();
                if (cardRect != null)
                {
                    cardRect.anchorMin = new Vector2(0, 0);
                    cardRect.anchorMax = new Vector2(0, 0);
                    cardRect.pivot = new Vector2(0.5f, 0.5f);
                    float x = spacingX * col + spacingX / 2f;
                    float y = spacingY * row + spacingY / 2f;
                    cardRect.anchoredPosition = new Vector2(x, y);
                }
            }
        }
    }
} 

