namespace CryptoInvestor.Infrastructure.Exceptions
{
    public static class ErrorCodes
    {
        public static string EmailInUse => "email_in_use";
        public static string UserNotFound => "user_not_found";
        public static string InvalidCredentials => "invalid_credentials";

        public static string FavouritesNotFound => "favourites_collection_not_found";
        public static string FavouritesAlreadyExists => "favourites_collection_already_exists";
        public static string InvalidCoin => "invalid_coin";

        public static string PortfolioNameInUse => "portfolio_name_in_use";
        public static string PortfolioNotFound => "portfolio_not_found";

        public static string TransactionNotFound => "transaction_not_found";
    }
}