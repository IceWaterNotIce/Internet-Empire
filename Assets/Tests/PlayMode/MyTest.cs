using UnityEngine;
using NUnit.Framework;

public class MyTest : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    [Test]
    void Test()
    {
        Assert.AreEqual(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
