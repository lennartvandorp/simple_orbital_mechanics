using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    [SerializeField] GameObject _projectilePrefab;
    GameObject _projectile;

    public static float G = 10f;
    public static float M { get { return Settings.Instance.EarthMass; } }



    [SerializeField] int _estimatedPoints = 200;
    [SerializeField] int _lineStepSize = 10;
    private LineRenderer _lineRenderer;

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //Spawn and launch the projectile
            _projectile = Instantiate(_projectilePrefab);
            _projectile.transform.position = transform.position;

            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _projectile.GetComponent<Projectile>().SetVelocity(worldPosition - transform.position);
        }
    }

    private void FixedUpdate()
    {
        CreateEstimationLine();
    }


    /// <summary>
    /// Calculates the path of the projectile and creates a line to show that estimation
    /// </summary>
    public void CreateEstimationLine()
    {
        Vector3[] points = new Vector3[_estimatedPoints];

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 currentVel = worldPosition - transform.position;
        Vector2 currentPos = transform.position;
        bool collidedWithEarth = false;

        for (int i = 0; i < points.Length && !collidedWithEarth; i++)
        {
            collidedWithEarth = CollidingWithEarth(currentPos);

            points[i] = currentPos;

            for (int j = 0; j < _lineStepSize; j++)
            {
                currentVel = CalculateVelocity(currentVel, currentPos);
                currentPos = currentPos + currentVel * Time.fixedDeltaTime;
            }
        }

        _lineRenderer.SetPositions(points);
    }


    /// <summary>
    /// Calculates the new velocity of a projectile influenced by gravity
    /// </summary>
    /// <param name="currentVelocity"></param>
    /// <param name="currentPosition"></param>
    /// <returns></returns>
    public static Vector2 CalculateVelocity(Vector2 currentVelocity, Vector2 currentPosition)
    {
        Vector3 earthPos = Settings.Instance.Earth.transform.position;
        Vector2 earthPos2D = new Vector2(earthPos.x, earthPos.y);

        Vector2 gravityDirection = earthPos2D - currentPosition;
        float R = gravityDirection.magnitude;


        // The calculation of gravity g = G*M/R^2
        // Where g = acceleration due to gravity
        // G = gravitational constant. In the real world this is 6.6743 × 10-11 m3 kg-1 s-2 But these numbers are not practical to work with
        // M = mass of the object of which gravity is measured
        // R = distance of the center of the mass to the object that is being accelerated
        float g = (G * M) / (R * R);

        Vector2 newVelocity = currentVelocity + gravityDirection.normalized * g * Time.fixedDeltaTime;
        return newVelocity;
    }

    public static bool CollidingWithEarth(Vector3 point)
    {
        Transform earthTrans = Settings.Instance.Earth.transform;
        float dist = Vector2.Distance(point, Settings.Instance.Earth.transform.position);
        return dist < 1f;
    }
}
