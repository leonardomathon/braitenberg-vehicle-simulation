using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Objects.Vehicle {
	[ExecuteInEditMode] 
	public class SensorMesh : MonoBehaviour {
		private const float POLE_BOTTOM_RADIUS = 0.15f;
		private const float POLE_TOP_RADIUS = 0.1f;
		private const float POLE_HEIGHT = 0.35f;
		private const float POLE_CAP = 1.05f;
		private const float POLE_ANGLE = 80;
		private const float CONE_POSITION = 0.6f;
		private const float CONE_LENGTH = 0.3f;
		private const float CONE_DEPTH = 0.06f;
		private const float CONE_INNER_HEIGHT = 0.05f;
		private const float CONE_OUTER_HEIGHT = 0.12f;
		
		public MeshFilter filter;

		private Mesh mesh = new Mesh(new List<Vector3>(), new List<Face>());

		public void SetAngle(float angle) {
			mesh = SensorCone(Vector3.zero, angle).Optimize();
			SetMesh();
		}
		private void SetMesh() {
			var vertices = mesh.vertices;
			var triangles = new List<int>();
			foreach (var face in mesh.faces) {
				triangles.Add(face.a);
				triangles.Add(face.b);
				triangles.Add(face.c);
			}

			var objectMesh = filter.mesh;

			objectMesh.Clear();
			objectMesh.vertices = vertices.ToArray();
			objectMesh.triangles = triangles.ToArray();
			objectMesh.RecalculateNormals();
		}

		private List<Vector3> Octagon(Vector3 origin, float radius) {
			return new List<Vector3> {origin + Vector3.up * radius, origin + Quaternion.Euler(45, 0, 0) * Vector3.up * radius, origin + Quaternion.Euler(90, 0, 0) * Vector3.up * radius, origin + Quaternion.Euler(135, 0, 0) * Vector3.up * radius, origin + Quaternion.Euler(180, 0, 0) * Vector3.up * radius, origin + Quaternion.Euler(225, 0, 0) * Vector3.up * radius, origin + Quaternion.Euler(270, 0, 0) * Vector3.up * radius, origin + Quaternion.Euler(315, 0, 0) * Vector3.up * radius};
		}
		private List<Face> OctagonFaces(int originIndex, List<int> indexes, bool invert = false) {
			return invert 
				? indexes.Select(index => new Face(originIndex, index + 1, index)).ToList() 
				: indexes.Select(index => new Face(originIndex, index, index + 1)).ToList();
		}
		private List<Face> OctagonCoverFaces(List<int> indexes, bool invert = false) {
			var result = new List<Face>();
			foreach (var index in indexes) {
				if (invert) {
					result.Add(new Face(index, index + 1, index + 8));
					result.Add(new Face(index + 1, index + 9, index + 8));
				} else {
					result.Add(new Face(index, index + 8, index + 1));
					result.Add(new Face(index + 1, index + 8, index + 9));
				}
			}
			return result;
		}

		private Mesh Sensor(Vector3 origin) {
			var mesh = SensorPole().Translate(origin);
			mesh.Add(SensorCone(origin, 300));
			return mesh;
		}
		private Mesh SensorPole() {
			var vertices = new List<Vector3>();
			var offset = new Vector3(POLE_HEIGHT, 0, 0);
			vertices.AddRange(Octagon(offset, POLE_TOP_RADIUS));
			vertices.AddRange(Octagon(-offset, POLE_BOTTOM_RADIUS));
			var faces = new List<Face>();
			faces.AddRange(OctagonCoverFaces(new List<int> {0, 1}));
			
			vertices.Add(offset * POLE_CAP);
			faces.AddRange(OctagonFaces(16, new List<int> {0, 1}));

			var mesh = new Mesh(vertices, faces);
			
			mesh.Add(mesh.MirrorY());
			mesh.Add(mesh.MirrorZ());
			return mesh.Rotate(Quaternion.Euler(0, 0, POLE_ANGLE)).Translate(Quaternion.Euler(0, 0, POLE_ANGLE) * new Vector3(-POLE_HEIGHT * CONE_POSITION, 0, 0));
		}
		private Mesh SensorCone(Vector3 origin, float angle) {
			var divisions =  (int) Math.Max(Math.Ceiling(angle / 45f), 1);
			var segmentAngle = angle / divisions;
			
			var mesh = new Mesh(new List<Vector3>(), new List<Face>());
			for (var i = 0; i < divisions; i++) {
				mesh.Add(SensorConeSegment(segmentAngle).Rotate(Quaternion.Euler(0, i * segmentAngle, 0)));
			}
			mesh.faces.Add(new Face(0, 5, 4));
			mesh.faces.Add(new Face(1, 5, 0));
			mesh.faces.Add(new Face((divisions - 1) * 7, (divisions - 1) * 7 + 2, (divisions - 1) * 7 + 3));
			mesh.faces.Add(new Face((divisions - 1) * 7 + 1, (divisions - 1) * 7, (divisions - 1) * 7 + 3));
			return mesh.Rotate(Quaternion.Euler(0, angle / -2, 0)).Translate(origin);
		}
		private Mesh SensorConeSegment(float width) {
			var vertices = new List<Vector3>();
			vertices.Add(new Vector3(0, CONE_INNER_HEIGHT, 0));
			vertices.Add(new Vector3(0, -CONE_INNER_HEIGHT, 0));
			vertices.Add(Quaternion.Euler(0, width, 0) * new Vector3(CONE_LENGTH, CONE_OUTER_HEIGHT, 0));
			vertices.Add(Quaternion.Euler(0, width, 0) * new Vector3(CONE_LENGTH, -CONE_OUTER_HEIGHT, 0));
			vertices.Add(new Vector3(CONE_LENGTH, CONE_OUTER_HEIGHT, 0));
			vertices.Add(new Vector3(CONE_LENGTH, -CONE_OUTER_HEIGHT, 0));
			vertices.Add(Quaternion.Euler(0, width / 2, 0) * new Vector3(CONE_LENGTH - CONE_DEPTH, 0, 0));
			// vertices.Add(Quaternion.Euler(0, width / 2, 0) * new Vector3(length, outerHeight, 0));
			// vertices.Add(Quaternion.Euler(0, width / 2, 0) * new Vector3(length, -outerHeight, 0));
			// vertices.Add(Quaternion.Euler(0, width / -2, 0) * new Vector3(length, outerHeight, 0));
			// vertices.Add(Quaternion.Euler(0, width / -2, 0) * new Vector3(length, -outerHeight, 0));

			var faces = new List<Face>();
			faces.Add(new Face(0, 4, 2));
			faces.Add(new Face(1, 3, 5));
			// faces.Add(new Face(2, 5, 3));
			// faces.Add(new Face(2, 4, 5));
			faces.Add(new Face(6, 2, 4));
			faces.Add(new Face(6, 3, 2));
			faces.Add(new Face(6, 4, 5));
			faces.Add(new Face(6, 5, 3));
			
			return new Mesh(vertices, faces);
		}
	}

	class Mesh {
		public List<Vector3> vertices;
		public List<Face> faces;

		public Mesh(List<Vector3> vertices, List<Face> faces) {
			this.vertices = vertices;
			this.faces = faces;
		}

		public void Add(Mesh other) {
			var vertexCount = vertices.Count;
			vertices.AddRange(other.vertices);
			foreach (var face in other.faces) {
				faces.Add(face.Offset(vertexCount));
			}
		}
		public Mesh Translate(Vector3 translation) {
			return new Mesh(vertices.Select(vertex => translation + vertex).ToList(), faces.ToList());
		}
		public Mesh Rotate(Quaternion rotation) {
			return new Mesh(vertices.Select(vertex => rotation * vertex).ToList(), faces.ToList());
		}
		public Mesh MirrorX() {
			return new Mesh(vertices.Select(vertex => new Vector3(-vertex.x, vertex.y, vertex.z)).ToList(), faces.ToList()).Invert();
		}
		public Mesh MirrorY() {
			return new Mesh(vertices.Select(vertex => new Vector3(vertex.x, -vertex.y, vertex.z)).ToList(), faces.ToList()).Invert();
		}
		public Mesh MirrorZ() {
			return new Mesh(vertices.Select(vertex => new Vector3(vertex.x, vertex.y, -vertex.z)).ToList(), faces.ToList()).Invert();
		}
		public Mesh Invert() {
			return new Mesh(vertices.ToList(), faces.Select(face => new Face(face.a, face.c, face.b)).ToList());
		}
		public Mesh Optimize() {
			var resultVertices = new List<Vector3>();
			var resultFaces = new List<Face>();

			var i = 0;
			foreach (var face in faces) {
				resultVertices.Add(vertices[face.a]);
				resultVertices.Add(vertices[face.b]);
				resultVertices.Add(vertices[face.c]);
				resultFaces.Add(new Face(i * 3,  i* 3 + 1, i * 3 + 2));
				i++;
			}
			return new Mesh(resultVertices, resultFaces);
		}
	}

	class Face {
		public int a;
		public int b;
		public int c;

		public Face(int a, int b, int c) {
			this.a = a;
			this.b = b;
			this.c = c;
		}

		public Face Offset(int offset) {
			return new Face(offset + a, offset + b, offset + c);
		}
	}
}