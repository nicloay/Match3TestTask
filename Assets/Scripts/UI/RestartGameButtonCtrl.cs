
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Match3.UI
{
	
	[RequireComponent(typeof(Button))]
	public class RestartGameButtonCtrl : MonoBehaviour {
		
		void Awake()
		{
			GetComponent<Button>().onClick.AddListener(() =>
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			});	
		}
		
	}
}
