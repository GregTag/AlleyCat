using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Window : MonoBehaviour
{
    public Object spikePrefab;
    public float spikeZ = 4;
    public float frameDelay = 300;
    public float throwProbability = 0.5f;
    private Animator animator;


    public void Activate()
    {
        animator.SetBool("IsOpen", true);
        enabled = true;
        Throw();
    }

    public void Deactivate()
    {
        animator.SetBool("IsOpen", false);
        enabled = false;
    }

    private void Throw()
    {
        var position = transform.position;
        position.z = spikeZ;
        Instantiate(spikePrefab, position, Quaternion.identity);

    }


    private void Start()
    {
        animator = GetComponent<Animator>();
        enabled = false;
    }

    private void Update()
    {
        if (Time.frameCount % frameDelay != 0)
        {
            return;
        }
        if (Random.Range(0f, 1f) < throwProbability)
        {
            Throw();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (enabled && other.gameObject.CompareTag("Player"))
        {
            FindAnyObjectByType<ScoreRenderer>().IncreaseScore(650);
            SceneManager.LoadScene("YouWin");
        }
    }
}
