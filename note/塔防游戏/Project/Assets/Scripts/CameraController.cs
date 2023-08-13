using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector2 horClamp;//13-40
    public Vector2 verClamp;//0-46
    public Vector2 scrollClamp;//15-30
    public float planeSpeed = 10;
    public float depthSpeed = 20;
    private void Update()
    {
        CameraSet();
    }
    private void CameraSet()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        float realX = Mathf.Clamp(transform.position.x + hor * Time.deltaTime * planeSpeed, horClamp.x, horClamp.y);
        float realZ = Mathf.Clamp(transform.position.z + ver * Time.deltaTime * planeSpeed, verClamp.x, verClamp.y);
        float realY = Mathf.Clamp(transform.position.y - scroll * Time.deltaTime * depthSpeed, scrollClamp.x, scrollClamp.y);

        transform.position = new Vector3(realX, realY, realZ);
    }
}
