public class Edge
{
    public Vertex A { get; }
    public Vertex B { get; }
    
    public Edge(Vertex a, Vertex b)
    {
        A = a;
        B = b;
    }
    
    // Override Equals to check for equality
    public override bool Equals(object obj)
    {
        if (obj is not Edge other) return false;
    
        return (A.Equals(other.A) && B.Equals(other.B)) || 
               (A.Equals(other.B) && B.Equals(other.A));
    }

    public override string ToString()
    {
        return $"{A} - {B}";
    }
}