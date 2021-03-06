﻿using UnityEngine;
using System.Collections;

namespace UTJ {

public class VolumetricLine
{
	// singleton
	static VolumetricLine instance_;
	public static VolumetricLine Instance { get { return instance_ ?? (instance_ = new VolumetricLine()); } }

	public enum Type
	{
		None,
		NoneA,
		Red,
		RedA,
		Blue,
		BlueA,
		Mazenta,
		MazentaA,
		Green,
		GreenA,
		Yellow,
		YellowA,
		Cyan,
		CyanA,
		White,
		WhiteA,
	}

	const int VOLUMETRICLINE_MAX = 256;

	private bool[] alive_table_;
	private int spawn_index_;

	private Vector2[] uv2_list_;

	private Vector3[][] vertices_;
	private Vector3[][] normals_;
	private Vector2[][] uv2s_;
	private Mesh mesh_;
	private Material material_;
	private MaterialPropertyBlock material_property_block_;

	public Mesh getMesh() { return mesh_; }
	public Material getMaterial() { return material_; }
	public MaterialPropertyBlock getMaterialPropertyBlock() { return material_property_block_; }

	public void init(Material material)
	{
		alive_table_ = new bool[VOLUMETRICLINE_MAX];
		for (var i = 0; i < VOLUMETRICLINE_MAX; ++i) {
			alive_table_[i] = false;
		}
		spawn_index_ = 0;

		uv2_list_ = new Vector2[VOLUMETRICLINE_MAX];

		vertices_ = new Vector3[2][] { new Vector3[VOLUMETRICLINE_MAX*6], new Vector3[VOLUMETRICLINE_MAX*6], };
		normals_ = new Vector3[2][] { new Vector3[VOLUMETRICLINE_MAX*6], new Vector3[VOLUMETRICLINE_MAX*6], };
		uv2s_ = new Vector2[2][] { new Vector2[VOLUMETRICLINE_MAX*6], new Vector2[VOLUMETRICLINE_MAX*6], };
		for (var i = 0; i < VOLUMETRICLINE_MAX; ++i) {
			destroy(i);
		}

		var triangles = new int[VOLUMETRICLINE_MAX * 12];
		for (var i = 0; i < VOLUMETRICLINE_MAX; ++i) {
			triangles[i*12+ 0] = i*6+0;
			triangles[i*12+ 1] = i*6+1;
			triangles[i*12+ 2] = i*6+2;
			triangles[i*12+ 3] = i*6+2;
			triangles[i*12+ 4] = i*6+1;
			triangles[i*12+ 5] = i*6+3;
			triangles[i*12+ 6] = i*6+2;
			triangles[i*12+ 7] = i*6+3;
			triangles[i*12+ 8] = i*6+4;
			triangles[i*12+ 9] = i*6+4;
			triangles[i*12+10] = i*6+3;
			triangles[i*12+11] = i*6+5;
		}

		var uvs = new Vector2[VOLUMETRICLINE_MAX*6];
		for (var i = 0; i < VOLUMETRICLINE_MAX; ++i) {
			uvs[i*6+0] = new Vector2(0f, 0f);
			uvs[i*6+1] = new Vector2(1f, 0f);
			uvs[i*6+2] = new Vector2(0f, 0.5f);
			uvs[i*6+3] = new Vector2(1f, 0.5f);
			uvs[i*6+4] = new Vector2(0f, 0.5f);
			uvs[i*6+5] = new Vector2(1f, 0.5f);
		}

		mesh_ = new Mesh();
		mesh_.MarkDynamic();
		mesh_.name = "VolumetricLine";
		mesh_.vertices = vertices_[0];
		mesh_.normals = normals_[0];
		mesh_.triangles = triangles;
		mesh_.uv = uvs;
		mesh_.uv2 = uv2s_[0];
		mesh_.bounds = new Bounds(CV.Vector3Zero, CV.Vector3One * 99999999);
		material_ = material;
		material_property_block_ = new MaterialPropertyBlock();
#if UNITY_5_3
		material_.SetColor("_Colors0", new Color(0f, 0f, 0f, 0f));
		material_.SetColor("_Colors1", new Color(0f, 0f, 0f, 0f));
		material_.SetColor("_Colors2", new Color(1f, 0f, 0f, 1f));
		material_.SetColor("_Colors3", new Color(1f, 0f, 0f, 1f));
		material_.SetColor("_Colors4", new Color(0f, 0f, 1f, 1f));
		material_.SetColor("_Colors5", new Color(0f, 0f, 1f, 1f));
		material_.SetColor("_Colors6", new Color(1f, 0f, 1f, 1f));
		material_.SetColor("_Colors7", new Color(1f, 0f, 1f, 1f));
		material_.SetColor("_Colors8", new Color(0f, 1f, 0f, 1f));
		material_.SetColor("_Colors9", new Color(0f, 1f, 0f, 1f));
		material_.SetColor("_Colors10", new Color(1f, 1f, 0f, 1f));
		material_.SetColor("_Colors11", new Color(1f, 1f, 0f, 1f));
		material_.SetColor("_Colors12", new Color(0f, 1f, 1f, 1f));
		material_.SetColor("_Colors13", new Color(0f, 1f, 1f, 1f));
		material_.SetColor("_Colors14", new Color(1f, 1f, 1f, 1f));
		material_.SetColor("_Colors15", new Color(1f, 1f, 1f, 1f));
#else
		var col_list = new Vector4[] {
			new Color(0f, 0f, 0f, 0f),
			new Color(0f, 0f, 0f, 0f),
			new Color(1f, 0f, 0f, 1f),
			new Color(1f, 0f, 0f, 1f),
			new Color(0f, 0f, 1f, 1f),
			new Color(0f, 0f, 1f, 1f),
			new Color(1f, 0f, 1f, 1f),
			new Color(1f, 0f, 1f, 1f),
			new Color(0f, 1f, 0f, 1f),
			new Color(0f, 1f, 0f, 1f),
			new Color(1f, 1f, 0f, 1f),
			new Color(1f, 1f, 0f, 1f),
			new Color(0f, 1f, 1f, 1f),
			new Color(0f, 1f, 1f, 1f),
			new Color(1f, 1f, 1f, 1f),
			new Color(1f, 1f, 1f, 1f),
		};
		material_property_block_.SetVectorArray("_Colors", col_list);
#endif
	}

	public void render(int front)
	{
		mesh_.vertices = vertices_[front];
		mesh_.normals = normals_[front];
		mesh_.uv2 = uv2s_[front];
		mesh_.RecalculateBounds(); // test
	}

	public void begin(int front)
	{
		var far = new Vector3(0f, 0f, 0f);
		for (var i = 0; i < VOLUMETRICLINE_MAX*6; ++i) {
			vertices_[front][i] = far;
		}
		var zero = new Vector2(0f, 0f);
		for (var i = 0; i < VOLUMETRICLINE_MAX*6; ++i) {
			uv2s_[front][i] = zero;
		}
	}

	public void end()
	{
	}

	public int spawn(float width, Type type)
	{
		int cnt = 0;
		while (alive_table_[spawn_index_]) {
			++spawn_index_;
			if (spawn_index_ >= VOLUMETRICLINE_MAX) {
				spawn_index_ = 0;
			}
			++cnt;
			if (cnt >= VOLUMETRICLINE_MAX) {
				Debug.LogError("EXCEED VolumetricLine POOL!");
				Debug.Assert(false);
				return -1;
			}
		}
		alive_table_[spawn_index_] = true;
		int id = spawn_index_;
		uv2_list_[id] = new Vector2(width, (float)type);
		return id;
	}

	public void renderUpdate(int front, int id, ref Vector3 head, ref Vector3 tail)
	{
		int idx = id * 6;
		vertices_[front][idx+0] = head;
		vertices_[front][idx+1] = head;
		vertices_[front][idx+2] = head;
		vertices_[front][idx+3] = head;
		vertices_[front][idx+4] = tail;
		vertices_[front][idx+5] = tail;

		var dx = tail.x - head.x;
		var dy = tail.y - head.y;
		var dz = tail.z - head.z;
		var len2 = dx*dx + dy*dy + dz*dz;
		float rlen;
		if (len2 <= 0f) {
			rlen = 1f;
			dx = 0f;
			dy = 1f;
			dz = 0f;
		} else {
			var len = Mathf.Sqrt(len2);
			rlen = 1f/len;
		}
		normals_[front][idx+0].x = dx*rlen;
		normals_[front][idx+0].y = dy*rlen;
		normals_[front][idx+0].z = dz*rlen;
		normals_[front][idx+1].x = dx*rlen;
		normals_[front][idx+1].y = dy*rlen;
		normals_[front][idx+1].z = dz*rlen;
		normals_[front][idx+2].x = dx*rlen;
		normals_[front][idx+2].y = dy*rlen;
		normals_[front][idx+2].z = dz*rlen;
		normals_[front][idx+3].x = dx*rlen;
		normals_[front][idx+3].y = dy*rlen;
		normals_[front][idx+3].z = dz*rlen;
		normals_[front][idx+4].x = dx*rlen;
		normals_[front][idx+4].y = dy*rlen;
		normals_[front][idx+4].z = dz*rlen;
		normals_[front][idx+5].x = dx*rlen;
		normals_[front][idx+5].y = dy*rlen;
		normals_[front][idx+5].z = dz*rlen;
		uv2s_[front][idx+0] = uv2_list_[id];
		uv2s_[front][idx+1] = uv2_list_[id];
		uv2s_[front][idx+2] = uv2_list_[id];
		uv2s_[front][idx+3] = uv2_list_[id];
		uv2s_[front][idx+4] = uv2_list_[id];
		uv2s_[front][idx+4].y += 1f;
		uv2s_[front][idx+5] = uv2_list_[id];
		uv2s_[front][idx+5].y += 1f;
	}
	
	public void destroy(int id)
	{
		alive_table_[id] = false;
	}
}

} // namespace UTJ {
