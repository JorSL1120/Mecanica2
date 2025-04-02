using TMPro;
using UnityEngine;

public class KeplerProblemRK4 : MonoBehaviour
{
    public Transform planet;
    public float G;
    [Header("Establece las condiciones iniciales:")]
    public Vector3 P0;
    public Vector3 V0;
    [Header("Constantes de Movimiento")]
    public float energy;
    public float eccentricity;
    public float majorSemiAxis;
    public float minorSemiAxis;

    public bool isActive = false;

    [SerializeField] private float time;
    private Vector3 Pf, Vf, angularMomentum, laplaceRungeLenz;

    void FixedUpdate()
    {
        if (!isActive)
        {
            planet.localPosition = P0;
            Debug.DrawRay(planet.position, V0, Color.red);
            GetAndShowOrbitParameters();
            planet.GetComponent<TrailRenderer>().Clear();
        }

        if (isActive)
        {
            float dt = Time.fixedDeltaTime;
            time -= dt;

            Vector3 k1 = V0;
            Vector3 l1 = a(P0);
            Vector3 k2 = V0 + 0.5f * l1 * dt;
            Vector3 l2 = a(P0 + 0.5f*k1 * dt);
            Vector3 k3 = V0 + 0.5f * l2 * dt;
            Vector3 l3 = a(P0 +0.5f*k2 * dt);
            Vector3 k4 = V0 + l3 * dt;
            Vector3 l4 = a(P0 + k3 * dt);

            Pf = P0 + dt*(k1 + 2*k2 + 2*k3 + k4)/6f;
            Vf = V0 + dt * (l1 + 2 * l2 + 2 * l3 + l4) / 6f;

            planet.localPosition = Pf;
            P0 = Pf;
            V0 = Vf;
        }
    }

    // Aceleración a = F/m
    Vector3 a(Vector3 p)
    {
        return -G * p / Mathf.Pow(p.magnitude, 3);
    }

    void GetAndShowOrbitParameters()
    {
        energy = 0.5f * Vector3.Dot(V0, V0) - G / P0.magnitude;
        angularMomentum = Vector3.Cross(P0, V0);
        laplaceRungeLenz = Vector3.Cross(angularMomentum, V0) + G * P0 / P0.magnitude;
        eccentricity = laplaceRungeLenz.magnitude / G;
        majorSemiAxis = -G / (2 * energy);
        minorSemiAxis = majorSemiAxis * Mathf.Sqrt(1 - eccentricity * eccentricity);
        time = 2 * Mathf.PI * Mathf.Sqrt(Mathf.Pow(majorSemiAxis, 3) / G);
    }
}
