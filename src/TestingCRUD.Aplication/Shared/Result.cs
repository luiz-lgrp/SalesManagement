namespace TestingCRUD.Aplication.Shared
{
    public class Result<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string>? Errors { get; set; }

        public static Result<T> Ok(T data) => new() { Success = true, Data = data };
        public static Result<T> Fail(IEnumerable<string> errors) => new() { Success = false, Errors = errors };
    }

}
