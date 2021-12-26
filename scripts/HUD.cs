using Godot;
using System;

public class HUD : Control
{
    [Export] private NodePath trainPath;
    private TrainCar train;

    private Label speedLabel;

    public override void _Ready()
    {
        train = GetNode(trainPath) as TrainCar;

        speedLabel = GetNode("SpeedLabel") as Label;
    }

    public override void _Process(float delta)
    {
        speedLabel.Text = String.Format("Speed: {0} km/h", (train?.CurrentSpeed ?? 0) / Consts.ScaleFactor);
    }
}
