using ExpensePrediction.DataTransferObjects.Category;

namespace ExpensePrediction.Frontend
{
    public static class Constants
    {
        private static readonly string BaseUri = "http://10.0.2.2:50458/api/";
        //private static readonly string BaseUri = "https://expensepredictionwebapi.azurewebsites.net/api/";

        #region storageKeys

        public static readonly string Token = "token";
        public static readonly string UserId = "userId";

        #endregion

        #region auth

        private static readonly string AuthControllerUri = BaseUri + "auth/";
        public static readonly string RegisterUri = AuthControllerUri + "register";
        public static readonly string GetTokenUri = AuthControllerUri + "get-token";

        #endregion

        #region account

        private static readonly string AccountControllerUri = BaseUri + "account/";
        public static readonly string GetUserUri = AccountControllerUri;
        public static readonly string EditUserUri = AccountControllerUri;
        public static readonly string ChangePasswordUri = AccountControllerUri + "change-password";

        #endregion

        #region category

        private static readonly string CategoryControllerUri = BaseUri + "category/";

        public static string GetCategoryUri(CategoryType type, string id)
        {
            return CategoryControllerUri + type + '/' + id;
        }

        public static string EditCategoryUri(CategoryType type)
        {
            return CategoryControllerUri + "edit/" + type;
        }

        public static string GetCategoriesUri(CategoryType type)
        {
            return CategoryControllerUri + type;
        }

        #endregion

        #region expense

        public static readonly string ExpenseControllerUri = BaseUri + "expense/";

        public static readonly string GetExpensesUri = ExpenseControllerUri;
        public static readonly string AddExpenseUri = ExpenseControllerUri + "add";
        public static readonly string EditExpenseUri = ExpenseControllerUri + "edit";

        public static string GetExpenseUri(string id)
        {
            return ExpenseControllerUri + id;
        }

        public static string GetLinkedExpensesUri(string id)
        {
            return ExpenseControllerUri + "linked/" + id;
        }

        #endregion
    }
}