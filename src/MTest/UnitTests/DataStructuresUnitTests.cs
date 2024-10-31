

using System.Collections.Concurrent;
using Xunit;

public class DataStructuresUnitTests
{
    [Fact]
    public void QuickPopDataStructure_IntValue_PopWithMaxValue()
    {
        var quickPop = new QuickPopDataStructure<int>();
        DataStructure_IntValue_PopWithMaxValue(quickPop);
    }

    [Fact]
    public void QuickPopDataStructure_PersonValue_PopWithMaxValue()
    {
        var quickPop = new QuickPopDataStructure<Person>(new PersonAgeComparer());
        DataStructure_PersonValue_PopWithMaxValue(quickPop);
    }

    [Fact]
    public async Task QuickPopDataStructure_ShouldBeThreadSafe()
    {
        var quickPop = new QuickPopDataStructure<int>();
        await DataStructure_ShouldBeThreadSafe(quickPop);
    }

    [Fact]
    public void QuickPopDataStructure_PopBeforePush_ThrowsInvalidOperationException()
    {
        var quickPop = new QuickPopDataStructure<int>();
        Assert.Throws<InvalidOperationException>(() => quickPop.Pop());
    }

    [Fact]
    public void QuickPushDataStructure_IntValue_PopWithMaxValue()
    {
        var quickPush = new QuickPushDataStructure<int>();
        DataStructure_IntValue_PopWithMaxValue(quickPush);
    }

    [Fact]
    public void QuickPushDataStructure_PersonValue_PopWithMaxValue()
    {
        var quickPush = new QuickPushDataStructure<Person>(new PersonAgeComparer());
        DataStructure_PersonValue_PopWithMaxValue(quickPush);
    }

    [Fact]
    public async Task QuickPushDataStructure_ShouldBeThreadSafe()
    {
        var quickPush = new QuickPushDataStructure<int>();
        await DataStructure_ShouldBeThreadSafe(quickPush);
    }

    [Fact]
    public void QuickPushDataStructure_PopBeforePush_ThrowsInvalidOperationException()
    {
        var quickPush = new QuickPushDataStructure<int>();
        Assert.Throws<InvalidOperationException>(() => quickPush.Pop());
    }

    private void DataStructure_IntValue_PopWithMaxValue(IDataStructure<int> dataStructure)
    {
        dataStructure.Push(1);
        dataStructure.Push(2);
        dataStructure.Push(4);
        dataStructure.Push(3);

        //4
        var p1 = dataStructure.Pop();
        //3
        var p2 = dataStructure.Pop();
        //2
        var p3 = dataStructure.Pop();

        dataStructure.Push(2);
        dataStructure.Push(4);

        //4
        var p4 = dataStructure.Pop();
        //2
        var p5 = dataStructure.Pop();
        //1
        var p6 = dataStructure.Pop();

        Assert.Equal(4, p1);
        Assert.Equal(3, p2);
        Assert.Equal(2, p3);
        Assert.Equal(4, p4);
        Assert.Equal(2, p5);
        Assert.Equal(1, p6);
    }

    private void DataStructure_PersonValue_PopWithMaxValue(IDataStructure<Person> dataStructure)
    {
        dataStructure.Push(new Person("Victor", 21));
        dataStructure.Push(new Person("Adam", 22));
        dataStructure.Push(new Person("Boris", 24));
        dataStructure.Push(new Person("John", 23));

        //Boris
        var p1 = dataStructure.Pop();
        //John
        var p2 = dataStructure.Pop();
        //Adam
        var p3 = dataStructure.Pop();

        dataStructure.Push(new Person("Albert", 22));
        dataStructure.Push(new Person("Bob", 24));

        //Bob
        var p4 = dataStructure.Pop();
        //Albert
        var p5 = dataStructure.Pop();
        //Victor
        var p6 = dataStructure.Pop();

        Assert.Equal($"Boris, Age: 24", p1.ToString());
        Assert.Equal($"John, Age: 23", p2.ToString());
        Assert.Equal($"Adam, Age: 22", p3.ToString());
        Assert.Equal($"Bob, Age: 24", p4.ToString());
        Assert.Equal($"Albert, Age: 22", p5.ToString());
        Assert.Equal($"Victor, Age: 21", p6.ToString());
    }

    private async Task DataStructure_ShouldBeThreadSafe(IDataStructure<int> dataStructure)
    {
        var numberOfTasks = 100;
        var operationsPerTask = 1000;

        var threadIds = new ConcurrentBag<int>();
        var poppedItems = new ConcurrentBag<int>();

        var tasks = new Task[numberOfTasks];

        for (int i = 0; i < numberOfTasks; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                //https://learn.microsoft.com/en-us/dotnet/api/system.threading.thread.managedthreadid
                threadIds.Add(Thread.CurrentThread.ManagedThreadId);

                for (int j = 0; j < operationsPerTask; j++)
                {
                    dataStructure.Push(j);
                    int poppedValue = dataStructure.Pop();
                    poppedItems.Add(poppedValue);

                }
            });
        }

        await Task.WhenAll(tasks);

        var threadsCount = threadIds.ToList().Count;
        Console.WriteLine($"Runned on {threadsCount}");

        // 100 tasks runned 1000 items loop  
        Assert.Equal(operationsPerTask * numberOfTasks, poppedItems.Count());

        // Each number was inserted exactly 100 times, as 
        for (int j = 0; j < operationsPerTask; j++)
        {
            Assert.Equal(numberOfTasks, poppedItems.Where(i => i == j).Count());
        }
    }
}
