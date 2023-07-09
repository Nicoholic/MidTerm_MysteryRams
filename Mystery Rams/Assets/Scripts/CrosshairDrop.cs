using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairDrop : MonoBehaviour
{
    public Image Limg;
    public Sprite[] Nimg;
    public Dropdown CHDD;

    public void ChangerImage(Dropdown CHDD)
    {
        Limg.sprite = Nimg[CHDD.value];
    }
}
