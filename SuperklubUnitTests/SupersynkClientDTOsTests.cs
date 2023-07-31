using Superklub;
using System.Diagnostics;
using System.Text.Json;

namespace SuperklubUnitTests
{
    [TestClass]
    public class SupersynkClientDTOsTests
    {
        [TestMethod]
        public void FromJSONStringTest()
        {
            string oneClientString = "{\"client_id\":\"ada\",\"properties\":[{\"key\":\"titi\",\"value\":\"toto\"}]}";
            string clientsString = "[" + oneClientString + "]";

            SupersynkClientDTOs DTOs = new SupersynkClientDTOs();
            DTOs.FromJSONString(clientsString);

            Assert.AreEqual(1, DTOs.List.Count);
            var dto = DTOs.List[0];
            Assert.AreEqual("ada", dto.ClientId);
            Assert.AreEqual(1, dto.Properties.Count);
            Assert.AreEqual("titi", dto.Properties[0].Key);
            Assert.AreEqual("toto", dto.Properties[0].Value);
        }
    }
}
