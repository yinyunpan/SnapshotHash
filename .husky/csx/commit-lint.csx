using System;
using System.Collections;

private var msg = File.ReadAllLines(Args[0])[0];
msg ??= string.Empty;
msg = msg.Trim();
if (msg.Length <= 5)
{
    Console.WriteLine(">>>>>>无效提交信息，必须大于5个字符<<<<<<");
    return 1;
}

return 0;
