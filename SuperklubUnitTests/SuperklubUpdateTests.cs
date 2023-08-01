using Superklub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperklubUnitTests
{
    [TestClass]
    public class SuperklubUpdateTests
    {
        [TestMethod]
        public void TestUpdateWithOneNodeToUpdate()
        {
            SupersynkClientDTOs oldDTOs = new SupersynkClientDTOs();
            SupersynkClientDTO oldDTO = new SupersynkClientDTO("ada");
            oldDTO.Data.Add("id=head;shape=ball");
            oldDTOs.Add(oldDTO);
            
            SupersynkClientDTOs newDTOs = new SupersynkClientDTOs();
            SupersynkClientDTO newDTO = new SupersynkClientDTO("ada");
            newDTO.Data.Add("id=head;shape=box");
            newDTOs.Add(newDTO);

            // Create update object
            SuperklubUpdate update = new SuperklubUpdate(oldDTOs, newDTOs);
            
            // Check
            Assert.AreEqual(0, update.disconnectedClients.Count);
            Assert.AreEqual(0, update.newConnectedClients.Count);
            Assert.AreEqual(0, update.nodesToCreate.Count);
            Assert.AreEqual(1, update.nodesToUpdate.Count);
            Assert.AreEqual(0, update.nodesToDelete.Count);

            Assert.AreEqual("ada:head", update.nodesToUpdate[0].Id);
            Assert.AreEqual("box", update.nodesToUpdate[0].Shape);
        }

        [TestMethod]
        public void TestUpdateWithOneNodeToCreate()
        {
            SupersynkClientDTOs oldDTOs = new SupersynkClientDTOs();

            SupersynkClientDTOs newDTOs = new SupersynkClientDTOs();
            SupersynkClientDTO newDTO = new SupersynkClientDTO("ada");
            newDTO.Data.Add("id=head;shape=box");
            newDTOs.Add(newDTO);

            // Create Update object
            SuperklubUpdate update = new SuperklubUpdate(oldDTOs, newDTOs);

            // Check
            Assert.AreEqual(0, update.disconnectedClients.Count);
            Assert.AreEqual(1, update.newConnectedClients.Count);
            Assert.AreEqual(1, update.nodesToCreate.Count);
            Assert.AreEqual(0, update.nodesToUpdate.Count);
            Assert.AreEqual(0, update.nodesToDelete.Count);

            Assert.AreEqual("ada:head", update.nodesToCreate[0].Id);
            Assert.AreEqual("box", update.nodesToCreate[0].Shape);
        }

        [TestMethod]
        public void TestUpdateWithOneNodeToDelete()
        {
            SupersynkClientDTOs oldDTOs = new SupersynkClientDTOs();
            SupersynkClientDTO oldDTO = new SupersynkClientDTO("ada");
            oldDTO.Data.Add("id=head;shape=ball");
            oldDTOs.Add(oldDTO);

            SupersynkClientDTOs newDTOs = new SupersynkClientDTOs();

            // Create update object
            SuperklubUpdate update = new SuperklubUpdate(oldDTOs, newDTOs);

            // Check
            Assert.AreEqual(1, update.disconnectedClients.Count);
            Assert.AreEqual(0, update.newConnectedClients.Count);
            Assert.AreEqual(0, update.nodesToCreate.Count);
            Assert.AreEqual(0, update.nodesToUpdate.Count);
            Assert.AreEqual(1, update.nodesToDelete.Count);

            Assert.AreEqual("ada:head", update.nodesToDelete[0].Id);
            Assert.AreEqual("ball", update.nodesToDelete[0].Shape);
        }
    }
}
