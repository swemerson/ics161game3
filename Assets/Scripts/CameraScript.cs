using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float lerpSpeed;
    public float mouseZoomSpeed;
    public float controllerZoomSpeed;
    public float minHeight;
    public float maxHeight;

    private Transform playerTransform;
    private Camera mainCamera;

	void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        mainCamera = GetComponent<Camera>();
	}
	
	void Update()
    {
        // Follow the players with a delay
        var cameraTarget = new Vector3(playerTransform.position.x, playerTransform.position.y, playerTransform.position.z - 10);
        transform.position = Vector3.Lerp(transform.position, cameraTarget, Time.smoothDeltaTime * lerpSpeed);

        // Zoom camera in/out
        mainCamera.orthographicSize += -1 * Input.GetAxis("Mouse ScrollWheel") * mouseZoomSpeed;
        mainCamera.orthographicSize += Input.GetAxis("Joy Right Shoulder") * controllerZoomSpeed;
        mainCamera.orthographicSize -= Input.GetAxis("Joy Left Shoulder") * controllerZoomSpeed;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minHeight, maxHeight);
    }

    public void ZoomFullOut()
    {
        mainCamera.orthographicSize = maxHeight;
    }
}