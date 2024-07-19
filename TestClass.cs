[MustOverride("DoSomething")]
public class TestBaseClass
{
    
    public virtual void DoSomething()
    {
        // Do something
    }
}
//编译后会报错提示该类没有复写DoSomething方法
public class TestDerivedClass : TestBaseClass
{
    
}

public class TestDerivedClass2 : TestBaseClass
{
    public override void DoSomething()
    {
        // Do something
    }
}