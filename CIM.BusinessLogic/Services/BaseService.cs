using CIM.Model;

namespace CIM.BusinessLogic.Services
{
    public class BaseService
    {
        public string CurrentUserId { get; set; } = "SomeAdmin";
        public string CurrentLanguage { get; set; } = "en";

        public CurrentUserModel CurrentUser { get; set; } = new CurrentUserModel { UserId = "64c679a2-795c-4ea9-a35a-a18822fa5b8e" }; //testadmin
    }
}