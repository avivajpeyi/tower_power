/// Makes the camera follow a transform
/// This script is just used for testing

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform follow;

    private float dampTime = 10f;
    private float yOffest = 1f;
    private float margin = 0.1f; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float targetY = follow.position.y + yOffest;

        if (Mathf.Abs(transform.position.y - targetY) > margin)
            targetY = Mathf.Lerp(transform.position.y, targetY,
                dampTime * Time.deltaTime);
        
        transform.position = new Vector3(
            transform.position.x,
            targetY, 
            transform.position.z);
    }
}
