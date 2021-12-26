using Godot;
using System;

public class Locomotive : TrainCar
{

    [Export] private float maxSpeed = 400;
    [Export] private float accelerationForce = 4000;

    private void Keybinds(float delta)
    {
        if (Input.IsActionPressed("accelerate"))
        {
            ApplyForce(accelerationForce);
        }
        if (Input.IsActionPressed("decelerate"))
        {
            ApplyForce(-accelerationForce);
        }
    }

    private void ConstrainSpeed()
    {
        CurrentSpeed = Mathf.Clamp(CurrentSpeed, -maxSpeed, maxSpeed);
    }

    public override void _Process(float delta)
    {
        Keybinds(delta);
        ConstrainSpeed();
        
        base._Process(delta);
    }
}