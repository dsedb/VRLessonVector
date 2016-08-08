using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject text_go_add_;
	public GameObject text_go_sub_;
	public GameObject text_go_cross_;
	public GameObject target_go_;

	private VectorManager vector_manager_;
	private AudioSource audio_source_;
	private bool hit_ = false;

	// private int score_;

	void Start()
	{
		text_go_add_.SetActive(false);
		text_go_sub_.SetActive(false);
		text_go_cross_.SetActive(false);
		target_go_.SetActive(false);

		var go = GameObject.Find("VectorManager");
		vector_manager_ = go.GetComponent<VectorManager>();
		audio_source_ = GetComponent<AudioSource>();

		StartCoroutine(loop());
	}
	
	public void onHit()
	{
		audio_source_.Play();
		hit_ = true;
	}

	IEnumerator show_question(VectorType type, float seconds)
	{
		vector_manager_.setType(type);
		hit_ = false;

		float range = 0f;
		switch (type) {
			case VectorType.Add:
				range = 4f;
				text_go_add_.SetActive(true);
				text_go_sub_.SetActive(false);
				text_go_cross_.SetActive(false);
				break;
			case VectorType.Sub:
				range = 4f;
				text_go_add_.SetActive(false);
				text_go_sub_.SetActive(true);
				text_go_cross_.SetActive(false);
				break;
			case VectorType.Cross:
				range = 4f;
				text_go_add_.SetActive(false);
				text_go_sub_.SetActive(false);
				text_go_cross_.SetActive(true);
				break;
		}

		var center = vector_manager_.getCenter();

		target_go_.SetActive(false);
		target_go_.transform.position = (Random.insideUnitSphere * range) + center;
		target_go_.SetActive(true);

		var time = 0f;
		while (time < seconds && !hit_) {
			time += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator loop()
	{
		for (;;) {
			// score_ = 0;
			for (var i = 0; i < 5; ++i) {
				yield return show_question(VectorType.Add, 9999f);
			}
			for (var i = 0; i < 5; ++i) {
				yield return show_question(VectorType.Sub, 9999f);
			}
			for (var i = 0; i < 5; ++i) {
				yield return show_question(VectorType.Cross, 9999f);
			}
			yield return null;
		}
	}
}
