using Superklub;

namespace SuperklubUnitTests
{
    [TestClass]
    public class SupersynkClientDTOTests
    {
        [TestMethod]
        public void ToJsonStringTest()
        {
            SupersynkClientDTO dto = new SupersynkClientDTO("ada");
            dto.Data.Add("titi");

            string jsonString = dto.ToJsonString();

            string expectedString = "{\"client_id\":\"ada\",\"data\":[\"titi\"]}";
            Assert.AreEqual(expectedString, jsonString);
        }
    }
}
