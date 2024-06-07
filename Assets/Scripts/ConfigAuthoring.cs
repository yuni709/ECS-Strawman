using Unity.Entities;
using UnityEngine;

public struct Config : IComponentData
{
    public Entity Prefab;
    public float SpawnRadius;
    public int SpawnCount;
    public uint RandomSeed;
}

public class ConfigAuthoring : MonoBehaviour
{
    // Prefab については、Monobehavior の中だと従来どおり参照として持っている
    public GameObject Prefab = null;
    public float SpawnRadius = 1;
    public int SpawnCount = 10;
    public uint RandomSeed = 100;

    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring src)
        {
            var data = new Config()
            {
                // GetEntity(): 従来の GameObject から Entity への変換する機能を持つ
                Prefab = GetEntity(src.Prefab, TransformUsageFlags.Dynamic),
                SpawnRadius = src.SpawnRadius,
                SpawnCount = src.SpawnCount,
                RandomSeed = src.RandomSeed
            };
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), data);
        }
    }
}