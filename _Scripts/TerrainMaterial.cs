using UnityEngine;

public class TerrainMaterial : MonoBehaviour
{
    private Terrain terr;
    private byte[,] splatIndex;
    private Vector3 size;
    private Vector3 tPos;
    private int width;
    private int height;

    void Start()
    {
        terr = GetComponent<Terrain>();
        CalcHiInflPrototypeIndexesPerPoint();
    }

    private void CalcHiInflPrototypeIndexesPerPoint()
    {
        TerrainData terrainData = terr.terrainData;
        size = terrainData.size;
        width = terrainData.alphamapWidth;
        height = terrainData.alphamapHeight;
        int prototypesLength = terrainData.splatPrototypes.Length;
        tPos = terr.GetPosition();

        float[,,] alphas = terrainData.GetAlphamaps(0, 0, width, height);
        splatIndex = new byte[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                byte ind = 0;
                float t = 0f;
                for (byte i = 0; i < prototypesLength; i++)
                      if (alphas[x, y, i] > t)
                {
                    t = alphas[x, y, i];
                    ind = i;
                }
                splatIndex[x, y] = ind;
            }
        }
    }

    public int GetMaterialIndex(Vector3 pos)
    {
        pos = pos - tPos;
        pos.x /= size.x;
        pos.z /= size.z;

        return splatIndex[(int)(pos.z * (width - 1)),
                  (int)(pos.x * (height - 1))];
    }
}