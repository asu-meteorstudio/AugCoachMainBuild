using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointSize : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject jointPrefab;

    private Vector3 scale;
    private Vector3 scale_;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scale = transform.localScale;
        scale_ = new Vector3(0.1f * scale.x, 0.1f * scale.y, 0.1f * scale.z);
        jointPrefab.transform.localScale = scale_;

    }
}
