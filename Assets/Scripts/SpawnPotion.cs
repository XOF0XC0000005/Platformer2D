using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPotion : MonoBehaviour
{
    [SerializeField] private Potion _potionPrefab;
    [SerializeField] private Transform _parentSpawner;

    private Transform[] _spawners;

    private void Start()
    {
        GetAllSpawners();
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        for (int i = 0; i < _spawners.Length; i++)
        {
            Instantiate(_potionPrefab, _spawners[i].transform.position, _spawners[i].transform.rotation);
            yield return new WaitForSeconds(2);
        }
    }

    private void GetAllSpawners()
    {
        _spawners = new Transform[_parentSpawner.childCount];

        for (int i = 0; i < _parentSpawner.childCount; i++)
        {
            _spawners[i] = _parentSpawner.GetChild(i);
        }
    }

}
