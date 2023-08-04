using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector2 _currentActingForce;
    private Vector2 _velocity;
    LineRenderer _forceLineRenderer;
    [SerializeField] float _forceLineLength;
    [SerializeField] GameObject _explosionPrefab;
    private void Start()
    {
        Settings.Instance.OnClearProjectiles.AddListener(DestroyThis);
        _forceLineRenderer = GetComponent<LineRenderer>();
    }

    public void SetVelocity(Vector2 velocity)
    {
        _velocity = velocity;
    }

    private void FixedUpdate()
    {
        Vector2 newVelocity = ProjectileLauncher.CalculateVelocity(_velocity, transform.position);


        //For the line indicating the force
        _currentActingForce = newVelocity - _velocity;
        UpdateForceLine(_currentActingForce);

        _velocity = newVelocity;
        transform.position += new Vector3(_velocity.x, _velocity.y) * Time.deltaTime;

        if (ProjectileLauncher.CollidingWithEarth(transform.position))
        {
            Destroy(gameObject);
        }
    }

    void DestroyThis() { Destroy(gameObject); }

    /// <summary>
    /// Draws a line to indicate the force acting on the satellite. 
    /// </summary>
    /// <param name="force"></param>
    void UpdateForceLine(Vector2 force)
    {
        if (Settings.Instance.ForceIndicatorsActive)
        {
            _forceLineRenderer.enabled = true;
            Vector3 force3 = new Vector3(force.x, force.y, 0f);
            _forceLineRenderer.SetPosition(0, transform.position);
            _forceLineRenderer.SetPosition(1, transform.position + force3 * _forceLineLength);
        }
        else { _forceLineRenderer.enabled = false; }
    }

    private void OnDestroy()
    {
        var newExplosion = Instantiate(_explosionPrefab);
        newExplosion.transform.position = transform.position;
        Settings.Instance.StartInvokeDestroy(newExplosion, 2f);

        Settings.Instance.OnClearProjectiles.RemoveListener(DestroyThis);
    }


}
