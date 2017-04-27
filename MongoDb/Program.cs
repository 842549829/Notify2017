namespace MongoDb
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args">args</param>
        public static void Main(string[] args)
        {
            TestData.InitDicData();
            var data = TestData.ViewTheData("k1");
        }
    }
}