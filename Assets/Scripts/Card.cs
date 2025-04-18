using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rotatable))]
public class Card : MonoBehaviour
{
    public int ID;

    public Rotatable Rotatable;
    [SerializeField] private Image _image;

    private void Awake()
    {
        Rotatable = GetComponent<Rotatable>();
    }

    public void SetFrontTexture(Texture2D texture)
    {
      
        if (_image != null && texture != null)
        {
            // Create a new instance of the material if needed
            if (_image.material != null)
            {

                Debug.Log(_image.material);
                _image.material = new Material(_image.material);
            }

            _image.material.SetTexture("_MainTex", texture);
        }
    }




}
