using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class CameraLerpToTransform : MonoBehaviour {

    // Specify the target the camera will track
    public Transform camTarget;
    public float trackingSpeed;
    public float minX;
    public float minY;
    public float maxX;
    public float maxY;
    // Tracking the player
    void FixedUpdate()
    {
        // This null check ensures that a valid Transform component was assigned to the
        // camTarget field on the script in the editor.
        if (camTarget != null) 
    {
            // Vector2.Lerp() performs linear interpolation between two vectors by the third
            //parameter’s value(a value between 0 and 1).
            var newPos = Vector2.Lerp(transform.position,
            camTarget.position,
            Time.deltaTime * trackingSpeed);
            var camPosition = new Vector3(newPos.x, newPos.y, -10f);
            var v3 = camPosition;
            var clampX = Mathf.Clamp(v3.x, minX, maxX);
            var clampY = Mathf.Clamp(v3.y, minY, maxY);
            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }
}
