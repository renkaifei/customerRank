namespace CustomerRank.Models;

public class CustomResponse<T>
{
    public int Code { get; set; }

    public T? Data { get; set; }

    public string? Message { get; set; }
}
