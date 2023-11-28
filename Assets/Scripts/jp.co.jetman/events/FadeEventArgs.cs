using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace jp.co.jetman.events
{
	public enum FadeEvent
	{
		Begin,
		Complete
	}

	public delegate void FadeEventHandler(object _sender, FadeEventArgs _e);

	public class FadeEventArgs : EventArgs 
	{
		private readonly FadeEvent _type;
		public FadeEvent type 
		{
			get 
			{
				return _type;
			}
		}

		public FadeEventArgs(FadeEvent _type)
		{
			this._type = _type;
		}

	}

}
