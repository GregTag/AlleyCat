using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windows : MonoBehaviour
{
    public int frameDelay = 300;
    public float appearProbability = 0.5f;
    public float disappearProbability = 0.2f;
    private GameObject[] windows;
    private int activeWindow = -1;

    private void DeactivateWindow()
    {
        windows[activeWindow].GetComponent<Window>().Deactivate();
        activeWindow = -1;
    }
    private void HandleFence(bool isReached)
    {
        enabled = isReached;
        if (!isReached && activeWindow != -1)
        {
            DeactivateWindow();
        }
    }
    private void Start()
    {
        FindAnyObjectByType<PlayerControl>().OnPlayerFenceReached += HandleFence;
        windows = GameObject.FindGameObjectsWithTag("Window");
        enabled = false;
    }

    private void Update()
    {
        if (Time.frameCount % frameDelay != 0)
        {
            return;
        }
        if (activeWindow == -1)
        {
            if (Random.Range(0f, 1f) < appearProbability)
            {
                activeWindow = UnityEngine.Random.Range(0, windows.Length);
                windows[activeWindow].GetComponent<Window>().Activate();
            }
        }
        else
        {
            if (Random.Range(0f, 1f) < disappearProbability)
            {
                DeactivateWindow();
            }
        }
    }
}
