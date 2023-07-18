using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search.Providers;
using UnityEngine;

public class SetFullScreen : MonoBehaviour
{
    public bool IsFullScreen => Screen.fullScreen;

    public void FullsScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}

