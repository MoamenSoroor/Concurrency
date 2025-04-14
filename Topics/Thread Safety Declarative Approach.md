# Converting Non-Thread-Safe C# Code to Thread-Safe (Declarative Approach)

Here's a set of rules and code examples to transform non-thread-safe code into thread-safe code using declarative programming patterns in C#:

## Rule 1: Immutable Data First
**Declarative Principle**: Prefer immutable data structures to avoid synchronization needs.

```csharp
// Non-thread-safe
public class UserProfile
{
    public string Name { get; set; }
    public int Age { get; set; }
}

// Thread-safe (immutable)
public class UserProfile
{
    public string Name { get; }
    public int Age { get; }
    
    public UserProfile(string name, int age)
    {
        Name = name;
        Age = age;
    }
    
    public UserProfile WithName(string name) => new UserProfile(name, Age);
    public UserProfile WithAge(int age) => new UserProfile(Name, age);
}
```

## Rule 2: Replace Mutable Collections with Thread-Safe Versions
**Declarative Principle**: Use thread-safe collections from `System.Collections.Concurrent`.

```csharp
// Non-thread-safe
private List<string> _items = new List<string>();

// Thread-safe
private ConcurrentBag<string> _items = new ConcurrentBag<string>();
// OR for more specific needs:
private ConcurrentDictionary<string, int> _itemCounts = new ConcurrentDictionary<string, int>();
```

## Rule 3: Use `Lazy<T>` for Thread-Safe Initialization
**Declarative Principle**: Defer and cache initialization safely.

```csharp
// Non-thread-safe (double-check locking pattern)
private SomeResource _resource;
private readonly object _lock = new object();
public SomeResource Resource
{
    get
    {
        if (_resource == null)
        {
            lock (_lock)
            {
                if (_resource == null)
                {
                    _resource = InitializeResource();
                }
            }
        }
        return _resource;
    }
}

// Thread-safe (declarative)
private readonly Lazy<SomeResource> _resource = new Lazy<SomeResource>(InitializeResource);
public SomeResource Resource => _resource.Value;
```

## Rule 4: Replace Locks with Declarative Alternatives
**Declarative Principle**: Use higher-level abstractions instead of manual locking.

```csharp
// Non-thread-safe
private int _counter;
public void IncrementCounter()
{
    lock(_lock)
    {
        _counter++;
    }
}

// Thread-safe (declarative)
private readonly object _lock = new object();
private int _counter;
public void IncrementCounter() => Interlocked.Increment(ref _counter);
```

## Rule 5: Use Async-Compatible Synchronization
**Declarative Principle**: Prefer async-compatible synchronization primitives.

```csharp
// Non-thread-safe (blocking)
private readonly object _lock = new object();
public void ProcessData()
{
    lock(_lock)
    {
        // Critical section
    }
}

// Thread-safe (async-compatible)
private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
public async Task ProcessDataAsync()
{
    await _semaphore.WaitAsync();
    try
    {
        // Critical section
    }
    finally
    {
        _semaphore.Release();
    }
}
```

## Rule 6: Functional Approach for State Transformation
**Declarative Principle**: Use functional patterns for state management.

```csharp
// Non-thread-safe
private Dictionary<string, int> _wordCounts = new Dictionary<string, int>();
public void AddWord(string word)
{
    if (!_wordCounts.ContainsKey(word))
    {
        _wordCounts[word] = 0;
    }
    _wordCounts[word]++;
}

// Thread-safe (functional approach)
private ConcurrentDictionary<string, int> _wordCounts = new ConcurrentDictionary<string, int>();
public void AddWord(string word) => 
    _wordCounts.AddOrUpdate(word, 1, (_, count) => count + 1);
```

## Rule 7: Use Declarative Parallelism
**Declarative Principle**: Use higher-level parallel constructs.

```csharp
// Non-thread-safe (manual threads)
public void ProcessItems(List<Item> items)
{
    var threads = new List<Thread>();
    foreach (var item in items)
    {
        var thread = new Thread(() => Process(item));
        thread.Start();
        threads.Add(thread);
    }
    // ... join threads
}

// Thread-safe (declarative)
public async Task ProcessItemsAsync(IEnumerable<Item> items)
{
    await Parallel.ForEachAsync(items, async (item, ct) => 
    {
        await ProcessAsync(item, ct);
    });
}
```

## Rule 8: Isolate Mutable State
**Declarative Principle**: Contain mutable state in well-defined boundaries.

```csharp
// Non-thread-safe
public class ShoppingCart
{
    private List<Item> _items = new List<Item>();
    
    public void AddItem(Item item) => _items.Add(item);
    public void RemoveItem(Item item) => _items.Remove(item);
}

// Thread-safe (isolated state)
public class ShoppingCart
{
    private readonly ImmutableList<Item> _items = ImmutableList<Item>.Empty;
    private readonly object _sync = new object();
    
    public ShoppingCart AddItem(Item item)
    {
        lock(_sync)
        {
            return new ShoppingCart { _items = _items.Add(item) };
        }
    }
    
    // Similar for RemoveItem
}
```

## Key Declarative Patterns Summary:

1. **Immutable data** - Eliminates need for synchronization
2. **Thread-safe collections** - Pre-built concurrent data structures
3. **Atomic operations** - `Interlocked` class for simple operations
4. **Functional state updates** - Treat state as immutable and transform it
5. **Async synchronization** - `SemaphoreSlim` instead of locks for async code
6. **Higher-level abstractions** - `Parallel`, `Lazy<T>`, `ConcurrentDictionary` methods

These declarative approaches make thread safety more maintainable by reducing explicit synchronization code and leveraging built-in thread-safe constructs.
