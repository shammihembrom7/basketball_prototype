using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    // Initializations
    private float deltaNx, deltaNy; // travel in Normalized PLane (Normalized PLane is the touch screen with 0,0 at the center.)
    private float startNx, startNy, currentNx, currentNy; // coords in Normalized Plane
    private float centerSx, centerSy; // center of touchScreen
    private float balancerUnit;

    Vector3 direction;
    [SerializeField] Rigidbody ball_rb;
    bool aiming;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.5f;
        centerSx = Screen.width / 2;
        centerSy = Screen.height / 2;
        balancerUnit = (float)20 / Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) Click();
        if (Input.GetMouseButton(0)) Drag();
        if (Input.GetMouseButtonUp(0)) Release();

        if (Input.GetKeyDown("space"))
        {
            ball_rb.AddForce((Vector3.right + Vector3.up * 2) * 450);
        }
    }

    void Click()
    {
        aiming = true;
        startNx = Input.mousePosition.x - centerSx;
        startNy = Input.mousePosition.y - centerSy;
    }

    void Drag()
    {
        currentNx = Input.mousePosition.x - centerSx;
        currentNy = Input.mousePosition.y - centerSy;

        deltaNx = ((currentNx - startNx) * balancerUnit);
        deltaNy = ((currentNy - startNy) * balancerUnit);
    }

    void Release()
    {
        if (aiming)
        {
            aiming = false;
            ball_rb.AddForce(new Vector3(deltaNx, deltaNy, 0) * -120);

            startNx = 0;
            startNy = 0;
            currentNx = 0;
            currentNy = 0;
        }
    }
}
