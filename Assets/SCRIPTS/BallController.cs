using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Initializations
    private float delta_nx, delta_ny; // travel in Normalized PLane (Normalized PLane is the touch screen with 0,0 at the center.)
    private float start_nx, start_ny, current_nx, current_ny; // coords in Normalized Plane
    private float center_sx, center_sy; // center of touchScreen
    private float balancer_unit;

    bool ring_start_hit;
    bool is_dead;
    bool is_shot;
    float flight_duration;

    Rigidbody ball_rb;
    bool aiming;

    // Start is called before the first frame update
    void Start()
    {
        center_sx = Screen.width / 2;
        center_sy = Screen.height / 2;
        balancer_unit = (float)20 / Screen.height;

        ball_rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!is_dead && !is_shot)
        {
            if (Input.GetMouseButtonDown(0)) Click();
            if (Input.GetMouseButton(0)) Drag();
            if (Input.GetMouseButtonUp(0)) Release();
        }

        if (is_shot && !is_dead)
        {
            flight_duration -= Time.deltaTime;
            if (flight_duration < 0)
            {
                Gameplay.instance.IncrementScore(false);
                ball_rb.constraints = RigidbodyConstraints.None;
                is_dead = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!is_dead)
        {
            if (other.CompareTag("start_detect"))
            {
                ring_start_hit = true;
            }
            if (other.CompareTag("end_detect"))
            {
                if (ring_start_hit)
                {
                    Gameplay.instance.IncrementScore(true);
                    ball_rb.constraints = RigidbodyConstraints.None;
                    is_dead = true;
                }
            }
        }
    }

    #region ControlFunctions
    void Click()
    {
        aiming = true;
        start_nx = Input.mousePosition.x - center_sx;
        start_ny = Input.mousePosition.y - center_sy;

        TrajectoryMaker.instance.Show();
    }

    void Drag()
    {
        current_nx = Input.mousePosition.x - center_sx;
        current_ny = Input.mousePosition.y - center_sy;

        delta_nx = ((current_nx - start_nx) * balancer_unit);
        delta_ny = ((current_ny - start_ny) * balancer_unit);

        flight_duration = TrajectoryMaker.instance.UpdateDots(transform.position, new Vector3(delta_nx, delta_ny, 0) * -120, ball_rb);
        flight_duration = (flight_duration * -2)+1;
    }

    void Release()
    {
        if (aiming)
        {
            aiming = false;
            ball_rb.AddForce(new Vector3(delta_nx, delta_ny, 0) * -120);
            is_shot = true;

            start_nx = 0;
            start_ny = 0;
            current_nx = 0;
            current_ny = 0;
        }

        TrajectoryMaker.instance.Hide();
    }
    #endregion ControlFunctions
}
