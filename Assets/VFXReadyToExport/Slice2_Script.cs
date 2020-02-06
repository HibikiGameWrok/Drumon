using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice2_Script : MonoBehaviour
{

    private bool ToBeDeleted = false;

    public int TimesToCut = 3;

    public bool RandomizeDirection = true;

    // Start is called before the first frame update
    void Start()
    {
        //if (RandomizeDirection)
        //transform.Rotate(Random.Range(0, 360.0f), Random.Range(0, 360.0f), Random.Range(0, 360.0f));
    }

    // Update is called once per frame
    void Update()
    {
        if (ToBeDeleted)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        //return;

        if (!col.CompareTag("Sliceable")) return;
        
        GenerateNewMesh(col.gameObject, TimesToCut);
        Destroy(col.gameObject);

        ToBeDeleted = true;

    }

    GameObject MeshDecimation(GameObject objToCut, Vector3 direction)
    {
        Mesh mesh0 = new Mesh();
        Mesh currentmesh = objToCut.GetComponent<MeshFilter>().mesh;

        List<int> dictionaryof = new List<int>();

        List<Vector3> CurrentVertices = new List<Vector3>();
        currentmesh.GetVertices(CurrentVertices);
        List<Vector3> NewVertices = new List<Vector3>();

        for (int i = 0; i < CurrentVertices.Count; ++i)
        {
            Vector3 thepos = objToCut.transform.localToWorldMatrix.MultiplyPoint3x4(CurrentVertices[i]);

            if (Vector3.Dot(Vector3.Normalize(thepos - transform.position), direction) >= 0)
            {
                NewVertices.Add(CurrentVertices[i]);
                dictionaryof.Add(i);
            }
        }
        mesh0.SetVertices(NewVertices);

        List<Vector3> CurrentNormals = new List<Vector3>();
        currentmesh.GetNormals(CurrentNormals);
        List<Vector3> NewNormals = new List<Vector3>();
        for (int i = 0; i < CurrentNormals.Count; ++i)
        {
            if (dictionaryof.Contains(i))
            {
                NewNormals.Add(CurrentNormals[i]);
            }
        }
        mesh0.SetNormals(NewNormals);

        List<Vector2> CurrentUVs = new List<Vector2>();
        currentmesh.GetUVs(0, CurrentUVs);
        List<Vector2> NewUVs = new List<Vector2>();
        for (int i = 0; i < CurrentUVs.Count; ++i)
        {
            if (dictionaryof.Contains(i))
            {
                NewUVs.Add(CurrentUVs[i]);
            }
        }
        mesh0.SetUVs(0, NewUVs);

        int[] CurrentTriangles = currentmesh.GetTriangles(0);
        List<int> NewTriangles = new List<int>();
        for (int i = 0; i < CurrentTriangles.Length; i += 3)
        {
            if (dictionaryof.Contains(CurrentTriangles[i]) && dictionaryof.Contains(CurrentTriangles[i + 1]) && dictionaryof.Contains(CurrentTriangles[i + 2]))
            {
                NewTriangles.Add(CurrentTriangles[i]);
                NewTriangles.Add(CurrentTriangles[i + 1]);
                NewTriangles.Add(CurrentTriangles[i + 2]);
            }
        }

        dictionaryof.Sort();

        for (int i = 0; i < NewTriangles.Count; ++i)
        {
            NewTriangles[i] = dictionaryof.IndexOf(NewTriangles[i]);
        }

        mesh0.SetTriangles(NewTriangles, 0);

        GameObject cutupObject0 = Instantiate(objToCut);
        cutupObject0.transform.position = objToCut.transform.position;
        cutupObject0.GetComponent<MeshFilter>().mesh = mesh0;
        cutupObject0.GetComponent<MeshCollider>().sharedMesh = mesh0;
        cutupObject0.GetComponent<Rigidbody>().AddForce(direction * 5, ForceMode.Impulse);

        cutupObject0.GetComponent<MeshCollider>().enabled = false;

        return cutupObject0;

    }

    void GenerateNewMesh(GameObject objToCut, int recursiveCounter)
    {
        //Vector3 testvector;

        //if (RandomizeDirection)
        //{
        //    float randomangle = Random.Range(0, Mathf.PI * 2);
        //    testvector = new Vector3(Mathf.Cos(randomangle), Mathf.Sin(randomangle), 0);
        //}
        //else
        //{
        //    testvector = transform.forward;
        //}

        //testvector.Normalize();

        if (RandomizeDirection)
            transform.Rotate(Random.Range(0, 360.0f), Random.Range(0, 360.0f), Random.Range(0, 360.0f));

        GameObject objA = MeshDecimation(objToCut, transform.forward);
        GameObject objB = MeshDecimation(objToCut, -transform.forward);

        recursiveCounter--;
        if (recursiveCounter <= 0) return;

        GenerateNewMesh(objA, recursiveCounter);
        GenerateNewMesh(objB, recursiveCounter);

        Destroy(objA);
        Destroy(objB);

    }

}
