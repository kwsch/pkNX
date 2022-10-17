#pragma once
#include "pch.h"

namespace pkNXHashDecoder {
    class __declspec(align(64)) FnvHashDecoder
    {
    public:
        std::unordered_map<uint64_t, std::string> StartDecoding(int length, int startChar, std::string_view prefix);

        constexpr static int MAX_LENGTH = 128;
        constexpr static std::string_view AllowedChars = "etaoinshrdlcumw_0123456789fgypbvkjxqz/."; // Sorted by most common first
        constexpr static size_t AllowedCharsCount = AllowedChars.size();

    private:
        std::string BuildString(size_t length) const;

        constexpr static uint64_t kFnvPrime_64 = 0x00000100000001b3;
        constexpr static uint64_t kOffsetBasis_64 = 0xCBF29CE484222645;

        std::array<uint32_t, MAX_LENGTH> charIndex{}; // Current AllowedChar index
        std::array<uint64_t, MAX_LENGTH> hashBases{}; // Current AllowedChar index
    };
}
