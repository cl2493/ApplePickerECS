using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public struct MoveWithMouse : IComponentData
{
}
// +++++++++++CODE UPDATED+++++++++++
public partial struct MoveWithMouseSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        // Get the current mouse position in world space
        var mousePosition2D = Input.mousePosition;
        mousePosition2D.z = -Camera.main.transform.position.z;
        var mousePosition3D = Camera.main.ScreenToWorldPoint(mousePosition2D);

        // Check if the game is in "Hard" mode using MainMenu's static property
        bool isHardMode = MainMenu.IsHardMode;

        // Update the position based on difficulty level
        foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<MoveWithMouse>())
        {
            var position = transform.ValueRO.Position;
            // Invert the x position only if in "Hard" mode
            position.x = isHardMode ? -mousePosition3D.x : mousePosition3D.x;
            transform.ValueRW.Position = position;
        }
    }
}
