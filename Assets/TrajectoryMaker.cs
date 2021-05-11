using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryMaker : MonoBehaviour
{
	[SerializeField] int dots_count;
	[SerializeField] GameObject dots_parent;
	[SerializeField] GameObject dot_prefab;
	[SerializeField] [Range(0.01f, 0.3f)] float dots_min_scale;
	[SerializeField] [Range(0.3f, 1f)] float dots_max_scale;

	Transform[] dots_list;
	Vector2 pos;

	float flight_duration;
	float step_time;
	float step_time_passed;


	void Start()
	{
		Hide();
		PrepareDots();
	}

	void PrepareDots()
	{
		dots_list = new Transform[dots_count];
		dot_prefab.transform.localScale = Vector3.one * dots_max_scale;

		float scale = dots_max_scale;
		float scaleFactor = scale / dots_count;

		for (int i = 0; i < dots_count; i++)
		{
			dots_list[i] = Instantiate(dot_prefab, null).transform;
			dots_list[i].parent = dots_parent.transform;

			dots_list[i].localScale = Vector3.one * scale;
			if (scale > dots_min_scale)
				scale -= scaleFactor;
		}
	}

	public void UpdateDots(Vector2 ballPos, Vector3 forceApplied, Rigidbody rb)
	{
		Vector3 velocity = (forceApplied / rb.mass) * Time.fixedDeltaTime;

		flight_duration = (2 * velocity.y) / Physics.gravity.y;
		flight_duration = flight_duration* 0.6f;
		step_time = flight_duration / dots_count;

		for(int i =0; i<dots_count; i++)
        {
			step_time_passed = step_time * i;
			pos = new Vector3(
				velocity.x*step_time_passed,
				velocity.y*step_time_passed - 0.5f* Physics.gravity.y*step_time_passed*step_time_passed,
				0);

			dots_list[i].position = -pos+ballPos;
		}

	}

	public void Show()
	{
		dots_parent.SetActive(true);
	}

	public void Hide()
	{
		dots_parent.SetActive(false);
	}
}
