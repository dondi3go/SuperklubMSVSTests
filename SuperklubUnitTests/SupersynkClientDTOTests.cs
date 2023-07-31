using Superklub;

namespace SuperklubUnitTests
{
    [TestClass]
    public class SupersynkClientDTOTests
    {
        [TestMethod]
        public void ToJSONStringTest()
        {
            SupersynkClientDTO dto = new SupersynkClientDTO("ada");
            dto.AddProperty("titi", "toto");

            string jsonString = dto.ToJSONString();

            string expectedString = "{\"client_id\":\"ada\",\"properties\":[{\"key\":\"titi\",\"value\":\"toto\"}]}";
            Assert.AreEqual(expectedString, jsonString);
        }
    }
}
