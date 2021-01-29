namespace Skidbladnir.Client.Freenom.Dns
{
    /// <summary>
    /// Static Freenom client factory
    /// </summary>
    public static class FreenomClientFactory
    {
        /// <summary>
        /// Create implementation of IFreenomClient
        /// </summary>
        public static IFreenomClient Create()
        {
            return new FreenomClient();
        }
    }
}