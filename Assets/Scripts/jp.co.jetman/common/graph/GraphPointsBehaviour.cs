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
	[RequireComponent(typeof(RectTransform))]
	public class GraphPointsBehaviour : MonoBehaviour
	{

		[SerializeField]
		private GraphMarkerBehaviour _prefabMarker;

		private List<GraphMarkerBehaviour> pointsCognition = new List<GraphMarkerBehaviour>();
		private List<GraphMarkerBehaviour> pointsOnTarget = new List<GraphMarkerBehaviour>();
		private List<GraphMarkerBehaviour> pointsClicked = new List<GraphMarkerBehaviour>();

		#region Private Methods
		private void draw(Vector2[] _points, int _index)
		{
			foreach (var p in _points)
			{
				var g = Instantiate(_prefabMarker);
				g.transform.parent = transform;
				g.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
				g.SetIconByIndex(_index);
				g.SetPosition(new Vector2(GraphLineBehaviour.CalcX(p.x), GraphLineBehaviour.CalcY(p.y)));

				switch (_index)
				{
					case 0:
						g.name = "Cognition";
						pointsCognition.Add(g);
						break;
					case 1:
						g.name = "OnTarget";
						pointsOnTarget.Add(g);
						break;
					case 2:
						g.name = "Clicked";
						pointsClicked.Add(g);
						break;
				}
			}
		}
		#endregion

		#region Public Methods
		public void DrawCognition(Vector2[] _points)
		{
			foreach (var g in pointsCognition)
			{
				g.Destroy();
			}
			pointsCognition.Clear();
			draw(_points, 0);
		}
		public void DrawOnTarget(Vector2[] _points)
		{
			foreach (var g in pointsOnTarget)
			{
				g.Destroy();
			}
			pointsOnTarget.Clear();
			draw(_points, 1);
		}
		public void DrawClicked(Vector2[] _points)
		{
			foreach (var g in pointsClicked)
			{
				g.Destroy();
			}
			pointsClicked.Clear();
			draw(_points, 2);
		}
		#endregion
		
	}

}
