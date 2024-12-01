using UnityEngine;
using NUnit.Framework;

public class MyTest : MonoBehaviour
{
    [Test]
    public void MyTestSimplePasses()
    {
        Assert.IsTrue(true);
    }
    
}
