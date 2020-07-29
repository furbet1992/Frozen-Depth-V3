﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlueIce : MonoBehaviour
{
	Mesh iceMesh;

	int[,] map;
	float scale = 1;
	float iceThreshold = 1;

	public int width = 20;
	public int height = 20;
	float depth = 5.0f;

	SquareGrid squareGrid;
	List<Vector3> vertices = new List<Vector3>();
	List<int> triangles = new List<int>();
	Dictionary<int, List<Triangle>> vertexTriangles = new Dictionary<int, List<Triangle>>();
	List<List<int>> outlines = new List<List<int>>();
	List<int> vertexClosedList = new List<int>();

	private void Start()
	{
		map = new int[width, height];

		for (int y = 0; y < height; ++y)
		{
			for (int x = 0; x < width; ++x)
			{
				map[x, y] = 100;
			}
		}

		squareGrid = new SquareGrid(map, scale, iceThreshold, transform);
		GenerateMesh();
		
	}

	public void GenerateMesh()
	{
		outlines.Clear();
		vertexClosedList.Clear();
		vertexTriangles.Clear();
		vertices.Clear();
		triangles.Clear();
		iceMesh = new Mesh();

		for (int y = 0; y < squareGrid.squares.GetLength(1); ++y)
		{
			for (int x = 0; x < squareGrid.squares.GetLength(0); ++x)
			{
				TriangulateSquare(squareGrid.squares[x, y]);
			}
		}

		iceMesh = new Mesh();
		iceMesh.vertices = vertices.ToArray();
		iceMesh.triangles = triangles.ToArray();
		iceMesh.RecalculateNormals();

		//AddSideFaces();
		//AddBackFaces();
		GetComponent<MeshFilter>().mesh = iceMesh;
	}

	void TriangulateSquare(Square a_square)
	{
		switch (a_square.configuration)
		{
		// 0 walls
			case 0:
				break;
		// 1 wall
			case 1:
				MeshFromPoints(a_square.bottomLeft, a_square.left, a_square.bottom);
				break;
			case 2:
				MeshFromPoints(a_square.bottomRight, a_square.bottom, a_square.right);
				break;
			case 4:
				MeshFromPoints(a_square.topRight, a_square.right, a_square.top);
				break;
			case 8:
				MeshFromPoints(a_square.topLeft, a_square.top, a_square.left);
				break;
		// 2 walls
			case 3:
				MeshFromPoints(a_square.bottomLeft, a_square.left, a_square.right, a_square.bottomRight);
				break;
			case 5:
				MeshFromPoints(a_square.bottomLeft, a_square.left, a_square.top, a_square.topRight, a_square.right, a_square.bottom);
				break;
			case 6:
				MeshFromPoints(a_square.bottomRight, a_square.bottom, a_square.top, a_square.topRight);
				break;
			case 9:
				MeshFromPoints(a_square.bottomLeft, a_square.topLeft, a_square.top, a_square.bottom);
				break;
			case 10:
				MeshFromPoints(a_square.bottomRight, a_square.bottom, a_square.left, a_square.topLeft, a_square.top, a_square.right);
				break;
			case 12:
				MeshFromPoints(a_square.topRight, a_square.right, a_square.left, a_square.topLeft);
				break;
		// 3 walls
			case 7:
				MeshFromPoints(a_square.bottomLeft, a_square.left, a_square.top, a_square.topRight, a_square.bottomRight);
				break;
			case 11:
				MeshFromPoints(a_square.bottomLeft, a_square.topLeft, a_square.top, a_square.right, a_square.bottomRight);
				break;
			case 13:
				MeshFromPoints(a_square.bottomLeft, a_square.topLeft, a_square.topRight, a_square.right, a_square.bottom);
				break;
			case 14:
				MeshFromPoints(a_square.bottomRight, a_square.bottom, a_square.left, a_square.topLeft, a_square.topRight);
				break;
		// 4 walls
			case 15:
				MeshFromPoints(a_square.bottomLeft, a_square.topLeft, a_square.topRight, a_square.bottomRight);
				vertexClosedList.Add(a_square.bottomLeft.vertexIndex);
				vertexClosedList.Add(a_square.topLeft.vertexIndex);
				vertexClosedList.Add(a_square.topRight.vertexIndex);
				vertexClosedList.Add(a_square.bottomRight.vertexIndex);
				break;
		}
	}

	void MeshFromPoints(params Node[] a_points)
	{
		AssignVertices(a_points);

		if (a_points.Length >= 3)
		{
			CreateTriangle(a_points[0], a_points[1], a_points[2]);
		}

		if (a_points.Length >= 4)
		{
			CreateTriangle(a_points[0], a_points[2], a_points[3]);
		}

		if (a_points.Length >= 5)
		{
			CreateTriangle(a_points[0], a_points[3], a_points[4]);
		}

		if (a_points.Length >= 6)
		{
			CreateTriangle(a_points[0], a_points[4], a_points[5]);
		}
	}

	void AssignVertices(Node[] a_points)
	{
		for (int i = 0; i < a_points.Length; ++i)
		{
			if (a_points[i].vertexIndex == -1)
			{
				a_points[i].vertexIndex = vertices.Count;
				vertices.Add(a_points[i].position);
			}
		}
	}

	void CreateTriangle(Node a_pointA, Node a_pointB, Node a_pointC)
	{
		triangles.Add(a_pointA.vertexIndex);
		triangles.Add(a_pointB.vertexIndex);
		triangles.Add(a_pointC.vertexIndex);

		Triangle triangle = new Triangle(a_pointA.vertexIndex, a_pointB.vertexIndex, a_pointC.vertexIndex);

		AddTriangleToDictionary(triangle.vertexA, triangle);
		AddTriangleToDictionary(triangle.vertexB, triangle);
		AddTriangleToDictionary(triangle.vertexC, triangle);
	}

	public class SquareGrid
	{
		public Square[,] squares;

		public SquareGrid(int[,] a_map, float a_scale, float a_iceThreshold, Transform a_origin)
		{
			int nodeCountX = a_map.GetLength(0);
			int nodeCountY = a_map.GetLength(1);
			float mapWidth = nodeCountX * a_scale;
			float mapHeight = nodeCountY * a_scale;

			ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];
			for (int y = 0; y < nodeCountY; ++y)
			{
				for (int x = 0; x < nodeCountX; ++x)
				{
					Vector3 position = new Vector3(-mapWidth * 0.5f + x * a_scale + a_scale * 0.5f, -mapHeight * 0.5f + y * a_scale + a_scale * 0.5f, 0.0f);
					controlNodes[x, y] = new ControlNode(position, a_map[x, y], a_scale);
				}
			}

			squares = new Square[nodeCountX - 1, nodeCountY - 1];
			for (int y = 0; y < nodeCountY - 1; ++y)
			{
				for (int x = 0; x < nodeCountX - 1 ; ++x)
				{
					squares[x, y] = new Square(a_scale, a_iceThreshold, controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y], a_origin);
				}
			}
		}
	}

	public class Square
	{
		public ControlNode topLeft;
		public ControlNode topRight;
		public ControlNode bottomRight;
		public ControlNode bottomLeft;

		public Node top;
		public Node right;
		public Node bottom;
		public Node left;

		public int configuration;

		public Square(float a_scale, float a_iceThreshold, ControlNode a_topLeft, ControlNode a_topRight, ControlNode a_bottomRight, ControlNode a_bottomLeft, Transform a_origin)
		{
			topLeft = a_topLeft;
			topRight = a_topRight;
			bottomRight = a_bottomRight;
			bottomLeft = a_bottomLeft;

			top = topLeft.rightNode;
			right = bottomRight.upNode;
			bottom = bottomLeft.rightNode;
			left = bottomLeft.upNode;

			InterpolateMidPointNode(top, topLeft, topRight, a_origin.right, a_iceThreshold, a_scale);
			InterpolateMidPointNode(right, bottomRight, topRight, a_origin.up, a_iceThreshold, a_scale);
			InterpolateMidPointNode(bottom, bottomLeft, bottomRight, a_origin.right, a_iceThreshold, a_scale);
			InterpolateMidPointNode(left, bottomLeft, topLeft, a_origin.up, a_iceThreshold, a_scale);

			if (topLeft.iceValue >= a_iceThreshold)
			{
				configuration += 8;
			}

			if (topRight.iceValue >= a_iceThreshold)
			{
				configuration += 4;
			}

			if (bottomRight.iceValue >= a_iceThreshold)
			{
				configuration += 2;
			}

			if (bottomLeft.iceValue >= a_iceThreshold)
			{
				configuration += 1;
			}

		}

		void InterpolateMidPointNode(Node a_midPoint, ControlNode a_parentNode, ControlNode a_adjacentNode, Vector3 a_direction, float a_iceThreshold, float a_scale)
		{
			if (!a_midPoint.interpolated &&
				(a_adjacentNode.iceValue >= a_iceThreshold) != (a_parentNode.iceValue >= a_iceThreshold) && 
				a_adjacentNode.iceValue - a_parentNode.iceValue != 0.0f)
			{
				a_midPoint.position = a_parentNode.position + a_direction.normalized * ((a_iceThreshold - (float)a_parentNode.iceValue) / ((float)a_adjacentNode.iceValue - (float)a_parentNode.iceValue)) * a_scale;
				a_midPoint.interpolated = true;
			}
		}
	}

	public class Node
	{
		public Vector3 position;
		public int vertexIndex = -1;
		public bool interpolated = false;

		public Node (Vector3 a_position)
		{
			position = a_position;
		}
	}

	public class ControlNode : Node
	{
		public int iceValue;
		public Node upNode;
		public Node rightNode;

		public ControlNode(Vector3 a_position, int a_iceValue, float a_scale) : base(a_position)
		{
			iceValue = a_iceValue;
			upNode = new Node(position + Vector3.up * a_scale * 0.5f);
			rightNode = new Node(position + Vector3.right * a_scale * 0.5f);
		}
	}

	struct Triangle
	{
		public int vertexA;
		public int vertexB;
		public int vertexC;
		int[] vertices;

		public Triangle(int a_vertexA, int a_vertexB, int a_vertexC)
		{
			vertexA = a_vertexA;
			vertexB = a_vertexB;
			vertexC = a_vertexC;

			vertices = new int[3];
			vertices[0] = vertexA;
			vertices[1] = vertexB;
			vertices[2] = vertexC;
		}

		public int this[int i]
		{
			get
			{
				return vertices[i];
			}
		}

		public bool Contains(int a_vertexIndex)
		{
			return (a_vertexIndex == vertexA || a_vertexIndex == vertexB || a_vertexIndex == vertexC);
		}
	}

	void AddTriangleToDictionary(int a_vertexIndex, Triangle a_triangle)
	{
		if (vertexTriangles.ContainsKey(a_vertexIndex))
		{
			vertexTriangles[a_vertexIndex].Add(a_triangle);
		}
		else
		{
			List<Triangle> triangles = new List<Triangle>();
			triangles.Add(a_triangle);
			vertexTriangles.Add(a_vertexIndex, triangles);
		}
	}
}
