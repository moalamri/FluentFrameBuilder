using System;

class Program {
  public static void Main (string[] args) {
    var frame = new FrameBuilder();
    var utils = new Utils();

    // Single byte in an array ready to send
    var enq = frame.ENQ;

    // Complete frame similar to LIS1-A standard
    var head = 
      frame
      .STX()
      .FrameNumber() // or FrameNumber(number) to set manually
      .MessageContent("H")
      .Pipe(1)
      .Text("MACH1")
      .Pipe(1)
      .Text("0")
      .ETX() // or ETB() for intermediate frame
      .Checksum()
      .CR()
      .LF()
      .ToArray();

    // <STX>0H|MACH1|0<ETX>EF<CR><LF>
    Console.WriteLine(utils.ToPrintable(head));
  }
}















