using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSlider : MonoBehaviour
{
    [SerializeField] Slider sliderRed;
    [SerializeField] Slider sliderGreen;
    [SerializeField] Slider sliderBlue;

    Image colorImage;
    void Start()
    {
        colorImage = GetComponent<Image>();
    }

    void Update()
    {
        if (colorImage != null)
        {
            colorImage.color = new Color(sliderRed.value, sliderGreen.value, sliderBlue.value);
        }
    }

}
