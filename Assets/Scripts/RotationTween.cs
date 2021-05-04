/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;

namespace FFStudio
{
    public class RotationTween : MonoBehaviour
    {
#region Fields
        public enum RotationMode { Local, World }

        [ Range( 0, 360 ), Label( "Target Angle (°)" )]
        public float targetAngle;
        [ Label( "Angular Speed (°/s)" ) ]
        public float angularSpeedInDegrees;
        
        public bool playOnStart;

        [ DisableIf( "IsPlaying" ) ]
        public bool loop;

        [ ShowIf( "loop" ) ]
        public LoopType loopType = LoopType.Restart;

        public RotationMode rotationMode;

        [ Dropdown( "GetVectorValues" ), Label( "Rotate Around" ) ]
        public Vector3 rotationVector;

        public Ease easing = Ease.Linear;

        public GameEvent[] fireTheseOnComplete;
        
        [ field: SerializeField, ReadOnly ]
        public bool IsPlaying { get; private set; }
        
/* Private Fields */

        private Tween tween;
        private float Duration => 360 / angularSpeedInDegrees;

        private DropdownList< Vector3 > GetVectorValues()
        {
            return new DropdownList< Vector3 >()
            {
                { "X",   Vector3.right      },
                { "Y",   Vector3.up         },
                { "Z",   Vector3.forward    }
            };
        }
#endregion

#region Unity API
        private void Start()
        {
            if( !enabled )
                return;

            if( playOnStart )
                Play();
        }
        
        private void OnDestroy()
        {
            KillTween();
        }
#endregion

#region API
        [ Button() ]
        public void Play()
        {
            if( tween == null )
                CreateAndStartTween();
            else
                tween.Play();

            IsPlaying = true;
        }
        
        [ Button(), EnableIf( "IsPlaying" ) ]
        public void Pause()
        {
            if( tween == null )
                return;

            tween.Pause();

            IsPlaying = false;
        }
        
        [ Button(), EnableIf( "IsPlaying" ) ]
        public void Stop()
        {
            if( tween == null )
                return;
                
            tween.Rewind();

            IsPlaying = false;
        }
        
        [ Button(), EnableIf( "IsPlaying" ) ]
        public void Restart()
        {
            if( tween == null )
                Play();
            else
            {
                tween.Restart();

                IsPlaying = true;
            }
        }
#endregion

#region Implementation
        private void CreateAndStartTween()
        {            
            if( rotationMode == RotationMode.Local )
                tween = transform.DOLocalRotate( rotationVector * targetAngle, Duration, RotateMode.FastBeyond360 );
            else
                tween = transform.DORotate( rotationVector * targetAngle, Duration, RotateMode.FastBeyond360 );
                
            tween.SetRelative()
                .SetEase( easing )
                .SetLoops( loop ? -1 : 0, loopType )
                .OnComplete( () => IsPlaying = false )
                .OnComplete( KillTween );
                
            for( var i = 0; i < fireTheseOnComplete.Length; i++ )
                tween.OnComplete( fireTheseOnComplete[ i ].Raise );
        }

        private void KillTween()
        {
            IsPlaying = false;
            
            tween.Kill();
            tween = null;
        }
#endregion
    }
}
