using ExpensePrediction.DataTransferObjects;

namespace ExpensePrediction.Frontend
{
    public static class Constants
    {
        private static readonly string BaseUri = "http://localhost:50458/api/";

        #region auth
        private static readonly string AuthControllerUri = "auth/";
        public static readonly string RegisterUri = AuthControllerUri + "register";
        public static readonly string GetTokenUri = AuthControllerUri + "get-token";
        #endregion

        #region account
        private static readonly string AccountControllerUri = "account/";
        public static readonly string GetUserUri = AccountControllerUri;
        public static readonly string EditUserUri = AccountControllerUri;
        public static readonly string ChangePasswordUri = AccountControllerUri + "change-password";
        #endregion

        #region category
        private static readonly string CategoryControllerUri = "category/";

        public static string GetCategoryUri(CategoryType type, string id) => CategoryControllerUri + type + '/' + id;
        public static string EditCategoryUri(CategoryType type) => CategoryControllerUri + "edit/" + type;
        public static string GetCategoriesUri(CategoryType type) => CategoryControllerUri + type;
        #endregion

        #region expense
        public static readonly string ExpenseControllerUri = "expense/";

        public static readonly string GetExpensesUri = ExpenseControllerUri;
        public static readonly string AddExpenseUri = ExpenseControllerUri + "add";
        public static readonly string EditExpenseUri = ExpenseControllerUri + "edit";
        public static string GetExpense(string id) => ExpenseControllerUri + id;
        public static string GetLinkedExpensesUri(string id) => ExpenseControllerUri + "linked/" + id;
        #endregion
    }
}
