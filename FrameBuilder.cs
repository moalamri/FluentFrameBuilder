using System;
using System.Text;
using System.Collections.Generic;

public class FrameBuilder
{
    private List<byte> frame { get; set; }
    private int frameNumber { get; set; }
    private byte
      stx = 0x02,
      etx = 0x03,
      etb = 0x17,
      eot = 0x04,
      enq = 0x05,
      ack = 0x06,
      nak = 0x15,
      nul = 0x00,
      cr = 0x0D,
      lf = 0x0A;

    private byte[] byteToBytes(byte b) => new List<byte> { b }.ToArray();
    private byte[] stringToBytes(string s) => Encoding.ASCII.GetBytes(s);
    public byte[] ENQ => byteToBytes(enq);
    public byte[] EOT => byteToBytes(eot);
    public byte[] ACK => byteToBytes(ack);
    public byte[] NAK => byteToBytes(nak);
    public byte[] NUL => byteToBytes(nul);

    public FrameBuilder STX()
    {
        frame.Add(stx);
        return this;
    }

    public FrameBuilder ETB()
    {
        frame.Add(etb);
        return this;
    }

    public FrameBuilder CR()
    {
        frame.Add(cr);
        return this;
    }

    public FrameBuilder LF()
    {
        frame.Add(lf);
        return this;
    }

    public FrameBuilder Text(string txt)
    {
        frame.AddRange(Encoding.ASCII.GetBytes(txt));
        return this;
    }

    private void reset()
    {
        frame = new();
    }

    public FrameBuilder ETX()
    {
        frame.Add(etx);
        return this;
    }

    public FrameBuilder X(string v, int times)
    {
        var c = new string(Convert.ToChar(v), times);
        return Text(c);
    }

    public FrameBuilder Pipe(int times)
    {
        return X("|", times);
    }

    public byte[] ToArray()
    {
        return frame.ToArray();
    }

    private string BytesToString(List<byte> bytes)
    {
        return Encoding.UTF8.GetString(bytes.ToArray());
    }

    public FrameBuilder Checksum()
    {
        var completeFrame = BytesToString(frame);
        int cs = 0;
        for (int i = 0; i < completeFrame.Length; i++)
        {
            cs += (int)completeFrame[i];
        }
        cs %= 256;
        string hex = cs.ToString("x").ToUpper();
        hex = hex.Length == 1 ? "0" + hex : hex;
        return Text(hex);
    }

    private void nextFrame()
    {
        frameNumber += 1;
    }

    public FrameBuilder Start()
    {
        reset();
        return this;
    }

    public FrameBuilder End()
    {
        nextFrame();
        return this;
    }

    public FrameBuilder FrameNumber(int number = -1)
    {
        if (number >= 0)
        {
            frameNumber = number;
            Text(number.ToString());
            return this;
        }
        Text(frameNumber.ToString());
        return this;
    }

    public FrameBuilder MessageContent(string dcm)
    {
        Text(dcm.ToUpper());
        return this;
    }

    public FrameBuilder()
    {
        frameNumber = 0;
        frame = new();
    }

}


public class Utils
{
    private readonly List<string> low = new()
    {
        "<NUL>",
        "<SOH>",
        "<STX>",
        "<ETX>",
        "<EOT>",
        "<ENQ>",
        "<ACK>",
        "<BEL>",
        "<BS>",
        "<HT>",
        "<LF>",
        "<VT>",
        "<FF>",
        "<CR>",
        "<SO>",
        "<SI>",
        "<DLE>",
        "<DC1>",
        "<DC2>",
        "<DC3>",
        "<DC4>",
        "<NAK>",
        "<SYN>",
        "<ETB>",
        "<CAN>",
        "<EM>",
        "<SUB>",
        "<ESC>",
        "<FS>",
        "<GS>",
        "<RS>",
        "<US>"
    };

    public string ToString(byte[] _frame)
    {
        return Encoding.UTF8.GetString(_frame);
    }

    public string ToPrintable(byte[] _frame)
    {
        string frame = "";
        foreach (char ch in _frame)
        {
            if ((int)ch <= 32)
            {
                frame += low[(int)ch];
            }
            else
            {
                frame += ch;
            }
        }
        return frame;
    }
}
