using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slice_Script : MonoBehaviour
{

    public bool GenerateMesh = true;

    [SerializeField]
    private Mesh meshToCut;

    [SerializeField]
    private GameObject DiscReference;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GenerateMesh)
        {
            GenerateMesh = false;
            GenerateNewMesh();
        }
    }

    void GenerateNewMesh()
    {

        Mesh mesh0 = null;//, mesh1 = null;
        Mesh currentmesh = meshToCut;//GetComponent<MeshFilter>().mesh;

        mesh0 = new Mesh();

        List<int> dictionaryof = new List<int>();

        int debug0 = 0, debug1 = 0, debug2 = 0;

        List<Vector3> CurrentVertices = new List<Vector3>();
        currentmesh.GetVertices(CurrentVertices);
        List<Vector3> NewVertices = new List<Vector3>();
        //for (int i = 0; i < CurrentVertices.Count; ++i)
        //{
        //    if (CurrentVertices[i].y >= variable)
        //    {
        //        NewVertices.Add(CurrentVertices[i]);
        //        dictionaryof.Add(i);
        //        debug0++;
        //    }
        //}

        for (int i = 0; i < CurrentVertices.Count; ++i)
        {
            Vector3 thepos = transform.localToWorldMatrix.MultiplyPoint3x4(CurrentVertices[i]);
            if (Vector3.Dot(Vector3.Normalize(thepos - DiscReference.transform.position), DiscReference.transform.forward) <= 0)
            {
                NewVertices.Add(CurrentVertices[i]);
                dictionaryof.Add(i);
                debug0++;
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
                debug1++;
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
                debug2++;
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



        //MeshTopology topology = currentmesh.GetTopology(0);      
        //mesh0.SetIndices(NewIndices.ToArray(), mesh0.GetTopology(0), 0);

        GetComponent<MeshFilter>().mesh = mesh0;
    }


}
