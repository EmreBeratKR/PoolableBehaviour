# Poolable Behaviour

[<img src="https://makaka.org/wp-content/uploads/2022/02/new-unity-asset-store-badge-full.png" width="200" />][assetstore]

[<img src="https://images.squarespace-cdn.com/content/v1/5bbc502865019fe7b132cdc0/1619022573920-HXS3VG6DNLBH6NYX2963/discord-button.png" width="200" />][discord]

[<img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" width="200" />][coffee]

[assetstore]: https://assetstore.unity.com/
[discord]: https://discord.gg/mKG9vkyEDX
[coffee]: https://www.buymeacoffee.com/emreberat
[releases]: https://github.com/EmreBeratKR/PoolableBehaviour/releases
[download]: https://github.com/EmreBeratKR/PoolableBehaviour/releases

## About

An Object Pooling solution for Unity

## How to Install

- Import it from [Asset Store][assetstore]
- Import [PoolableBehaviour.unitypackage][releases] from **releases**
- Clone or [Download][download] this repository and move to your Unity project's **Assets** folder

## How to Use

Let's say we want to spawn some amount of balls each frame with lifetime of one seconds
<br><br>
**1**-Normal Way: ```without PoolableBehaviour```

- Ball Class
```cs
public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody body;


    private void Start()
    {
        body.velocity = Random.insideUnitSphere.normalized * Random.Range(10f, 30f);
        Destroy(gameObject, 1f);
    }
}
```

- Ball Spawner Class
```cs
public class BallSpawner : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private int iterationCount;


    private void Update()
    {
        for (int i = 0; i < iterationCount; i++)
        {
            Instantiate(ballPrefab);
        }
    }


    public void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
```
- Performance Test: ```ran for 1 mins each```
```
| Spawn Rate |        CPU Time |          FPS | Garbage Allocation per Frame |
|:----------:|:---------------:|:------------:|:----------------------------:|
|          1 |  0.003884473 ms | 257.4352 fps |                        120 B |
|         10 |  0.009580554 ms | 104.3781 fps |                       1.2 KB |
|        100 |  0.044309641 ms |  22.5685 fps |                      11.7 KB |
```
