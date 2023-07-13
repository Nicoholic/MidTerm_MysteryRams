using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search.Providers;
using UnityEngine;

public class SetFullScreen : MonoBehaviour
{
    public void FullsScreen(bool isfullScreen)
    {
        Screen.fullScreen = isfullScreen;
    }
}
