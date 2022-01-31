using Godot;
using System;

public class CameraZoom : Camera2D
{
    [Export] private float zoomSpeed = 0.05f;

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("zoom_out"))
        {
            Zoom *= 1 + (zoomSpeed * delta);
        }
        else if (Input.IsActionPressed("zoom_in"))
        {
            Zoom /= 1 + (zoomSpeed * delta);
        }
    }
}
