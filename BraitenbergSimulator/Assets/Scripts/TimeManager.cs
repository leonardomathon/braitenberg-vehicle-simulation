using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    // Current game speed
    [SerializeField]
    private float[] speedControls = new float[]{
        0.25f, 0.5f, 1f, 1.5f, 2f
    };

    private int speedControlPointer = 2;
    private bool paused;

    #region singleton
    private static TimeManager _instance;

    public static TimeManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    void Update()
    {
        if (!paused)
        {
            if (Time.timeScale != speedControls[speedControlPointer])
            {
                Time.timeScale = speedControls[speedControlPointer];
            }
        }
        else
        {
            Time.timeScale = 0f;
            Debug.Log("Paused");
        }


        if (Input.GetKeyDown(KeyCode.RightBracket))
            GameSpeedUp();

        if (Input.GetKeyDown(KeyCode.LeftBracket))
            GameSpeedDown();

        if (Input.GetKeyDown(KeyCode.Space))
            TogglePause();

    }

    public float GetCurrentGameSpeed()
    {
        // Return 0 when paused
        if (paused)
        {
            return 0f;
        }

        return speedControls[speedControlPointer];
    }

    public bool IsPaused()
    {
        return paused;
    }

    public void TogglePause()
    {
        paused = !paused;
    }

    public void GameSpeedUp()
    {
        // Only speedup if allowed
        if (speedControlPointer + 1 < speedControls.Length)
        {
            Debug.Log("Increased speed");
            speedControlPointer++;
        }
    }

    public void GameSpeedDown()
    {
        // Only speeddown if allowed
        if (speedControlPointer - 1 >= 0)
        {
            Debug.Log("Decreased speed");
            speedControlPointer--;
        }
    }
}
