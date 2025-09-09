public class BaseResponseDto<T>
{
    public bool Success { get; set; }
    public string Messages { get; set; } = string.Empty;
    public T? Data { get; set; }
    public object? Errors { get; set; }
    public DateTime TImeStamp { get; set; } = DateTime.UtcNow;

}