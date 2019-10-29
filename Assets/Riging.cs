using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Riging : MonoBehaviour
{
    public int nbIt;
    public Vector3 startPoint;
    public Vector3 endPoint;
    public List<Vector3> points;
    public float[] distances;

    public GameObject spherePrefab;


    private GameObject endSphere;
    private List<GameObject> objectPoints;
    private void Start()
    {
        ComputeDistances();

        objectPoints = new List<GameObject>();
        for (int i = 0; i < points.Count; i++)
        {
            objectPoints.Add(Instantiate(spherePrefab, points[i], Quaternion.identity));
        }

        Instantiate(spherePrefab, startPoint, Quaternion.identity);
        endSphere = Instantiate(spherePrefab, endPoint, Quaternion.identity);
    }
    private void Update()
    {
        endSphere.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);

        if (Input.mouseScrollDelta.y < 0)
        {
            Camera.main.orthographicSize += 1f;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            Camera.main.orthographicSize -= 1f;
        }

        Fabrik();
    }

    public void ComputeDistances()
    {
        //calcul des distances
        distances = new float[points.Count - 1];
        for (int i = 0; i < distances.Length; i++)
        {
            distances[i] = Vector3.Distance(points[i], points[i + 1]);
        }
    }

    public void Propag()
    {

    }

    public void Fabrik()
    {
        for (int it = 0; it < nbIt; it++)
        {
            int indexDistance = distances.Length - 1;
            //aller
            for (int i = points.Count - 1; i >= 0; i--)
            {
                //test pour le traitement du premier (le dernier du squelette) point qui est différent
                if (i == points.Count - 1)
                {
                    points[i] = endSphere.transform.position;
                }
                else
                {
                    Vector3 dir = points[i] - points[i + 1];
                    dir = Vector3.Normalize(dir);

                    points[i] = points[i + 1] + (dir * distances[indexDistance]);
                    indexDistance--;
                }
            }

            //retour
            indexDistance = 0;
            for (int i = 0; i < points.Count; i++)
            {
                //test pour le traitement du premier (le dernier du squelette) point qui est différent
                if (i == 0)
                {
                    points[i] = startPoint;
                }
                else
                {
                    Vector3 dir = points[i] - points[i - 1];
                    dir = Vector3.Normalize(dir);

                    points[i] = points[i - 1] + (dir * distances[indexDistance]);
                    indexDistance++;
                }
            }

            //dessin
            //points
            for (int i = 0; i < objectPoints.Count; i++)
            {
                objectPoints[i].transform.position = points[i];
            }
            //lines
            for (int i = 0; i < objectPoints.Count - 1; i++)
            {
                objectPoints[i].GetComponent<LineRenderer>().SetPosition(0, objectPoints[i].transform.position);
                objectPoints[i].GetComponent<LineRenderer>().SetPosition(1, objectPoints[i + 1].transform.position);
            }
        }
    }
}
