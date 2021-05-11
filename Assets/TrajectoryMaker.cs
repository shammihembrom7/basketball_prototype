using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryMaker : MonoBehaviour
{
	[SerializeField] int dotsNumber;
	[SerializeField] GameObject dotsParent;
	[SerializeField] GameObject dotPrefab;
	[SerializeField] float dotSpacing;
	[SerializeField] [Range(0.01f, 0.3f)] float dotMinScale;
	[SerializeField] [Range(0.3f, 1f)] float dotMaxScale;

	Transform[] dotsList;
	Vector2 pos;

	float timeStamp;


	void Start()
	{
		Hide();
		PrepareDots();
	}

	void PrepareDots()
	{
		dotsList = new Transform[dotsNumber];
		dotPrefab.transform.localScale = Vector3.one * dotMaxScale;

		float scale = dotMaxScale;
		float scaleFactor = scale / dotsNumber;

		for (int i = 0; i < dotsNumber; i++)
		{
			dotsList[i] = Instantiate(dotPrefab, null).transform;
			dotsList[i].parent = dotsParent.transform;

			dotsList[i].localScale = Vector3.one * scale;
			if (scale > dotMinScale)
				scale -= scaleFactor;
		}
	}

	public void UpdateDots(Vector2 ballPos, Vector3 forceApplied, Rigidbody rb)
	{
		Vector3 velocity = (forceApplied / rb.mass) * Time.fixedDeltaTime;

		float flight_duration = (2 * velocity.y) / Physics.gravity.y;
		flight_duration = flight_duration* 0.6f;
		float step_time = flight_duration / dotsNumber;
		float step_time_passed;

		for(int i =0; i<dotsNumber; i++)
        {
			step_time_passed = step_time * i;
			pos = new Vector3(
				velocity.x*step_time_passed,
				velocity.y*step_time_passed - 0.5f* Physics.gravity.y*step_time_passed*step_time_passed,
				0);

			dotsList[i].position = -pos+ballPos;
		}

	}

	public void Show()
	{
		dotsParent.SetActive(true);
	}

	public void Hide()
	{
		dotsParent.SetActive(false);
	}
}
