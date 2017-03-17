using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour {
	private GUIStyle textStyle = new GUIStyle();

	public void LoadGame()
	{
		SceneManager.LoadScene (1);
	}

	private bool showInstructions = false;


	public void LoadInstructions()
	{
		showInstructions = true;
	}

	void OnGUI ()
	{
		var texture = new Texture2D (128,128);
		for (int y = 0; y < texture.height; y++) {
			for (int x = 0; x < texture.width; x++) {
				Color color = new Color (0.1F, 0.1F, 0.3F, 1.0F);
				texture.SetPixel(x, y, color);
			}
		}
		texture.Apply();

		textStyle.fontSize = 24;
		textStyle.alignment = TextAnchor.MiddleCenter;
		textStyle.normal.textColor = Color.white;
		textStyle.normal.background = texture;

		if (showInstructions) 
		{
			GUI.Label (new Rect(0,0,Screen.width, Screen.height), "Player 1 Controls\nMove: W, A, S, D\nDash: Space\nAim: Move Mouse Cursor\nReload: R\nShoot: Mouse 1\nMelee: Mouse 2\nRestart: Backspace\n\nPlayer 2 Controls (PS4 controller)\nMove: Left Joystick\nDash: Left Bumper\nAim: Right Joystick Left/Right\nReload: Right Bumper\nShoot: Left Trigger\nMelee: Right Joystick Down\n\nTo return to main menu click screen\nTo exit in game click ESC", textStyle);
		}
	}

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) {
			showInstructions = false;
		}
	}
}