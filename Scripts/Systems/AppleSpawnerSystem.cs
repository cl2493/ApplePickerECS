using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateAfter(typeof(TimerSystem))]
public partial struct AppleSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<BeginSimulationEntityCommandBufferSystem.Singleton>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        // Get the delta time
        float deltaTime = SystemAPI.Time.DeltaTime;

        // Determine the difficulty levels
        bool isMediumMode = MainMenu.IsMediumMode;
        bool isHardMode = MainMenu.IsHardMode;

        // Initialize a random instance with a seed
        var random = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(1, 10000));

        // Schedule the job and pass the random instance and difficulty level
        new SpawnJob
        {
            ECB = ecb,
            DeltaTime = deltaTime,
            Random = random,
            IsMediumMode = isMediumMode,
            IsHardMode = isHardMode
        }.Schedule();
    }

    [BurstCompile]
    private partial struct SpawnJob : IJobEntity
    {
        public EntityCommandBuffer ECB;
        public float DeltaTime;
        public Unity.Mathematics.Random Random; // Store the random instance
        public bool IsMediumMode; // Indicate if medium mode is active
        public bool IsHardMode; // Indicate if hard mode is active

        private void Execute(in LocalTransform transform, in AppleSpawner spawner, ref Timer timer)
        {
            // Check if timer is greater than 0
            if (timer.Value > 0)
            {
                timer.Value -= DeltaTime; // Use the passed delta time
                return; // Exit if the timer hasn't expired
            }

            // Reset the timer for the next spawn
            timer.Value = spawner.Interval;

            // Generate a random number between 0 and 1 using the Random instance
            float randomValue = Random.NextFloat();

            // Spawn a normal apple with 90% probability
            if (randomValue < 0.5f) // 90% chance for normal apple
            {
                var normalAppleEntity = ECB.Instantiate(spawner.NormalPrefab);
                ECB.SetComponent(normalAppleEntity, LocalTransform.FromPosition(transform.Position));
            }
            // Spawn a poison apple with 20% probability if medium or hard mode
            else if ((IsMediumMode || IsHardMode) && randomValue >= 0.5f) // 10% chance for poison apple
            {
                var poisonAppleEntity = ECB.Instantiate(spawner.PoisonPrefab);
                ECB.SetComponent(poisonAppleEntity, LocalTransform.FromPosition(transform.Position));
            }
        }
    }
}
