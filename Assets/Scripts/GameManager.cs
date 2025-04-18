using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Rotatable> Rotatables;
    
    private List<Rotatable> _flippedCardsQueue = new List<Rotatable>();
    [SerializeField] private List<Texture2D> _cardsTextures;

    void Start()
    {
        Rotatable.Rotating += OnCardRotated;
        StartCoroutine(StartGame());
    }
    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1);
        Rotatables = FindObjectsOfType<Rotatable>().ToList();

        if (Rotatables.Count == 0) yield break;
        if (Rotatables.Count % 2 != 0) yield break;

        int pairCount = Rotatables.Count / 2;
        List<int> cardIDs = new();

        // Generate pairs of numbers from 0 to 51
        for (int i = 0; i < 52; i++)
        {
            cardIDs.Add(i / 2); // Each number appears twice
        }

        Shuffle(cardIDs);

        for (int i = 0; i < Rotatables.Count; i++)
        {
            Card card = Rotatables[i].GetComponent<Card>();
            if (card != null)
            {
                int cardID = cardIDs[i];
                card.ID = cardID;

                if (cardID >= 0 && cardID < _cardsTextures.Count)
                {
                    card.SetFrontTexture(_cardsTextures[cardID]);
                }
            }
        }
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    void OnDestroy()
    {
        Rotatable.Rotating -= OnCardRotated;
    }

    private void OnCardRotated(Rotatable card)
    {
        if (_flippedCardsQueue.Contains(card))
            return; 

        _flippedCardsQueue.Add(card);

        if (_flippedCardsQueue.Count % 2 == 0)
        {
            
            int last = _flippedCardsQueue.Count;
            Rotatable first = _flippedCardsQueue[last - 2];
            Rotatable second = _flippedCardsQueue[last - 1];

            StartCoroutine(HandlePair(first, second));
        }
    }

    private IEnumerator HandlePair(Rotatable card1, Rotatable card2)
    {
        while (card1.IsAnimating || card2.IsAnimating)
        {
            yield return null;
        }

        Card cardComponent1 = card1.GetComponent<Card>();
        Card cardComponent2 = card2.GetComponent<Card>();

        bool isMatch = false;

        if (cardComponent1 != null && cardComponent2 != null)
        {
            isMatch = cardComponent1.ID == cardComponent2.ID;
        }

        if (isMatch)
        {
            HideCard(card1);
            HideCard(card2);
        }
        else
        {
            card1.ResetRotation();
            card2.ResetRotation();
        }

        _flippedCardsQueue.Remove(card1);
        _flippedCardsQueue.Remove(card2);
    }


    private void HideCard(Rotatable card)
    {
        
        card.GetComponent<Graphic>();
        card.enabled = false;
        var images = card.GetComponentsInChildren<Graphic>();
        foreach (var image in images)
        {
            image.enabled = false;
        }

        var rotatable = card.GetComponent<Rotatable>();
        if (rotatable != null) rotatable.enabled = false;
    }

}
