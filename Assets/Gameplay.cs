using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    // Initializations
    private float delta_nx, delta_ny; // travel in Normalized PLane (Normalized PLane is the touch screen with 0,0 at the center.)
    private float start_nx, start_ny, current_nx, current_ny; // coords in Normalized Plane
    private float center_sx, center_sy; // center of touchScreen
    private float balancer_unit;

    [SerializeField] Rigidbody ball_rb;
    bool aiming;

    TrajectoryMaker trajectory;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.5f;
        center_sx = Screen.width / 2;
        center_sy = Screen.height / 2;
        balancer_unit = (float)20 / Screen.height;

        trajectory = GetComponent<TrajectoryMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) Click();
        if (Input.GetMouseButton(0)) Drag();
        if (Input.GetMouseButtonUp(0)) Release();

    }

    #region ControlFunctions
    void Click()
    {
        aiming = true;
        start_nx = Input.mousePosition.x - center_sx;
        start_ny = Input.mousePosition.y - center_sy;

        trajectory.Show();
    }

    void Drag()
    {
        current_nx = Input.mousePosition.x - center_sx;
        current_ny = Input.mousePosition.y - center_sy;

        delta_nx = ((current_nx - start_nx) * balancer_unit);
        delta_ny = ((current_ny - start_ny) * balancer_unit);

        trajectory.UpdateDots(ball_rb.transform.position, new Vector3(delta_nx, delta_ny,0) * -120,ball_rb);
    }

    void Release()
    {
        if (aiming)
        {
            aiming = false;
            ball_rb.AddForce(new Vector3(delta_nx, delta_ny, 0) * -120);

            start_nx = 0;
            start_ny = 0;
            current_nx = 0;
            current_ny = 0;
        }

        trajectory.Hide();
    }
    #endregion ControlFunctions
}
