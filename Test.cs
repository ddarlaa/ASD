namespace ASD;

public class Test(int data)
{
    public int Data { get; } = data;

    public bool Equals(Test? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Data == other.Data;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Test);
    }

    public override int GetHashCode()
    {
        return Data;
    }

    public override string ToString()
    {
        return Data.ToString();
    }
}