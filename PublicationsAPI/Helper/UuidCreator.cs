namespace PublicationsAPI.Helper
{
    public static class UuidCreator
    {
        public static string CreateUuid(){
            return Guid.NewGuid().ToString("N");
        }
    }
}