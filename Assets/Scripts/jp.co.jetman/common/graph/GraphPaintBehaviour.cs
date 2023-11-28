using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.NetworkInformation;

using UnityEngine.UI;
using System.IO;
using System.Text;

namespace jp.co.jetman.common.graph
{
	[RequireComponent(typeof(CanvasRenderer))]
	[RequireComponent(typeof(RectTransform))]
	public class GraphPaintBehaviour : Graphic
	{

		private List<Vector2> _vertices;
		public List<Vector2> vertices
		{
			set
			{
				_vertices = value;

				if (_vertices != null)
				{
					var count = _vertices.Count;
					if (count > 1)
					{
						isDrawable = true;
					}
					else
					{
						isDrawable = false;
					}
				}
			}
		}

		private bool isDrawable = false;


		#region Graphic
		protected override void OnPopulateMesh(VertexHelper vh)
		{
			vh.Clear();

			if (isDrawable)
			{
				var count = _vertices.Count;
				var color = new Color32(255, 255, 255, 255);

				var counter = 0;
				for (var i = 1; i < count; i++)
				{
					var index = counter * 3;
					vh.AddTriangle(index, index + 1, index + 2);

					var v0 = new UIVertex();
					v0.position = new Vector3(_vertices[i].x, _vertices[i].y, 0.0f);
					v0.color = color;
					vh.AddVert(v0);

					var v1 = new UIVertex();
					v1.position = new Vector3(_vertices[i - 1].x, 0.0f, 0.0f);
					v1.color = color;
					vh.AddVert(v1);

					var v2 = new UIVertex();
					v2.position = new Vector3(_vertices[i].x, 0.0f, 0.0f);
					v2.color = color;
					vh.AddVert(v2);

					
					index = (counter + 1) * 3;
					vh.AddTriangle(index, index + 1, index + 2);
					
					v0 = new UIVertex();
					v0.position = new Vector3(_vertices[i - 1].x, _vertices[i - 1].y, 0.0f);
					v0.color = color;
					vh.AddVert(v0);

					v1 = new UIVertex();
					v1.position = new Vector3(_vertices[i - 1].x, 0.0f, 0.0f);
					v1.color = color;
					vh.AddVert(v1);

					v2 = new UIVertex();
					v2.position = new Vector3(_vertices[i].x, _vertices[i].y, 0.0f);
					v2.color = color;
					vh.AddVert(v2);

					counter += 2;
				}
			}
		}
		#endregion

		#region Private Methods
		#endregion

		#region Public Methods
		public void Draw()
		{
			if (isDrawable)
			{
				SetVerticesDirty();
			}
		}
		#endregion
		
	}

}
