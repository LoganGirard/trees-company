using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Playables;

public class Clicker : MonoBehaviour
{
    public GameObject powerPrefab;

    Camera Camera;
    // Start is called before the first frame update
    void Start()
    {
        Camera = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.gameObject.name);
                // the object identified by hit.transform was clicked
                // do whatever you want

                int x = hit.transform.gameObject.GetComponent<PlayableBase>().X;
                int y = hit.transform.gameObject.GetComponent<PlayableBase>().Y;

                Destroy(BoardManager.gridGameObjects[y, x]);

                BoardManager.gridGameObjects[y, x] 
                    = Instantiate(powerPrefab, new Vector3(x, y, 0f), Quaternion.identity);

            }
        }
    }
}
