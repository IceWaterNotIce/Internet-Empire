using UnityEngine;
using NUnit.Framework;

public class MyTest 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SetUp]
    public void SetUp()
    {
        Debug.Log("SetUp");
    }

    // Update is called once per frame
    [Test]
    public void Test()
    {
        Debug.Log("Test");
        Assert.IsTrue(true);
    }
    
}
