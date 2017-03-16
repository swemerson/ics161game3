using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float lerpSpeed;
    public float mouseZoomSpeed;
    public float controllerZoomSpeed;
    public float minHeight;
    public float maxHeight;

    private GameObject[] players;
    private Camera mainCamera;

	void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        mainCamera = GetComponent<Camera>();
	}
	
	void Update()
    {
        // Follow the players with a delay
        var cameraTarget = new Vector3((players[0].transform.position.x + players[1].transform.position.x)/2, (players[0].transform.position.y + players[1].transform.position.y)/2, -10);
        transform.position = Vector3.Lerp(transform.position, cameraTarget, Time.smoothDeltaTime * lerpSpeed);
    }

    public void ZoomFullOut()
    {
        mainCamera.orthographicSize = maxHeight;
    }
}