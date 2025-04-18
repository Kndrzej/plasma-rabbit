using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Rotatable : MonoBehaviour, IRotatable
{
    [SerializeField] private float targetYRotation = -180f;
    [SerializeField] private GameObject cardImage;
    [SerializeField] private float rotationDuration = 0.5f;

    private bool isAnimating = false;
    private float animationTimer = 0f;
    private Quaternion initialRotation;
    private Quaternion finalRotation;

    public void Rotate()
    {
        if (isAnimating) return;

        RectTransform rectTransform = cardImage.GetComponent<RectTransform>();
        Vector3 currentEulerAngles = rectTransform.localEulerAngles;
        float newYRotation = currentEulerAngles.y == 0f ? targetYRotation : 0f;

        initialRotation = rectTransform.localRotation;
        finalRotation = Quaternion.Euler(currentEulerAngles.x, newYRotation, currentEulerAngles.z);

        animationTimer = 0f;
        isAnimating = true;
    }

    private void Update()
    {
        if (!isAnimating) return;

        animationTimer += Time.deltaTime;
        float progress = Mathf.Clamp01(animationTimer / rotationDuration);

        RectTransform rectTransform = cardImage.GetComponent<RectTransform>();
        rectTransform.localRotation = Quaternion.Lerp(initialRotation, finalRotation, progress);

        if (progress >= 1f)
        {
            isAnimating = false;
        }
    }
}
