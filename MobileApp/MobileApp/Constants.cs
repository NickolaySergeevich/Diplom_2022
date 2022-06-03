namespace MobileApp
{
    internal static class Constants
    {
        // Addresses
        public const string LoginAddress = "http://diplom.std-918.ist.mospolytech.ru/api/login/";
        public const string UserInformationAddress = "http://diplom.std-918.ist.mospolytech.ru/api/get_user_information/";
        public const string OrgInformationAddress = "http://diplom.std-918.ist.mospolytech.ru/api/get_org_information/";
        public const string RegistrationExpertAddress = "http://diplom.std-918.ist.mospolytech.ru/api/registration_expert/";
        public const string RegistrationOrgAddress = "http://diplom.std-918.ist.mospolytech.ru/api/registration_org/";
        public const string RegistrationPartnerAddress = "http://diplom.std-918.ist.mospolytech.ru/api/registration_partners/";
        public const string RegistrationUserAddress = "http://diplom.std-918.ist.mospolytech.ru/api/registration_user/";
        public const string UpdateUserAddress = "http://diplom.std-918.ist.mospolytech.ru/api/update_user/";
        public const string RegistrationNastAddress = "http://diplom.std-918.ist.mospolytech.ru/api/registration_nast/";
        public const string TasksAddress = "http://diplom.std-918.ist.mospolytech.ru/api/tasks/";
        public const string TasksByUserAddress = "http://diplom.std-918.ist.mospolytech.ru/api/get_tasks_by_user/";
        public const string GetNastIdAddress = "http://diplom.std-918.ist.mospolytech.ru/api/get_nast_by_name/";
        public const string GetUserIdAddress = "http://diplom.std-918.ist.mospolytech.ru/api/get_user_by_name/";
        public const string SignUpToTaskAddress = "http://diplom.std-918.ist.mospolytech.ru/api/tasks/sign_up_to_task/";
        public const string RemoveFromTaskAddress = "http://diplom.std-918.ist.mospolytech.ru/api/tasks/remove_from_task/";
        public const string GetTeamsAddress = "http://diplom.std-918.ist.mospolytech.ru/api/get_teams/";

        // Server codes
        public const string ServerError = "404";
        public const string NoDataInDb = "502";

        // Users role
        public const int JustUser = 1;
        public const int NastUser = 2;
        public const int OrgUser = 3;
        public const int PartnerUser = 4;
        public const int ExpertUser = 5;
    }
}
