using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float DestroyTime = 1.0f;
    public Vector3 Offset = new Vector3(0, 2f, 0);
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestroyTime);

        transform.localPosition += Offset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
