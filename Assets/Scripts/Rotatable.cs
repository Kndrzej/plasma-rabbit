using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Rotatable : MonoBehaviour, IRotatable
{
    [SerializeField] private float _angle = -180f;
    [SerializeField] private GameObject CardImage;

    public void Rotate()
    {
        RectTransform rt = CardImage.GetComponent<RectTransform>();
        Vector3 angles = rt.localEulerAngles;

        if (angles.y != 0f)
        {
            Debug.LogWarning("second");
            angles.y = 0f;
        }
        else
        {
            angles.y = _angle;
            Debug.LogWarning("first");
        }

        rt.localEulerAngles = angles;
    }
}
