using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreRenderer : MonoBehaviour
{
    [SerializeField]
    private bool isGameOver = false;
    private TextMeshProUGUI mesh;
    private int score = 0;

    public void IncreaseScore(int value)
    {
        score += value;
        mesh.SetText("Score: " + score.ToString());
    }

    void Start()
    {
        mesh = GetComponent<TextMeshProUGUI>();
        if (isGameOver)
        {
            mesh.SetText("Score: " + PlayerPrefs.GetInt("Score", 0).ToString());
            return;
        }
        SceneManager.sceneUnloaded += (scene) =>
        {
            PlayerPrefs.SetInt("Score", score);
        };

    }
}
