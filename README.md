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

**1**-Normal Way: ```without PoolableBehaviour``` ```really bad```

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
<br><br>
**2**-PoolableBehaviour First Way: ```separated PoolableBehaviour``` ```no garbage allocation```

- To Create Ball Prefab

```1) Create a Sphere GameObject```
<br>
```2) Rename it to 'Ball'```
<br>
```3) Create Ball.cs script and copy the code below```
<br>
```4) Attach Ball script to 'Ball'```
<br>
```5) Attach RigiBody to 'Ball'```
<br>
```6) Assign RigidBody reference to Ball script```
<br>
```7) Drag 'Ball' to Assets folder to make a prefab```

- Ball Class
```cs
public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private PoolableBehaviour poolable;


    private float m_SpawnTime;


    private void Awake()
    {
        poolable.callbacks.onAfterInitialized.AddListener(OnAfterInitialized);
    }

    private void OnDestroy()
    {
        poolable.callbacks.onAfterInitialized.RemoveListener(OnAfterInitialized);
    }


    private void Update()
    {
        if (Time.time - m_SpawnTime > 1f)
        {
            poolable.Release();
        }
    }


    public void OnAfterInitialized()
    {
        body.velocity = Random.insideUnitSphere.normalized * Random.Range(10f, 30f);
        m_SpawnTime = Time.time;
    }
}
```

- To Create Ball Spawner

```1) Create an Empty GameObject```
<br>
```2) Rename it to 'Ball Spawner'```
<br>
```3) Create BallSpawner.cs script and copy the code below```
<br>
```4) Attach BallSpawner script to 'Ball Spawner'```
<br>
```5) Attach PoolableBehaviourSpawner to 'Ball Spawner'```
<br>
```6) Assign PoolableBehaviourSpawner reference to BallSpawner script```
<br>
```7) Assign the 'Ball' prefab reference to PoolableBehaviourSpawner component```
<br>
```8) Create an Empty child GameObject inside BallSpawner GameObject```
<br>
```9) Rename the Empty child to 'Balls'```
<br>
```10) Assign the 'Balls' Transform reference to PoolableBehaviourSpawner as Parent```
<br>
```11) Set prefillCount [min: 0]```
<br>
```12) Set capacity [infinite: -1]```

- Ball Spawner Class
```cs
public class BallSpawner : MonoBehaviour
{
    [SerializeField] private PoolableBehaviourSpawner poolableSpawner;
    [SerializeField] private int iterationCount;
    [SerializeField] private bool spawn;


    private void Update()
    {
        if (!spawn) return;

        for (int i = 0; i < iterationCount; i++)
        {
            poolableSpawner.Spawn();
        }
    }
}
```
- Performance Test: ```ran for 1 mins each```
```
| Spawn Rate |        CPU Time |          FPS | Garbage Allocation per Frame |
|:----------:|:---------------:|:------------:|:----------------------------:|
|          1 |  0.002996361 ms | 333.7382 fps |                          0 B |
|         10 |  0.006783063 ms | 147.4261 fps |                          0 B |
|        100 |  0.036871261 ms |  27.1214 fps |                          0 B |
```
