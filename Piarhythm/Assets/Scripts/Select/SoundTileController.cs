using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTileController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		DirectFront();
	}


	public void DirectFront()
	{
		Vector3 euler = transform.parent.GetComponent<RectTransform>().rotation.eulerAngles;
		GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0.0f, -euler.y, 0.0f));
	}
}
