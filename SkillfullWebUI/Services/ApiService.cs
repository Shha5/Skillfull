namespace SkillfullWebUI.Services
{
    public class ApiService
    {
        private readonly ILogger<ApiService> _logger;
        private readonly HttpClient _apiClient;

        public ApiService(ILogger<ApiService> logger, HttpClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        //get all skills 

        //get skill details

        //assign skill to user

        //get skills of a user

        //assign a task to skill of a user

        //get all tasks for a user
    }
}
