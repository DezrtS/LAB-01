using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTarget : MonoBehaviour
{
    public Camera mainCamera;
    public NavControl navControl;
    public LayerMask raycastLayers;
    public GameObject target;
    public Transform lastTarget;
    bool forcePosition = false;

    private Vector3 hitPosition;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, raycastLayers))
            {
                hitPosition = hit.point;
                Debug.Log("Hit Position: " + hitPosition);
                Destroy(target);
                target = new GameObject("Force Target");
                target.transform.position = hitPosition;
                if (!forcePosition)
                {
                    lastTarget = navControl.target;
                }
                forcePosition = true;
                navControl.target = target.transform;
            }
        }
    }

    private void FixedUpdate()
    {
        if (forcePosition)
        {
            if (Vector3.Distance(transform.position, hitPosition) <= 0.5f)
            {
                forcePosition = false;
                navControl.target = lastTarget;
            }
        }
    }
}
