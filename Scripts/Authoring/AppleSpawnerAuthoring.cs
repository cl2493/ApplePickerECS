using Unity.Entities;
using UnityEngine;

public struct AppleSpawner : IComponentData
{
    public Entity PoisonPrefab; // Reference to poison apple prefab
    public Entity NormalPrefab; // Reference to normal apple prefab
    public float Interval; // Interval for spawning
}

[DisallowMultipleComponent]
public class AppleSpawnerAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject poisonApplePrefab; // Poison apple prefab
    [SerializeField] private GameObject applePrefab; // Normal apple prefab
    [SerializeField] private float appleSpawnInterval = 1f;
    private class AppleSpawnerAuthoringBaker : Baker<AppleSpawnerAuthoring>
    {
        public override void Bake(AppleSpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);


            // Create a single AppleSpawner component
            var appleSpawner = new AppleSpawner
            {
                NormalPrefab = GetEntity(authoring.applePrefab, TransformUsageFlags.Dynamic), // Normal apples
                PoisonPrefab = GetEntity(authoring.poisonApplePrefab, TransformUsageFlags.Dynamic), // Poison apples
                Interval = authoring.appleSpawnInterval, // Set spawn interval
            };

            // Add the AppleSpawner component to the entity
            AddComponent(entity, appleSpawner);
            // Add the Timer component with an initial value
            AddComponent(entity, new Timer { Value = 2f });
        }
    }
}
