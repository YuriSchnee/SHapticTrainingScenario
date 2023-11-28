using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.NetworkInformation;

using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Linq;

namespace jp.co.jetman.common.graph
{
	[RequireComponent(typeof(CanvasRenderer))]
	[RequireComponent(typeof(RectTransform))]
	public class GraphLineBehaviour : Graphic
	{
		private readonly float LINE_WIDTH = 1.0f;

		private Vector2[] data;
		private List<Vector2> vertPoints = new List<Vector2>();

		static public readonly float GRAPH_MAX_HEIGHT = 195.0f;
		static public readonly float GRAPH_MAX_WIDTH = 560.0f;
		static public float CalcX(float _x)
		{
			return _x * GRAPH_MAX_WIDTH;
		}
		static public float CalcY(float _y)
		{
			return _y * GRAPH_MAX_HEIGHT;
		}


		[SerializeField]
		private GraphPaintBehaviour _paint;

		#region Private Methods
		private void drawLineSegment(VertexHelper _vh, Vector2 _start, Vector2 _end)
		{
			float width = LINE_WIDTH;
			Vector2 perpendicular = (new Vector2(_end.y, _start.x) - new Vector2(_start.y, _end.x)).normalized * width;
			Vector2 v1 = _start - perpendicular;
			Vector2 v2 = _start + perpendicular;
			Vector2 v3 = _end + perpendicular;
			Vector2 v4 = _end - perpendicular;

			_vh.AddVert(v1, color, new Vector2(0, 0));
			_vh.AddVert(v2, color, new Vector2(1, 0));
			_vh.AddVert(v3, color, new Vector2(1, 1));
			_vh.AddVert(v4, color, new Vector2(0, 1));

			int offset = _vh.currentVertCount - 4;
			_vh.AddTriangle(offset + 0, offset + 1, offset + 2);
			_vh.AddTriangle(offset + 2, offset + 3, offset + 0);
		}
		#endregion

		#region Graphic

		protected override void OnPopulateMesh(VertexHelper _vh)
		{
			_vh.Clear();

			if (vertPoints.Count < 2)
				return;

			for (int i = 1; i < vertPoints.Count; i++)
			{
				drawLineSegment(_vh, vertPoints[i - 1], vertPoints[i]);
			}
		}
		#endregion

		#region Public Methods
		public void Draw(Vector2[] _data)
		{
			data = _data;

			vertPoints.Clear();
			for (var i = 0; i < data.Length; i++)
			{
				vertPoints.Add(new Vector2(CalcX(data[i].x), CalcY(data[i].y)));
			}
			SetVerticesDirty();

			// Paint
			if (_paint != null)
			{
				_paint.vertices = vertPoints;
				_paint.Draw();
			}
		}
		#endregion

	}

}
