using UnityEngine;
using System.Collections;

public class TestRoomRenerer : MonoBehaviour {

	private UTJ.BatchLineRenderer batch_line_renderer_;

	void Start()
	{
		batch_line_renderer_ = GetComponent<UTJ.BatchLineRenderer>();
		batch_line_renderer_.begin();
		float width = 10f;
		float step = 1f;
		var col = new Color(0.2f, 0.2f, 0.1f);
		for (var w = -5f; w < 15f; w += 10f) {
			for (var v = -width*0.5f; v < width*0.5f; v += step) {
				var v0 = new Vector3(v, w, -width*0.5f);
				var v1 = new Vector3(v, w, width*0.5f);
				batch_line_renderer_.draw(ref v0, ref v1, ref col, ref col);
			}
			for (var v = -width*0.5f; v < width*0.5f; v += step) {
				var v0 = new Vector3(-width*0.5f, w, v);
				var v1 = new Vector3( width*0.5f, w, v);
				batch_line_renderer_.draw(ref v0, ref v1, ref col, ref col);
			}
		}
		for (var w = -5f; w < 15f; w += 10f) {
			for (var v = -width*0.5f; v < width*0.5f; v += step) {
				var v0 = new Vector3(w, -width*0.5f, v);
				var v1 = new Vector3(w,  width*0.5f, v);
				batch_line_renderer_.draw(ref v0, ref v1, ref col, ref col);
			}
			for (var v = -width*0.5f; v < width*0.5f; v += step) {
				var v0 = new Vector3(w, v, -width*0.5f);
				var v1 = new Vector3(w, v,  width*0.5f);
				batch_line_renderer_.draw(ref v0, ref v1, ref col, ref col);
			}
		}
		for (var w = -5f; w < 15f; w += 10f) {
			for (var v = -width*0.5f; v < width*0.5f; v += step) {
				var v0 = new Vector3(-width*0.5f, v, w);
				var v1 = new Vector3( width*0.5f, v, w);
				batch_line_renderer_.draw(ref v0, ref v1, ref col, ref col);
			}
			for (var v = -width*0.5f; v < width*0.5f; v += step) {
				var v0 = new Vector3(v, -width*0.5f, w);
				var v1 = new Vector3(v,  width*0.5f, w);
				batch_line_renderer_.draw(ref v0, ref v1, ref col, ref col);
			}
		}
		batch_line_renderer_.end();
	}
	
}
