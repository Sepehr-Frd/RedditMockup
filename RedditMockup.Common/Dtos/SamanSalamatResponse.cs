namespace RedditMockup.Common.Dtos;

public class SamanSalamatResponse<T>
{

    public T? Data { get; set; }

    public bool IsSuccess { get; set; }

    public string? Message { get; set; }

}

public class SamanSalamatResponse
{

    public dynamic? Data { get; set; }

    public bool IsSuccess { get; set; }

    public string? Message { get; set; }

}