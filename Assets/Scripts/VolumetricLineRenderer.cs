using UnityEngine;
using System.Collections;

namespace UTJ {

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class VolumetricLineRenderer : MonoBehaviour {

	public Material material_;
	private MeshFilter mf_;
	private MeshRenderer mr_;

	void Awake()
	{
		VolumetricLine.Instance.init(material_);
	}

	void Start()
	{
		mf_ = GetComponent<MeshFilter>();
		mr_ = GetComponent<MeshRenderer>();
		mf_.sharedMesh = VolumetricLine.Instance.getMesh();
		mr_.sharedMaterial = VolumetricLine.Instance.getMaterial();
		mr_.SetPropertyBlock(VolumetricLine.Instance.getMaterialPropertyBlock());
	}
}

} // namespace UTJ {
