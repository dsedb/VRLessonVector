using UnityEngine;
using System.Collections;

public class VectorManager : MonoBehaviour {

	public GameObject hand_a_go;
	public GameObject hand_b_go;
	// public GameObject vector_a_go;
	// public GameObject vector_b_go;
	// public GameObject vector_add_go;
	// public GameObject vector_sub_go;
	// public GameObject vector_cross_go;

	private Transform hand_transform_a_;
	private Transform hand_transform_b_;
	// private VolumetricLines.VolumetricLineBehavior hand_line_a_;
	// private VolumetricLines.VolumetricLineBehavior hand_line_b_;
	// private VolumetricLines.VolumetricLineBehavior line_a_;
	// private VolumetricLines.VolumetricLineBehavior line_b_;
	// private VolumetricLines.VolumetricLineBehavior line_add_;
	// private VolumetricLines.VolumetricLineBehavior line_sub_;
	// private VolumetricLines.VolumetricLineBehavior line_cross_;

	private int test_line_id_;
	private int hand_a_line_id_;

	private Vector3 hand_offset = new Vector3(0f, -0.02f, 0.04f);

	private Vector3 vector_a_;
	private Vector3 vector_b_;

	private bool display_add_ = false;
	private bool display_sub_ = false;
	private bool display_cross_ = false;

	enum PadButton
	{
		None,
		Left,
		Right,
		Up,
		Down,
	};

	void Start()
	{
		hand_transform_a_ = hand_a_go.transform;
		hand_transform_b_ = hand_b_go.transform;
		// hand_line_a_ = hand_a_go.GetComponent<VolumetricLines.VolumetricLineBehavior>();
		// hand_line_b_ = hand_b_go.GetComponent<VolumetricLines.VolumetricLineBehavior>();
		// line_a_ = vector_a_go.GetComponent<VolumetricLines.VolumetricLineBehavior>();
		// line_b_ = vector_b_go.GetComponent<VolumetricLines.VolumetricLineBehavior>();
		// line_add_ = vector_add_go.GetComponent<VolumetricLines.VolumetricLineBehavior>();
		// line_sub_ = vector_sub_go.GetComponent<VolumetricLines.VolumetricLineBehavior>();
		// line_cross_ = vector_cross_go.GetComponent<VolumetricLines.VolumetricLineBehavior>();

		test_line_id_ = UTJ.VolumetricLine.Instance.spawn(0.25f /* width */, UTJ.VolumetricLine.Type.None);
		hand_a_line_id_ = UTJ.VolumetricLine.Instance.spawn(0.25f /* width */, UTJ.VolumetricLine.Type.None);
	}

	void OnDestroy()
	{
		UTJ.VolumetricLine.Instance.destroy(test_line_id_);
		UTJ.VolumetricLine.Instance.destroy(hand_a_line_id_);
	}

	private float get_value(SteamVR_Controller.DeviceRelation device_id)
	{
		float value = 0f;
		var deviceIndex = SteamVR_Controller.GetDeviceIndex(device_id);
		if (deviceIndex != -1) {
			SteamVR_Controller.Device device = SteamVR_Controller.Input(deviceIndex);
			var val = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger);
			value = val.x;
		}
		return value;
	}

	private PadButton get_button(SteamVR_Controller.DeviceRelation device_id)
	{
		PadButton value = PadButton.None;
		var deviceIndex = SteamVR_Controller.GetDeviceIndex(device_id);
		if (deviceIndex != -1) {
			SteamVR_Controller.Device device = SteamVR_Controller.Input(deviceIndex);
			bool pressed = device.GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
			if (pressed) {
				var val = device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
				if (val.x + val.y < 0f) {
					if (val.x - val.y < 0f) {
						value = PadButton.Left;
					} else {
						value = PadButton.Down;
					}
				} else {
					if (val.x - val.y < 0f) {
						value = PadButton.Up;
					} else {
						value = PadButton.Right;
					}
				}
			}
		}
		return value;
	}

	void Update ()
	{
		float right_value = get_value(SteamVR_Controller.DeviceRelation.Rightmost);
		float left_value = get_value(SteamVR_Controller.DeviceRelation.Leftmost);
		
		switch (get_button(SteamVR_Controller.DeviceRelation.Rightmost)) {
			case PadButton.None:
				break;
			case PadButton.Left:
				display_cross_ = !display_cross_;
				break;
			case PadButton.Right:
				display_sub_ = !display_sub_;
				break;
			case PadButton.Up:
				break;
			case PadButton.Down:
				display_add_ = !display_add_;
				break;
		}

		UTJ.VolumetricLine.Instance.begin(0 /* front */);
	    {
			Vector3 head = new Vector3(0f, 0f, 10f);
			Vector3 tail = new Vector3(0f, 10f, 10f);
			UTJ.VolumetricLine.Instance.renderUpdate(0 /* front */,
													 test_line_id_,
													 ref head,
													 ref tail);
		}
	    {
			Vector3 head = hand_transform_a_.TransformPoint(hand_offset);
			Vector3 tail = hand_transform_a_.TransformPoint(hand_offset + new Vector3(0f, 0f, (1f+right_value)*0.5f));
			UTJ.VolumetricLine.Instance.renderUpdate(0 /* front */,
													 hand_a_line_id_,
													 ref head,
													 ref tail);
		}
		UTJ.VolumetricLine.Instance.end();

		Debug.Log("aa");
		UTJ.VolumetricLine.Instance.render(0 /* front */);

		// hand_line_a_.StartPos = hand_offset;
		// hand_line_a_.EndPos = new Vector3(0f, 0f, (1f + right_value)*0.5f);
		// hand_line_b_.StartPos = hand_offset;
		// hand_line_b_.EndPos = new Vector3(0f, 0f, (1f + left_value)*0.5f);

		// var center = new Vector3(0f, 1f, 2f);
		// vector_a_go.transform.position = center;
		// vector_a_go.transform.rotation = hand_transform_a_.rotation;
		// vector_b_go.transform.position = center;
		// vector_b_go.transform.rotation = hand_transform_b_.rotation;

		// vector_add_go.SetActive(display_add_);
		// vector_sub_go.SetActive(display_sub_);
		// vector_cross_go.SetActive(display_cross_);

		// vector_add_go.transform.position = center;
		// vector_add_go.transform.rotation = Quaternion.identity;
		// vector_sub_go.transform.position = center;
		// vector_sub_go.transform.rotation = Quaternion.identity;
		// vector_cross_go.transform.position = center;
		// vector_cross_go.transform.rotation = Quaternion.identity;

		// var endpos_a = new Vector3(0f, 0f, 1f + right_value);
		// var endpos_b = new Vector3(0f, 0f, 1f + left_value);
		// line_a_.StartPos = Vector3.zero;
		// line_a_.EndPos = endpos_a;
		// line_b_.StartPos = Vector3.zero;
		// line_b_.EndPos = endpos_b;
		// line_add_.StartPos = Vector3.zero;
		// line_add_.EndPos = (vector_a_go.transform.TransformVector(endpos_a) +
		// 					vector_b_go.transform.TransformVector(endpos_b));
		// line_sub_.StartPos = Vector3.zero;
		// line_sub_.EndPos = (vector_a_go.transform.TransformVector(endpos_a) -
		// 					vector_b_go.transform.TransformVector(endpos_b));
		// line_cross_.StartPos = Vector3.zero;
		// line_cross_.EndPos = Vector3.Cross(vector_a_go.transform.TransformVector(endpos_a),
		// 								   vector_b_go.transform.TransformVector(endpos_b));
	}
}
