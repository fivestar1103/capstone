public class Vertex
{
    public float X { get; }
    public float Y { get; }

    public Vertex(float x, float y)
    {
        X = x;
        Y = y;
    }

    // Override Equals to check for equality based on coordinates
    public override bool Equals(object obj)
    {
        if (obj is not Vertex other) return false;
        return X == other.X && Y == other.Y;
    }

    // Override GetHashCode
    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + X.GetHashCode();
            hash = hash * 23 + Y.GetHashCode();
            return hash;
        }
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}