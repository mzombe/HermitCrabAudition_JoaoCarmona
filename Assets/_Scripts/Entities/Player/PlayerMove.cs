using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Player
{
    [SerializeField] private FloatingJoystick _joyStick;
    private float _speed{get{
        return _player.m_Settings.Speed;
    }}
    private float jumpPower{get{
        return _player.m_Settings.JumpPower;
    }}

    [Header ("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float size;
    [Range(0f, 0.5f)]
    [SerializeField] private float distanceBetween;
    [SerializeField] private Vector3 position;
    private Vector2 moveVector;
    private Vector3 dir;
    private float _ySpeed;
    private Vector3 impact = Vector3.zero;
    private bool _isRight;
    //public float Yspeed { get {return _ySpeed;} set {value = _ySpeed;}}
    public bool IsRight { get { return _isRight; }}
    public PlayerMove (Settings settings) : base(settings)
    {

    }

    private void Start() {
        TurnRight();
    }

    private void Update(){
        _player.PlayerAnimator.SetBool("isGrounded", GroundCheck());

        if(GroundCheck()){
            
        }else{
            
        }

        if(!_joyStick.Pressed)
            _player.PlayerAnimator.SetBool("isRun", false);

        Gravity();
        WalkSide();
    }

    private void Gravity(){
        _ySpeed += !GroundCheck() ? Physics.gravity.y * Time.deltaTime : 0;

        if(GroundCheck()){
            if(_joyStick.Vertical < -0.95f){
                _ySpeed += Physics.gravity.y * Time.deltaTime;
            }else{
                _ySpeed = 0;
            }

            //Jump
            if(_joyStick.Vertical > 0.95f){
                _player.PlayerAnimator.SetTrigger("Jump");
                _ySpeed = jumpPower;
            }

        }

        if (impact.magnitude > 0.2){
            _player.PlayerController.Move(impact * Time.deltaTime);
            impact = Vector3.Lerp(impact, Vector3.zero, -Physics.gravity.y * Time.deltaTime);
        }else{
            impact = Vector3.zero;
            _player.PlayerController.Move(new Vector3(0,_ySpeed,0) * Time.deltaTime);
        }        
    }

    private void WalkSide(){
        if(!_joyStick.Pressed){
            _player.PlayerAnimator.speed = 1;
            return;
        }

        _player.PlayerController.isTrigger = false;
        moveVector = (Vector3.up * _joyStick.Horizontal + Vector3.left * _joyStick.Vertical);

        if (_joyStick.Horizontal != 0 || _joyStick.Vertical != 0){
            if(moveVector.y > 0) dir = Vector3.right;
            if(moveVector.y < 0) dir = Vector3.left;

            if(moveVector.y > 0) TurnRight();
            if(moveVector.y < 0) TurnLeft();
            
        }else{
            dir = Vector3.zero;
        }

        _player.PlayerController.Move(new Vector3(dir.x * (_speed * moveVector.magnitude), 0f, 0f)* Time.deltaTime );

        int i = moveVector != Vector2.zero ? 1:0; 
        if(i > 0){
            _player.PlayerAnimator.SetBool("isRun", true);
            _player.PlayerAnimator.speed = moveVector.magnitude;
        }else{
            _player.PlayerAnimator.SetBool("isRun", false);
            _player.PlayerAnimator.speed = 1;
        }
    }

    private void TurnRight(){
        _isRight = true;
        if(GameManager.Instance.GetCurrentStyle() == GameStyle.STYLE3D)
            LeanTween.rotateY(_player.Visual, 0, 0.2f);
        else if (GameManager.Instance.GetCurrentStyle() == GameStyle.STYLE2D)
            _player.Visual.transform.rotation = Quaternion.Euler(0, 0f, 0);
    }
    private void TurnLeft(){
        _isRight = false;
        if(GameManager.Instance.GetCurrentStyle() == GameStyle.STYLE3D)
            LeanTween.rotateY(_player.Visual, -180, 0.2f);
        else if (GameManager.Instance.GetCurrentStyle() == GameStyle.STYLE2D)
            _player.Visual.transform.rotation = Quaternion.Euler(0, -180f, 0);
    }

    private bool GroundCheck(){
        if(Physics.Raycast(new Vector3(transform.position.x + position.x, transform.position.y + position.y, transform.position.z), -transform.up, size, groundLayer) ||
            Physics.Raycast(new Vector3(transform.position.x + position.x, transform.position.y + position.y, transform.position.z) + new Vector3(distanceBetween,0,0), -transform.up, size, groundLayer) ||
            Physics.Raycast(new Vector3(transform.position.x + position.x, transform.position.y + position.y, transform.position.z) + new Vector3(-distanceBetween,0,0), -transform.up, size, groundLayer) ){
            return true;
        }else{
            return false;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector3(transform.position.x + position.x, transform.position.y + position.y, transform.position.z) + new Vector3(distanceBetween,0,0), - transform.up * size);
        Gizmos.DrawRay(new Vector3(transform.position.x + position.x, transform.position.y + position.y, transform.position.z), - transform.up * size);
        Gizmos.DrawRay(new Vector3( transform.position.x + position.x, transform.position.y + position.y, transform.position.z) + new Vector3(-distanceBetween,0,0), - transform.up * size);
    }

    public void AddForceController(Vector3 dir, float force){
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; 
        impact += dir.normalized * force;
    }

    public bool PlayerIdle(){
        return _joyStick.Vertical == 0 && _joyStick.Horizontal == 0 && !_joyStick.Pressed;
    }

    public bool JoystickUP(){
        return _joyStick.Vertical > 0;
    }
}
