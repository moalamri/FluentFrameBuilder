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

> To have an auto increment frame number you must start the chain with `Start()` and end it with `End()` before calling `ToArray()` that also will clear the current frame so you can build a new one.

### Online demo

[https://replit.com/@moalamri/Fluent-Frame-Builder?v=1](https://replit.com/@moalamri/Fluent-Frame-Builder?v=1)

Works with `.Net (cross-platform)` and `.Net Framework (Windows only)`
### Todo
- a chainable method to add custom (user defined) bytes.
- a method to clean a set of bytes from ASCII characters.
- a method to use a fixed bytes length to support machines like Beckman AU480
