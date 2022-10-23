### Fluent frame builder
This helper class can save you time and make writing frames data more readable and easy to build/edit.
Using the chaining methods you can construct your frame in many different standards.
> There's almost no unified standard even from the same manufacturer, so this fluent frame builder should make it easier among different machines following different standards.

### Examples
```cs
    var build = new FrameBuilder();
    var utils = new Utils();

    // Complete frame similar to LIS1-A standard
    var frame = 
      build
      .STX()
      .FrameNumber() // or FrameNumber(number) to set manually
      .MessageContent("H")
      .Pipe(1)
      .Text("MACH1")
      .Pipe(1)
      .ETX() // or ETB() for intermediate frames
      .Checksum()
      .CR()
      .LF()
      .ToArray();

    // <STX>0H|MACH1|0<ETX>EF<CR><LF>
    Console.WriteLine(utils.ToPrintable(head));
```

Check the online demo
[https://replit.com/@moalamri/FluentFrameBuilder](https://replit.com/@moalamri/FluentFrameBuilder)

### WIP...