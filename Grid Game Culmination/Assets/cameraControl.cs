using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{

    public int xBounds;
    public int yBounds;
    public int zoomOut;
    public int zoomIn;
    private bool zoomedIn = false;
    public Vector3 zoomTarget;
    public float cameraMoveSpeed;
    public Vector3 xyPos;
    public GridManager gridManager;
    private bool selected;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
    }
    
    public void takeInputs(){
        Vector3 pos = transform.position;
        if (Input.GetKeyUp(KeyCode.Space))
        {
            zoomedIn = !zoomedIn;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            xyPos.y += (cameraMoveSpeed / 320);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            xyPos.y -= (cameraMoveSpeed / 320);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            xyPos.x -= (cameraMoveSpeed / 320);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            xyPos.x += (cameraMoveSpeed / 320);
        }
    }

    public void zoomSmooth()
    {
        Vector3 pos = transform.position;
        if (zoomTarget != pos)
        {
            var position = transform.position;
            Vector3 targetPos = Vector3.Lerp(position, zoomTarget, Time.deltaTime * cameraMoveSpeed);
            transform.position = targetPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        takeInputs();
        zoomTarget = new Vector3(xyPos.x, xyPos.y, zoomedIn ? zoomIn : zoomOut);
        zoomSmooth();
    }
}
