using UnityEngine;

public class SistemaPlanetario : MonoBehaviour
{
    public Transform sun;
    public Transform[] planets;
    public float G = 10f;

    [System.Serializable]
    public class PlanetData
    {
        public Vector3 P0;
        public Vector3 V0;
        [HideInInspector] public Vector3 Pf, Vf;

        [Header("Constantes de Movimiento")]
        public float energy;
        public float eccentricity;
        public float majorSemiAxis;
        public float minorSemiAxis;

        [SerializeField] public float time;
    }

    public PlanetData[] planetData;
    public bool isActive = false;

    void Start()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].localPosition = planetData[i].P0;
            planets[i].GetComponent<TrailRenderer>().Clear();
            GetAndShowOrbitParameters(i);
        }
    }

    void FixedUpdate()
    {
        if (!isActive)
        {
            for (int i = 0; i < planets.Length; i++)
            {
                planets[i].localPosition = planetData[i].P0;
                Debug.DrawRay(planets[i].position, planetData[i].V0, Color.red);
            }
            return;
        }

        float dt = Time.fixedDeltaTime;
        for (int i = 0; i < planets.Length; i++)
        {
            Vector3 P0 = planetData[i].P0;
            Vector3 V0 = planetData[i].V0;

            Vector3 k1 = V0;
            Vector3 l1 = a(P0);
            Vector3 k2 = V0 + 0.5f * l1 * dt;
            Vector3 l2 = a(P0 + 0.5f * k1 * dt);
            Vector3 k3 = V0 + 0.5f * l2 * dt;
            Vector3 l3 = a(P0 + 0.5f * k2 * dt);
            Vector3 k4 = V0 + l3 * dt;
            Vector3 l4 = a(P0 + k3 * dt);

            planetData[i].Pf = P0 + dt * (k1 + 2 * k2 + 2 * k3 + k4) / 6f;
            planetData[i].Vf = V0 + dt * (l1 + 2 * l2 + 2 * l3 + l4) / 6f;

            planets[i].localPosition = planetData[i].Pf;
            planetData[i].P0 = planetData[i].Pf;
            planetData[i].V0 = planetData[i].Vf;
        }
    }

    Vector3 a(Vector3 p)
    {
        return -G * p / Mathf.Pow(p.magnitude, 3);
    }

    void GetAndShowOrbitParameters(int index)
    {
        PlanetData planet = planetData[index];
        planet.energy = 0.5f * Vector3.Dot(planet.V0, planet.V0) - G / planet.P0.magnitude;
        Vector3 angularMomentum = Vector3.Cross(planet.P0, planet.V0);
        Vector3 laplaceRungeLenz = Vector3.Cross(angularMomentum, planet.V0) + G * planet.P0 / planet.P0.magnitude;
        planet.eccentricity = laplaceRungeLenz.magnitude / G;
        planet.majorSemiAxis = -G / (2 * planet.energy);
        planet.minorSemiAxis = planet.majorSemiAxis * Mathf.Sqrt(1 - planet.eccentricity * planet.eccentricity);
        planet.time = 2 * Mathf.PI * Mathf.Sqrt(Mathf.Pow(planet.majorSemiAxis, 3) / G);
    }
}
