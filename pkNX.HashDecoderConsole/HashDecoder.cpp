#include "pch.h"

#include "HashDecoder.h"
#include "Timer.h"

using namespace pkNXHashDecoder;

std::string FnvHashDecoder::BuildString(const size_t length) const
{
    // Create string from chars
    std::vector<char> chars(length);
    for (int j = 0; j < length; ++j)
        chars[j] = AllowedChars[charIndex[j]];

    return { chars.begin(), chars.end() };
}

std::unordered_map<uint64_t, std::string> FnvHashDecoder::StartDecoding(const int length, const int startChar)
{
    std::unordered_map<uint64_t, std::string> foundKeys;

    // Reset all chars to starting point
    charIndex[0] = (uint32_t)startChar;

    // Build the hash of all chars except the last one
    hashBases[0] = kOffsetBasis_64;
    for (int i = 0; i < length - 1; ++i)
    {
        hashBases[i] ^= AllowedChars[0];
        hashBases[i] *= kFnvPrime_64;

        // Move the hash to the next index as basis
        hashBases[i + 1] = hashBases[i];
    }

    //LoopTimer timer{};
    while (charIndex[0] == startChar)
    {
        // Loop through all AllowedChars
        const uint64_t hashBase = hashBases[length - 1];
        for (int i = 0; i < AllowedCharsCount; ++i)
        {
            uint64_t hash = hashBase;
            hash ^= AllowedChars[i];
            hash *= kFnvPrime_64;

            if (hash == 9252365659083253459ul)
            {
                charIndex[length - 1] = i;
                foundKeys.emplace(hash, BuildString(length));
            }
        }

        // Already found all keys
        if (!foundKeys.empty())
            return foundKeys;

        bool carry = false;
        int c = length - 2;
        for (; c >= 0; c--)
        {
            ++charIndex[c];
            carry = charIndex[c] >= AllowedCharsCount;

            if (!carry)
                break;

            charIndex[c] = 0;
        }

        // Reached the end
        if (carry && c == 0)
            break;

        // Rebuild the hash basis
        hashBases[c] = hashBases[c - 1];
        for (int i = c; i < length - 1; ++i)
        {
            hashBases[i] ^= AllowedChars[charIndex[i]];
            hashBases[i] *= kFnvPrime_64;

            // Move the hash to the next index as basis
            hashBases[i + 1] = hashBases[i];

            // Automatically test for shorter length strings
            if (hashBases[i] == 9252365659083253459ul)
            {
                foundKeys.emplace(hashBases[i], BuildString(i));
            }
        }

        /*if (c < length - 4)
            timer.Loop();

        if (c < length - 6)
            timer.Print();*/
    }

    return foundKeys;
}
