using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 80;
    public float ZoomScale = 0.1f;

    Vector3 RotationPoint;
    public Vector3 PerspectiveOffset;
    public float FocusSmoothTime;
    public float ZoomSmoothTime;

    [Range(0, 5)]
    public float ProportionalFieldOfView = 1.0f;

    public float lowerAngle = 70;
    public float upperAngle = 330;

    // ref variables:
    float FocusVelocity;
    float ZoomVelocity;

    // Camera view
    Camera Camera;
    Vector3 CameraPosition;
    Quaternion CameraRotation;
    Bounds AllBounds;
    GameObject[] FocusableGameObjects;

    // Mouse stuff
    Vector3 MouseLastPos;
    float mouseRotationScale = 0.1f;

    private void Start()
    {
        Camera = GetComponent<Camera>();
        GetFocusableGameObjects();
        AllBounds = CalculateBounds(FocusableGameObjects);
        MouseLastPos = Input.mousePosition;
    }

    private void Update()
    {
        MoveCamera();

        GetFocusableGameObjects();
        AllBounds = CalculateBounds(FocusableGameObjects);
        RefocusCamera();
        ZoomCamera();
    }

    private Vector3 GetMovement()
    {
        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement.x--;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movement.x++;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movement.y++;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movement.y--;
        }

        if (Input.GetMouseButton(1))
        {
            var mouseDelta = mouseRotationScale * (Input.mousePosition - MouseLastPos);
            Debug.Log(MouseLastPos + ", " + Input.mousePosition + ", " + mouseDelta);

            movement.x -= mouseDelta.x;
            movement.y -= mouseDelta.y;
        }

        // Mouse stuff
        MouseLastPos = Input.mousePosition;

        return movement;
    }

    private void MoveCamera()
    {
        Vector3 movement = GetMovement();
        transform.RotateAround(AllBounds.center, Vector3.up, -movement.x * Time.deltaTime * rotationSpeed);
        var backupPosition = transform.position;
        var backupRotation = transform.rotation;

        transform.RotateAround(AllBounds.center, Camera.transform.right, movement.y * Time.deltaTime * rotationSpeed);
        Camera.transform.LookAt(AllBounds.center);

        if (transform.eulerAngles.x > lowerAngle && transform.eulerAngles.x < upperAngle)
        {
            transform.position = backupPosition;
            transform.rotation = backupRotation;
        }

        Camera.transform.LookAt(AllBounds.center);
    }

    void RefocusCamera()
    {
        // The focus-in is to the sphere surrounding the objects' bounds,
        float viewAngle = CalculateFieldOfViewForBounds(AllBounds);

        float newFieldOfView;

        // Assuming width bigger than height so we don't need to care about viewAngle < 1
        newFieldOfView = viewAngle * ProportionalFieldOfView;

        Camera.fieldOfView = Mathf.SmoothDamp(Camera.fieldOfView, newFieldOfView, ref FocusVelocity, FocusSmoothTime);
    }

    void ZoomCamera()
    {
        var newFieldOfView = ProportionalFieldOfView - Input.mouseScrollDelta.y * ZoomScale;
        if (newFieldOfView > 0.5 && newFieldOfView < 1.5)
        {
            ProportionalFieldOfView = Mathf.SmoothDamp(ProportionalFieldOfView, newFieldOfView, ref ZoomVelocity, ZoomSmoothTime);
        }
    }

    float CalculateFieldOfViewForBounds(Bounds bounds)
    {
        float x = bounds.size.x;
        float y = bounds.size.y;
        float z = bounds.size.z;

        // FieldOfView is the vertical angle of the camera's view
        float boundsDiagonal = Mathf.Sqrt(x * x + y * y + z * z);
        float distanceFromFocus = (Camera.transform.position - bounds.center).magnitude;
        return Mathf.Rad2Deg * 2 * Mathf.Atan(boundsDiagonal / (distanceFromFocus * 2));
    }

    private void GetFocusableGameObjects()
    {
        FocusableGameObjects = GameObject.FindGameObjectsWithTag("TileObjects");
    }

    private static Bounds GetBoundsForGameObject(GameObject go)
    {
        MeshRenderer mr = go.GetComponent<MeshRenderer>();

        return mr == null ?
            new Bounds(go.transform.position, new Vector3(1, 1, 1)) :
            mr.bounds;
    }

    private static Bounds CalculateBounds(GameObject[] GameObjects)
    {
        // This adds point 0,0,0 which should never be a problem for our case
        Bounds newBounds = new Bounds();

        foreach (GameObject go in GameObjects)
        {
            newBounds.Encapsulate(GetBoundsForGameObject(go));
        }

        return newBounds;
    }
}