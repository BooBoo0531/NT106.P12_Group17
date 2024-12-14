using System.Net.Sockets;

public class SnakeDTO
{

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum State
    {
        Alive,
        Dead
    }
    public string Username { get; set; }
    public State CurrentState { get; set; }
    public Direction CurrentDirection { get; set; }
    public float PositionX { get; set; } = 0;
    public float PositionY { get; set; } = 0;
    public int SnakeBodySize { get; set; } = 0;

    public long Score { get; set; } = 0;

    public SnakeDTO()
    {
        CurrentDirection = Direction.Right;
        CurrentState = State.Alive;
    }
}
