using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Rotatable : MonoBehaviour, IRotatable
{
    public bool IsFlipped = false;
    
    [SerializeField] private float _targetYRotation = -180f;
    [SerializeField] private GameObject _cardImage;
    [SerializeField] private float _rotationDuration = 0.5f;

    private bool _isAnimating = false;
    private float _animationTimer = 0f;
    private Quaternion _initialRotation;
    private Quaternion _finalRotation;


    public void Rotate()
    {
        if (_isAnimating) return;
        IsFlipped = !IsFlipped;

        RectTransform rectTransform = _cardImage.GetComponent<RectTransform>();
        Vector3 currentEulerAngles = rectTransform.localEulerAngles;
        float newYRotation = currentEulerAngles.y == 0f ? _targetYRotation : 0f;

        _initialRotation = rectTransform.localRotation;
        _finalRotation = Quaternion.Euler(currentEulerAngles.x, newYRotation, currentEulerAngles.z);

        _animationTimer = 0f;
        _isAnimating = true;

    }

    private void Update()
    {
        if (!_isAnimating) return;

        _animationTimer += Time.deltaTime;
        float progress = Mathf.Clamp01(_animationTimer / _rotationDuration);

        RectTransform rectTransform = _cardImage.GetComponent<RectTransform>();
        rectTransform.localRotation = Quaternion.Lerp(_initialRotation, _finalRotation, progress);

        if (progress >= 1f)
        {
            _isAnimating = false;
        }
    }
}
