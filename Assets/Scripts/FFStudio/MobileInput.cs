using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

namespace FFStudio
{
    public class MobileInput : MonoBehaviour
    {
		[Header( "Fired Events" )]
		public SwipeInputEvent swipeInputEvent;
		public IntGameEvent tapInputEvent;
		public GameEvent fingerUpdate;
		int swipeThreshold;
		private void Awake()
		{
			swipeThreshold = Screen.width * GameSettings.Instance.swipeThreshold / 100;
		}
		public void Swiped( Vector2 delta )
		{
			swipeInputEvent.ReceiveInput( delta );
		}
		public void Tapped( int count )
		{
			tapInputEvent.eventValue = count;

			tapInputEvent.Raise();
		}

		public void FingerUpdate()
		{
			fingerUpdate.Raise();
		}
    }
}