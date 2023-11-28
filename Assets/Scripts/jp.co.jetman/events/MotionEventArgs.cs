using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace jp.co.jetman.events
{
	public enum MotionEvent
	{
		Begin,
		Complete
	}

	public delegate void MotionEventHandler(object _sender, MotionEventArgs _e);

	public class MotionEventArgs : EventArgs 
	{
		private readonly MotionEvent _type;
		public MotionEvent type 
		{
			get 
			{
				return _type;
			}
		}

		public MotionEventArgs(MotionEvent _type)
		{
			this._type = _type;
		}

	}

}
