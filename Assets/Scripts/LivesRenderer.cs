using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LivesRenderer : MonoBehaviour
{
    private TextMeshProUGUI mesh;

    private void Start()
    {
        mesh = GetComponent<TextMeshProUGUI>();
        FindAnyObjectByType<PlayerControl>().OnPlayerDeath += UpdateLives;
    }

    private void UpdateLives(int lives)
    {
        mesh.SetText("Lives: " + lives.ToString());
    }
}
