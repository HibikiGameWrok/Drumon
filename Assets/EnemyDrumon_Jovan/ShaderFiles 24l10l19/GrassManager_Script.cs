using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassManager_Script : MonoBehaviour {

    Renderer[] AllGrassRenderers;
    private float variable = 100;
    public float Speed = 0.15f;

    public int pixWidth = 100;
    public int pixHeight = 100;
    public float xOrg = 0;
    public float yOrg = 0;
    public float scale = -20;
    private Texture2D noiseTex;
    private Color[] pix;

    private GameObject PlayerReference;

    // Use this for initialization
    void Start () {
        PlayerReference = GameObject.FindGameObjectWithTag("Player");

        AllGrassRenderers = transform.GetComponentsInChildren<Renderer>();

        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];

        //for (int i = 0; i < AllGrassRenderers.Length; ++i)
        //{
        //    AllGrassRenderers[i].material.SetTexture("_NoiseTex", noiseTex);
        //}
    }
	
	// Update is called once per frame
	void Update () {
        CalcNoise();

        //variable -= Time.deltaTime * Speed;
        //if (variable <= 0) variable += 100;
        //for (int i = 0; i < AllGrassRenderers.Length; ++i)
        //{
        //    AllGrassRenderers[i].material.SetFloat("WindOffset", variable);
        //}

        if (PlayerReference)
            for (int i = 0; i < AllGrassRenderers.Length; ++i)
            {
                AllGrassRenderers[i].material.SetVector("_PlayerPos", new Vector3(PlayerReference.transform.position.x, PlayerReference.transform.position.y, PlayerReference.transform.position.z));
            }
    }

    void CalcNoise()
    {
        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }
}
