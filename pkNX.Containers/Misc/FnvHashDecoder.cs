using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace pkNX.Containers;

public class FnvHashDecoder
{
    public const string AllowedChars = "etaoinshrdlcumw_0123456789fgypbvkjxqz/."; // Sorted by most common first

    public HashSet<ulong> HashesToTest = new() { 9252365659083253459, 17205188591700921149, 1234775724179408742 };

    // 64 bit implementation
    private const ulong kFnvPrime_64 = 0x00000100000001b3;
    private const ulong kOffsetBasis_64 = 0xCBF29CE484222645;

    public Dictionary<ulong, string> FoundKeys = new();

    public Dictionary<ulong, string> Run(int minLength, int maxLength)
    {
        Debug.Assert(FnvHash.HashFnv1a_64("stickyball") == 0x15cb1f582d2b05ab);

        int length = maxLength;

        var tasks = new List<Task>();

        for (int i = 0; i < AllowedChars.Length; ++i)
        {
            int startChar = i;
            tasks.Add(Task.Run(() =>
                {
                    var dict = pkNXHashDecoder.FnvHashDecoderLib.StartDecoding(length, startChar);

                    if (dict.Count != 0)
                    {
                        foreach (var x in dict)
                        {
                            FoundKeys.Add(x.Key, x.Value);
                        }
                    }
                    //StartLoop(length, startChar);
                }));
        }

        Task t = Task.WhenAll(tasks);
        try
        {
            t.Wait();
        }
        catch { }

        return FoundKeys;
    }

    private void StartLoop(int length, int startChar)
    {
        Span<char> chars = stackalloc char[length];
        Span<byte> allowedCharIndex = stackalloc byte[length]; // Current AllowedChar

        // Reset all chars to starting point
        chars[0] = AllowedChars[startChar];
        allowedCharIndex[0] = (byte)startChar;

        for (int i = 1; i < length; i++)
            chars[i] = AllowedChars[0];

        while (allowedCharIndex[0] == startChar)
        {
            // Build the hash of all chars except the last one
            ulong hashBase = kOffsetBasis_64;
            for (int i = 0; i < length - 1; ++i)
            {
                hashBase ^= chars[i];
                hashBase *= kFnvPrime_64;

                // Automatically test for shorter length strings
                if (hashBase == 9252365659083253459)
                {
                    FoundKeys.Add(hashBase, new string(chars[0..i]));
                }
            }

            // Loop through all AllowedChars
            for (int j = 0; j < AllowedChars.Length; ++j)
            {
                ulong hash = hashBase;
                hash ^= AllowedChars[j];
                hash *= kFnvPrime_64;

                if (hash == 9252365659083253459)
                {
                    chars[length - 1] = AllowedChars[j];
                    FoundKeys.Add(hash, new string(chars));
                }
            }

            int carry = 0;
            int c = length - 2;
            for (; c >= 0; c--)
            {
                ++allowedCharIndex[c];
                carry = allowedCharIndex[c] / AllowedChars.Length;
                allowedCharIndex[c] %= (byte)AllowedChars.Length;

                chars[c] = AllowedChars[allowedCharIndex[c]];

                if (carry == 0)
                    break;
            }

            // Reached the end
            if (carry != 0 && c == 0)
                break;

            // Already found all keys
            if (FoundKeys.Count != 0)
                return;
        }
    }
}
