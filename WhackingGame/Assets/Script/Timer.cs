using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float duration_time;
    public float current_time = 0f;

    public TMP_Text timerText;

    public GameObject gameOverPanel;
    public GameObject gameUIPanel;

    public Spawner spawner;

    // Start is called before the first frame update
    void Start()
    {
        current_time = duration_time;
    }

    // Update is called once per frame
    void Update()
    {
       current_time -=1* Time.deltaTime;
       timerText.text = current_time.ToString("0");

        if(current_time < 0)
        {
            current_time = 0;
            AudioManager.instance.PlaySFX("GameOver");
            gameOverPanel.SetActive(true);
            gameUIPanel.SetActive(false);

            spawner.StopSpawning();
        }
    }
}
