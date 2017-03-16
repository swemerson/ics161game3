using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float lerpSpeed;
    public float mouseZoomSpeed;
    public float controllerZoomSpeed;
    public float minHeight;
    public float maxHeight;

    private GameObject player1;
    private GameObject player2;
    private Transform player1Transform;
    private Transform player2Transform;
    private Camera mainCamera;

	void Start()
    {
        player1 = GameObject.FindGameObjectWithTag("Player");
        player2 = GameObject.FindGameObjectWithTag("Player2");
        if (player1 != null)
        {
            player1Transform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }
        if (player2 != null)
        {
            player2Transform = GameObject.FindGameObjectWithTag("Player2").GetComponent<Transform>();
        }
        mainCamera = GetComponent<Camera>();
	}
	
	void Update()
    {
        Vector3 cameraTarget;

        // Follow the players with a delay
        if (player1 != null && player2 != null && player1.activeSelf && player2.activeSelf)
        {
            cameraTarget = new Vector3((player1Transform.position.x + player2Transform.position.x) / 2, (player1Transform.position.y + player2Transform.position.y) / 2, -10);
        }
        else if (player1 != null && player1.activeSelf)
        {
            cameraTarget = new Vector3(player1Transform.position.x, player1Transform.position.y, -10);
        }
        else
        {
            cameraTarget = new Vector3(player2Transform.position.x, player2Transform.position.y, -10);
        }
                
        transform.position = Vector3.Lerp(transform.position, cameraTarget, Time.smoothDeltaTime * lerpSpeed);

        mainCamera.orthographicSize = Vector2.Distance(player1Transform.position, player2Transform.position);
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minHeight, maxHeight);
    }

    public void ZoomFullOut()
    {
        mainCamera.orthographicSize = maxHeight;
    }
}