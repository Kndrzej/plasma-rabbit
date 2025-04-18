using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } 

    public List<Rotatable> Rotatables;
    public List<Rotatable> FlippedCardsQueue = new List<Rotatable>();

    private Dictionary<Rotatable, float> _cardFlipTimes = new Dictionary<Rotatable, float>(); 
    [SerializeField] private List<Texture2D> _cardsTextures;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private AudioClip _winClip;
    [SerializeField] private AudioClip _matchClip;
    [SerializeField] private AudioClip _noMatchClip;
    [SerializeField] private AudioClip _flipClip;
    private int _scoreValue = 0;

    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        Rotatable.Rotating += OnCardRotated;
        StartCoroutine(StartGame());
        _score.text = "Score: " + _scoreValue; 
    }

    void Update()
    {
       
        List<Rotatable> cardsToFlip = new List<Rotatable>();
        List<Rotatable> flippedCardsQueueCopy = new List<Rotatable>(FlippedCardsQueue);
        foreach (var card in flippedCardsQueueCopy)
        {
            if (_cardFlipTimes.ContainsKey(card) && Time.time - _cardFlipTimes[card] > 1.25f)
            {
                cardsToFlip.Add(card);
            }
        }

        foreach (var card in cardsToFlip)
        {
            card.Rotate();
            FlippedCardsQueue.Remove(card);
            _cardFlipTimes.Remove(card); 
        }
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1);
        Rotatables = FindObjectsOfType<Rotatable>().ToList();

        if (Rotatables.Count == 0) yield break;
        if (Rotatables.Count % 2 != 0) yield break;

        int pairCount = Rotatables.Count / 2;
        List<int> cardIDs = new();
        HashSet<int> usedNumbers = new();

        while (usedNumbers.Count < pairCount)
        {
            int randomID = Random.Range(0, 52);
            if (usedNumbers.Add(randomID))
            {
                cardIDs.Add(randomID);
                cardIDs.Add(randomID);
            }
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

    private void OnDestroy()
    {
        Rotatable.Rotating -= OnCardRotated;
    }

    private void OnCardRotated(Rotatable card)
    {
        if (FlippedCardsQueue.Contains(card)) return;
        _audioSource.PlayOneShot(_flipClip);
        FlippedCardsQueue.Add(card);
        _cardFlipTimes[card] = Time.time;

        if (FlippedCardsQueue.Count % 2 == 0)
        {
            int last = FlippedCardsQueue.Count;
            Rotatable first = FlippedCardsQueue[last - 2];
            Rotatable second = FlippedCardsQueue[last - 1];

            StartCoroutine(HandlePair(first, second));
        }
    }

    private IEnumerator HandlePair(Rotatable card1, Rotatable card2)
    {
        Card cardComponent1 = card1.GetComponent<Card>();
        Card cardComponent2 = card2.GetComponent<Card>();

        bool isMatch = false;

        if (cardComponent1 != null && cardComponent2 != null)
        {
            isMatch = cardComponent1.ID == cardComponent2.ID;
        }

        if (isMatch)
        {
            _audioSource.PlayOneShot(_matchClip);
            _scoreValue += 1;
            _score.text = "Score: " + _scoreValue;
            HideCard(card1);
            HideCard(card2);

        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            _audioSource.PlayOneShot(_noMatchClip);
            Debug.Log("no match");

            card1.RotateToZero();
            card2.RotateToZero();
            yield return new WaitForSeconds(1);
            
        }
        yield return new WaitForSeconds(1);
        FlippedCardsQueue.Remove(card1);
        FlippedCardsQueue.Remove(card2);

    }

    private void HideCard(Rotatable card)
    {
        var graphic = card.GetComponent<Graphic>();
        if (graphic != null) graphic.enabled = false;

        var images = card.GetComponentsInChildren<Graphic>();
        foreach (var image in images)
        {
            image.enabled = false;
        }

        if (card.TryGetComponent(out Collider col)) col.enabled = false;
        if (card.TryGetComponent(out Rotatable rotatable)) rotatable.enabled = false;

        // Check if all cards are matched
        bool allHidden = Rotatables.All(r =>
        {
            var g = r.GetComponent<Graphic>();
            return g == null || !g.enabled;
        });

        if (allHidden && _winScreen != null)
        {
            _audioSource.PlayOneShot(_winClip);
            _winScreen.SetActive(true);
            _winScreen.GetComponent<WinScreen>().SetScore(_scoreValue);
        }
    }

}
