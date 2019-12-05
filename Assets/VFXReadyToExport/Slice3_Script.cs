using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice3_Script : MonoBehaviour
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

    GameObject[] MeshDecimation(GameObject objToCut, Vector3 direction)
    {
        Mesh mesh0 = new Mesh(), mesh1 = new Mesh();
        Mesh currentmesh = objToCut.GetComponent<MeshFilter>().mesh;

        List<int> dictionaryof = new List<int>();
        List<int> dictionaryof2 = new List<int>();

        List<Vector3> CurrentVertices = new List<Vector3>();
        currentmesh.GetVertices(CurrentVertices);
        List<Vector3> NewVertices = new List<Vector3>();
        
        List<Vector3> NewVertices2 = new List<Vector3>();

        Vector3 yourpos = objToCut.transform.worldToLocalMatrix.MultiplyPoint3x4(transform.position);

        for (int i = 0; i < CurrentVertices.Count; ++i)
        {
            if (Vector3.Dot(Vector3.Normalize(CurrentVertices[i] - yourpos), direction) >= 0)
            {
                NewVertices.Add(CurrentVertices[i]);
                dictionaryof.Add(i);
            }
            else
            {
                NewVertices2.Add(CurrentVertices[i]);
                dictionaryof2.Add(i);
            }
        }
        mesh0.SetVertices(NewVertices);
        mesh1.SetVertices(NewVertices2);

        List<Vector3> CurrentNormals = new List<Vector3>();
        currentmesh.GetNormals(CurrentNormals);
        List<Vector3> NewNormals = new List<Vector3>();

        List<Vector3> NewNormals2 = new List<Vector3>();

        for (int i = 0; i < CurrentNormals.Count; ++i)
        {
            if (dictionaryof.Contains(i))
            {
                NewNormals.Add(CurrentNormals[i]);
            }
            else if (dictionaryof2.Contains(i))
            {
                NewNormals2.Add(CurrentNormals[i]);
            }
        }
        mesh0.SetNormals(NewNormals);
        mesh1.SetNormals(NewNormals2);

        List<Vector2> CurrentUVs = new List<Vector2>();
        currentmesh.GetUVs(0, CurrentUVs);
        List<Vector2> NewUVs = new List<Vector2>();

        List<Vector2> NewUVs2 = new List<Vector2>();

        for (int i = 0; i < CurrentUVs.Count; ++i)
        {
            if (dictionaryof.Contains(i))
            {
                NewUVs.Add(CurrentUVs[i]);
            }
            else if (dictionaryof2.Contains(i))
            {
                NewUVs2.Add(CurrentUVs[i]);
            }
        }
        mesh0.SetUVs(0, NewUVs);
        mesh1.SetUVs(0, NewUVs2);

        int[] CurrentTriangles = currentmesh.GetTriangles(0);
        List<int> NewTriangles = new List<int>();

        List<int> NewTriangles2 = new List<int>();

        for (int i = 0; i < CurrentTriangles.Length; i += 3)
        {
            if (dictionaryof.Contains(CurrentTriangles[i]) && dictionaryof.Contains(CurrentTriangles[i + 1]) && dictionaryof.Contains(CurrentTriangles[i + 2]))
            {
                NewTriangles.Add(CurrentTriangles[i]);
                NewTriangles.Add(CurrentTriangles[i + 1]);
                NewTriangles.Add(CurrentTriangles[i + 2]);
            }
            else if (dictionaryof2.Contains(CurrentTriangles[i]) && dictionaryof2.Contains(CurrentTriangles[i + 1]) && dictionaryof2.Contains(CurrentTriangles[i + 2]))
            {
                NewTriangles2.Add(CurrentTriangles[i]);
                NewTriangles2.Add(CurrentTriangles[i + 1]);
                NewTriangles2.Add(CurrentTriangles[i + 2]);
            }
        }

        dictionaryof.Sort();
        dictionaryof2.Sort();

        for (int i = 0; i < NewTriangles.Count; ++i)
        {
            NewTriangles[i] = dictionaryof.IndexOf(NewTriangles[i]);
        }
        for (int i = 0; i < NewTriangles2.Count; ++i)
        {
            NewTriangles2[i] = dictionaryof2.IndexOf(NewTriangles2[i]);
        }
        
        mesh0.SetTriangles(NewTriangles, 0);
        mesh1.SetTriangles(NewTriangles2, 0);

        GameObject cutupObject0 = Instantiate(objToCut);
        cutupObject0.transform.position = objToCut.transform.position;
        cutupObject0.GetComponent<MeshFilter>().mesh = mesh0;
        cutupObject0.GetComponent<MeshCollider>().sharedMesh = mesh0;
        cutupObject0.GetComponent<Rigidbody>().AddForce(direction * 5, ForceMode.Impulse);
        cutupObject0.GetComponent<MeshCollider>().enabled = false;

        GameObject cutupObject1 = Instantiate(objToCut);
        cutupObject1.transform.position = objToCut.transform.position;
        cutupObject1.GetComponent<MeshFilter>().mesh = mesh0;
        cutupObject1.GetComponent<MeshCollider>().sharedMesh = mesh0;
        cutupObject1.GetComponent<Rigidbody>().AddForce(direction * 5, ForceMode.Impulse);
        cutupObject1.GetComponent<MeshCollider>().enabled = false;

        GameObject[] toreturn = new GameObject[2];
        toreturn[0] = cutupObject0;
        toreturn[1] = cutupObject1;

        return toreturn;

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

        GameObject[] obj = MeshDecimation(objToCut, transform.forward);

        recursiveCounter--;
        if (recursiveCounter <= 0) return;

        GenerateNewMesh(obj[0], recursiveCounter);
        GenerateNewMesh(obj[1], recursiveCounter);

        Destroy(obj[0]);
        Destroy(obj[1]);

    }

}
