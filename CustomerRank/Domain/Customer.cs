namespace Domain;

public class Customer
{
    public long CustomerId { get; set; }

    public decimal ScoreValue { get; set; }

    public int Rank { get; set; }

    public bool IsZeroScore()
    {
        return ScoreValue == 0;
    }

    public void UpdateScore(decimal score)
    { 
        ScoreValue = score + ScoreValue;
    }
}
