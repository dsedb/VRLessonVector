﻿using UnityEngine;
using System.Collections;

public class VectorManager : MonoBehaviour {

	public GameObject hand_a_go;
	public GameObject hand_b_go;
	public GameObject collider_go;

	private Transform hand_transform_a_;
	private Transform hand_transform_b_;
	private int hand_a_line_id_;
	private int hand_b_line_id_;
	private int vector_a_line_id_;
	private int vector_b_line_id_;
	private int vector_add_line_id_;
	private int vector_sub_line_id_;
	private int vector_cross_line_id_;

	private Vector3 hand_offset_ = new Vector3(0f, -0.02f, 0.04f);
	private Vector3 center_ = new Vector3(0f, 1f, 5f);
	private VectorType type_ = VectorType.None;

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
		hand_a_line_id_ = UTJ.VolumetricLine.Instance.spawn(0.05f /* width */, UTJ.VolumetricLine.Type.Red);
		hand_b_line_id_ = UTJ.VolumetricLine.Instance.spawn(0.05f /* width */, UTJ.VolumetricLine.Type.Blue);
		vector_a_line_id_ = UTJ.VolumetricLine.Instance.spawn(0.1f /* width */, UTJ.VolumetricLine.Type.Red);
		vector_b_line_id_ = UTJ.VolumetricLine.Instance.spawn(0.1f /* width */, UTJ.VolumetricLine.Type.Blue);
		vector_add_line_id_ = UTJ.VolumetricLine.Instance.spawn(0.1f /* width */, UTJ.VolumetricLine.Type.Green);
		vector_sub_line_id_ = UTJ.VolumetricLine.Instance.spawn(0.1f /* width */, UTJ.VolumetricLine.Type.Yellow);
		vector_cross_line_id_ = UTJ.VolumetricLine.Instance.spawn(0.1f /* width */, UTJ.VolumetricLine.Type.Cyan);
	}

	void OnDestroy()
	{
		UTJ.VolumetricLine.Instance.destroy(hand_a_line_id_);
		UTJ.VolumetricLine.Instance.destroy(hand_b_line_id_);
		UTJ.VolumetricLine.Instance.destroy(vector_a_line_id_);
		UTJ.VolumetricLine.Instance.destroy(vector_b_line_id_);
		UTJ.VolumetricLine.Instance.destroy(vector_add_line_id_);
		UTJ.VolumetricLine.Instance.destroy(vector_sub_line_id_);
		UTJ.VolumetricLine.Instance.destroy(vector_cross_line_id_);
	}

	public Vector3 getCenter()
	{
		return center_;
	}

	public void setType(VectorType type)
	{
		type_ = type;
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
		
#if false
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
#else
		switch (type_) {
			case VectorType.None:
				display_add_ = false;
				display_sub_ = false;
				display_cross_ = false;
				break;
			case VectorType.Add:
				display_add_ = true;
				display_sub_ = false;
				display_cross_ = false;
				break;
			case VectorType.Sub:
				display_add_ = false;
				display_sub_ = true;
				display_cross_ = false;
				break;
			case VectorType.Cross:
				display_add_ = false;
				display_sub_ = false;
				display_cross_ = true;
				break;
		}
#endif

		UTJ.VolumetricLine.Instance.begin(0 /* front */);

		var right_vector = new Vector3(0f, 0f, (1f+right_value)*0.5f);
		var left_vector = new Vector3(0f, 0f, (1f+left_value)*0.5f);		
	    {
			Vector3 head = hand_transform_a_.TransformPoint(hand_offset_ + right_vector);
			Vector3 tail = hand_transform_a_.TransformPoint(hand_offset_);
			UTJ.VolumetricLine.Instance.renderUpdate(0 /* front */,
													 hand_a_line_id_,
													 ref head,
													 ref tail);
		}
	    {
			Vector3 head = hand_transform_b_.TransformPoint(hand_offset_ + left_vector);
			Vector3 tail = hand_transform_b_.TransformPoint(hand_offset_);
			UTJ.VolumetricLine.Instance.renderUpdate(0 /* front */,
													 hand_b_line_id_,
													 ref head,
													 ref tail);
		}
		var vector_a = hand_transform_a_.TransformVector(right_vector) * 2f;
		var vector_b = hand_transform_b_.TransformVector(left_vector) * 2f;
	    {
			Vector3 head = vector_a + center_;
			Vector3 tail = center_;
			UTJ.VolumetricLine.Instance.renderUpdate(0 /* front */,
													 vector_a_line_id_,
													 ref head,
													 ref tail);
		}
	    {
			Vector3 head = vector_b + center_;
			Vector3 tail = center_;
			UTJ.VolumetricLine.Instance.renderUpdate(0 /* front */,
													 vector_b_line_id_,
													 ref head,
													 ref tail);
		}
	    if (display_add_) {
			Vector3 head = (vector_a + vector_b) + center_;
			Vector3 tail = center_;
			UTJ.VolumetricLine.Instance.renderUpdate(0 /* front */,
													 vector_add_line_id_,
													 ref head,
													 ref tail);
			collider_go.transform.position = head;
		}
	    if (display_sub_) {
			Vector3 head = (vector_a - vector_b) + center_;
			Vector3 tail = center_;
			UTJ.VolumetricLine.Instance.renderUpdate(0 /* front */,
													 vector_sub_line_id_,
													 ref head,
													 ref tail);
			collider_go.transform.position = head;
		}
	    if (display_cross_) {
			Vector3 head = Vector3.Cross(vector_a, vector_b) + center_;
			Vector3 tail = center_;
			UTJ.VolumetricLine.Instance.renderUpdate(0 /* front */,
													 vector_cross_line_id_,
													 ref head,
													 ref tail);
			collider_go.transform.position = head;
		}
		UTJ.VolumetricLine.Instance.end();

		UTJ.VolumetricLine.Instance.render(0 /* front */);
	}
}
