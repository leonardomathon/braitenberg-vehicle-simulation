using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode] public class ModelBuilder : MonoBehaviour {
	public MeshFilter filter;
	public string exportFolder;

	private Mesh mesh = new Mesh(new List<Vector3>(), new List<Face>());

	private void Start() {
		CreateMesh();
	}
	private void Update() {
		if (mesh.faces.Count == 0) {
			CreateMesh();
		}
	}
	private void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		foreach (var face in mesh.faces) {
			Gizmos.DrawLine(mesh.vertices[face.a], mesh.vertices[face.b]);
			Gizmos.DrawLine(mesh.vertices[face.b], mesh.vertices[face.c]);
			Gizmos.DrawLine(mesh.vertices[face.c], mesh.vertices[face.a]);
			// Gizmos.DrawSphere(vertex, 0.01f);
		}
	}

	private void CreateMesh() {
		// Body
		var body = new Mesh(new List<Vector3>(), new List<Face>());
		body.Add(Side(new Vector3(0.3f, -0.2f, 0), 0.5f, 1.7f, 0.7f, 1.1f, 1.2f));
		body.Add(Side(new Vector3(0.3f, -0.2f, 0), 0.5f, 1.7f, 0.7f, 1.1f, 1.2f).MirrorX());
		body.Add(Center(new Vector3(0, -0.2f, 0),0.3f, 1.7f, 1.1f, 1.2f));
		
		// Logos
		var logos = new Mesh(new List<Vector3>(), new List<Face>());
		logos.Add(Logo(new Vector3(0.45f, 1, 0), 0.1f, 0.3f));
		logos.Add(Logo(new Vector3(0.45f, 1, 0), 0.1f, 0.3f).MirrorX());
		
		// Wheel covers
		var covers = new Mesh(new List<Vector3>(), new List<Face>());
		covers.Add(WheelCover(new Vector3(1, 0, 0), 0.2f));
		covers.Add(WheelCover(new Vector3(1, 0, 0), 0.2f).MirrorX());
		
		// Wheels
		var wheels = new Mesh(new List<Vector3>(), new List<Face>());
		wheels.Add(Wheel(new Vector3(1, 0, 0), 0.2f));
		wheels.Add(Wheel(new Vector3(1, 0, 0), 0.2f).MirrorX());
		
		// Sensors
		var sensors = new Mesh(new List<Vector3>(), new List<Face>());
		sensors.Add(SensorCone(new Vector3(0.25f + 0.03646613f, 1.5f + 0.2068096f, 0), 300));
		sensors.Add(SensorCone(new Vector3(0.25f + 0.03646613f, 1.5f + 0.2068096f, 0), 300).MirrorX());
		
		// Sensor poles
		var poles = new Mesh(new List<Vector3>(), new List<Face>());
		poles.Add(SensorPole().Translate(new Vector3(0.25f + 0.03646613f, 1.5f + 0.2068096f, 0)));
		poles.Add(SensorPole().Translate(new Vector3(0.25f + 0.03646613f, 1.5f + 0.2068096f, 0)).MirrorX());

		mesh.Add(body);
		mesh.Add(logos);
		mesh.Add(covers);
		mesh.Add(wheels);
		mesh.Add(sensors);
		mesh.Add(poles);
		SetMesh();
		ExportFile("Vehicle.obj", mesh.ExportObj());
		ExportFile("Body.obj", body.ExportObj());
		ExportFile("Logos.obj", logos.ExportObj());
		ExportFile("WheelCovers.obj", covers.ExportObj());
		ExportFile("WheelChassis.obj", Chassis(0.2f).ExportObj());
		ExportFile("WheelTire.obj", Tire(0.2f).ExportObj());
		ExportFile("SensorPoles.obj", poles.ExportObj());
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
	private List<Face> OctagonCoverSideFaces(List<int> indexes, bool invert = false) {
		var result = new List<Face>();
		foreach (var index in indexes) {
			if (invert) {
				result.Add(new Face(index, index + 16, index + 1));
				result.Add(new Face(index + 1, index + 16, index + 17));
			} else {
				result.Add(new Face(index, index + 1, index + 16));
				result.Add(new Face(index + 1, index + 17, index + 16));
			}
		}
		return result;
	}
	private Mesh WheelCover(Vector3 origin, float width) {
		var vertices = new List<Vector3>();
		var offset = new Vector3(width, 0, 0);
		vertices.AddRange(Octagon(origin + offset, 0.7f));
		vertices.AddRange(Octagon(origin - offset, 0.7f));
		vertices.AddRange(Octagon(origin + offset, 0.6f));
		vertices.AddRange(Octagon(origin - offset, 0.6f));
		vertices.AddRange(Octagon(origin - offset * 3, 0.5f));
		var faces = new List<Face>();
		faces.AddRange(OctagonCoverFaces(new List<int> {0, 1, 2, 3, 4}));
		faces.AddRange(OctagonCoverFaces(new List<int> {16, 17, 18, 19, 20}, true));
		faces.AddRange(OctagonCoverSideFaces(new List<int> {0, 1, 2, 3, 4}));
		faces.AddRange(WheelCoverSlopeFaces(new List<int> {8, 9, 10, 11, 12}));
		faces.Add(new Face(0, 16, 24));
		faces.Add(new Face(0, 24, 8));
		faces.Add(new Face(5, 13, 21));
		faces.Add(new Face(21, 13, 29));

		// Back face, might cause issues with vehicle axle
		vertices.Add(origin - offset);
		faces.AddRange(OctagonFaces(40, new List<int> {24, 25, 26, 27, 28}));
		faces.Add(new Face(24, 40, 29));
		return new Mesh(vertices, faces).Rotate(Quaternion.Euler(-112.5f, 0, 0));
	}
	private List<Face> WheelCoverSlopeFaces(List<int> indexes) {
		var result = new List<Face>();
		foreach (var index in indexes) {
			result.Add(new Face(index, index + 24, index + 1));
			result.Add(new Face(index + 1, index + 24, index + 25));
		}
		return result;
	}

	private Mesh Side(Vector3 origin, float width, float height, float cornerDepth, float innerDepth, float upperDepth) {
		var vertices = new List<Vector3>();
		vertices.Add(origin + new Vector3(width, 0, 0));
		vertices.Add(origin + new Vector3(width, 0, cornerDepth));
		vertices.Add(origin + new Vector3(0, 0, innerDepth));
		vertices.Add(origin + new Vector3(0, height, 0));
		vertices.Add(origin + new Vector3(0, height, upperDepth));

		var faces = new List<Face>();
		faces.Add(new Face(0, 3, 1));
		faces.Add(new Face(1, 3, 4));
		faces.Add(new Face(1, 4, 2));
		faces.Add(new Face(2, 4, 2));

		var mesh = new Mesh(vertices, faces);
		mesh.Add(mesh.MirrorZ());
		return mesh;
	}
	private Mesh Center(Vector3 origin, float width, float height, float bottomDepth, float upperDepth) {
		var vertices = new List<Vector3>();
		vertices.Add(origin + new Vector3(width, 0, bottomDepth));
		vertices.Add(origin + new Vector3(-width, 0, bottomDepth));
		vertices.Add(origin + new Vector3(width, height, upperDepth));
		vertices.Add(origin + new Vector3(-width, height, upperDepth));
		vertices.Add(origin + new Vector3(width, height, 0));
		vertices.Add(origin + new Vector3(-width, height, 0));

		var faces = new List<Face>();
		faces.Add(new Face(0, 2, 1));
		faces.Add(new Face(1, 2, 3));
		faces.Add(new Face(2, 4, 5));
		faces.Add(new Face(2, 5, 3));

		var mesh = new Mesh(vertices, faces);
		mesh.Add(mesh.MirrorZ());
		return mesh;
	}
	private Mesh Logo(Vector3 origin, float width, float radius) {
		var vertices = new List<Vector3>();
		var offset = new Vector3(width, 0, 0);
		vertices.AddRange(Octagon(offset, radius));
		vertices.AddRange(Octagon(-offset, radius));
		var faces = new List<Face>();
		faces.AddRange(OctagonCoverFaces(new List<int> {0, 1}));
		vertices.Add(offset);
		faces.AddRange(OctagonFaces(16, new List<int> {0, 1}));

		var mesh = new Mesh(vertices, faces);

		// mesh.Add(Spoke(origin, 0.15f, 0.4f, 0.05f).Rotate(Quaternion.Euler(22.5f, 0, 0)));
		// mesh.Add(Spoke(origin, 0.15f, 0.4f, 0.05f).Rotate(Quaternion.Euler(67.5f, 0, 0)));
		
		mesh.Add(mesh.MirrorY());
		mesh.Add(mesh.MirrorZ());
		return mesh.Translate(origin);
	}

	private Mesh Wheel(Vector3 origin, float width) {
		var mesh = Chassis(width);
		mesh.Add(Tire(width));

		// mesh.Add(Spoke(origin, 0.15f, 0.4f, 0.05f).Rotate(Quaternion.Euler(22.5f, 0, 0)));
		// mesh.Add(Spoke(origin, 0.15f, 0.4f, 0.05f).Rotate(Quaternion.Euler(67.5f, 0, 0)));
		// mesh.Add(mesh.MirrorY());
		// mesh.Add(mesh.MirrorZ());
		return mesh.Translate(origin);
	}
	private Mesh Chassis(float width) {
		var vertices = new List<Vector3>();
		var offset = new Vector3(width, 0, 0);
		vertices.AddRange(Octagon(offset, 0.1f));
		vertices.AddRange(Octagon(-offset, 0.1f));
		vertices.AddRange(Octagon(offset * 0.75f, 0.4f));
		var faces = new List<Face>();
		faces.AddRange(OctagonCoverFaces(new List<int> {0, 1}));
		
		vertices.Add(offset);
		faces.AddRange(OctagonFaces(24, new List<int> {0, 1}));
		vertices.Add(offset);
		faces.AddRange(OctagonFaces(25, new List<int> {8, 9}, true));
		
		vertices.Add(offset * 0.75f);
		faces.AddRange(OctagonFaces(26, new List<int> {16, 17}));

		var mesh = new Mesh(vertices, faces);

		// mesh.Add(Spoke(origin, 0.15f, 0.4f, 0.05f).Rotate(Quaternion.Euler(22.5f, 0, 0)));
		// mesh.Add(Spoke(origin, 0.15f, 0.4f, 0.05f).Rotate(Quaternion.Euler(67.5f, 0, 0)));
		
		mesh.Add(mesh.MirrorY());
		mesh.Add(mesh.MirrorZ());
		return mesh;
	}
	private Mesh Tire(float width) {
		var vertices = new List<Vector3>();
		var offset = new Vector3(width, 0, 0);
		vertices.AddRange(Octagon(offset, 0.5f));
		vertices.AddRange(Octagon(-offset, 0.5f));
		vertices.AddRange(Octagon(offset, 0.4f));
		vertices.AddRange(Octagon(-offset, 0.4f));
		var faces = new List<Face>();
		faces.AddRange(OctagonCoverFaces(new List<int> {0, 1}));
		faces.AddRange(OctagonCoverFaces(new List<int> {16, 17}, true));
		faces.AddRange(OctagonCoverSideFaces(new List<int> {0, 1}));
		faces.AddRange(OctagonCoverSideFaces(new List<int> {8, 9}, true));

		var mesh = new Mesh(vertices, faces);
		mesh.Add(mesh.MirrorY());
		mesh.Add(mesh.MirrorZ());
		return mesh;
	}
	private Mesh Spoke(Vector3 origin, float width, float length, float depth) {
		var vertices = new List<Vector3>();
		vertices.Add(origin + new Vector3(width, 0, depth));
		vertices.Add(origin + new Vector3(-width, 0, depth));
		vertices.Add(origin + new Vector3(width, 0, -depth));
		vertices.Add(origin + new Vector3(-width, 0, -depth));
		vertices.Add(origin + new Vector3(width, length, depth));
		vertices.Add(origin + new Vector3(-width, length, depth));
		vertices.Add(origin + new Vector3(width, length, -depth));
		vertices.Add(origin + new Vector3(-width, length, -depth));
		var faces = new List<Face>();
		faces.Add(new Face(0, 2, 6));
		faces.Add(new Face(0, 6, 4));
		faces.Add(new Face(1, 0, 4));
		faces.Add(new Face(1, 4, 5));
		faces.Add(new Face(2, 3, 7));
		faces.Add(new Face(2, 7, 6));
		faces.Add(new Face(3, 1, 5));
		faces.Add(new Face(3, 5, 7));
		return new Mesh(vertices, faces);
	}

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
		var divisions =  (int) Math.Ceiling(angle / 45f);
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

	private void ExportFile(string name, string content) {
		var writer = File.CreateText(exportFolder + name);
		writer.Write(content);
		writer.Close();
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

	public string ExportObj() {
		// var exportedVertices = "";
		// var exportedNormals = "";
		// var exportedFaces = "";
		//
		// var count = 0;
		// foreach (var face in faces) {
		// 	var normal = face.Normal(vertices);
		// 	foreach (var vertex in new[] {
		// 		vertices[face.a],
		// 		vertices[face.b],
		// 		vertices[face.c]
		// 	}) {
		// 		exportedVertices += "v " + vertex.x + " " + vertex.y + " " + vertex.z + "\n";
		// 		exportedNormals += "vn " + normal.x + " " + normal.y + " " + normal.z + "\n";
		// 	}
		// 	exportedFaces += "f " + (count * 3 + 1) + " " + (count * 3 + 2) + " " + (count * 3 + 3) + "\n";
		// 	count++;
		// }
		//
		// return exportedVertices + exportedNormals + exportedFaces;
		
		var result = vertices.Aggregate("", (current, vertex) => current + "v " + vertex.x + " " + vertex.y + " " + vertex.z + "\n");
		return faces.Aggregate(result, (current, face) => current + "f " + (face.a+1) + " " + (face.b+1) + " " + (face.c+1) + "\n");
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
	public Vector3 Normal(List<Vector3> vertices) {
		var u = vertices[b] - vertices[1];
		var v = vertices[c] - vertices[1];
		
		return new Vector3(
			u.y * v.z - u.z * v.y,
			u.z * v.x - u.x * v.z,
			u.x * v.y - u.y * v.x
		);
	}
}