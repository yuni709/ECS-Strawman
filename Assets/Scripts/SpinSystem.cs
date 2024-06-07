using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct SpinSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        var elapsed = (float)SystemAPI.Time.ElapsedTime;

        foreach (var (spinner, xform) in
            SystemAPI.Query<RefRO<Spinner>,
                            RefRW<LocalTransform>>())
        {
            // Add rotation to the existing rotation.
            var t = spinner.ValueRO.AngularSpeed * elapsed;
            var rot = quaternion.RotateY(t);
            // xform.ValueRW.Rotation = math.mul(rot, xform.ValueRO.Rotation);
            // xform.ValueRW.Rotation = xform.ValueRW.Rotation.RotateY(t);
            xform.ValueRW.Rotation = math.mul(rot, xform.ValueRO.Rotation);

        }
    }
}