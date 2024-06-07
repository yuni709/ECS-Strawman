using Unity.Entities;
using UnityEngine;

public struct Spinner : IComponentData
{
    public float AngularSpeed;
}

public class SpinnerAuthoring : MonoBehaviour
{
    public float _angularSpeed = 1;

    class Baker : Baker<SpinnerAuthoring>
    {
        public override void Bake(SpinnerAuthoring src)
        {
            var data = new Spinner(){ AngularSpeed = src._angularSpeed };
            AddComponent(GetEntity(TransformUsageFlags.Dynamic), data);
        }
    }
}