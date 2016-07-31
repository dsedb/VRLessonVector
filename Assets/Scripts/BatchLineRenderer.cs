using UnityEngine;
using System.Collections;

namespace UTJ {

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class BatchLineRenderer : MonoBehaviour
{
	const int LINE_MAX = 32500;

	public Material material_;
	private MeshFilter mf_;
	private MeshRenderer mr_;
	private int max_;
	private Mesh mesh_;
	private Vector3[] vertices_;
	private Color[] colors_;
	private readonly Vector3 zero = new Vector3(0f, 0f, 0f);
	private readonly Color black = new Color(0f, 0f, 0f, 0f);

	private int idx_;

	private void init(int max)
	{
		Debug.Assert(max <= LINE_MAX);
		max_ = max;
		vertices_ = new Vector3[max_*2];
		for (var i = 0; i < max_*2; ++i) {
			vertices_[i] = zero;
		}
		var indices = new int[max_*2];
		for (var i = 0; i < max_*2; ++i) {
			indices[i] = i;
		}
		colors_ = new Color[max_*2];
		for (var i = 0; i < max_*2; ++i) {
			colors_[i] = black;
		}

		mesh_ = new Mesh();
		mesh_.name = "batch_lines";
		mesh_.vertices = vertices_;
		mesh_.colors = colors_;
		mesh_.bounds = new Bounds(Vector3.zero, Vector3.one * 99999999);
		mesh_.SetIndices(indices, MeshTopology.Lines, 0);
	}
	
	void Awake()
	{
		init(32500 /* max */);
		mf_ = GetComponent<MeshFilter>();
		mr_ = GetComponent<MeshRenderer>();
		mf_.sharedMesh = mesh_;
		mr_.sharedMaterial = material_;
	}

	
	public void begin()
	{
		idx_ = 0;
	}

	public void draw(ref Vector3 p0, ref Vector3 p1,
					 ref Color c0, ref Color c1)
	{
		vertices_[idx_*2+0] = p0;
		vertices_[idx_*2+1] = p1;
		colors_[idx_*2+0] = c0;
		colors_[idx_*2+1] = c1;
		++idx_;
	}

	public void end()
	{
		mesh_.vertices = vertices_;
		mesh_.colors = colors_;
	}
}

} // namespace UTJ {
