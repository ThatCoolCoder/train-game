using Godot;
using System;

public class Track : Path2D
{
    private Line2D line2D;

    public override void _Ready()
    {
        line2D = GetNode("Line2D") as Line2D;
    }
    
    public override void _Process(float delta)
    {
        line2D.Points = Curve.GetBakedPoints();
    }
}
