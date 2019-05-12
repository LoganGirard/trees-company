using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Playables;
using UnityEngine.UI;

public class Clicker : MonoBehaviour
{
    public GameObject treePrefab;
    public GameObject powerPrefab;
    public GameObject housePrefab;

    public Sprite TreeCardSprite;
    public Sprite PowerCardSprite;
    public Sprite HouseCardSprite;

    public AudioClip[] PlaceStuffClips;

    public GameObject CurrentCardSelection;

    private GameObject CurrentPrefabSelection;

    Transform PreviousHighlighted;

    Camera Camera;
    // Start is called before the first frame update
    void Start()
    {
        Camera = FindObjectOfType<Camera>();

        CurrentPrefabSelection = treePrefab;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.name.Contains("Floor"))
            {
                MeshRenderer mr;

                if (PreviousHighlighted != null)
                {   
                    mr = PreviousHighlighted.GetComponent<MeshRenderer>();
                    mr.material.shader = Shader.Find("Unlit/Texture");
                }
                PreviousHighlighted = hit.transform;
                mr = PreviousHighlighted.GetComponent<MeshRenderer>();
                mr.material.shader = Shader.Find("Outlined/Custom");
            }
            SetHighlights(hit.transform, 0);

            if (Input.GetMouseButtonDown(0))
            {
                PlaceGameObject(CurrentPrefabSelection, hit);
            }
        }

        SwitchCurrentPrefabSelection();
    }

    void SetHighlights(Transform initialTransform, int level)
    {
        if (level > 10) return;

        MeshRenderer mr = initialTransform.GetComponent<MeshRenderer>();

        if (mr != null)
        {
            mr.material.shader = Shader.Find("Outlined/Custom");
        }

        for (int i = 0;  i < initialTransform.childCount; i++)
        {
            level++;
            SetHighlights(transform.GetChild(i), level);
        }
    }

    void PlaceGameObject(GameObject prefab, RaycastHit hit)
    {
        // Only let the user add stuff on floor tiles
        if (hit.transform.gameObject.name.Contains("Floor"))
        {
            Debug.Log(hit.transform.gameObject.name);
            // the object identified by hit.transform was clicked
            // do whatever you want

            int xIndex = hit.transform.gameObject.GetComponent<PlayableBase>().X;
            int yIndex = hit.transform.gameObject.GetComponent<PlayableBase>().Y;

            Destroy(BoardManager.gridGameObjects[yIndex, xIndex]);

            BoardManager.gridGameObjects[yIndex, xIndex]
                = Instantiate(prefab, hit.transform.position, Quaternion.identity);

            SoundManager.instance.RandomizeSfx(PlaceStuffClips);

            // BoardManager.gridGameObjects[yIndex, xIndex].transform.SetParent(BoardManager.boardHolder);
        }
        else
        {
            Debug.Log("Can't click non-floor tiles!");
        }
    }

    // Switch current prefab and reflect changes in UI
    void SwitchCurrentPrefabSelection()
    {
        if(Input.GetKey(KeyCode.A))
        {
            CurrentPrefabSelection = treePrefab;

            CurrentCardSelection.GetComponent<Image>().sprite = TreeCardSprite;

        }

        else if (Input.GetKey(KeyCode.S))
        {
            CurrentPrefabSelection = housePrefab;

            CurrentCardSelection.GetComponent<Image>().sprite = HouseCardSprite;
        }

        else if (Input.GetKey(KeyCode.D))
        {
            CurrentPrefabSelection = powerPrefab;

            CurrentCardSelection.GetComponent<Image>().sprite = PowerCardSprite;
        }
    }
    
}
