using Superklub;
using System.Diagnostics;
using System.Text.Json;

namespace SuperklubUnitTests
{
    [TestClass]
    public class SupersynkClientDTOsTests
    {
        [TestMethod]
        public void FromJsonStringTest()
        {
            string oneClientString = "{\"client_id\":\"ada\",\"data\":[\"titi\"]}";
            string clientsString = "[" + oneClientString + "]";

            SupersynkClientDTOs? DTOs = SupersynkClientDTOs.FromJsonString(clientsString);

            Assert.IsNotNull(DTOs);
            Assert.AreEqual(1, DTOs.Count);
            var dto = DTOs[0];
            Assert.AreEqual("ada", dto.ClientId);
            Assert.AreEqual(1, dto.Data.Count);
            Assert.AreEqual("titi", dto.Data[0]);
        }
    }
}
