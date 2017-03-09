using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuScript : MonoBehaviour {

	public void LoadGame()
	{
		SceneManager.LoadScene (1);
	}
}
