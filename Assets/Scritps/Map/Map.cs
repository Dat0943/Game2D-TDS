using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform playerSpawnPoint;
    public Transform[] aiSpawnPoints;

    public Transform RandomAISpawnPoint
    {
        get
        {
            if(aiSpawnPoints == null || aiSpawnPoints.Length <= 0) return null;

			int randomIdx = Random.Range(0, aiSpawnPoints.Length);
			return aiSpawnPoints[randomIdx];
		}
    }
}
