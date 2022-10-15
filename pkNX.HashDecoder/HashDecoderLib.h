#pragma once
#include "../pkNX.HashDecoderConsole/HashDecoder.h"

using namespace System;

namespace pkNXHashDecoder {
    private ref class FnvHashDecoderWrapper
    {
    public:
        std::unordered_map<uint64_t, std::string> StartDecoding(int length, int startChar)
        {
            return nativeHandle_->StartDecoding(length, startChar);
        }

    private:
        FnvHashDecoder* nativeHandle_ = new FnvHashDecoder();
    };

    public ref class FnvHashDecoderLib
    {
    public:
        static System::Collections::Generic::Dictionary<System::UInt64, System::String^>^ StartDecoding(int length, int startChar)
        {
            FnvHashDecoderWrapper decoder{};
            auto map = decoder.StartDecoding(length, startChar);

            auto dict = gcnew System::Collections::Generic::Dictionary<System::UInt64, System::String^>();

            for (const auto& entry : map)
                dict->Add(entry.first, gcnew System::String(entry.second.c_str()));

            return dict;
        }
    };
}
