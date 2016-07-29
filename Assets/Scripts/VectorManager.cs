using UnityEngine;
using System.Collections;

public class VectorManager : MonoBehaviour {

	public GameObject hand_a_go;
	public GameObject hand_b_go;
	public GameObject vector_a_go;
	public GameObject vector_b_go;
	public GameObject vector_add_go;
	public GameObject vector_sub_go;
	public GameObject vector_cross_go;

	private Transform hand_transform_a_;
	private Transform hand_transform_b_;
	private VolumetricLines.VolumetricLineBehavior hand_line_a_;
	private VolumetricLines.VolumetricLineBehavior hand_line_b_;
	private VolumetricLines.VolumetricLineBehavior line_a_;
	private VolumetricLines.VolumetricLineBehavior line_b_;
	private VolumetricLines.VolumetricLineBehavior line_add_;
	private VolumetricLines.VolumetricLineBehavior line_sub_;
	private VolumetricLines.VolumetricLineBehavior line_cross_;

	private Vector3 hand_offset = new Vector3(0f, -0.02f, 0.04f);

	private Vector3 vector_a_;
	private Vector3 vector_b_;

	void Start()
	{
		hand_transform_a_ = hand_a_go.transform;
		hand_transform_b_ = hand_b_go.transform;
		hand_line_a_ = hand_a_go.GetComponent<VolumetricLines.VolumetricLineBehavior>();
		hand_line_b_ = hand_b_go.GetComponent<VolumetricLines.VolumetricLineBehavior>();
		line_a_ = vector_a_go.GetComponent<VolumetricLines.VolumetricLineBehavior>();
		line_b_ = vector_b_go.GetComponent<VolumetricLines.VolumetricLineBehavior>();
		line_add_ = vector_add_go.GetComponent<VolumetricLines.VolumetricLineBehavior>();
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

	void Update ()
	{
		float right_value = get_value(SteamVR_Controller.DeviceRelation.Rightmost);
		float left_value = get_value(SteamVR_Controller.DeviceRelation.Leftmost);
		
		hand_line_a_.StartPos = hand_offset;
		hand_line_a_.EndPos = new Vector3(0f, 0f, 1f + right_value);
		hand_line_b_.StartPos = hand_offset;
		hand_line_b_.EndPos = new Vector3(0f, 0f, 1f + left_value);

		vector_a_go.transform.position = Vector3.zero;
		vector_a_go.transform.rotation = hand_transform_a_.rotation;
		vector_b_go.transform.position = Vector3.zero;
		vector_b_go.transform.rotation = hand_transform_b_.rotation;

		line_a_.StartPos = vector_a_go.transform.position;
		line_a_.EndPos = new Vector3(0f, 0f, 1f + right_value);
		line_b_.StartPos = vector_b_go.transform.position;
		line_b_.EndPos = new Vector3(0f, 0f, 1f + left_value);
	}
}
