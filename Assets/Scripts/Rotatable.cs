using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Rotatable : MonoBehaviour, IRotatable
{
    public bool IsFlipped = false;
    public bool IsAnimating = false;

    public static event Action<Rotatable> Rotating;

    [SerializeField] private float _targetYRotation = -180f;
    [SerializeField] private GameObject _cardImage;
    [SerializeField] private float _rotationDuration = 0.5f;

    private float _animationTimer = 0f;
    private Quaternion _initialRotation;
    private Quaternion _finalRotation;
    
    public void ResetRotation()
    {
        IsFlipped = false;
        Debug.Log($"{name} reset");
    }

    public void Rotate()
    {
        if (IsAnimating) return;
        /*I rotate before animation to match: "The system
        should allow continuous card flipping without requiring users to wait for card
        comparisons to finish before selecting additional cards.
        " requirement.
         */
        IsFlipped = !IsFlipped;
        Rotating?.Invoke(this);

        RectTransform rectTransform = _cardImage.GetComponent<RectTransform>();
        Vector3 currentEulerAngles = rectTransform.localEulerAngles;
        float newYRotation = currentEulerAngles.y == 0f ? _targetYRotation : 0f;

        _initialRotation = rectTransform.localRotation;
        _finalRotation = Quaternion.Euler(currentEulerAngles.x, newYRotation, currentEulerAngles.z);

        _animationTimer = 0f;
        IsAnimating = true;
    }

    private void Update()
    {
        if (!IsAnimating) return;

        _animationTimer += Time.deltaTime;
        float progress = Mathf.Clamp01(_animationTimer / _rotationDuration);

        RectTransform rectTransform = _cardImage.GetComponent<RectTransform>();
        rectTransform.localRotation = Quaternion.Lerp(_initialRotation, _finalRotation, progress);

        if (progress >= 1f)
        {
            IsAnimating = false;
        }
    }
}
