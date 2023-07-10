namespace SkillfullWebUI.Models
{
    public class ApiServiceGetResponseModel<T>
    {
        public bool Result { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Content { get; set; }
    }
}
