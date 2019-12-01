using CIM.Model;

namespace CIM.BusinessLogic.Services
{
    public class BaseService
    {
        public string CurrentUserId { get; set; } = "SomeAdmin";
        public string CurrentLanguage { get; set; } = "en";

        public CurrentUserModel CurrentUser { get; set; }
    }
}