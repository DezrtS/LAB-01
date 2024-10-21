using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float maxShotSpeed = 5;
    Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        rig.AddForce(new Vector3(Random.Range(-maxShotSpeed, maxShotSpeed), 0, Random.Range(-maxShotSpeed, maxShotSpeed)), ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        rig.velocity = rig.velocity.normalized * maxShotSpeed;
    }
}
