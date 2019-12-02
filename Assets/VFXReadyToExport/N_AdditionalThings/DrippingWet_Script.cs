using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrippingWet_Script : MonoBehaviour
{
    List<Vector3> vertices = new List<Vector3>();
    List<Vector3> normals = new List<Vector3>();
    
    public bool IsDripping = false;

    public double timerMax = 0.1;
    private double timer = 0.1;

    [SerializeField]
    private GameObject DropletPrefab;

    private GameObject objReference;

    // Start is called before the first frame update
    void Start()
    {
        timer = timerMax;

        Mesh theMesh;
        if (transform.GetComponentInChildren<MeshFilter>() == null)
        {
            theMesh = transform.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
            objReference = transform.GetComponentInChildren<SkinnedMeshRenderer>().gameObject;
        }
        else
        {
            theMesh = transform.GetComponentInChildren<MeshFilter>().mesh;
            objReference = transform.GetComponentInChildren<MeshFilter>().gameObject;
        }
        theMesh.GetVertices(vertices);
        theMesh.GetNormals(normals);

        //for (int i = 0; i < normals.Count; ++i)
        //{
        //    if (Vector3.Angle(new Vector3(0, 1, 0), normals[i]) < 90)
        //    {
        //        PointsToDrip.Add(vertices[i]);
        //    }
        //}

    }

    // Update is called once per frame
    void Update()
    {
        if (IsDripping)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer += timerMax;

                int randompoint = Random.Range(0, vertices.Count - 1);
                int counter = 20;
                while (counter > 0 && Vector3.Angle(new Vector3(0, 1, 0), normals[randompoint]) < 90)
                {
                    randompoint = Random.Range(0, vertices.Count - 1);
                }

                GameObject droplet = Instantiate(DropletPrefab);
                //droplet.transform.position = transform.GetChild(0).localToWorldMatrix.MultiplyPoint3x4(vertices[randompoint]);
                droplet.transform.position = objReference.transform.localToWorldMatrix.MultiplyPoint3x4(vertices[randompoint]);

                Destroy(droplet, 3);

            }
        }
    }
}
