namespace MobileApp
{
    internal static class Constants
    {
        // Addresses
        public const string LoginAddress = "http://diplom.std-918.ist.mospolytech.ru/api/login/";
        public const string RegistrationAddress = "http://diplom.std-918.ist.mospolytech.ru/api/registration/";
        public const string TasksAddress = "http://diplom.std-918.ist.mospolytech.ru/api/tasks/";
        public const string GetUserIdAddress = "http://diplom.std-918.ist.mospolytech.ru/api/get_user_by_name/";
        public const string SignUpToTaskAddress = "http://diplom.std-918.ist.mospolytech.ru/api/tasks/sign_up_to_task/";
        public const string RemoveFromTaskAddress = "http://diplom.std-918.ist.mospolytech.ru/api/tasks/remove_from_task/";

        // Server codes
        public const string ServerError = "404";
        public const string NoDataInDb = "502";
    }
}
