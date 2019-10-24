using UnityEngine;
using System.Collections;

// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

public class SampleNoiseGen : MonoBehaviour
{
    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;

    [SerializeField]
    private Texture2D noiseTex0;
    private Texture2D noiseTex1;
    private Vector2 noiseOffset0;
    private Vector2 noiseOffset1;

    private Color[] pix;

    private float timer = 0;

    [SerializeField]
    private Renderer targetrenderer;

    void Start()
    {

        pix = new Color[pixWidth * pixHeight];

        if (!noiseTex0)
            noiseTex0 = new Texture2D(pixWidth, pixHeight);

        noiseTex1 = new Texture2D(pixWidth, pixHeight);

        noiseTex1 = CalcNoise(Random.Range(0, 10000.0f));
        targetrenderer.material.SetTexture("_MyNoise0", noiseTex0);
        targetrenderer.material.SetTexture("_MyNoise1", noiseTex1);

        noiseOffset0 = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        noiseOffset1 = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        targetrenderer.material.SetVector("_MyNoiseOffset0", noiseOffset0);
        targetrenderer.material.SetVector("_MyNoiseOffset1", noiseOffset1);
    }

    Texture2D CalcNoise(float randomvalue)
    {
        Texture2D noiseTex = new Texture2D(pixWidth, pixHeight);

        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord + randomvalue, yCoord + randomvalue);

                float DistFromCenter = Mathf.Sqrt( Mathf.Pow((float)noiseTex.width / 2 - x, 2) + Mathf.Pow((float)noiseTex.height / 2 - y, 2) );
                //DistFromCenter -= (float)noiseTex.width / 4;
                float OppDistFromCenter = 1 - Mathf.Abs(Mathf.Clamp((float)noiseTex.width / 2 - DistFromCenter, 0, (float)noiseTex.width / 2)) / ((float)noiseTex.width / 2);

                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample) * Mathf.Clamp(OppDistFromCenter * 8, 0, 1);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();

        return noiseTex;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 4;
            noiseTex0 = new Texture2D(pixWidth, pixHeight);
            noiseTex0.SetPixels(noiseTex1.GetPixels());
            noiseTex0.Apply();
            noiseTex1 = CalcNoise(Random.Range(0, 10000.0f));
            targetrenderer.material.SetTexture("_MyNoise0", noiseTex0);
            targetrenderer.material.SetTexture("_MyNoise1", noiseTex1);

            noiseOffset0 = noiseOffset1;
            noiseOffset1 = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            targetrenderer.material.SetVector("_MyNoiseOffset0", noiseOffset0);
            targetrenderer.material.SetVector("_MyNoiseOffset1", noiseOffset1);
        }

        targetrenderer.material.SetFloat("_MyNoiseProgress", 4 - timer);

    }
}