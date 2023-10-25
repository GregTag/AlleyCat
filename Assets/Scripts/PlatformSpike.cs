using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

enum SpikeState
{
    Hidden,
    Appearing,
    Visible,
    Disappearing
}

public class PlatformSpike : MonoBehaviour
{
    public float appearProbability = 0.3f;
    public float disappearProbability = 0.3f;
    public int frameDelay = 120;
    private SpikeState state = SpikeState.Hidden;

    private void ToggleActive()
    {
        const float large_number = 1000000f;
        var position = transform.position;
        position.y += state == SpikeState.Appearing ? large_number : -large_number;
        transform.position = position;
    }

    private void Start()
    {
        // Hide spikes at the start of the game
        ToggleActive();
    }

    private void Update()
    {
        if (Time.frameCount % frameDelay != 0)
        {
            return;
        }
        switch (state)
        {
            case SpikeState.Hidden:
                if (Random.Range(0f, 1f) < appearProbability)
                {
                    state = SpikeState.Appearing;
                }
                break;
            case SpikeState.Appearing:
                ToggleActive();
                state = SpikeState.Visible;
                break;
            case SpikeState.Visible:
                if (Random.Range(0f, 1f) < disappearProbability)
                {
                    state = SpikeState.Disappearing;
                }
                break;
            case SpikeState.Disappearing:
                ToggleActive();
                state = SpikeState.Hidden;
                break;
        }
    }
}
