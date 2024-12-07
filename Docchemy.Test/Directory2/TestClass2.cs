namespace Docchemy.Test.Directory2
{
    /// <summary>
    /// It is a Test Class for the doc create project.
    /// </summary>
    public class TestClass2
    {
        /// <summary>
        /// This method is for the test method 2.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="age"></param>
        void TestMethod2(string lastname, Guid id)
        {
            var user = new
            {
                lastname = lastname,
                id = id
            };// this variable is a annonymous type. It returns lastname and id.

        }
    }
}
