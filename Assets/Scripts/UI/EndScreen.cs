using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    public Button RestartButton;
    public Button EndButton;

    public bool restart { get; private set; }
    public bool end { get; private set; }

    private void Awake()
    {
        restart = false;
        end = false;
    }

    public void RestartGame()
    {
        restart = true;
        end = false;
    }

    public void EndGame()
    {
        restart = false;
        end = true;
    }

    public void ResetEndScreen()
    {
        restart = false;
        end = false;
    }
}
