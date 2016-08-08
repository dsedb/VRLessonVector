using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {

	private GameManager game_manager_;

	void Start()
	{
		game_manager_ = GameObject.Find("game_manager").GetComponent<GameManager>();
	}
	
    void OnTriggerEnter(Collider other)
	{
		Debug.Log("hit");
		game_manager_.onHit();
	}
}
