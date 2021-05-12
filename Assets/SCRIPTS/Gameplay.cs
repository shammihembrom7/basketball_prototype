using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    public static Gameplay instance;

    [SerializeField] GameObject ball_prefab;
    GameObject current_ball;

    int score;
    List<GameObject> leftover_balls = new List<GameObject>();
    List<float> time_stamps = new List<float>();
    float next_time_stamp;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.5f;
        SpawnNewBall();
    }

    // Update is called once per frame
    void Update()
    {
        if (leftover_balls.Count > 0)
        {
            if (time_stamps[0] < Time.time)
            {
                Destroy(leftover_balls[0]);
                leftover_balls.RemoveAt(0);
                time_stamps.RemoveAt(0);
            }
        }
    }

    public void IncrementScore()
    {
        SpawnNewBall();
    }

    void SpawnNewBall()
    {
        if (current_ball != null)
        {
            current_ball.GetComponent<Renderer>().material.color = Color.grey;
            current_ball.layer = 9;

            leftover_balls.Add(current_ball);
            time_stamps.Add(Time.time + 5);
        }

        current_ball = Instantiate(ball_prefab);
        current_ball.transform.position = new Vector3(Random.Range(-12,10),0,0);
    }
}
