using Superklub;

namespace SuperklubUnitTests
{
    [TestClass]
    public class SuperklubNodeConverterTests
    {
        [TestMethod]
        public void TestConvertToString()
        {
            SuperklubNodeRecord node = new SuperklubNodeRecord();
            node.Id = "a";
            node.Position = (1.1f, 2.2f, 3.3f);
            node.Rotation = (0f, 1f, 0f, 0f);
            node.Shape = "pill";
            node.Color = "green";

            string str = SuperklubNodeConverter.ConvertToString(node);

            Assert.AreEqual("id=a;pos=1.1,2.2,3.3;rot=0,1,0,0;shape=pill;color=green", str);
        }

        [TestMethod]
        public void TestParseTokenWithWrongInput()
        {
            var nodeRecord = new SuperklubNodeRecord();
            
            Assert.IsFalse(SuperklubNodeConverter.ParseToken("pos=", nodeRecord));
            Assert.IsFalse(SuperklubNodeConverter.ParseToken("pos=.", nodeRecord));
            Assert.IsFalse(SuperklubNodeConverter.ParseToken("pos=1,2", nodeRecord));
            Assert.IsFalse(SuperklubNodeConverter.ParseToken("pos=1,2,3,4", nodeRecord));
            Assert.IsFalse(SuperklubNodeConverter.ParseToken("pos=1,2,a", nodeRecord));

            Assert.IsFalse(SuperklubNodeConverter.ParseToken("rot=", nodeRecord));
            Assert.IsFalse(SuperklubNodeConverter.ParseToken("rot=.", nodeRecord));
            Assert.IsFalse(SuperklubNodeConverter.ParseToken("rot=1,2,3", nodeRecord));
            Assert.IsFalse(SuperklubNodeConverter.ParseToken("rot=1,2,3,4,5", nodeRecord));
            Assert.IsFalse(SuperklubNodeConverter.ParseToken("rot=1,2,3,a", nodeRecord));
        }

        [TestMethod]
        public void TestParseTokenWithRightInput()
        {
            var nodeRecord = new SuperklubNodeRecord();

            Assert.IsTrue(SuperklubNodeConverter.ParseToken("pos=1,2,3", nodeRecord));
            Assert.AreEqual((1f, 2f, 3f), nodeRecord.Position);

            Assert.IsTrue(SuperklubNodeConverter.ParseToken("pos = 1 , 2 , 3 ", nodeRecord));
            Assert.AreEqual((1f, 2f, 3f), nodeRecord.Position);

            Assert.IsTrue(SuperklubNodeConverter.ParseToken("rot=1,2,3,4", nodeRecord));
            Assert.AreEqual((1f, 2f, 3f, 4f), nodeRecord.Rotation);

            Assert.IsTrue(SuperklubNodeConverter.ParseToken("rot = 1 , 2 , 3 , 4 ", nodeRecord));
            Assert.AreEqual((1f, 2f, 3f, 4f), nodeRecord.Rotation);
        }

        [TestMethod]
        public void TestConvertFromSupersynk()
        {
            SupersynkClientDTO dto = new SupersynkClientDTO("ada");
            dto.Data.Add("id=head;pos=1,2,3;rot=0,1,0,0;shape=box;color=blue");
            
            var nodes = SuperklubNodeConverter.ConvertFromSupersynk(dto);

            Assert.IsNotNull(nodes);
            Assert.AreEqual(1, nodes.Count);
            var node = nodes[0];
            Assert.AreEqual("ada:head", node.Id);
            Assert.AreEqual((1, 2, 3), node.Position);
            Assert.AreEqual((0, 1, 0, 0), node.Rotation);
            Assert.AreEqual("box", node.Shape);
            Assert.AreEqual("blue", node.Color);
        }
    }
}