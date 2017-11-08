using UnityEngine;

namespace AISandbox {
    public class TankActor : MonoBehaviour, IActor {
        private const float MAX_SPEED           = 25.0f;
        private const float STEERING_SPEED      = 12.5f;
        private const float NO_INPUT_DECEL      = -75.0f;
        private const float ROT_SPEED           = 5.0f;
        private const float STEERING_LINE_SCALE = 4.0f;

        public Vector2 initialVelocity = Vector2.zero;
        public bool wrapScreen = true;

        [SerializeField]
        private bool _DrawVectors = true;
        public bool DrawVectors {
            get {
                return _DrawVectors;
            }
            set {
                _DrawVectors = value;
                _left_steering_line.gameObject.SetActive( _DrawVectors );
                _right_steering_line.gameObject.SetActive( _DrawVectors );
            }
        }
        public LineRenderer _left_steering_line;
        public LineRenderer _right_steering_line;

        public Renderer _sprite;
        private bool _screenWrapX = false;
        private bool _screenWrapY = false;

        public Vector2 _left_steering = Vector2.zero;
        public Vector2 _right_steering = Vector2.zero;
        private Vector3 _velocity = Vector3.zero;
        public float _orientation = 0.0f;

        private void Start() {
            _velocity = initialVelocity;
            DrawVectors = _DrawVectors;
        }

        public void SetInput( float y_axis, float y2_axis ) {
            _left_steering.y = Mathf.Clamp( y2_axis, -1.0f, 1.0f );
            _right_steering.y = Mathf.Clamp( y_axis, -1.0f, 1.0f );
        }

        public float MaxSpeed {
            get { return MAX_SPEED; }
        }

        public Vector3 Velocity {
            get { return _velocity; }
        }

        private Vector3 ScreenWrap() {
            Vector3 position = transform.position;
            if( wrapScreen )
			{
                if( _sprite.isVisible ) 
				{
                    _screenWrapX = false;
                    _screenWrapY = false;
                  //  return position;
                } 
				
					/*
					Vector3 tanksPos = transform.position;
					if(!_screenWrapX && (tanksPos.x >34f || tanksPos.x < -34f))
					{
						position.x = -position.x;
						_screenWrapX = true;
					}
					if(!_screenWrapY && (tanksPos.y >18f || tanksPos.y < -18f))
					{
						position.y = -position.y;
						_screenWrapY = true;
					}
					*/


					Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
                    if( !_screenWrapX) 
					{
						if(viewportPosition.x >1)
						{
							position.x = Camera.main.ViewportToWorldPoint(Vector2.zero).x;
							_screenWrapX = true;
						}
						else if(viewportPosition.x < 0)
						{
							position.x = Camera.main.ViewportToWorldPoint(Vector2.one).x;
							_screenWrapX = true;
						}
                    }

					if( !_screenWrapY) 
					{
						if(viewportPosition.y >1)
						{
							position.y = Camera.main.ViewportToWorldPoint(Vector2.zero).y;
							_screenWrapY = true;
						}
						else if(viewportPosition.y < 0)
						{
							position.y = Camera.main.ViewportToWorldPoint(Vector2.one).y;
							_screenWrapY = true;
						}
					}
                    

            }
            return position;
        }

		public void ResetOrientation()
		{

		}

        private void Update() {
            Vector3 position = ScreenWrap();

            float speed = _left_steering.y * STEERING_SPEED + _right_steering.y * STEERING_SPEED;
            float rot = (_right_steering.y - _left_steering.y) * ROT_SPEED;

            _orientation += rot;
            transform.rotation = Quaternion.Euler( 0.0f, 0.0f, _orientation );

            _velocity = transform.up * speed;
            if( _left_steering.y == 0.0f && _right_steering.y == 0.0f && _velocity.sqrMagnitude > 0.0f ) {
                _velocity += Vector3.ClampMagnitude( _velocity.normalized * NO_INPUT_DECEL * Time.deltaTime, _velocity.magnitude );
            }
            _velocity = Vector3.ClampMagnitude( _velocity, MAX_SPEED );

            position += _velocity * Time.deltaTime;
            transform.position = position;

            //_left_steering_line.transform.rotation = Quaternion.identity;
            _left_steering_line.SetPosition( 1, _left_steering * STEERING_LINE_SCALE );
            //_right_steering_line.transform.rotation = Quaternion.identity;
            _right_steering_line.SetPosition( 1, _right_steering * STEERING_LINE_SCALE );
        }
    }
}
