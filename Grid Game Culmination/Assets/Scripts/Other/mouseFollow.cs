using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseFollow : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 20);
    }
}
