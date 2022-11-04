### Fluent frame builder
This helper class is used with the low-level protocol for interacting with medical instruments. It will make writing frames data more readable and easy to build/edit.
Using the chaining methods you can construct your frame in many different standards and specifications.
> There's almost no unified standard even from the same manufacturer, so this fluent frame builder should make it easier among different machines following different standards.

### Examples
```cs
var build = new FrameBuilder();
var utils = new Utils();

// Complete frame similar to LIS1-A standard
var head = build
    .STX()
    .FrameNumber() // or FrameNumber(number) to set manually: from 0-7
    .MessageContent("H") // the data content of the message 
    .Pipe(1) // number of vertical pipe character |
    .Text("MACH1") // any string is also chainable at any position 
    .Pipe(1)
    .ETX() // or ETB() for intermediate frames
    .Checksum() // calculate and append the 8-bit checksum hex
    .CR()
    .LF()
    .ToArray(); // important
 
// the chain must end with ToArray() method to avoid returning the class instance.
// adding ToArray() will return a set of bytes ready to be written into the buffer.

// you can also use ToPrintable(byte[]) utility to print a readable form of the frame
Console.WriteLine(utils.ToPrintable(head));
// <STX>0H|MACH1|0<ETX>EF<CR><LF>

```
Or add your own bytes:
```cs
    var build = new FrameBuilder();
    var utils = new Utils();

    var custom = 
      build
      .Byte(0x17)
      .Bytes(new byte[] {0x00, 0x01})
      .ToArray();

    Console.WriteLine(utils.ToPrintable(custom));
    // <ETB><NUL><SOH>
```

Auto frame number
> To have an auto increment frame number you must start the chain with `Start()` and end it with `End()` before calling `ToArray()` that also will clear the current frame so you can build a new one with the same instance of `FrameBuilder()`.
```cs
    var build = new FrameBuilder();
    var utils = new Utils();

    var H = 
      build
      .Start()
      .STX()
      .FrameNumber()
      .MessageContent("H")
      .ETX()
      .End()
      .ToArray();

    Console.WriteLine(utils.ToPrintable(H));
    // <STX>0H<ETX>

    var P = 
      build
      .Start()
      .STX()
      .FrameNumber()
      .MessageContent("P")
      .ETX()
      .End()
      .ToArray();

    Console.WriteLine(utils.ToPrintable(P));
    // <STX>1P<ETX>

    var Q = 
      build
      .Start()
      .STX()
      .FrameNumber()
      .MessageContent("Q")
      .Pipe(1)
      .X("N", 5) // This method is to repeat any text for set amount
      .ETX()
      .End()
      .ToArray();

    Console.WriteLine(utils.ToPrintable(Q));
    // <STX>2Q|NNNNNN<ETX>
```


### Online demo

[https://replit.com/@moalamri/Fluent-Frame-Builder?v=1](https://replit.com/@moalamri/Fluent-Frame-Builder?v=1)

Works with `.Net (cross-platform)`
### Todo
- [x] a chainable method to add custom (user defined) bytes.
- [ ] a utility method to clean ASCII characters from a set of bytes.
- [ ] a method to use a fixed bytes length to support machines like Beckman AU480