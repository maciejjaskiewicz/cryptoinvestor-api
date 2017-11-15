namespace CryptoInvestor.Core.Exceptions
{
    public static class ErrorCodes
    {
        public static string InvalidEmail => "invalid_email";
        public static string InvalidPassword => "invalid_password";
        public static string InvalidUsername => "invalid_username";
        public static string InvalidFirstName => "invalid_firstName";
        public static string InvalidLastName => "invalid_lastName";
        public static string InvalidGender => "invalid_gender";

        public static string InvalidCoinSymbol => "invalid_coin_symbol";
        public static string InvalidCoinName => "invalid_coin_name";
        public static string InvalidPurchasePrice => "invalid_purchase_price";
        public static string InvalidPurchaseDate => "invalid_purchase_date";
        public static string InvalidSoldPrice => "invalid_sold_price";
        public static string InvalidSoldDate => "invalid_sold_date";
        public static string InvalidCurrency => "invalid_currency";

        public static string InvalidPortfolioName => "invalid_portfolio_name";
        public static string InvalidTransaction => "invalid_transaction";

        public static string InvalidIconUrl => "invalid_icon_url";

        public static string InvalidCoin => "invalid_coin";
        public static string CoinNotFound => "coin_not_found";
        public static string CoinAlreadyExists => "coin_already_exists";

        public static string InvalidAmount => "invalid_amount";
    }
}