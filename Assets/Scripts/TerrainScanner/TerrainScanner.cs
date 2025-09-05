using System;
using UnityEngine;

public class TerrainScanner : MonoBehaviour
{
    public GameObject TerrainScannerPrefab;
    public float duration = 10; //持续时间
    public float size = 500;    //大小

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnTerrainScanner();
        }
    }

    private void SpawnTerrainScanner()
    {
        GameObject terrainScanner = Instantiate(TerrainScannerPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;    // 调用地形扫描函数在相机位置实例化预制件
        ParticleSystem terrainScannerPS = terrainScanner.transform.GetChild(0).GetComponent<ParticleSystem>();

        if (terrainScannerPS != null)
        {
            var main = terrainScannerPS.main;
            main.startLifetime = duration;  // 起始生命周期改为持续时间
            main.startSize = size;  // 起始大小改为大小
        }
        else 
            Debug.Log("The first child doesn't have a particle system.");
        
        Destroy(terrainScanner, duration + 1);  // 在持续时间后销毁该游戏对象
    }
}
