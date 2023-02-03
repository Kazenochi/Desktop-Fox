using DesktopFox;
using System.Diagnostics;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void LockList_RandomQueueTest()
        {
            DesktopFox.LockListQueue locklist = new(50);
            
            for(int i = 0; i < 50; i++)
            {
                var item = locklist.GetNewRandomItem();
                Debug.WriteLine("Zurückgegebenes Item - Count: " + i + "   Rückgabewert: " + item);
                Assert.AreNotEqual(item, -1);
            }           
        }

        [TestMethod]
        public void LockList_LinearQueueTest()
        {
            DesktopFox.LockListQueue locklist = new(50);
            int lastItem = -1;
            for (int i = 0; i < 50; i++)
            {
                var item = locklist.GetNextItem();
                Debug.WriteLine("Zurückgegebenes Item - Count: " + i + "   Rückgabewert: " + item);
                Assert.IsTrue(item == lastItem + 1);
                lastItem = item;
            }
        }

        [TestMethod]
        public void LockList_LinearSwitchQueueTest()
        {
            DesktopFox.LockListQueue locklist = new(50);
            int lastItem;
            for (int i = 0; i < 50; i++)
            { 
                Debug.WriteLine($"Nr. {i}");
                lastItem = -1;
                for (int j = 0; j < 10; j++)
                {
                    var linearItem = locklist.GetNextItem();

                    Debug.WriteLine("Linear Item - Rückgabewert: " + linearItem);
                    Assert.IsTrue(linearItem == lastItem + 1);
                    lastItem = linearItem;
                }

                var randomItem = locklist.GetNewRandomItem();
                Debug.WriteLine("Randome Item - Rückgabewert: " + randomItem);

                lastItem = -1;
                for(int j = 0; j < 50; j++)
                {
                    var linearItem = locklist.GetNextItem();

                    Debug.WriteLine("Linear Item - Rückgabewert: " + linearItem);
                    Assert.IsTrue(linearItem == lastItem + 1);
                    lastItem = linearItem;
                }        
            }
        }
    }
}
