using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NaughtyAttributes;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoliageController : MonoBehaviour
{
    

    [SerializeField] private List<FoliagePiece> foliagePieces;
    [SerializeField][InfoBox("Causes foliage to spawn more often. Anything " +
        "above 2 is probably a bad idea.")] private int foliageDensity;
    [SerializeField][InfoBox("The range of time (in seconds) between spawns " +
        "once each piece of foliage has left the screen.")] private Vector2 
        spawnDelay;
    [SerializeField] private float spawnXValue;
    [SerializeField] private float despawnXValue;

    /// <summary>
    /// starts the foliage coroutines
    /// </summary>
    private void Start()
    {
        for (int i = 0; i < foliageDensity; i++)
        {
            StartCoroutine(FoliageMover());
        }
    }

    private IEnumerator FoliageMover()
    {
        // delay foliage spawn 
        yield return new WaitForSeconds(Random.Range(spawnDelay.x, spawnDelay.
            y));

        Debug.Log("Foliage spawned");


        // instantiate random prefab
        FoliagePiece selectedFoliagePiece = foliagePieces[(int)Random.Range(0,
            foliagePieces.Count)];

        GameObject currentFoliage = Instantiate(selectedFoliagePiece.prefab);


        // set foliage spawn position
        Vector3 foliageSpawnPosition = new Vector3
            (
            spawnXValue,
            Random.Range(selectedFoliagePiece.spawnHeightRange.x 
                ,selectedFoliagePiece.spawnHeightRange.y),
            currentFoliage.transform.position.z
            );

        currentFoliage.transform.position = foliageSpawnPosition;

        // move instance until it reaches despawn value
        while (currentFoliage.transform.position.x > despawnXValue)
        {
            yield return null;
            currentFoliage.transform.Translate(Vector2.left * 
                selectedFoliagePiece.moveSpeed * Time.deltaTime);
        }

        // delete instance
        Destroy(currentFoliage);

        //restart coroutine
        yield return StartCoroutine(FoliageMover());
    }

}

[Serializable]
public class FoliagePiece
{
    public GameObject prefab;
    [MinMaxSlider(-5f, 5f)] public Vector2 spawnHeightRange;
    [InfoBox("Keep equal to the speed of the background parallax for best " +
        "results (3).")] public float moveSpeed;
}