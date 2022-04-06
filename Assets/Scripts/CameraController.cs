using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform _target;
    public float _lerpSpeed;

    private Vector3 _tempPosiston;
    [SerializeField] float _minX, _minY, _maxX, _maxY;
    void FixedUpdate()
    {
        if (_target == null)
            return;
        _tempPosiston = _target.position;    
        _tempPosiston.z = -10;

        if (_target.position.x < _minX)
            _tempPosiston.x = _minX;
        if (_target.position.y < _minY)
            _tempPosiston.y = _minY;

        if (_target.position.x > _maxX)
            _tempPosiston.x = _maxX;
        if (_target.position.y > _maxY)
            _tempPosiston.y = _maxY;

        transform.position = Vector3.Lerp(transform.position, _tempPosiston, _lerpSpeed * Time.deltaTime);
    }
}
