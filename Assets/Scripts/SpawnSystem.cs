using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

// Instance の生成処理を他の Systemよりも早く実行するために UpdateInGroupAttribute を指定
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial struct SpawnSystem : ISystem
{
    public void OnCreate(ref SystemState state)
        // World 内に Config Component が存在する場合のみ実行されるようにしている
        // Subscene は 1frame から読み込まれていることが確定しないため、チェックが必要
        => state.RequireForUpdate<Config>();

    // [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // World 内に存在する Component を Singleton として取得
        var config = SystemAPI.GetSingleton<Config>();

        // Tips: 大量の Entity の生成はまとめて実行するとパフォーマンスが向上する
        // Prefab, Spawn 数をｓ指定して Entity を生成
        var instances = state.EntityManager.Instantiate
          (config.Prefab, config.SpawnCount, Allocator.Temp);


        var rand = new Random(config.RandomSeed);
        foreach (var entity in instances)
        {
            // SystemAPI から RW を使って各 Component へのアクセサを取得
            var xform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            var dancer = SystemAPI.GetComponentRW<Dancer>(entity);
            var walker = SystemAPI.GetComponentRW<Walker>(entity);
            // LocalTransform の初期値をランダムに設定
            xform.ValueRW = LocalTransform.FromPositionRotation
              (rand.NextOnDisk() * config.SpawnRadius, rand.NextYRotation());

            // Dancer, Walker の初期値をランダムに設定
            dancer.ValueRW = Dancer.Random(rand.NextUInt());
            walker.ValueRW = Walker.Random(rand.NextUInt());
        }

        // これがないと次の Frame も Spawn 処理を繰り返してしまう
        state.Enabled = false;
    }
}
