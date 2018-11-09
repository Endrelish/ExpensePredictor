using ExpensePrediction.DataTransferObjects;

namespace ExpensePrediction.Frontend
{
    public static class Constants
    {
        private static readonly string BaseUrl = "http://localhost:50458/api/";

        #region auth
        private static readonly string AuthControllerUrl = "auth/";
        public static readonly string RegisterUrl = AuthControllerUrl + "register";
        public static readonly string GetTokenUrl = AuthControllerUrl + "get-token";
        #endregion

        #region account
        private static readonly string AccountControllerUrl = "account/";
        public static readonly string GetUserUrl = AccountControllerUrl;
        public static readonly string EditUserUrl = AccountControllerUrl;
        public static readonly string ChangePasswordUrl = AccountControllerUrl + "change-password";
        #endregion

        #region category
        private static readonly string CategoryControllerUrl = "category/";

        public static string GetCategoryUrl(CategoryType type, string id) => CategoryControllerUrl + type + '/' + id;
        public static string EditCategoryUrl(CategoryType type) => CategoryControllerUrl + "edit/" + type;
        public static string GetCategoriesUrl(CategoryType type) => CategoryControllerUrl + type;
        #endregion

        #region expense
        public static readonly string ExpenseControllerUrl = "expense/";

        public static readonly string GetExpensesUrl = ExpenseControllerUrl;
        public static readonly string AddExpenseUrl = ExpenseControllerUrl + "add";
        public static readonly string EditExpenseUrl = ExpenseControllerUrl + "edit";
        public static string GetExpense(string id) => ExpenseControllerUrl + id;
        public static string GetLinkedExpensesUrl(string id) => ExpenseControllerUrl + "linked/" + id;
        #endregion
    }
}
