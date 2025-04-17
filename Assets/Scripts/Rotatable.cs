using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Rotatable : MonoBehaviour, IRotatable
{
    [SerializeField] private float _angle = 180f;
    public void Rotate()
    {
        // Rotate around Z by 180° for a flat UI image
        RectTransform rt = GetComponent<RectTransform>();
        rt.localRotation = rt.localRotation * Quaternion.Euler(0f, _angle, 0);
    }
}
